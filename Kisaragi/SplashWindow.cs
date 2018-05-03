using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kisaragi
{
	/// <summary>
	/// Kisaragi スプラッシュウィンドウ 管理クラス
	/// </summary>
	public partial class SplashWindow : Form
	{

		#region Constractor

		public SplashWindow(int interval)
		{
			InitializeComponent();

			var timer = new Timer();
			timer.Interval = interval;
			timer.Tick += (s, e) => this.Close();
			timer.Start();

			this.Paint += new PaintEventHandler((s, e) =>
				ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, this.Width, this.Height), Border3DStyle.Raised));

			this.Shown += (s, e) => this.Refresh();
		}

		#endregion

	}
}