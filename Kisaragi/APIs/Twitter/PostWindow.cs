using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using static System.Console;

namespace Kisaragi.APIs.Twitter
{
	/// <summary>
	/// Twitter 投稿フォームを管理します。
	/// </summary>
	public partial class PostWindow : Form
	{

		#region Properties

		/// <summary>
		/// Twitter へ アクセスするための ラッパークラス インスタンス
		/// </summary>
		private Twitter _Twitter { get; set; }

		/// <summary>
		/// Twitter 画像ID
		/// </summary>
		private string _MediaId { get; set; }

		/// <summary>
		/// Twitter へ投稿する画像のファイルパス
		/// </summary>
		private string _PicturePath { get; set; }

		#endregion

		#region Field Variable

		/// <summary>
		/// Tweet 時の エンドポイントURL
		/// </summary>
		private const string _Update = "https://api.twitter.com/1.1/statuses/update.json";

		/// <summary>
		/// Chunk Upload 時の エンドポイントURL
		/// </summary>
		private const string _Upload = "https://upload.twitter.com/1.1/media/upload.json";

		/// <summary>
		/// マウスのクリック位置
		/// </summary>
		private Point _Position;

		#endregion

		#region Constractor

		public PostWindow(Twitter twitter)
		{
			InitializeComponent();
			this._Twitter = twitter;
		}

		#endregion

		#region Method's

		/// <summary>
		/// Twitter 投稿フォームがロードされた時に実行されます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void PostWindow_Load(object sender, EventArgs e)
		{
			// 各種イベント登録
			this.MouseDown += _PostWindowFormMouseDown;
			this.MouseMove += _PostWindowFormMouseMove;
			PostButton.Click += _IsPostStatusChanged;
			CloseButton.Click += _IsCloseStatusChanged;
			PostForm.DragEnter += _IsPostFormDragEnter;
			PostForm.DragDrop += _IsPostFormDragDrop;

			// テキストボックス内キーアサイン：Twitter へ投稿 (Ctrl + Enter)
			this.PostForm.KeyDown += async (s, ee) =>
			{
				if ((ee.KeyData & Keys.Control) == Keys.Control && (ee.KeyData & Keys.Enter) == Keys.Enter)
				{
					await _SelectTweetTypeAsync();
					PostForm.Text = string.Empty;
					Status.Text = "準備完了...";
				}
			};

			// テキストボックスの状態変化時
			this.PostForm.TextChanged += (s, ee) =>
			{
				// 文字数をデクリメント(140文字から)
				var length = 140 - this.PostForm.TextLength;
				StringCounter.Text = length.ToString();
			};

			// Twitter プロフィールを取得します。
			await _GetMineTwitterProfileAsync();

			// アイコンをクリックした際、自身のページへ遷移
			UserImage.Click += (s, ee) => Process.Start("https://twitter.com/" + _Twitter.ScreenName);
		}

		/// <summary>
		/// 投稿タイプを選択します。
		/// </summary>
		/// <returns></returns>
		private async Task _SelectTweetTypeAsync()
		{
			if (string.IsNullOrEmpty(_PicturePath) && string.IsNullOrEmpty(_MediaId))
			{
				// 画像なし
				await _PostAsync(_Update, "status", this.PostForm.Text);
			}
			else
			{
				// 画像あり
				await _PostAsync(_Update, "status", this.PostForm.Text, "media_ids", _MediaId);
			}
		}

		/// <summary>
		/// Post ボタンイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void _IsPostStatusChanged(object sender, EventArgs e) => await _SelectTweetTypeAsync();

		/// <summary>
		/// Close ボタンイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _IsCloseStatusChanged(object sender, EventArgs e) => _Dispose();

		/// <summary>
		/// ♰後始末♰
		/// </summary>
		private void _Dispose() => this.Close();

		/// <summary>
		/// Twitter へ投稿します。
		/// </summary>
		/// <returns></returns>
		private async Task _PostAsync(string url, string type, string tweet, string mediaType = null, string mediaId = null)
		{
			ServicePointManager.Expect100Continue = false;
			var query = new Dictionary<string, string>();

			// 画像付きツイートを実施するかどうか
			if (string.IsNullOrEmpty(mediaType) && string.IsNullOrEmpty(mediaId))
			{
				// 画像なし
				query = new Dictionary<string, string> { { type, tweet } };
			}
			else
			{
				// 画像あり
				query = new Dictionary<string, string> { { type, tweet }, { mediaType, mediaId } };
			}

			var result = await _Twitter.Request(url, HttpMethod.Post, query);
			WriteLine($"result = {result}");

			Status.Text = "Twitter へ投稿しました！";
		}

		/// <summary>
		/// MouseDown イベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _PostWindowFormMouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				_Position = new Point(e.X, e.Y);
		}

		/// <summary>
		/// MouseMove イベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _PostWindowFormMouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				this.Location = new Point(this.Location.X + e.X - _Position.X, this.Location.Y + e.Y - _Position.Y);
		}

		/// <summary>
		/// RichTextBox に画像がドラッグされている時のイベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _IsPostFormDragEnter(object sender, DragEventArgs e)
		{
			//ファイルがドラッグされている場合、カーソルを変更する
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// RichTextBox に画像がドラッグされた後のイベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void _IsPostFormDragDrop(object sender, DragEventArgs e)
		{
			//ドロップされたファイルの一覧を取得
			var fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (fileName.Length <= 0) return;
			this._PicturePath = fileName[0];

			// ドロップ先の確認：RichTextBox
			var target = sender as RichTextBox;
			if (target == null) return;

			using (var image = new FileStream(this._PicturePath, FileMode.Open, FileAccess.Read))
			{
				// Twitter へ chunk upload を事前に行い、media_id を取得
				var id = await _Twitter.Request(_Upload, HttpMethod.Post, new Dictionary<string, string>() { }, image);
				dynamic deserialize = JsonConvert.DeserializeObject(id);
				this._MediaId = deserialize.media_id_string.Value;
			}
		}

		/// <summary>
		/// Twitter プロフィールを取得します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async Task _GetMineTwitterProfileAsync()
		{
			try
			{
				// ScreenName
				var screenName = _Twitter.ScreenName;
				var query = new Dictionary<string, string> { { "screen_name", screenName } };

				// Profile 取得
				var req = await _Twitter.Request("https://api.twitter.com/1.1/users/show.json", HttpMethod.Get, query);

				// Deserialize 実施
				dynamic json = JsonConvert.DeserializeObject(req);

				// UIスレッド更新
				this.Invoke((MethodInvoker)delegate
				{
					UserName.Text = json.name;
					ScreenName.Text = $"@{json.screen_name}";
					UserImage.ImageLocation = json.profile_image_url_https;
					Status.Text = "Twitter へ接続しました...";
				});
			}
			catch (Exception ex) { WriteLine($"User is not found.\r\n{ex.Message}\r\n{ex.StackTrace}"); };
		}

		#endregion

	}
}
