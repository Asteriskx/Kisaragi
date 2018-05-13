namespace Kisaragi.APIs.Twitter
{
	partial class PostWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostWindow));
			this.PostForm = new System.Windows.Forms.RichTextBox();
			this.UserName = new System.Windows.Forms.Label();
			this.ScreenName = new System.Windows.Forms.Label();
			this.PostButton = new System.Windows.Forms.Button();
			this.StringCounter = new System.Windows.Forms.Label();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.Status = new System.Windows.Forms.ToolStripStatusLabel();
			this.UserImage = new System.Windows.Forms.PictureBox();
			this.CloseButton = new System.Windows.Forms.Button();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UserImage)).BeginInit();
			this.SuspendLayout();
			// 
			// PostForm
			// 
			this.PostForm.BackColor = System.Drawing.Color.Black;
			this.PostForm.EnableAutoDragDrop = true;
			this.PostForm.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.PostForm.ForeColor = System.Drawing.Color.White;
			this.PostForm.Location = new System.Drawing.Point(12, 88);
			this.PostForm.Name = "PostForm";
			this.PostForm.Size = new System.Drawing.Size(320, 96);
			this.PostForm.TabIndex = 0;
			this.PostForm.Text = "";
			// 
			// UserName
			// 
			this.UserName.AutoSize = true;
			this.UserName.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UserName.ForeColor = System.Drawing.Color.White;
			this.UserName.Location = new System.Drawing.Point(88, 12);
			this.UserName.Name = "UserName";
			this.UserName.Size = new System.Drawing.Size(53, 23);
			this.UserName.TabIndex = 2;
			this.UserName.Text = "Name";
			// 
			// ScreenName
			// 
			this.ScreenName.AutoSize = true;
			this.ScreenName.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.ScreenName.ForeColor = System.Drawing.Color.White;
			this.ScreenName.Location = new System.Drawing.Point(89, 35);
			this.ScreenName.Name = "ScreenName";
			this.ScreenName.Size = new System.Drawing.Size(87, 17);
			this.ScreenName.TabIndex = 3;
			this.ScreenName.Text = "@ScreenName";
			// 
			// PostButton
			// 
			this.PostButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.PostButton.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.PostButton.ForeColor = System.Drawing.Color.White;
			this.PostButton.Location = new System.Drawing.Point(92, 59);
			this.PostButton.Name = "PostButton";
			this.PostButton.Size = new System.Drawing.Size(75, 23);
			this.PostButton.TabIndex = 4;
			this.PostButton.Text = "Post";
			this.PostButton.UseVisualStyleBackColor = true;
			// 
			// StringCounter
			// 
			this.StringCounter.AutoSize = true;
			this.StringCounter.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.StringCounter.ForeColor = System.Drawing.Color.White;
			this.StringCounter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.StringCounter.Location = new System.Drawing.Point(300, 67);
			this.StringCounter.Name = "StringCounter";
			this.StringCounter.Size = new System.Drawing.Size(29, 18);
			this.StringCounter.TabIndex = 5;
			this.StringCounter.Text = "140";
			this.StringCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// statusStrip
			// 
			this.statusStrip.BackColor = System.Drawing.Color.DimGray;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
			this.statusStrip.Location = new System.Drawing.Point(0, 192);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(344, 22);
			this.statusStrip.TabIndex = 6;
			this.statusStrip.Text = "statusStrip1";
			// 
			// Status
			// 
			this.Status.Font = new System.Drawing.Font("Yu Gothic UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Status.ForeColor = System.Drawing.Color.White;
			this.Status.Name = "Status";
			this.Status.Size = new System.Drawing.Size(51, 17);
			this.Status.Text = "準備完了";
			// 
			// UserImage
			// 
			this.UserImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.UserImage.Location = new System.Drawing.Point(12, 12);
			this.UserImage.Name = "UserImage";
			this.UserImage.Size = new System.Drawing.Size(70, 70);
			this.UserImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.UserImage.TabIndex = 1;
			this.UserImage.TabStop = false;
			// 
			// CloseButton
			// 
			this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CloseButton.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.CloseButton.ForeColor = System.Drawing.Color.White;
			this.CloseButton.Location = new System.Drawing.Point(173, 59);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 23);
			this.CloseButton.TabIndex = 7;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// PostWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(344, 214);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.StringCounter);
			this.Controls.Add(this.PostButton);
			this.Controls.Add(this.ScreenName);
			this.Controls.Add(this.UserName);
			this.Controls.Add(this.UserImage);
			this.Controls.Add(this.PostForm);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PostWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "投稿フォーム";
			this.Load += new System.EventHandler(this.PostWindow_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UserImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox PostForm;
		private System.Windows.Forms.PictureBox UserImage;
		private System.Windows.Forms.Label UserName;
		private System.Windows.Forms.Label ScreenName;
		private System.Windows.Forms.Button PostButton;
		private System.Windows.Forms.Label StringCounter;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel Status;
		private System.Windows.Forms.Button CloseButton;
	}
}