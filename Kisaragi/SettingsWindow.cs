using System;
using System.IO;
using System.Windows.Forms;

namespace Kisaragi
{
	/// <summary>
	/// Kisaragi の設定管理クラス
	/// </summary>
	public partial class SettingsWindow : Form
	{

		#region Properties

		public TimeSpan NotifyTime { get; private set; }
		public string VoicePath { get; private set; }

		#endregion

		#region Constractor

		public SettingsWindow(string voicePath = null)
		{
			InitializeComponent();
			this.VoicePath = voicePath;

			// Invoke to window load.
			this.Load += (s, e) => VoiceFile.Text = this.VoicePath;

			this.ButtonOk.Click += (s, e) =>
			{
				NotifyTime = TimeSpan.FromMinutes((double)UpDnNotifyTime.Value);
				this.VoicePath = VoiceFile.Text;

				DialogResult = DialogResult.OK;
				this.Close();
			};

			this.VoiceSetting.Click += (s, e) =>
			{
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
					VoiceFile.Text = folderBrowserDialog.SelectedPath + @"\";
			};
		}

		#endregion
	}
}
