namespace Spritely
{
	partial class BgImageForm
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
			this.pbTools = new System.Windows.Forms.PictureBox();
			this.pbBgImage = new System.Windows.Forms.PictureBox();
			this.cbZoom = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.pbTools)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbBgImage)).BeginInit();
			this.SuspendLayout();
			// 
			// pbTools
			// 
			this.pbTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbTools.Location = new System.Drawing.Point(5, 5);
			this.pbTools.Name = "pbTools";
			this.pbTools.Size = new System.Drawing.Size(55, 231);
			this.pbTools.TabIndex = 0;
			this.pbTools.TabStop = false;
			this.pbTools.MouseLeave += new System.EventHandler(this.pbTools_MouseLeave);
			this.pbTools.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTools_MouseMove);
			this.pbTools.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTools_MouseDown);
			this.pbTools.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTools_Paint);
			this.pbTools.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTools_MouseUp);
			// 
			// pbBgImage
			// 
			this.pbBgImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pbBgImage.Location = new System.Drawing.Point(65, 5);
			this.pbBgImage.Name = "pbBgImage";
			this.pbBgImage.Size = new System.Drawing.Size(326, 291);
			this.pbBgImage.TabIndex = 1;
			this.pbBgImage.TabStop = false;
			this.pbBgImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbBgImage_MouseMove);
			this.pbBgImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbBgImage_MouseDown);
			this.pbBgImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pbBgImage_Paint);
			this.pbBgImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbBgImage_MouseUp);
			// 
			// cbZoom
			// 
			this.cbZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbZoom.FormattingEnabled = true;
			this.cbZoom.Items.AddRange(new object[] {
            "1x",
            "2x",
            "4x",
            "8x",
            "16x",
            "32x"});
			this.cbZoom.Location = new System.Drawing.Point(5, 243);
			this.cbZoom.Name = "cbZoom";
			this.cbZoom.Size = new System.Drawing.Size(55, 21);
			this.cbZoom.TabIndex = 2;
			this.cbZoom.TabStop = false;
			this.cbZoom.SelectedIndexChanged += new System.EventHandler(this.cbZoom_SelectedIndexChanged);
			// 
			// BgImageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(395, 301);
			this.Controls.Add(this.cbZoom);
			this.Controls.Add(this.pbBgImage);
			this.Controls.Add(this.pbTools);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(206, 297);
			this.Name = "BgImageForm";
			this.Text = "Background Image";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BgImageForm_FormClosing);
			this.Resize += new System.EventHandler(this.BgImageForm_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pbTools)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbBgImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbTools;
		private System.Windows.Forms.PictureBox pbBgImage;
		private System.Windows.Forms.ComboBox cbZoom;
	}
}