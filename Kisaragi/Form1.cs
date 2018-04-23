using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Collections.Generic;

using Kisaragi.Util;
using Kisaragi.Helper;
using Kisaragi.TwitterAPI;
using Kisaragi.TwitterAPI.OAuth;

using static System.Console;

namespace Kisaragi
{
	/// <summary>
	/// Kisaragi メインクラス
	/// </summary>
	public partial class Kisaragi : Form
	{

		#region Field Valiable

		/// <summary>
		/// イベントが購読される準備が完了したか
		/// </summary>
		private bool _IsSubscribed = false;

		/// <summary>
		/// Consumer Key / Consumer Key Secret.
		/// </summary>
		private const string _ConsumerKey = "your consumer key.";
		private const string _ConsumerKeySecret = "your consumer key secret.";

		/// <summary>
		/// マウスのクリック位置
		/// </summary>
		private Point _Position;

		#endregion

		#region Properties

		/// <summary>
		/// Kisaragi 時報管理クラス インスタンス
		/// 監視間隔時間は、1 sec.
		/// </summary>
		private TimerSignal _TimerSignal { get; set; } = new TimerSignal(1000);

		/// <summary>
		/// Kisaragi 時報システムのヘルパクラス インスタンス
		/// </summary>
		public SoundHelpers SoundHelpers { get; set; } = new SoundHelpers();

		/// <summary>
		/// 音声ファイル再生に関しての情報を管理するクラス(Jsonファイル R/W) インスタンス
		/// </summary>
		private SettingJson _Json { get; set; } = new SettingJson();

		/// <summary>
		/// Twitter へ アクセスするための ラッパークラス インスタンス
		/// </summary>
		private Twitter _Twitter { get; set; }

		/// <summary>
		/// HttpClientHandler の管理を行います。
		/// </summary>
		private HttpClientHandler _ClientHandler = new HttpClientHandler();

		/// <summary>
		/// HttpClient の管理を行います。
		/// </summary>
		public HttpClient HttpClient { get; protected set; }

		#endregion

		#region Constractor

		public Kisaragi()
		{
			InitializeComponent();

			HttpClient = new HttpClient(_ClientHandler);
			_Twitter = new Twitter(_ConsumerKey, _ConsumerKeySecret, HttpClient);

			_TimerSignal.MonitoringTimeChanged += _IsMonitoringTimeChanged;
		}

		#endregion

		#region Kisaragi MainThread Method's.

		/// <summary>
		/// Kisaragi が起動した時に実行されるメソッド
		/// </summary>
		private async void Form1_Load(object sender, EventArgs e)
		{
			this.MouseDown += new MouseEventHandler(KisaragiFormMouseDown);
			this.MouseMove += new MouseEventHandler(KisaragiFormMouseMove);

			// bitmap を Icon に変換する
			var handle = Properties.Resources.logo.GetHicon();
			this.notifyIcon.Icon = Icon.FromHandle(handle);

			// タスクトレイ通知部分 設定
			this.notifyIcon.Text = "Kisaragi";
			this.notifyIcon.BalloonTipTitle = "Kisaragi 時報";
			this.notifyIcon.ContextMenuStrip = this.ContextMenu;
			this.notifyIcon.Visible = true;

			if (!_IsSubscribed)
			{
				try
				{
					//音声設定ファイルがない場合、設定ファイルを作成
					if (!File.Exists("voiceSettings.json"))
						await _Json.CreateVoiceSettingFileAsync();
					else
						WriteLine("voiceSettings.json は既に生成済みです。");

					await _Json.LoadSettingFileAsync();
					await _TimerSignal.InvokingTimerSignalEventIgnitionAsync();
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"音声ファイルがありません。フォルダパスを確認して下さい。{ex.Message}");
				}

				_IsSubscribed = true;
			}

			_WelcomeKisaragi();
			_ReadOAuthSettings();
			_SettingKisaragiTasktray();
		}

		private void _ReadOAuthSettings()
		{
			var settings = Properties.Settings.Default;

			if (string.IsNullOrEmpty((string)settings["AccessToken"]))
			{
				// アクセストークンを設定ファイルに保存する
				settings["AccessToken"] = _Twitter.AccessToken;
				settings["AccessTokenSecret"] = _Twitter.AccessTokenSecret;
				settings["UserId"] = _Twitter.UserId;
				settings["ScreenName"] = _Twitter.ScreenName;
				settings.Save();
			}
			else
			{
				WriteLine("設定ファイルは既に存在しています。");

				var accessToken = (string)settings["AccessToken"];
				var accessTokenSecret = (string)settings["AccessTokenSecret"];
				var userId = (string)settings["UserId"];
				var screenName = (string)settings["ScreenName"];

				// 設定ファイルから読み込む
				_Twitter = new Twitter(_ConsumerKey, _ConsumerKeySecret, accessToken, accessTokenSecret, userId, screenName, this.HttpClient);
			}

			// ----------------- Key Check ----------------------
			WriteLine($"CK: {_Twitter.ConsumerKey}");
			WriteLine($"CKS: {_Twitter.ConsumerKeySecret}");
			WriteLine($"AT: {_Twitter.AccessToken}");
			WriteLine($"ATS: {_Twitter.AccessTokenSecret}");
		}

		/// <summary>
		/// タスクトレイアイコンの動作ロジック定義
		/// </summary>
		private void _SettingKisaragiTasktray()
		{
			// Twitter 認証(&O)
			OAuth.Click += async (s, e) =>
			{
				// 認証を実施します
				await _Twitter.AuthorizeAsync();

				// Form が Close したと同時に ShowDialog も完了する。
				var oauth = new OAuthWindow();
				if (oauth.ShowDialog() == DialogResult.OK)
					await _Twitter.GetAccessTokenAsync(oauth.PinCode);

				// 認証完了メッセージの投稿
				await PostTwitterAsync("OAuth completed for Kisaragi.");
			};

			// バージョン情報(&V) 
			VersionInfo.Click += (s, e) =>
				new VersionWindow().Show();

			// テスト投稿(&P)
			testPost.Click += async (s, e) =>
				await PostTwitterAsync("Kisaragi 投稿テスト");

			// Kisaragi の終了(&E)
			ExitKisaragi.Click += (s, e) =>
			{
				this.Invoke(new Func<Task>(async () => await SoundHelpers._PlayingVoiceAsync(_Json.Voice[24])));
				new KisaragiMessageBox("時報システムを終了します。", "Kisaragi 時報システム終了", 1500);
				this.Close();
			};
		}

		/// <summary>
		/// Kisaragi が終了される時に実行されるメソッド
		/// </summary>
		private void Form1_FormClosing(object sender, EventArgs e) => _IsSubscribed = false;

		/// <summary>
		/// MouseDown イベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void KisaragiFormMouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				_Position = new Point(e.X, e.Y);
		}

		/// <summary>
		/// MouseMove イベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void KisaragiFormMouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				this.Location = new Point(this.Location.X + e.X - _Position.X, this.Location.Y + e.Y - _Position.Y);
		}

		/// <summary>
		/// Kisaragi を起動した後に出るフォームにておもてなしを行います。
		/// </summary>
		/// <returns></returns>
		private void _WelcomeKisaragi()
		{
			// UserName
			UserName.Text = Environment.UserName;

			// Picture
			ProfileIcon.Image = Properties.Resources.logo;

			// bio
			UserTweet.Text = "Welcome to Kisaragi.";
		}

		/// <summary>
		/// Torst & Baloon Notify Settings.
		/// </summary>
		private async Task _SettingNotifyProperties(Utils<int> e)
		{
			ServicePointManager.Expect100Continue = false;
			this.notifyIcon.BalloonTipText = $"{e.Args} 時だにゃーん('ω')\r\n#Kisaragiちゃん #Kisaragi時報システム";
			await PostTwitterAsync(notifyIcon.BalloonTipText);
		}

		/// <summary>
		/// 一定時間経過した時に発生するイベントメソッド
		/// </summary>
		private async void _IsMonitoringTimeChanged(object sender, Utils<int> e)
		{
			var os = Environment.OSVersion;

			if (os.Version.Major >= 6 && os.Version.Minor >= 2)
				await _SettingNotifyProperties(e); // トースト通知 : Windows 8 以降
			else
				await _SettingNotifyProperties(e); // バルーン通知 : Windows 8 以前

			this.notifyIcon.ShowBalloonTip(1000);

			// ♰U s i n g♰
			this.Invoke(new Func<Task>(async () => await SoundHelpers._PlayingVoiceAsync(_Json.Voice[e.Args])));

		}
		/// <summary>
		/// Twitter へ 投稿します。
		/// </summary>
		/// <returns></returns>
		private async Task PostTwitterAsync(string postData)
		{
			ServicePointManager.Expect100Continue = false;
			var query = new Dictionary<string, string> { { "status", postData } };
			var result = await _Twitter.Request("https://api.twitter.com/1.1/statuses/update.json", HttpMethod.Post, query);
			if (result != null)
				WriteLine(" ----------- 投稿に成功しました。------------------");
		}

		#endregion

	}
}