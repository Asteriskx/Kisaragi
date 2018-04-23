namespace Kisaragi
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
			this.OAuth = new System.Windows.Forms.ToolStripMenuItem();
			this.testPost = new System.Windows.Forms.ToolStripMenuItem();
			this.VersionInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.ExitKisaragi = new System.Windows.Forms.ToolStripMenuItem();
			this.ProfileIcon = new System.Windows.Forms.PictureBox();
			this.UserID = new System.Windows.Forms.Label();
			this.UserTweet = new System.Windows.Forms.RichTextBox();
			this.UserName = new System.Windows.Forms.Label();
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
            this.OAuth,
            this.testPost,
            this.VersionInfo,
            this.ExitKisaragi});
			this.ContextMenu.Name = "ContextMenu";
			this.ContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.ContextMenu.Size = new System.Drawing.Size(195, 92);
			// 
			// OAuth
			// 
			this.OAuth.Name = "OAuth";
			this.OAuth.Size = new System.Drawing.Size(194, 22);
			this.OAuth.Text = "Twitter 認証(&O)";
			// 
			// testPost
			// 
			this.testPost.Name = "testPost";
			this.testPost.Size = new System.Drawing.Size(194, 22);
			this.testPost.Text = "テスト投稿(&P)";
			// 
			// VersionInfo
			// 
			this.VersionInfo.Name = "VersionInfo";
			this.VersionInfo.Size = new System.Drawing.Size(194, 22);
			this.VersionInfo.Text = "バージョン情報について(&V)";
			// 
			// ExitKisaragi
			// 
			this.ExitKisaragi.Name = "ExitKisaragi";
			this.ExitKisaragi.Size = new System.Drawing.Size(194, 22);
			this.ExitKisaragi.Text = "Kisaragi の終了(&E)";
			// 
			// ProfileIcon
			// 
			this.ProfileIcon.Location = new System.Drawing.Point(12, 12);
			this.ProfileIcon.Name = "ProfileIcon";
			this.ProfileIcon.Size = new System.Drawing.Size(125, 125);
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
			this.UserID.Location = new System.Drawing.Point(143, 39);
			this.UserID.Name = "UserID";
			this.UserID.Size = new System.Drawing.Size(119, 18);
			this.UserID.TabIndex = 2;
			this.UserID.Text = "Author : @Astrisk_";
			// 
			// UserTweet
			// 
			this.UserTweet.BackColor = System.Drawing.Color.Black;
			this.UserTweet.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.UserTweet.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UserTweet.ForeColor = System.Drawing.Color.White;
			this.UserTweet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.UserTweet.Location = new System.Drawing.Point(148, 65);
			this.UserTweet.Name = "UserTweet";
			this.UserTweet.Size = new System.Drawing.Size(297, 72);
			this.UserTweet.TabIndex = 3;
			this.UserTweet.Text = "";
			// 
			// UserName
			// 
			this.UserName.AutoSize = true;
			this.UserName.BackColor = System.Drawing.Color.Black;
			this.UserName.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UserName.ForeColor = System.Drawing.Color.White;
			this.UserName.Location = new System.Drawing.Point(144, 15);
			this.UserName.Name = "UserName";
			this.UserName.Size = new System.Drawing.Size(51, 24);
			this.UserName.TabIndex = 5;
			this.UserName.Text = "Hoge";
			// 
			// Kisaragi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(457, 149);
			this.Controls.Add(this.UserName);
			this.Controls.Add(this.UserTweet);
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
		private System.Windows.Forms.ToolStripMenuItem OAuth;
		private System.Windows.Forms.ToolStripMenuItem testPost;
		private System.Windows.Forms.PictureBox ProfileIcon;
		private System.Windows.Forms.Label UserID;
		private System.Windows.Forms.RichTextBox UserTweet;
		private System.Windows.Forms.Label UserName;
	}
}