﻿using System;
using System.Threading.Tasks;

using Kisaragi.Util;

namespace Kisaragi.Helper
{
	/// <summary>
	/// Kisaragi 時報管理クラス
	/// </summary>
	public class TimerSignal
	{

		#region Properties

		/// <summary>
		/// Kisaragi 時報システムのヘルパクラス インスタンス
		/// </summary>
		public Helpers Helpers { get; set; } = new Helpers();

		/// <summary>
		/// 音声ファイル再生に関しての情報を管理するクラス(Jsonファイル R/W) インスタンス
		/// </summary>
		public SettingJson Setting { get; set; } = new SettingJson();

		/// <summary>
		/// 時間の変化を監視するために使用するタイマ
		/// </summary>
		private System.Timers.Timer _Polling { get; set; }

		#endregion

		#region Field Valiable

		/// <summary>
		/// 経過時間(過去)を保存します
		/// </summary>
		public int elapsedTime;

		/// <summary>
		/// 監視する間隔
		/// </summary>
		private int _interval;

		#endregion

		#region Defined EventHandler

		/// <summary>
		/// 時間が経過し、変化したタイミングを監視するイベントハンドラ
		/// 現状は、1時間
		/// </summary>
		public event EventHandler<Utils<int>> MonitoringTimeChanged;

		#endregion

		#region Constractor

		public TimerSignal(int interval)
		{
			elapsedTime = DateTime.Now.Hour;
			_interval = interval;
		}

		#endregion

		#region EventIgnition Logic.

		/// <summary>
		/// 非同期で時間の変化に伴って、イベントを発火させます。
		/// </summary>
		public async Task InvokingTimerSignalEventIgnitionAsync()
		{
			_Polling = new System.Timers.Timer(_interval);

			// ポーリング
			_Polling.Elapsed += (s, e) =>
			{
				// 現在時間の監視
				if (elapsedTime != DateTime.Now.Hour)
				{
					// 今回値で前回時間を更新
					elapsedTime = DateTime.Now.Hour;

					// ♰イベント発火♰
					MonitoringTimeChanged?.Invoke(null, new Utils<int>(elapsedTime));
				}
			};
			_Polling.Start();
		}

		#endregion

	}
}