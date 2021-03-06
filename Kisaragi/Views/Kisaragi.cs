﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

using Kisaragi.APIs.Twitter;
using Kisaragi.APIs.OAuth;
using Kisaragi.Helper;
using Kisaragi.Models;
using Kisaragi.Util;

using static System.Console;

namespace Kisaragi.Views
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
		/// 経過時間(過去)を保存します
		/// </summary>
		public int elapsedTime;

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
		/// 現在時刻の監視実施用。
		/// </summary>
		private System.Timers.Timer _Polling { get; set; }

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

		#region EventHandler 

		/// <summary>
		/// 現在時刻を表示する際に使用するイベントハンドラ
		/// </summary>
		private EventHandler _CurrentTimeChanged;

		#endregion

		#region Constractor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Kisaragi() => this.InitializeComponent();

		#endregion

		#region Kisaragi MainThread Method's.

		/// <summary>
		/// Kisaragi が起動した時に実行されるメソッド
		/// </summary>
		private async void Kisaragi_Load(object sender, EventArgs e)
		{
			// インスタンス設定
			this._OnTimeSignal = new TimerSignal(1000, this.MultiMsg);
			this._AlarmSignal = new TimerSignal(1000, this, this.MultiMsg);

			// 時間設定
			this.elapsedTime = DateTime.Now.Second;

			// 各種イベントハンドラ登録
			this._CurrentTimeChanged += this._IsCurrentTimeChanged;
			this._OnTimeSignal.MonitoringTimeChanged += this._IsMonitoringTimeChanged;
			this.checkBoxNotifyTime.Click += this._IsCheckBoxNotifyTimeChanged;
			this.checkBoxPostTwitter.Click += this._IsCheckBoxPostTwitterChanged;
			this.checkBoxNotifyVoice.Click += this._IsCheckBoxNotifyVoiceChanged;
			this.MouseDown += this._KisaragiFormMouseDown;
			this.MouseMove += this._KisaragiFormMouseMove;

			// 各種メソッドコール
			await this._WelcomeKisaragi();
			this._UpdateViewTime();
			this._SettingKisaragiTasktray();
			this._ReadOAuthSettings();

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
		/// Kisaragi が終了される時に実行されるメソッド
		/// </summary>
		private void Kisaragi_FormClosing(object sender, EventArgs e)
		{
			var settings = Properties.Settings.Default;

			// 各種チェックボックスの値を保存
			settings["AlarmCheck"] = this.checkBoxNotifyTime.Checked;
			settings["PostTwitterCheck"] = this.checkBoxPostTwitter.Checked;
			settings["VoiceCheck"] = this.checkBoxNotifyVoice.Checked;
			settings.Save();

			this._IsSubscribed = false;
		}

		/// <summary>
		/// メイン画面：現在時刻更新イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _IsCurrentTimeChanged(object sender, EventArgs e) => this._UpdateViewTime();

		/// <summary>
		/// アラーム機能：チェックボックスクリックイベント発生時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _IsCheckBoxNotifyTimeChanged(object sender, EventArgs e) => this._UsingNotifyAlarm();

		/// <summary>
		/// Twitter 連携機能：チェックボックスクリックイベント発生時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void _IsCheckBoxPostTwitterChanged(object sender, EventArgs e) => await this._UsingPostTwitterAsync();

		/// <summary>
		/// ボイス機能：チェックボックスクリックイベント発生時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void _IsCheckBoxNotifyVoiceChanged(object sender, EventArgs e) => await this._UsingNotifyVoiceAsync();

		/// <summary>
		/// メイン画面の現在時刻を更新します。
		/// </summary>
		private void _UpdateViewTime()
		{
			this._Polling = new System.Timers.Timer(1000);

			if (!this.checkBoxNotifyTime.Checked)
			{
				this._Polling.Elapsed += (s, e) =>
				{
					if (elapsedTime != DateTime.Now.Second)
					{
						// 今回値で前回時間を更新
						elapsedTime = DateTime.Now.Second;

						this.MultiMsg.Invoke((Action)(() =>
						{
							this.MultiMsg.Text = DateTime.Now.ToString();
						}));

						this._CurrentTimeChanged?.Invoke(this, EventArgs.Empty);
					}
				};

				// ポーリング開始
				this._Polling.Start();
			}
		}

		/// <summary>
		/// アラーム機能のイベント設定を行います。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _UsingNotifyAlarm()
		{
			if (this.checkBoxNotifyTime.Checked)
				this._AlarmSignal.AlarmStateChanged += this._IsAlarmStateChanged;
			else
				this._AlarmSignal.AlarmStateChanged -= this._IsAlarmStateChanged;

			this._AlarmSignal.InvokingAlarmEventIgnition(this.checkBoxNotifyTime.Checked);
		}

		/// <summary>
		/// Twitter 連携に関するイベント設定を行います。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async Task _UsingPostTwitterAsync()
		{
			if (this.checkBoxPostTwitter.Checked)
			{
				var settings = Properties.Settings.Default;
				var consumerKey = (string)settings["ConsumerKey"] ?? null;
				var consumerSecret = (string)settings["ConsumerSecret"] ?? null;
				var accessToken = (string)settings["AccessToken"] ?? null;
				var accessTokenSecret = (string)settings["AccessTokenSecret"] ?? null;

				if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret) ||
					string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
				{
					// Consumer Key / Secret をここで入力
					var key = new KeyWindow(consumerKey, consumerSecret);
					if (key.ShowDialog() == DialogResult.OK)
						(this._ConsumerKey, this._ConsumerSecret) = key.CkPair;

					// 入力された Consumer Key / Secret を元に インスタンス生成
					this._Twitter = new Twitter(this._ConsumerKey, this._ConsumerSecret, this._HttpClient);

					if (this._ConsumerKey != null && this._ConsumerSecret != null)
					{
						// 認証を実施します
						await this._Twitter.AuthorizeAsync();

						// Form が Close したと同時に ShowDialog も完了する。
						var oauth = new OAuthWindow();
						if (oauth.ShowDialog() == DialogResult.OK)
							await this._Twitter.GetAccessTokenAsync(oauth.PinCode);

						// 認証完了メッセージの投稿
						await this.PostTwitterAsync($"Authentication completion for Kisaragi.\r\n{DateTimeOffset.Now}");

						// 各種認証キーを設定ファイルに保存する
						settings["ConsumerKey"] = this._Twitter.ConsumerKey;
						settings["ConsumerSecret"] = this._Twitter.ConsumerSecret;
						settings["AccessToken"] = this._Twitter.AccessToken;
						settings["AccessTokenSecret"] = this._Twitter.AccessTokenSecret;
						settings["UserId"] = this._Twitter.UserId;
						settings["ScreenName"] = this._Twitter.ScreenName;
						settings.Save();
					}
					else
					{
						this.checkBoxPostTwitter.Checked = false;
						MessageBox.Show("Twitter 連携をキャンセルします。\r\n" +
							"再度認証するには、チェックボックスをクリックしてください。", "認証未実施", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
					new KisaragiMessageBox("Twitter 連携用認証キーは既に存在しています。" +
						"\r\nTwitter 連携を行うにはチェックを入れたままにしてください。", "通知", 2000);
			}
		}

		/// <summary>
		/// ボイス利用に関するイベント設定を行います。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async Task _UsingNotifyVoiceAsync()
		{
			if (this.checkBoxNotifyVoice.Checked)
			{
				var path = Properties.Settings.Default;

				// 音声設定ファイルがない場合
				if (!File.Exists(_SaveFileName))
				{
					var settings = new SettingsWindow();

					if (settings.ShowDialog() == DialogResult.OK)
						this.NotifyTime = settings.NotifyTime;

					path["VoicePath"] = path.VoicePath;
					path.Save();

					this._Json = new NotifyVoiceSettingJsonObject(_SaveFileName, settings.VoicePath);

					if (!_IsSubscribed)
					{
						try
						{
							await this._Json.SaveFileAsync();
							await this._Json.LoadFileAsync();
							this._OnTimeSignal.InvokingTimerSignalEventIgnition();
						}
						catch (IOException io)
						{
							WriteLine($"Exception = {io.Message}");
						}

						this._IsSubscribed = true;
					}
				}
				else
				{
					if (!this._IsSubscribed)
					{
						new KisaragiMessageBox("音声ありモードにて、Kisaragi は動作します。\r\n" +
							"※音声ファイル有無に関しては、設定にて変更できます。", "Kisaragi 設定モード : 音声あり", 1500);

						this._Json = new NotifyVoiceSettingJsonObject(_SaveFileName, path.VoicePath);
						await this._Json.LoadFileAsync();
						this._OnTimeSignal.InvokingTimerSignalEventIgnition();
					}
					this._IsSubscribed = true;
				}
			}
		}

		/// <summary>
		/// Kisaragi を起動した後に出るフォームにておもてなしを行います。
		/// </summary>
		/// <returns></returns>
		private async Task _WelcomeKisaragi()
		{
			new KisaragiMessageBox("音声なしモードにて、Kisaragi は動作します。\r\n" +
				"※音声ファイル有無に関しては、設定にて変更できます。", "Kisaragi 設定モード : 音声なし", 1500);

			this.UserName.Text = Environment.UserName;
			var settings = Properties.Settings.Default;

			// 各種チェックボックスの値を復元
			this.checkBoxNotifyTime.Checked = (bool)settings["AlarmCheck"];
			this.checkBoxPostTwitter.Checked = (bool)settings["PostTwitterCheck"];
			this.checkBoxNotifyVoice.Checked = (bool)settings["VoiceCheck"];

			// アラーム機能 : 前回値チェックあり
			if (this.checkBoxNotifyTime.Checked) this._UsingNotifyAlarm();

			// Twitter 連携機能：前回値チェックあり
			if (this.checkBoxPostTwitter.Checked) await this._UsingPostTwitterAsync();

			// ボイス機能：前回値チェックあり
			if (this.checkBoxNotifyVoice.Checked) await this._UsingNotifyVoiceAsync();
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
			var userId = (string)settings["UserId"];
			var screenName = (string)settings["ScreenName"];

			// 設定ファイルから読み込む
			_Twitter = new Twitter(consumerKey, consumerSecret, accessToken, accessTokenSecret, userId, screenName, this._HttpClient);

			WriteLine("-------------------- Authorize Key ---------------------");
			WriteLine($"CK: {this._Twitter.ConsumerKey ?? null}");
			WriteLine($"CKS: {this._Twitter.ConsumerSecret ?? null}");
			WriteLine($"AT: {this._Twitter.AccessToken ?? null}");
			WriteLine($"ATS: {this._Twitter.AccessTokenSecret ?? null}");
			WriteLine($"uID: {this._Twitter.UserId ?? null}");
			WriteLine($"SN: {this._Twitter.ScreenName ?? null}");
			WriteLine("-------------------- Authorize Key ---------------------");
		}

		/// <summary>
		/// タスクトレイアイコンの動作ロジック定義
		/// </summary>
		private void _SettingKisaragiTasktray()
		{
			// Settings(&S)
			this.Settings.Click += (s, e) =>
			{
				var settings = new SettingsWindow();
				if (settings.ShowDialog() == DialogResult.OK)
					this.NotifyTime = settings.NotifyTime;
			};

			// Kisaragi フォーム画面サイズ：Defaultモード
			this.MonitorDefault.Click += (s, e) => this.WindowState = FormWindowState.Normal;

			// Kisaragi フォーム画面サイズ：Minimumモード
			this.MonitorMinimum.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

			// Twitter 投稿フォーム(&T)
			this.TwitterForm.Click += (s, e) => new PostWindow(this._Twitter).Show();

			// バージョン情報(&V) 
			this.VersionInfo.Click += (s, e) => new VersionWindow().Show();

			// Kisaragi の終了(&E)
			this.ExitKisaragi.Click += (s, e) =>
			{
				if (this.checkBoxNotifyVoice.Checked)
					this.Invoke(new Func<Task>(async () => await this._SoundHelpers._PlayingVoiceAsync(_Json.Voice[24])));

				new KisaragiMessageBox("Kisaragi を終了します。", "Kisaragi 終了", 1500);
				this.Close();
			};
		}

		/// <summary>
		/// MouseDown イベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _KisaragiFormMouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				this._Position = new Point(e.X, e.Y);
		}

		/// <summary>
		/// MouseMove イベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _KisaragiFormMouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				this.Location = new Point(this.Location.X + e.X - this._Position.X, this.Location.Y + e.Y - this._Position.Y);
		}

		/// <summary>
		/// 一定時間経過した時に発生するイベントメソッド
		/// </summary>
		private async void _IsMonitoringTimeChanged(object sender, Utils<int> e)
		{
			this._State = _EventState.OnTime;
			await this._NotifyAsync(e);
		}

		/// <summary>
		/// アラーム機能イベントメソッド
		/// </summary>
		private async void _IsAlarmStateChanged(object sender, Utils<int> e)
		{
			this._State = _EventState.Alarm;
			await this._NotifyAsync(e);
		}

		/// <summary>
		/// 通知イベントメソッド
		/// </summary>
		private async Task _NotifyAsync(Utils<int> e)
		{
			var os = Environment.OSVersion;

			if (os.Version.Major >= 6 && os.Version.Minor >= 2)
				await this._SettingNotifyProperties(e); // トースト通知 : Windows 8 以降
			else
				await this._SettingNotifyProperties(e); // バルーン通知 : Windows 8 以前

			this.notifyIcon.ShowBalloonTip(2000);

			if (this.checkBoxNotifyVoice.Checked)
				this.Invoke(new Func<Task>(async () => await this._SoundHelpers._PlayingVoiceAsync(this._Json.Voice[e.Args])));
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

			if (this.checkBoxPostTwitter.Checked)
				await this.PostTwitterAsync(message);
		}

		/// <summary>
		/// Twitter へ 投稿します。
		/// </summary>
		/// <returns></returns>
		private async Task PostTwitterAsync(string postData)
		{
			var query = new Dictionary<string, string> { { "status", postData } };
			var result = await this._Twitter.Request("https://api.twitter.com/1.1/statuses/update.json", HttpMethod.Post, query);
			WriteLine($"result = {result}");
		}

		#endregion

	}
}