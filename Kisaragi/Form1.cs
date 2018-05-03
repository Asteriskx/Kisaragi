using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

using Kisaragi.Helper;
using Kisaragi.TwitterAPI;
using Kisaragi.TwitterAPI.OAuth;
using Kisaragi.Util;

using static System.Console;

namespace Kisaragi
{
	/// <summary>
	/// Kisaragi メインクラス
	/// </summary>
	public partial class Kisaragi : Form
	{

		#region Constants Variable

		/// <summary>
		/// Kisaragi 時報通知パターン
		/// 0 : 1時間毎
		/// 1 : 任意の時間
		/// 2 : その他
		/// </summary>
		private enum _EventState { OnTime, Alarm, Others }

		/// <summary>
		/// 音声ファイルの規定場所:保存ファイル名
		/// </summary>
		private const string _SaveFileName = "voiceSettings.json";

		#endregion

		#region Field Valiable

		/// <summary>
		/// イベントが購読される準備が完了したか
		/// </summary>
		private bool _IsSubscribed = false;

		/// <summary>
		/// マウスのクリック位置
		/// </summary>
		private Point _Position;

		#endregion

		#region Properties

		/// <summary>
		/// HttpClient の管理を行います。
		/// </summary>
		private HttpClient _HttpClient { get; set; } = new HttpClient(new HttpClientHandler());

		/// <summary>
		/// 音声ファイル再生に関しての情報を管理するクラス(Jsonファイル R/W) インスタンス
		/// </summary>
		private NotifyVoiceSettingJsonObject _Json { get; set; }

		/// <summary>
		/// Kisaragi 時報システムのヘルパクラス インスタンス
		/// </summary>
		private SoundHelpers _SoundHelpers { get; set; } = new SoundHelpers();

		/// <summary>
		/// Kisaragi 時報管理クラス 定刻通知用インスタンス
		/// </summary>
		private TimerSignal _OnTimeSignal { get; set; }
		private TimerSignal _AlarmSignal { get; set; }

		/// <summary>
		/// 任意の通知時間を設定します。
		/// 初期値：1 min
		/// </summary>
		public TimeSpan NotifyTime { get; set; } = new TimeSpan(0, 1, 0);

		/// <summary>
		/// Twitter へ アクセスするための ラッパークラス インスタンス
		/// </summary>
		private Twitter _Twitter { get; set; }

		/// <summary>
		/// Kisaragi 時報通知パターン イベント管理
		/// </summary>
		private _EventState _State { get; set; }

		/// <summary>
		/// Consumer Key を管理します。
		/// </summary>
		private string _ConsumerKey { get; set; }

		/// <summary>
		/// Consumer Key Secret を管理します。
		/// </summary>
		private string _ConsumerSecret { get; set; }

		#endregion

		#region Constractor

		public Kisaragi()
		{
			InitializeComponent();

			this._OnTimeSignal = new TimerSignal(1000, this.MultiMsg);
			this._AlarmSignal = new TimerSignal(1000, this, this.MultiMsg);

			// 任意の通知時間設定
			checkBoxNotifyTime.Click += (s, e) =>
			{
				if (checkBoxNotifyTime.Checked)
					this._AlarmSignal.AlarmStateChanged += _IsAlarmStateChanged;
				else
					this._AlarmSignal.AlarmStateChanged -= _IsAlarmStateChanged;

				this._AlarmSignal.InvokingAlarmEventIgnition(checkBoxNotifyTime.Checked);
			};

			// Twitter 連携要否
			checkBoxPostTwitter.Click += async (s, e) =>
			{
				if (checkBoxPostTwitter.Checked)
				{
					var settings = Properties.Settings.Default;
					var consumerKey = (string)settings["ConsumerKey"];
					var consumerSecret = (string)settings["ConsumerSecret"];
					var accessToken = (string)settings["AccessToken"];
					var accessTokenSecret = (string)settings["AccessTokenSecret"];

					if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret) ||
						string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
					{
						// Consumer Key / Secret をここで入力
						var key = new KeyWindow(consumerKey, consumerSecret);
						if (key.ShowDialog() == DialogResult.OK)
							(this._ConsumerKey, this._ConsumerSecret) = key.CkPair;

						// 入力された Consumer Key / Secret を元に インスタンス生成
						this._Twitter = new Twitter(_ConsumerKey, _ConsumerSecret, this._HttpClient);

						if (this._ConsumerKey != null && _ConsumerSecret != null)
						{
							// 認証を実施します
							await _Twitter.AuthorizeAsync();

							// Form が Close したと同時に ShowDialog も完了する。
							var oauth = new OAuthWindow();
							if (oauth.ShowDialog() == DialogResult.OK)
								await _Twitter.GetAccessTokenAsync(oauth.PinCode);

							// 認証完了メッセージの投稿
							await PostTwitterAsync($"OAuth completed for Kisaragi.\r\n{DateTimeOffset.Now}");

							// 各種認証キーを設定ファイルに保存する
							settings["ConsumerKey"] = _Twitter.ConsumerKey;
							settings["ConsumerSecret"] = _Twitter.ConsumerKeySecret;
							settings["AccessToken"] = _Twitter.AccessToken;
							settings["AccessTokenSecret"] = _Twitter.AccessTokenSecret;
							settings.Save();
						}
						else
						{
							checkBoxPostTwitter.Checked = false;
							MessageBox.Show("Twitter 連携をキャンセルします。\r\n" +
								"再度認証するには、チェックボックスをクリックしてください。", "認証未実施", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					else
						new KisaragiMessageBox("Twitter 連携用認証キーは既に存在しています。" +
							"\r\nTwitter 連携を行うにはチェックを入れたままにしてください。", "通知", 2000);
				}
			};

			// 音声ファイル使用要否
			checkBoxNotifyVoice.Click += async (s, e) =>
			{
				if (checkBoxNotifyVoice.Checked)
				{
					var path = Properties.Settings.Default;

					// 音声設定ファイルがない場合
					if (!File.Exists(_SaveFileName))
					{
						var settings = new SettingsWindow();

						if (settings.ShowDialog() == DialogResult.OK)
							this.NotifyTime = settings.NotifyTime;

						path["VoicePath"] = this.NotifyTime;
						path.Save();

						this._Json = new NotifyVoiceSettingJsonObject(_SaveFileName, settings.VoicePath);

						if (!_IsSubscribed)
						{
							try
							{
								await _Json.SaveFileAsync();
								await _Json.LoadFileAsync();
								_OnTimeSignal.InvokingTimerSignalEventIgnition();
							}
							catch (IOException io)
							{
								WriteLine($"Exception = {io.Message}");
							}

							_IsSubscribed = true;
						}
					}
					else
					{
						if (!_IsSubscribed)
						{
							new KisaragiMessageBox("音声ありモードにて、Kisaragi は動作します。\r\n" +
								"※音声ファイル有無に関しては、設定にて変更できます。", "Kisaragi 設定モード : 音声あり", 1500);

							this._Json = new NotifyVoiceSettingJsonObject(_SaveFileName, path.VoicePath);
							await _Json.LoadFileAsync();
							_OnTimeSignal.InvokingTimerSignalEventIgnition();
						}
						_IsSubscribed = true;
					}
				}
			};
		}

		#endregion

		#region Kisaragi MainThread Method's.

		/// <summary>
		/// Kisaragi が起動した時に実行されるメソッド
		/// </summary>
		private void Form1_Load(object sender, EventArgs e)
		{
			// 各種イベントハンドラ登録
			this._OnTimeSignal.MonitoringTimeChanged += _IsMonitoringTimeChanged;
			this.MouseDown += KisaragiFormMouseDown;
			this.MouseMove += KisaragiFormMouseMove;

			// 各種メソッドコール
			_WelcomeKisaragi();
			_SettingKisaragiTasktray();
			_ReadOAuthSettings();

			// bitmap を Icon に変換する
			var handle = Properties.Resources.logo.GetHicon();
			this.notifyIcon.Icon = Icon.FromHandle(handle);

			// タスクトレイ通知部分 設定
			this.notifyIcon.Text = "Kisaragi";
			this.notifyIcon.BalloonTipTitle = "Kisaragi 時報";
			this.notifyIcon.ContextMenuStrip = this.ContextMenu;
			this.notifyIcon.Visible = true;
		}

		/// <summary>
		/// Kisaragi を起動した後に出るフォームにておもてなしを行います。
		/// </summary>
		/// <returns></returns>
		private void _WelcomeKisaragi()
		{
			new KisaragiMessageBox("音声なしモードにて、Kisaragi は動作します。\r\n" +
				"※音声ファイル有無に関しては、設定にて変更できます。", "Kisaragi 設定モード : 音声なし", 1500);

			UserName.Text = Environment.UserName;
		}

		/// <summary>
		/// 認証情報の読み込み 及び 設定を行います。
		/// </summary>
		private void _ReadOAuthSettings()
		{
			var settings = Properties.Settings.Default;
			var consumerKey = (string)settings["ConsumerKey"];
			var consumerSecret = (string)settings["ConsumerSecret"];
			var accessToken = (string)settings["AccessToken"];
			var accessTokenSecret = (string)settings["AccessTokenSecret"];

			// 設定ファイルから読み込む
			_Twitter = new Twitter(consumerKey, consumerSecret, accessToken, accessTokenSecret, this._HttpClient);

			WriteLine("-------------------- Authorize Key ---------------------");
			WriteLine($"CK: {_Twitter.ConsumerKey ?? null}");
			WriteLine($"CKS: {_Twitter.ConsumerKeySecret ?? null}");
			WriteLine($"AT: {_Twitter.AccessToken ?? null}");
			WriteLine($"ATS: {_Twitter.AccessTokenSecret ?? null}");
			WriteLine("-------------------- Authorize Key ---------------------");
		}

		/// <summary>
		/// タスクトレイアイコンの動作ロジック定義
		/// </summary>
		private void _SettingKisaragiTasktray()
		{
			// Settings(&S)
			Settings.Click += (s, e) =>
			{
				var settings = new SettingsWindow();
				if (settings.ShowDialog() == DialogResult.OK)
					this.NotifyTime = settings.NotifyTime;
			};

			// バージョン情報(&V) 
			VersionInfo.Click += (s, e) => new VersionWindow().Show();

			// Kisaragi の終了(&E)
			ExitKisaragi.Click += (s, e) =>
			{
				if (checkBoxNotifyVoice.Checked)
					this.Invoke(new Func<Task>(async () => await _SoundHelpers._PlayingVoiceAsync(_Json.Voice[24])));

				new KisaragiMessageBox("Kisaragi を終了します。", "Kisaragi 終了", 1500);
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
		/// 一定時間経過した時に発生するイベントメソッド
		/// </summary>
		private async void _IsMonitoringTimeChanged(object sender, Utils<int> e)
		{
			this._State = _EventState.OnTime;
			await _NotifyAsync(e);
		}

		/// <summary>
		/// アラーム機能イベントメソッド
		/// </summary>
		private async void _IsAlarmStateChanged(object sender, Utils<int> e)
		{
			this._State = _EventState.Alarm;
			await _NotifyAsync(e);
		}

		/// <summary>
		/// 通知イベントメソッド
		/// </summary>
		private async Task _NotifyAsync(Utils<int> e)
		{
			var os = Environment.OSVersion;

			if (os.Version.Major >= 6 && os.Version.Minor >= 2)
				await _SettingNotifyProperties(e); // トースト通知 : Windows 8 以降
			else
				await _SettingNotifyProperties(e); // バルーン通知 : Windows 8 以前

			this.notifyIcon.ShowBalloonTip(2000);

			if (checkBoxNotifyVoice.Checked)
				this.Invoke(new Func<Task>(async () => await _SoundHelpers._PlayingVoiceAsync(_Json.Voice[e.Args])));
		}

		/// <summary>
		/// トースト通知・バルーン通知のテキスト設定、Twitter 投稿を行います。
		/// </summary>
		private async Task _SettingNotifyProperties(Utils<int> e)
		{
			// 1時間毎の通知モード or アラームモード
			var message = (this._State == _EventState.OnTime) ?
				this.notifyIcon.BalloonTipText = $"{e.Args} 時をお知らせします('ω')\r\n#Kisaragi #Kisaragi時報システム" :
				this.notifyIcon.BalloonTipText = $"タイマー終了！('ω')\r\n#Kisaragi #Kisaragi時報システム";

			if (checkBoxPostTwitter.Checked)
				await PostTwitterAsync(message);
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
			WriteLine($"result = {result}");
		}

		#endregion

	}
}