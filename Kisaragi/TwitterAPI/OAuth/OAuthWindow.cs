using System.Windows.Forms;

namespace Kisaragi.TwitterAPI.OAuth
{
	/// <summary>
	/// Kisaragi が Twitter へアクセスする際に認証を行う画面管理を行います。
	/// </summary>
	public partial class OAuthWindow : Form
	{
		public string PinCode { get; set; }

		public OAuthWindow()
		{
			InitializeComponent();

			OKButton.Click += (s, v) =>
			{
				PinCode = pinCode.Text;
				DialogResult = DialogResult.OK;

				this.Dispose();
				this.Close();
			};

			CancelButton.Click += (s, v) =>
			{
				this.Dispose();
				this.Close();
			};

			this.MaximizeBox = false;
			this.MinimizeBox = false;
		}
	}
}
