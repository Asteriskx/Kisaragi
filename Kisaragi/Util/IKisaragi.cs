using System;

namespace Kisaragi.Util
{
	/// <summary>
	/// Kisaragi にて使用するイベントを提供します。
	/// </summary>
	public interface IKisaragi
	{
		/// <summary>
		/// 時間が経過し、変化したタイミングを監視するイベントハンドラ
		/// </summary>
		event EventHandler<Utils<int>> MonitoringTimeChanged;

		/// <summary>
		/// アラーム機能を監視するイベントハンドラ
		/// </summary>
		event EventHandler<Utils<int>> AlarmStateChanged;
	}
}
