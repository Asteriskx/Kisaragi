namespace Kisaragi
{
	partial class VersionWindow
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
			this.MainTitle = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.CloseButton = new System.Windows.Forms.Button();
			this.gitIcon = new System.Windows.Forms.PictureBox();
			this.Twitter = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.gitIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Twitter)).BeginInit();
			this.SuspendLayout();
			// 
			// MainTitle
			// 
			this.MainTitle.AutoSize = true;
			this.MainTitle.Font = new System.Drawing.Font("メイリオ", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.MainTitle.Location = new System.Drawing.Point(12, 9);
			this.MainTitle.Name = "MainTitle";
			this.MainTitle.Size = new System.Drawing.Size(131, 44);
			this.MainTitle.TabIndex = 0;
			this.MainTitle.Text = "Kisaragi";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(136, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(262, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = " - 快適なPC Lifeを送るための時報アプリ - ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("メイリオ", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(51, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(158, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "Author : Asterisk.";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("メイリオ", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(135, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(222, 25);
			this.label3.TabIndex = 3;
			this.label3.Text = "まったり開発中(*\'ω\'*)～～";
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(351, 123);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 23);
			this.CloseButton.TabIndex = 4;
			this.CloseButton.Text = "閉じる";
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// gitIcon
			// 
			this.gitIcon.Location = new System.Drawing.Point(20, 96);
			this.gitIcon.Name = "gitIcon";
			this.gitIcon.Size = new System.Drawing.Size(50, 50);
			this.gitIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.gitIcon.TabIndex = 5;
			this.gitIcon.TabStop = false;
			// 
			// Twitter
			// 
			this.Twitter.Location = new System.Drawing.Point(71, 96);
			this.Twitter.Name = "Twitter";
			this.Twitter.Size = new System.Drawing.Size(50, 50);
			this.Twitter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.Twitter.TabIndex = 6;
			this.Twitter.TabStop = false;
			// 
			// VersionWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(438, 158);
			this.Controls.Add(this.Twitter);
			this.Controls.Add(this.gitIcon);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.MainTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "VersionWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Kisaragi について";
			this.Load += new System.EventHandler(this.VersionWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.gitIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Twitter)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label MainTitle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.PictureBox gitIcon;
		private System.Windows.Forms.PictureBox Twitter;
	}
}