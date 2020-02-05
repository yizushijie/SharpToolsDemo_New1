namespace Harry.LabTools.LabGenForm
{
	partial class CEmbeddedForm
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
			this.cPanelEx_EmbeddedForm = new Harry.LabTools.LabControlPlus.CPanelEx();
			this.SuspendLayout();
			// 
			// cPanelEx_EmbeddedForm
			// 
			this.cPanelEx_EmbeddedForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cPanelEx_EmbeddedForm.Location = new System.Drawing.Point(0, 0);
			this.cPanelEx_EmbeddedForm.Name = "cPanelEx_EmbeddedForm";
			this.cPanelEx_EmbeddedForm.Size = new System.Drawing.Size(800, 450);
			this.cPanelEx_EmbeddedForm.TabIndex = 1;
			// 
			// CEmbeddedForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.cPanelEx_EmbeddedForm);
			this.Name = "CEmbeddedForm";
			this.Text = "嵌入应用程序";
			this.ResumeLayout(false);

		}

		#endregion

		private LabControlPlus.CPanelEx cPanelEx_EmbeddedForm;
	}
}