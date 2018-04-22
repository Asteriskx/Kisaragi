using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Collections.Generic;

using Kisaragi.Util;
using Kisaragi.Helper;
using Kisaragi.TwitterAPI;
using Kisaragi.TwitterAPI.OAuth;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
		const string _ConsumerKey = "your consumer key.";
		const string _ConsumerKeySecret = "your consumer key secret.";

		//マウスのクリック位置を記憶
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

			// イベント登録 (Cで言うところの関数ポインタちっくな something.)
			_TimerSignal.MonitoringTimeChanged += _IsMonitoringTimeChanged;
			this._IsMonitoringTimelineChanged += _IsMonitoringTimeLineChanged;
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

			// bitmap を Icon に変換するためにごにょごにょしてる
			var handle = Properties.Resources.logo.GetHicon();
			this.notifyIcon.Icon = Icon.FromHandle(handle);

			// タスクトレイ通知部分 設定
			this.notifyIcon.Text = "Kisaragi";
			this.notifyIcon.BalloonTipTitle = "Kisaragi 時報";
			this.notifyIcon.ContextMenuStrip = this.ContextMenu;
			this.notifyIcon.Visible = true;

			if (!_IsSubscribed)
			{
				//音声設定ファイルがない場合、設定ファイルを作成
				if (!File.Exists("voiceSettings.json"))
					await _Json.CreateVoiceSettingFileAsync();
				else
					WriteLine("voiceSettings.json は既に生成済みです。");

				await _Json.LoadSettingFileAsync();
				await _TimerSignal.InvokingTimerSignalEventIgnitionAsync();

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
				Console.WriteLine("設定ファイルは既に存在しています。");

				var accessToken = (string)settings["AccessToken"];
				var accessTokenSecret = (string)settings["AccessTokenSecret"];
				var userId = (string)settings["UserId"];
				var screenName = (string)settings["ScreenName"];

				// 設定ファイルから読み込む
				_Twitter = new Twitter(_ConsumerKey, _ConsumerKeySecret, accessToken, accessTokenSecret, userId, screenName, this.HttpClient);
			}

			// ----------------- Key Check ----------------------
			Console.WriteLine($"CK: {_Twitter.ConsumerKey}");
			Console.WriteLine($"CKS: {_Twitter.ConsumerKeySecret}");
			Console.WriteLine($"AT: {_Twitter.AccessToken}");
			Console.WriteLine($"ATS: {_Twitter.AccessTokenSecret}");
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
			testPost.Click += async (s, ee) =>
				await PostTwitterAsync("Kisaragi 投稿テスト");

			// テスト取得(&T)
			GetTimeline.Click += (s, ee) =>
				_TestGetTimelineforTwitter();

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
			ProfileIcon.Image = Properties.Resources.asterisk;

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
				Console.WriteLine(" ----------- 投稿に成功しました。------------------");
		}

		private EventHandler _IsMonitoringTimelineChanged;
		private System.Timers.Timer _Polling { get; set; } = new System.Timers.Timer(1000);
		private int CheckTimer { get; set; } = 0;
		private const int RATE_LIMIT = 15;

		/// <summary>
		/// Twitte Timeline 取得テスト用
		/// </summary>
		/// <returns></returns>
		private void _TestGetTimelineforTwitter()
		{
			_Polling.Elapsed += (s, e) =>
			{
				if (CheckTimer < RATE_LIMIT)
					++CheckTimer;
				else
				{
					_IsMonitoringTimelineChanged?.Invoke(s, e);
					CheckTimer = 0;
				}

				//Invoke((MethodInvoker)delegate { elapsed.Text = $"{CheckTimer}s"; });
				//Console.WriteLine($"{CheckTimer} s経過");
			};

			_Polling.Start();
		}

		/// <summary>
		/// タイムラインの変化を監視します。(15sec 間隔でポーリング)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void _IsMonitoringTimeLineChanged(object sender, EventArgs e)
		{
			try
			{
				var req = await _Twitter.Request(
					"https://api.twitter.com/1.1/statuses/home_timeline.json", HttpMethod.Get, new Dictionary<string, string> { { "count", "1" } });

				dynamic json = JsonConvert.DeserializeObject(req);

				var userTweet = from dynamic items in (json as JArray) select items;
				foreach (var tweet in userTweet)
				{
					Invoke((MethodInvoker)delegate
					{
						UserName.Text = tweet.user.name.Value;
						UserID.Text = $"@{tweet.user.screen_name.Value}";
						UserTweet.Text = tweet.text.Value;
						ProfileIcon.ImageLocation = tweet.user.profile_image_url.Value;
					});
					Console.WriteLine($"@{tweet.user.screen_name.Value} -> {tweet.text.Value}");
				}
			}
			catch (Exception ex) { WriteLine($"Timeline is not found.\r\n{ex.Message}\r\n{ex.StackTrace}"); };
		}

		#endregion

	}
}