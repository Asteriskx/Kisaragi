using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

		#endregion

		#region Field Variable

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

			// テキストボックス内キーアサイン：Twitter へ投稿 (Ctrl + Enter)
			this.PostForm.KeyDown += async (s, ee) =>
			{
				if ((ee.KeyData & Keys.Control) == Keys.Control && (ee.KeyData & Keys.Enter) == Keys.Enter)
					await _PostAsync();
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
			UserImage.Click += (s, ee) => Process.Start("https://twitter.com/" + _Twitter.ScreenName);
		}

		/// <summary>
		/// Post ボタンイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void _IsPostStatusChanged(object sender, EventArgs e) => await _PostAsync();

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
		private async Task _PostAsync()
		{
			ServicePointManager.Expect100Continue = false;
			var query = new Dictionary<string, string> { { "status", this.PostForm.Text } };
			var result = await _Twitter.Request("https://api.twitter.com/1.1/statuses/update.json", HttpMethod.Post, query);
			WriteLine($"result = {result}");

			// UIスレッド更新
			this.Invoke((MethodInvoker)delegate
			{
				Status.Text = "Twitter へ投稿しました！";
			});
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
					UserImage.ImageLocation = json.profile_image_url;
					Status.Text = "Twitter へ接続しました...";
				});
			}
			catch (Exception ex) { WriteLine($"User is not found.\r\n{ex.Message}\r\n{ex.StackTrace}"); };
		}

		#endregion
	}
}
