using System.Diagnostics;
using System.Windows.Forms;

namespace Kisaragi.APIs.OAuth
{
	/// <summary>
	/// Consumer Key / Secret の取得クラス
	/// </summary>
	public partial class KeyWindow : Form
	{

		#region Properties

		/// <summary>
		/// Consumer Key / Secret.
		/// </summary>
		public (string ck, string cks) CkPair { get; private set; }

		#endregion

		#region Constractor 

		public KeyWindow(string ck, string cks)
		{
			InitializeComponent();

			consumerKey.Text += ck;
			consumerSecret.Text += cks;

			appLink.Click += (s, e) => Process.Start(appLink.Text);

			OKButton.Click += (s, e) =>
			{
				this.CkPair = (consumerKey.Text, consumerSecret.Text);
				DialogResult = DialogResult.OK;

				this.Dispose();
				this.Close();
			};

			CancelButton.Click += (s, e) =>
			{
				this.Dispose();
				this.Close();
			};

			this.MaximizeBox = false;
			this.MinimizeBox = false;
		}

		#endregion

	}
}
