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
		/// 現状は、1時間
		/// </summary>
		event EventHandler<Utils<int>> MonitoringTimeChanged;
	}
}
