using System.Drawing;
using System.Windows.Forms;

namespace Kisaragi.Views
{
	/// <summary>
	/// Kisaragi スプラッシュウィンドウ 管理クラス
	/// </summary>
	public partial class SplashWindow : Form
	{

		#region Constractor
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="interval">スプラッシュ表示時間</param>
		public SplashWindow(int interval)
		{
			this.InitializeComponent();

			var timer = new Timer();
			timer.Interval = interval;
			timer.Tick += (s, e) => this.Close();
			timer.Start();

			this.Paint += (s, e) => ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, this.Width, this.Height), Border3DStyle.Raised);
			this.Shown += (s, e) => this.Refresh();
		}

		#endregion

	}
}