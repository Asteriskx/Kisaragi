using System;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Windows.Forms;

namespace Kisaragi.Helper
{
	/// <summary>
	/// 時報のメッセージウィンドウ管理クラス。
	/// </summary>
	internal class KisaragiMessageBox : IDisposable
	{

		#region Field Variable

		private System.Threading.Timer _timer;
		private string _caption;
		private bool _disposed = false;

		#endregion

		#region Property

		private SafeHandle _handle { get; set; } = new SafeFileHandle(IntPtr.Zero, true);

		#endregion

		#region DLL's

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		#endregion

		#region Constractor 

		public KisaragiMessageBox(string text, string caption, int timeout)
		{
			this._caption = caption;
			_timer = new System.Threading.Timer(_OnTimerElapsed, null, timeout, Timeout.Infinite);
			MessageBox.Show(text, _caption);
		}

		#endregion

		#region Method

		private void _OnTimerElapsed(object state)
		{
			var mbWnd = FindWindow(null, _caption);

			if (mbWnd != IntPtr.Zero)
				SendMessage(mbWnd, 0x0010, IntPtr.Zero, IntPtr.Zero);

			_timer.Dispose();
		}

		#endregion

		#region Dispose

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
				_handle.Dispose();

			_disposed = true;
		}

		#endregion
	}
}
