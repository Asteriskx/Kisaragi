namespace Kisaragi.Views
{
	partial class SettingsWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.UpDnNotifyTime = new System.Windows.Forms.NumericUpDown();
			this.VoiceFile = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.VoiceSetting = new System.Windows.Forms.Button();
			this.ButtonOk = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)(this.UpDnNotifyTime)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "NotifyTime：";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 18);
			this.label2.TabIndex = 1;
			this.label2.Text = "VoiceFile (Directory)：";
			// 
			// UpDnNotifyTime
			// 
			this.UpDnNotifyTime.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UpDnNotifyTime.Location = new System.Drawing.Point(148, 7);
			this.UpDnNotifyTime.Name = "UpDnNotifyTime";
			this.UpDnNotifyTime.Size = new System.Drawing.Size(120, 25);
			this.UpDnNotifyTime.TabIndex = 2;
			this.UpDnNotifyTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// VoiceFile
			// 
			this.VoiceFile.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.VoiceFile.Location = new System.Drawing.Point(148, 38);
			this.VoiceFile.Name = "VoiceFile";
			this.VoiceFile.Size = new System.Drawing.Size(465, 25);
			this.VoiceFile.TabIndex = 3;
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(13, 71);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(600, 2);
			this.panel1.TabIndex = 4;
			// 
			// VoiceSetting
			// 
			this.VoiceSetting.Location = new System.Drawing.Point(391, 79);
			this.VoiceSetting.Name = "VoiceSetting";
			this.VoiceSetting.Size = new System.Drawing.Size(109, 23);
			this.VoiceSetting.TabIndex = 5;
			this.VoiceSetting.Text = "voice setting";
			this.VoiceSetting.UseVisualStyleBackColor = true;
			// 
			// ButtonOk
			// 
			this.ButtonOk.Location = new System.Drawing.Point(506, 79);
			this.ButtonOk.Name = "ButtonOk";
			this.ButtonOk.Size = new System.Drawing.Size(109, 23);
			this.ButtonOk.TabIndex = 6;
			this.ButtonOk.Text = "OK";
			this.ButtonOk.UseVisualStyleBackColor = true;
			// 
			// SettingsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 110);
			this.Controls.Add(this.ButtonOk);
			this.Controls.Add(this.VoiceSetting);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.VoiceFile);
			this.Controls.Add(this.UpDnNotifyTime);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SettingsWindow";
			this.Text = "SettingsWindow";
			((System.ComponentModel.ISupportInitialize)(this.UpDnNotifyTime)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown UpDnNotifyTime;
		private System.Windows.Forms.TextBox VoiceFile;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button VoiceSetting;
		private System.Windows.Forms.Button ButtonOk;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}