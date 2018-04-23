using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Kisaragi
{
	/// <summary>
	/// Kisaragi スプラッシュウィンドウ 管理クラス
	/// </summary>
	public partial class SplashWindow : Form
	{
		#region Constractor

		public SplashWindow()
		{
			InitializeComponent();
			this.Paint += new PaintEventHandler(GenerateWindow);
		}

		#endregion

		#region Method's

		/// <summary>
		/// ウィンドウの生成を行います。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GenerateWindow(object sender, PaintEventArgs e) =>
			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, this.Width, this.Height), Border3DStyle.Raised);

		/// <summary>
		/// 生成したウィンドウの表示を行います。
		/// </summary>
		public void Showing()
		{
			this.Show();
			this.Refresh();
			Thread.Sleep(3000);
			this.Close();
		}

		#endregion

	}
}
