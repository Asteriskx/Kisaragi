using System;
using System.Windows.Forms;

namespace Kisaragi.Views
{
	/// <summary>
	/// Kisaragi の設定管理クラス
	/// </summary>
	public partial class SettingsWindow : Form
	{

		#region Properties
		
		/// <summary>
		/// 
		/// </summary>
		public TimeSpan NotifyTime { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public string VoicePath { get; private set; }

		#endregion

		#region Constractor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="voicePath"></param>
		public SettingsWindow(string voicePath = null)
		{
			this.InitializeComponent();
			this.VoicePath = voicePath;

			// Invoke to window load.
			this.Load += (s, e) => this.VoiceFile.Text = this.VoicePath;

			this.ButtonOk.Click += (s, e) =>
			{
				this.NotifyTime = TimeSpan.FromMinutes((double)UpDnNotifyTime.Value);
				this.VoicePath = this.VoiceFile.Text;

				this.DialogResult = DialogResult.OK;
				this.Close();
			};

			this.VoiceSetting.Click += (s, e) =>
			{
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
					this.VoiceFile.Text = folderBrowserDialog.SelectedPath + @"\";
			};
		}

		#endregion
	}
}
