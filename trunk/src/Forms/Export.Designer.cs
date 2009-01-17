namespace Spritely
{
	partial class Export
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
			this.bExport = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.gbExportAs = new System.Windows.Forms.GroupBox();
			this.rbUpdateProject = new System.Windows.Forms.RadioButton();
			this.rbProject = new System.Windows.Forms.RadioButton();
			this.rbSprites = new System.Windows.Forms.RadioButton();
			this.gbLocation = new System.Windows.Forms.GroupBox();
			this.bBrowse = new System.Windows.Forms.Button();
			this.tbLocation = new System.Windows.Forms.TextBox();
			this.gbSystem = new System.Windows.Forms.GroupBox();
			this.rbNDS = new System.Windows.Forms.RadioButton();
			this.rbGBA = new System.Windows.Forms.RadioButton();
			this.gbExportAs.SuspendLayout();
			this.gbLocation.SuspendLayout();
			this.gbSystem.SuspendLayout();
			this.SuspendLayout();
			// 
			// bExport
			// 
			this.bExport.Location = new System.Drawing.Point(343, 174);
			this.bExport.Name = "bExport";
			this.bExport.Size = new System.Drawing.Size(75, 23);
			this.bExport.TabIndex = 0;
			this.bExport.Text = "Export";
			this.bExport.UseVisualStyleBackColor = true;
			this.bExport.Click += new System.EventHandler(this.bExport_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(262, 174);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 1;
			this.bCancel.Text = "Cancel";
			this.bCancel.UseVisualStyleBackColor = true;
			// 
			// gbExportAs
			// 
			this.gbExportAs.Controls.Add(this.rbUpdateProject);
			this.gbExportAs.Controls.Add(this.rbProject);
			this.gbExportAs.Controls.Add(this.rbSprites);
			this.gbExportAs.Location = new System.Drawing.Point(12, 12);
			this.gbExportAs.Name = "gbExportAs";
			this.gbExportAs.Size = new System.Drawing.Size(263, 86);
			this.gbExportAs.TabIndex = 2;
			this.gbExportAs.TabStop = false;
			this.gbExportAs.Text = "Export as...";
			// 
			// rbUpdateProject
			// 
			this.rbUpdateProject.AutoSize = true;
			this.rbUpdateProject.Location = new System.Drawing.Point(12, 39);
			this.rbUpdateProject.Name = "rbUpdateProject";
			this.rbUpdateProject.Size = new System.Drawing.Size(116, 17);
			this.rbUpdateProject.TabIndex = 2;
			this.rbUpdateProject.TabStop = true;
			this.rbUpdateProject.Text = "Update project files";
			this.rbUpdateProject.UseVisualStyleBackColor = true;
			// 
			// rbProject
			// 
			this.rbProject.AutoSize = true;
			this.rbProject.Location = new System.Drawing.Point(12, 59);
			this.rbProject.Name = "rbProject";
			this.rbProject.Size = new System.Drawing.Size(104, 17);
			this.rbProject.TabIndex = 1;
			this.rbProject.TabStop = true;
			this.rbProject.Text = "Complete project";
			this.rbProject.UseVisualStyleBackColor = true;
			// 
			// rbSprites
			// 
			this.rbSprites.AutoSize = true;
			this.rbSprites.Location = new System.Drawing.Point(12, 19);
			this.rbSprites.Name = "rbSprites";
			this.rbSprites.Size = new System.Drawing.Size(176, 17);
			this.rbSprites.TabIndex = 0;
			this.rbSprites.TabStop = true;
			this.rbSprites.Text = "Sprite and background files only";
			this.rbSprites.UseVisualStyleBackColor = true;
			// 
			// gbLocation
			// 
			this.gbLocation.Controls.Add(this.bBrowse);
			this.gbLocation.Controls.Add(this.tbLocation);
			this.gbLocation.Location = new System.Drawing.Point(12, 113);
			this.gbLocation.Name = "gbLocation";
			this.gbLocation.Size = new System.Drawing.Size(406, 52);
			this.gbLocation.TabIndex = 3;
			this.gbLocation.TabStop = false;
			this.gbLocation.Text = "Directory Path";
			// 
			// bBrowse
			// 
			this.bBrowse.Location = new System.Drawing.Point(320, 19);
			this.bBrowse.Name = "bBrowse";
			this.bBrowse.Size = new System.Drawing.Size(75, 23);
			this.bBrowse.TabIndex = 1;
			this.bBrowse.Text = "Browse...";
			this.bBrowse.UseVisualStyleBackColor = true;
			this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// tbLocation
			// 
			this.tbLocation.Location = new System.Drawing.Point(10, 22);
			this.tbLocation.Name = "tbLocation";
			this.tbLocation.ReadOnly = true;
			this.tbLocation.Size = new System.Drawing.Size(301, 20);
			this.tbLocation.TabIndex = 0;
			// 
			// gbSystem
			// 
			this.gbSystem.Controls.Add(this.rbNDS);
			this.gbSystem.Controls.Add(this.rbGBA);
			this.gbSystem.Location = new System.Drawing.Point(281, 13);
			this.gbSystem.Name = "gbSystem";
			this.gbSystem.Size = new System.Drawing.Size(137, 68);
			this.gbSystem.TabIndex = 4;
			this.gbSystem.TabStop = false;
			this.gbSystem.Text = "System";
			// 
			// rbNDS
			// 
			this.rbNDS.AutoSize = true;
			this.rbNDS.Location = new System.Drawing.Point(11, 41);
			this.rbNDS.Name = "rbNDS";
			this.rbNDS.Size = new System.Drawing.Size(86, 17);
			this.rbNDS.TabIndex = 1;
			this.rbNDS.TabStop = true;
			this.rbNDS.Text = "Nintendo DS";
			this.rbNDS.UseVisualStyleBackColor = true;
			// 
			// rbGBA
			// 
			this.rbGBA.AutoSize = true;
			this.rbGBA.Location = new System.Drawing.Point(11, 19);
			this.rbGBA.Name = "rbGBA";
			this.rbGBA.Size = new System.Drawing.Size(116, 17);
			this.rbGBA.TabIndex = 0;
			this.rbGBA.TabStop = true;
			this.rbGBA.Text = "Gameboy Advance";
			this.rbGBA.UseVisualStyleBackColor = true;
			// 
			// Export
			// 
			this.AcceptButton = this.bExport;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(432, 211);
			this.Controls.Add(this.gbSystem);
			this.Controls.Add(this.gbLocation);
			this.Controls.Add(this.gbExportAs);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bExport);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Export";
			this.Text = "Export";
			this.gbExportAs.ResumeLayout(false);
			this.gbExportAs.PerformLayout();
			this.gbLocation.ResumeLayout(false);
			this.gbLocation.PerformLayout();
			this.gbSystem.ResumeLayout(false);
			this.gbSystem.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button bExport;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.GroupBox gbExportAs;
		private System.Windows.Forms.RadioButton rbProject;
		private System.Windows.Forms.RadioButton rbSprites;
		private System.Windows.Forms.GroupBox gbLocation;
		private System.Windows.Forms.Button bBrowse;
		private System.Windows.Forms.TextBox tbLocation;
		private System.Windows.Forms.GroupBox gbSystem;
		private System.Windows.Forms.RadioButton rbNDS;
		private System.Windows.Forms.RadioButton rbGBA;
		private System.Windows.Forms.RadioButton rbUpdateProject;
	}
}