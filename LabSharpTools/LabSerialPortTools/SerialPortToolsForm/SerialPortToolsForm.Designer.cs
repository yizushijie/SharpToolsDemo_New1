namespace LabSerialPortTools
{
	partial class SerialPortToolsForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.cPanelPlus1 = new Harry.LabTools.LabControlPlus.CPanelPlus();
			this.SuspendLayout();
			// 
			// cPanelPlus1
			// 
			this.cPanelPlus1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cPanelPlus1.Location = new System.Drawing.Point(8, 8);
			this.cPanelPlus1.Name = "cPanelPlus1";
			this.cPanelPlus1.Size = new System.Drawing.Size(1070, 619);
			this.cPanelPlus1.TabIndex = 0;
			// 
			// SerialPortToolsForm
			// 
			this.ClientSize = new System.Drawing.Size(1086, 635);
			this.Controls.Add(this.cPanelPlus1);
			this.Name = "SerialPortToolsForm";
			this.Padding = new System.Windows.Forms.Padding(8);
			this.Text = "串口调试助手";
			this.ResumeLayout(false);

		}

		#endregion

		private Harry.LabTools.LabControlPlus.CPanelPlus cPanelPlus1;
	}
}

