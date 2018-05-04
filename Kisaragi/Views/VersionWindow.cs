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

		public VersionWindow()
		{
			InitializeComponent();

			// バージョン情報が押下された際、最初に動作します。
			this.Load += (s, e) =>
			{
				kisaragi.Image = Properties.Resources.logo;
				gitIcon.Image = Properties.Resources.GitHub;
				Twitter.Image = Properties.Resources.Twitter;
			};

			// キャンセルボタン押下
			CloseButton.Click += (s, e) =>
			{
				this.Dispose();
				this.Close();
			};

			Twitter.Click += (s, e) => Process.Start("https://twitter.com/Astrisk_");
			gitIcon.Click += (s, e) => Process.Start("https://github.com/Asteriskx/Kisaragi");

			this.MaximizeBox = false;
			this.MinimizeBox = false;
		}

		#endregion

	}
}
