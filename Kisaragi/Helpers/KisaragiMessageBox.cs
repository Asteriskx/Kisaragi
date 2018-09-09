using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Kisaragi.Helper
{
	/// <summary>
	/// 時報のメッセージウィンドウ管理クラス。
	/// </summary>
	internal class KisaragiMessageBox
	{
		#region Property

		/// <summary>
		/// 表示時間管理用のタイマー
		/// </summary>
		private System.Threading.Timer _Timer { get; set; }

		#endregion Property

		#region DLL's

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		#endregion DLL's

		#region Constructor 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="text">表示メッセージ</param>
		/// <param name="caption">画面タイトル</param>
		/// <param name="timeout">ウィンドウクローズまでの時間</param>
		public KisaragiMessageBox(string text, string caption, int timeout)
		{
			this._Timer = new System.Threading.Timer((state) =>
			{
				var window = FindWindow(null, caption);

				if (window != IntPtr.Zero)
					SendMessage(window, 0x0010, IntPtr.Zero, IntPtr.Zero);

				this._Timer.Dispose();
			}, null, timeout, Timeout.Infinite);

			MessageBox.Show(text, caption);
		}

		#endregion Constructor
	}
}
