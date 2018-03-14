using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Kisaragi
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

		#region first running method.

		/// <summary>
		/// バージョン情報が押下された際、最初に動作します。
		/// </summary>
		private void VersionWindow_Load(object sender, EventArgs e)
		{
			gitIcon.Image = Properties.Resources.GitHub;
			Twitter.Image = Properties.Resources.Twitter;
		}

		#endregion
	}
}
