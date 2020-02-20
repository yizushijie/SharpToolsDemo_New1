using Harry.LabTools.LabControlPlus;
namespace Harry.LabTools.LabMdiForm
{
	partial class LabLoginForm
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
			this.groupBox_UserLogin = new System.Windows.Forms.GroupBox();
			this.button_LogOut = new System.Windows.Forms.Button();
			this.button_LogIn = new System.Windows.Forms.Button();
			this.label_UserPassword = new System.Windows.Forms.Label();
			this.label_UserName = new System.Windows.Forms.Label();
			this.comboBox_UserPassWord = new Harry.LabTools.LabControlPlus.CComboBoxEx();
			this.comboBox_UserName = new Harry.LabTools.LabControlPlus.CComboBoxEx();
			this.groupBox_UserLogin.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox_UserLogin
			// 
			this.groupBox_UserLogin.Controls.Add(this.comboBox_UserPassWord);
			this.groupBox_UserLogin.Controls.Add(this.comboBox_UserName);
			this.groupBox_UserLogin.Controls.Add(this.button_LogOut);
			this.groupBox_UserLogin.Controls.Add(this.button_LogIn);
			this.groupBox_UserLogin.Controls.Add(this.label_UserPassword);
			this.groupBox_UserLogin.Controls.Add(this.label_UserName);
			this.groupBox_UserLogin.Location = new System.Drawing.Point(138, 102);
			this.groupBox_UserLogin.Name = "groupBox_UserLogin";
			this.groupBox_UserLogin.Size = new System.Drawing.Size(201, 160);
			this.groupBox_UserLogin.TabIndex = 0;
			this.groupBox_UserLogin.TabStop = false;
			this.groupBox_UserLogin.Text = "用户登录";
			// 
			// button_LogOut
			// 
			this.button_LogOut.Location = new System.Drawing.Point(111, 120);
			this.button_LogOut.Name = "button_LogOut";
			this.button_LogOut.Size = new System.Drawing.Size(75, 23);
			this.button_LogOut.TabIndex = 5;
			this.button_LogOut.Text = "注销";
			this.button_LogOut.UseVisualStyleBackColor = true;
			// 
			// button_LogIn
			// 
			this.button_LogIn.Location = new System.Drawing.Point(21, 120);
			this.button_LogIn.Name = "button_LogIn";
			this.button_LogIn.Size = new System.Drawing.Size(75, 23);
			this.button_LogIn.TabIndex = 0;
			this.button_LogIn.Text = "登录";
			this.button_LogIn.UseVisualStyleBackColor = true;
			// 
			// label_UserPassword
			// 
			this.label_UserPassword.AutoSize = true;
			this.label_UserPassword.Location = new System.Drawing.Point(34, 81);
			this.label_UserPassword.Name = "label_UserPassword";
			this.label_UserPassword.Size = new System.Drawing.Size(35, 12);
			this.label_UserPassword.TabIndex = 1;
			this.label_UserPassword.Text = "密 码";
			// 
			// label_UserName
			// 
			this.label_UserName.AutoSize = true;
			this.label_UserName.Location = new System.Drawing.Point(28, 42);
			this.label_UserName.Name = "label_UserName";
			this.label_UserName.Size = new System.Drawing.Size(41, 12);
			this.label_UserName.TabIndex = 0;
			this.label_UserName.Text = "用户名";
			// 
			// comboBox_UserPassWord
			// 
			this.comboBox_UserPassWord.FormattingEnabled = true;
			this.comboBox_UserPassWord.Location = new System.Drawing.Point(77, 78);
			this.comboBox_UserPassWord.mToolTipText = "鼠标右键按下显示密码";
			this.comboBox_UserPassWord.Name = "comboBox_UserPassWord";
			this.comboBox_UserPassWord.Size = new System.Drawing.Size(100, 20);
			this.comboBox_UserPassWord.TabIndex = 7;
			this.comboBox_UserPassWord.Text = "123456";
			// 
			// comboBox_UserName
			// 
			this.comboBox_UserName.FormattingEnabled = true;
			this.comboBox_UserName.Location = new System.Drawing.Point(77, 39);
			this.comboBox_UserName.mToolTipText = "";
			this.comboBox_UserName.Name = "comboBox_UserName";
			this.comboBox_UserName.Size = new System.Drawing.Size(100, 20);
			this.comboBox_UserName.TabIndex = 6;
			this.comboBox_UserName.Text = "Admin";
			// 
			// LabLoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 420);
			this.Controls.Add(this.groupBox_UserLogin);
			this.Name = "LabLoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "LabLoginForm";
			this.groupBox_UserLogin.ResumeLayout(false);
			this.groupBox_UserLogin.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox_UserLogin;
		private System.Windows.Forms.Label label_UserPassword;
		private System.Windows.Forms.Label label_UserName;
		private System.Windows.Forms.Button button_LogOut;
		private System.Windows.Forms.Button button_LogIn;
		private CComboBoxEx comboBox_UserPassWord;
		private CComboBoxEx comboBox_UserName;
	}
}