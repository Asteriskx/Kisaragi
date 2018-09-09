using System.Diagnostics;
using System.Windows.Forms;

namespace Kisaragi.Views
{
	/// <summary>
	/// Kisaragi バージョン情報を管理するクラス
	/// </summary>
	public partial class VersionWindow : Form
	{

		#region Constractor 
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public VersionWindow()
		{
			this.InitializeComponent();

			// バージョン情報が押下された際、最初に動作します。
			this.Load += (s, e) =>
			{
				this.kisaragi.Image = Properties.Resources.logo;
				this.gitIcon.Image = Properties.Resources.GitHub;
				this.Twitter.Image = Properties.Resources.Twitter;
			};

			// キャンセルボタン押下
			this.CloseButton.Click += (s, e) =>
			{
				this.Dispose();
				this.Close();
			};

			this.Twitter.Click += (s, e) => Process.Start("https://twitter.com/Astrisk_");
			this.gitIcon.Click += (s, e) => Process.Start("https://github.com/Asteriskx/Kisaragi");

			this.MaximizeBox = false;
			this.MinimizeBox = false;
		}

		#endregion

	}
}
