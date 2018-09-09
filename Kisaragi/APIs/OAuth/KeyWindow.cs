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

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ck">Consumer Key</param>
		/// <param name="cks">Consumer Secret</param>
		public KeyWindow(string ck, string cks)
		{
			this.InitializeComponent();

			this.consumerKey.Text += ck;
			this.consumerSecret.Text += cks;

			this.appLink.Click += (s, e) => Process.Start(this.appLink.Text);

			this.OKButton.Click += (s, e) =>
			{
				this.CkPair = (this.consumerKey.Text, this.consumerSecret.Text);
				this.DialogResult = DialogResult.OK;

				this.Dispose();
				this.Close();
			};

			this.CancelButton.Click += (s, e) =>
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
