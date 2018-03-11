using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using static System.Console;

using Kisaragi.Helper;
using Kisaragi.Util;

namespace Kisaragi
{
	/// <summary>
	/// Kisaragi メインクラス
	/// </summary>
	public partial class Kisaragi : Form
	{

		#region Properties

		/// <summary>
		/// Kisaragi 時報管理クラス インスタンス
		/// 監視間隔時間は、1 sec.
		/// </summary>
		private TimerSignal _TimerSignal { get; set; } = new TimerSignal(1000);

		/// <summary>
		/// 音声ファイル再生に関しての情報を管理するクラス(Jsonファイル R/W) インスタンス
		/// </summary>
		private SettingJson _Json { get; set; } = new SettingJson();

		#endregion

		/// <summary>
		/// イベントが購読される準備が完了したか
		/// </summary>
		private bool _isSubscribed = false;

		#region Constractor

		public Kisaragi()
		{
			InitializeComponent();

			// イベント登録 (Cで言うところの関数ポインタちっくな something.)
			_TimerSignal.MonitoringTimeChanged += _IsMonitoringTimeChanged;

			// bitmap を Icon に変換するためにごにょごにょしてる
			var handle = Properties.Resources.logo.GetHicon();
			this.notifyIcon.Icon = Icon.FromHandle(handle);

			this.notifyIcon.Text = "Kisaragi";
			this.notifyIcon.BalloonTipTitle = "Kisaragi 時報";
			this.notifyIcon.Visible = true;
		}

		#endregion

		/// <summary>
		/// Kisaragi が起動した時に実行されるメソッド
		/// </summary>
		private async void Form1_Load(object sender, EventArgs e)
		{
			if (!_isSubscribed)
			{

				//音声設定ファイルがない場合、設定ファイルを作成
				if (File.Exists("voiceSettings.json") == false)
					await _Json.CreateVoiceSettingFileAsync();
				else
					WriteLine("voiceSettings.json は既に生成済みです。");

				await _Json.LoadSettingFileAsync();
				await _TimerSignal.SelectTimerSignalVoiceAsync();
				_isSubscribed = true;
			}
		}

		/// <summary>
		/// Kisaragi が終了される時に実行されるメソッド
		/// </summary>
		private void Form1_FormClosing(object sender, EventArgs e) =>
			_isSubscribed = false;

		/// <summary>
		/// Torst & Baloon Notify Settings.
		/// </summary>
		private void SettingNotifyProperties(Utils<int> e) =>
			this.notifyIcon.BalloonTipText = $"{e.Args} 時だにゃーん('ω')";

		/// <summary>
		/// 一定時間経過した時に発生するイベントメソッド
		/// </summary>
		private async void _IsMonitoringTimeChanged(object sender, Utils<int> e)
		{
			var os = Environment.OSVersion;

			if (os.Version.Major >= 6 && os.Version.Minor >= 2)
				SettingNotifyProperties(e); // トースト通知 : Windows 8 以降
			else
				SettingNotifyProperties(e); // バルーン通知 : Windows 8 以前

			this.notifyIcon.ShowBalloonTip(1000);

			// ♰Dead♰
			/*
			await Task.Factory.StartNew(async () =>
			{
				await _timerSignal._helpers._PlayingVoiceAsync(_json.Settings[e.Args]);
			}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
			*/

			// ♰U s i n g♰
			this.Invoke(new Action(async () => await _TimerSignal.Helpers._PlayingVoiceAsync(_Json.Settings[e.Args])));
		}
	}
}
