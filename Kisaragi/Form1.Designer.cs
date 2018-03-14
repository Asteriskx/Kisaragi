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
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ExitKisaragi = new System.Windows.Forms.ToolStripMenuItem();
			this.VersionInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenu.SuspendLayout();
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
            this.ExitKisaragi,
            this.VersionInfo});
			this.ContextMenu.Name = "ContextMenu";
			this.ContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.ContextMenu.Size = new System.Drawing.Size(195, 70);
			// 
			// ExitKisaragi
			// 
			this.ExitKisaragi.Name = "ExitKisaragi";
			this.ExitKisaragi.Size = new System.Drawing.Size(194, 22);
			this.ExitKisaragi.Text = "Kisaragi の終了(&E)";
			// 
			// VersionInfo
			// 
			this.VersionInfo.Name = "VersionInfo";
			this.VersionInfo.Size = new System.Drawing.Size(194, 22);
			this.VersionInfo.Text = "バージョン情報について(&V)";
			// 
			// Kisaragi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(386, 168);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Kisaragi";
			this.Opacity = 0D;
			this.Text = "Kisagagi";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip ContextMenu;
		private System.Windows.Forms.ToolStripMenuItem ExitKisaragi;
		private System.Windows.Forms.ToolStripMenuItem VersionInfo;
	}
}