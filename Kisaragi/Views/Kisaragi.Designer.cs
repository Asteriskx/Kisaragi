namespace Kisaragi.Views
{
	partial class Kisaragi
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kisaragi));
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Settings = new System.Windows.Forms.ToolStripMenuItem();
			this.MonitorSize = new System.Windows.Forms.ToolStripMenuItem();
			this.MonitorDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.MonitorMinimum = new System.Windows.Forms.ToolStripMenuItem();
			this.VersionInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.ExitKisaragi = new System.Windows.Forms.ToolStripMenuItem();
			this.ProfileIcon = new System.Windows.Forms.PictureBox();
			this.UserID = new System.Windows.Forms.Label();
			this.UserName = new System.Windows.Forms.Label();
			this.MultiMsg = new System.Windows.Forms.Label();
			this.checkBoxNotifyTime = new System.Windows.Forms.CheckBox();
			this.checkBoxPostTwitter = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBoxNotifyVoice = new System.Windows.Forms.CheckBox();
			this.ContextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProfileIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// notifyIcon
			// 
			this.notifyIcon.Text = "notifyIcon1";
			this.notifyIcon.Visible = true;
			// 
			// ContextMenu
			// 
			this.ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Settings,
            this.MonitorSize,
            this.VersionInfo,
            this.ExitKisaragi});
			this.ContextMenu.Name = "ContextMenu";
			this.ContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.ContextMenu.Size = new System.Drawing.Size(167, 92);
			// 
			// Settings
			// 
			this.Settings.Name = "Settings";
			this.Settings.Size = new System.Drawing.Size(166, 22);
			this.Settings.Text = "Settings(&S)";
			// 
			// MonitorSize
			// 
			this.MonitorSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MonitorDefault,
            this.MonitorMinimum});
			this.MonitorSize.Name = "MonitorSize";
			this.MonitorSize.Size = new System.Drawing.Size(166, 22);
			this.MonitorSize.Text = "画面(&M)";
			// 
			// MonitorDefault
			// 
			this.MonitorDefault.Name = "MonitorDefault";
			this.MonitorDefault.Size = new System.Drawing.Size(129, 22);
			this.MonitorDefault.Text = "通常(&D)";
			// 
			// MonitorMinimum
			// 
			this.MonitorMinimum.Name = "MonitorMinimum";
			this.MonitorMinimum.Size = new System.Drawing.Size(129, 22);
			this.MonitorMinimum.Text = "最小化(&M)";
			// 
			// VersionInfo
			// 
			this.VersionInfo.Name = "VersionInfo";
			this.VersionInfo.Size = new System.Drawing.Size(166, 22);
			this.VersionInfo.Text = "バージョン情報(&V)";
			// 
			// ExitKisaragi
			// 
			this.ExitKisaragi.Name = "ExitKisaragi";
			this.ExitKisaragi.Size = new System.Drawing.Size(166, 22);
			this.ExitKisaragi.Text = "Kisaragi の終了(&E)";
			// 
			// ProfileIcon
			// 
			this.ProfileIcon.BackgroundImage = global::Kisaragi.Properties.Resources.logo;
			this.ProfileIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.ProfileIcon.InitialImage = null;
			this.ProfileIcon.Location = new System.Drawing.Point(12, 12);
			this.ProfileIcon.Name = "ProfileIcon";
			this.ProfileIcon.Size = new System.Drawing.Size(145, 152);
			this.ProfileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.ProfileIcon.TabIndex = 1;
			this.ProfileIcon.TabStop = false;
			// 
			// UserID
			// 
			this.UserID.AutoSize = true;
			this.UserID.BackColor = System.Drawing.Color.Black;
			this.UserID.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UserID.ForeColor = System.Drawing.Color.White;
			this.UserID.Location = new System.Drawing.Point(175, 39);
			this.UserID.Name = "UserID";
			this.UserID.Size = new System.Drawing.Size(119, 18);
			this.UserID.TabIndex = 2;
			this.UserID.Text = "Author : @Astrisk_";
			// 
			// UserName
			// 
			this.UserName.AutoSize = true;
			this.UserName.BackColor = System.Drawing.Color.Black;
			this.UserName.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UserName.ForeColor = System.Drawing.Color.White;
			this.UserName.Location = new System.Drawing.Point(174, 12);
			this.UserName.Name = "UserName";
			this.UserName.Size = new System.Drawing.Size(51, 24);
			this.UserName.TabIndex = 5;
			this.UserName.Text = "Hoge";
			// 
			// MultiMsg
			// 
			this.MultiMsg.AutoSize = true;
			this.MultiMsg.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.MultiMsg.ForeColor = System.Drawing.Color.White;
			this.MultiMsg.Location = new System.Drawing.Point(174, 60);
			this.MultiMsg.Name = "MultiMsg";
			this.MultiMsg.Size = new System.Drawing.Size(131, 18);
			this.MultiMsg.TabIndex = 6;
			this.MultiMsg.Text = "Welcome to Kisaragi.";
			// 
			// checkBoxNotifyTime
			// 
			this.checkBoxNotifyTime.AutoSize = true;
			this.checkBoxNotifyTime.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.checkBoxNotifyTime.ForeColor = System.Drawing.Color.White;
			this.checkBoxNotifyTime.Location = new System.Drawing.Point(169, 94);
			this.checkBoxNotifyTime.Name = "checkBoxNotifyTime";
			this.checkBoxNotifyTime.Size = new System.Drawing.Size(141, 22);
			this.checkBoxNotifyTime.TabIndex = 7;
			this.checkBoxNotifyTime.Text = "Custom NotifyTime";
			this.checkBoxNotifyTime.UseVisualStyleBackColor = true;
			// 
			// checkBoxPostTwitter
			// 
			this.checkBoxPostTwitter.AutoSize = true;
			this.checkBoxPostTwitter.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.checkBoxPostTwitter.ForeColor = System.Drawing.Color.White;
			this.checkBoxPostTwitter.Location = new System.Drawing.Point(169, 118);
			this.checkBoxPostTwitter.Name = "checkBoxPostTwitter";
			this.checkBoxPostTwitter.Size = new System.Drawing.Size(127, 22);
			this.checkBoxPostTwitter.TabIndex = 8;
			this.checkBoxPostTwitter.Text = "Post with Twitter";
			this.checkBoxPostTwitter.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Location = new System.Drawing.Point(166, 86);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(270, 2);
			this.panel1.TabIndex = 9;
			// 
			// checkBoxNotifyVoice
			// 
			this.checkBoxNotifyVoice.AutoSize = true;
			this.checkBoxNotifyVoice.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.checkBoxNotifyVoice.ForeColor = System.Drawing.Color.White;
			this.checkBoxNotifyVoice.Location = new System.Drawing.Point(169, 143);
			this.checkBoxNotifyVoice.Name = "checkBoxNotifyVoice";
			this.checkBoxNotifyVoice.Size = new System.Drawing.Size(128, 22);
			this.checkBoxNotifyVoice.TabIndex = 10;
			this.checkBoxNotifyVoice.Text = "Using NotifyVoice";
			this.checkBoxNotifyVoice.UseVisualStyleBackColor = true;
			// 
			// Kisaragi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(458, 176);
			this.Controls.Add(this.checkBoxNotifyVoice);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.checkBoxPostTwitter);
			this.Controls.Add(this.checkBoxNotifyTime);
			this.Controls.Add(this.MultiMsg);
			this.Controls.Add(this.UserName);
			this.Controls.Add(this.UserID);
			this.Controls.Add(this.ProfileIcon);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Kisaragi";
			this.Text = "Kisagagi";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ContextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ProfileIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip ContextMenu;
		private System.Windows.Forms.ToolStripMenuItem ExitKisaragi;
		private System.Windows.Forms.ToolStripMenuItem VersionInfo;
		private System.Windows.Forms.PictureBox ProfileIcon;
		private System.Windows.Forms.Label UserID;
		private System.Windows.Forms.Label UserName;
		private System.Windows.Forms.ToolStripMenuItem Settings;
		private System.Windows.Forms.Label MultiMsg;
		private System.Windows.Forms.CheckBox checkBoxNotifyTime;
		private System.Windows.Forms.CheckBox checkBoxPostTwitter;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkBoxNotifyVoice;
		private System.Windows.Forms.ToolStripMenuItem MonitorSize;
		private System.Windows.Forms.ToolStripMenuItem MonitorDefault;
		private System.Windows.Forms.ToolStripMenuItem MonitorMinimum;
	}
}