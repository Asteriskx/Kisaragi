using System;
using Kisaragi.Util;
using System.Windows.Forms;

namespace Kisaragi.Models
{
	/// <summary>
	/// Kisaragi 時報管理クラス
	/// </summary>
	public class TimerSignal : IKisaragi
	{

		#region Properties

		/// <summary>
		/// 時間の変化を監視するために使用するタイマ
		/// </summary>
		private System.Timers.Timer _Polling { get; set; }
		private System.Timers.Timer _Alarm { get; set; }
		private System.Timers.Timer _Power { get; set; }

		private Views.Kisaragi _Kisaragi { get; set; }
		private Control _Msg { get; set; }

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
		/// </summary>
		public event EventHandler<Utils<int>> MonitoringTimeChanged;

		/// <summary>
		/// アラーム機能を監視するイベントハンドラ
		/// </summary>
		public event EventHandler<Utils<int>> AlarmStateChanged;

		#endregion

		#region Constractor

		public TimerSignal(int interval, Control msg)
		{
			elapsedTime = DateTime.Now.Hour;
			this._interval = interval;
			this._Msg = msg;
		}

		public TimerSignal(int interval, Views.Kisaragi kisaragi, Control msg)
		{
			elapsedTime = DateTime.Now.Hour;
			this._interval = interval;
			this._Kisaragi = kisaragi;
			this._Msg = msg;
		}

		#endregion

		#region EventIgnition Logic.

		/// <summary>
		/// 時間の変化に伴って、イベントを発火させます。
		/// </summary>
		public void InvokingTimerSignalEventIgnition()
		{
			try
			{
				this._Polling = new System.Timers.Timer(_interval);

				// ポーリング
				this._Polling.Elapsed += (s, e) =>
				{

					// 現在時間の監視
					if (elapsedTime != DateTime.Now.Hour)
					{
						// 今回値で前回時間を更新
						elapsedTime = DateTime.Now.Hour;

						// Main UI に残り時間を表示させる
						this._Msg.Invoke((Action)(() =>
						{
							this._Msg.Text = $"{elapsedTime} 時をお知らせします！('ω')";
						}));

						// ♰イベント発火♰
						this.MonitoringTimeChanged?.Invoke(null, new Utils<int>(elapsedTime));
					}
				};

				// ポーリング開始
				this._Polling.Start();
			}
			catch (Exception e)
			{
				this._Polling.Stop();
				this._Polling.Dispose();
				throw new ApplicationException($"InvokingTimerSignalEventIgnitionAsync() にてエラー発生。{e.Message}");
			}
		}

		/// <summary>
		/// 時間の変化に伴って、イベントを発火させます。
		/// </summary>
		public void InvokingAlarmEventIgnition(bool state)
		{
			try
			{
				if (state)
				{
					this._Alarm = new System.Timers.Timer(_interval);
					var convert = this._Kisaragi.NotifyTime.TotalSeconds;
					var timer = TimeSpan.FromSeconds(convert);

					// ポーリング
					this._Alarm.Elapsed += (s, e) =>
					{
						if (timer > TimeSpan.Zero)
						{
							timer = timer.Subtract(TimeSpan.FromSeconds(1));

							// Main UI に残り時間を表示させる
							this._Msg.Invoke((Action)(() =>
							{
								this._Msg.Text = $"タイマー終了まで：{timer}";
							}));
						}
						else
						{
							// アラームストップ
							this.StopAlarm();
						}
					};

					// ポーリング開始
					this._Alarm.Start();
				}
				else
				{
					// アラームストップ
					this.StopAlarm();
				}

			}
			catch (Exception e)
			{
				this._Alarm.Stop();
				this._Alarm.Dispose();
				throw new ApplicationException($"InvokingAlarmEventIgnitionAsync() にてエラー発生。{e.Message}");
			}
		}

		/// <summary>
		/// TODO : 電源管理イベント
		/// </summary>
		public void InvokingPowerEventIgnition(string powerMode)
		{
			/*
			try
			{
				if (state)
				{
					this._Power = new System.Timers.Timer(_interval);
					var convert = this._Kisaragi.NotifyTime.TotalSeconds;
					var timer = TimeSpan.FromSeconds(convert);

					// ポーリング
					_Power.Elapsed += (s, e) =>
					{
						if (timer > TimeSpan.Zero)
						{
							timer = timer.Subtract(TimeSpan.FromSeconds(1));

							// Main UI に残り時間を表示させる
							this._Msg.Invoke((Action)(() =>
							{
								this._Msg.Text = $"{powerMode}まで：{timer}";
							}));
						}
						else
						{
							// アラームストップ
							StopAlarm();
						}
					};

					// ポーリング開始
					_Power.Start();
				}
				else
				{
					// アラームストップ
					StopAlarm();
				}

			}
			catch (Exception e)
			{
				_Power.Stop();
				_Power.Dispose();
				throw new ApplicationException($"InvokingPowerEventIgnition() にてエラー発生。{e.Message}");
			}
			*/
		}

		/// <summary>
		/// Alarm 終了処理
		/// </summary>
		public void StopAlarm()
		{
			this._Alarm.Stop();

			this.AlarmStateChanged?.Invoke(null, new Utils<int>(25));

			// Main UI 更新
			this._Msg.Invoke((Action)(() =>
			{
				this._Msg.Text = $"タイマー終了！('ω')";
			}));
		}

		#endregion

	}
}
