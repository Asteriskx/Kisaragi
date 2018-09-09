using System.Windows.Forms;

namespace Kisaragi.APIs.OAuth
{
	/// <summary>
	/// Kisaragi が Twitter へアクセスする際に認証を行う画面管理を行います。
	/// </summary>
	public partial class OAuthWindow : Form
	{

		#region Properties
		
		/// <summary>
		/// PIN Code
		/// </summary>
		public string PinCode { get; set; }

		#endregion

		#region Constractor 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public OAuthWindow()
		{
			this.InitializeComponent();

			this.OKButton.Click += (s, v) =>
			{
				this.PinCode = pinCode.Text;
				this.DialogResult = DialogResult.OK;

				this.Dispose();
				this.Close();
			};

			this.CancelButton.Click += (s, v) =>
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
