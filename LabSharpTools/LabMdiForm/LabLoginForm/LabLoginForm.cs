using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Harry.LabTools.LabControlPlus;

namespace Harry.LabTools.LabMdiForm
{
	public partial class LabLoginForm : Form
	{

		#region 变量定义

		#endregion

		#region 属性定义

		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public LabLoginForm()
		{
			InitializeComponent();
			this.StartUpInit();
		}
		#endregion

		#region 析构函数

		#endregion

		#region 公共函数

		#endregion

		#region 保护函数

		#endregion

		#region 私有函数
		/// <summary>
		/// 
		/// </summary>
		private void StartUpInit()
		{
			this.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);

			//---限定窗体的大小
			this.MinimumSize = this.Size;
			this.MaximumSize = this.Size;

			//---控件首先获取焦点
			this.button_LogIn.TabIndex = 0;

			this.button_LogIn.Click += new EventHandler(this.Button_Click);
			this.button_LogOut.Click += new EventHandler(this.Button_Click);
		}


		#endregion

		#region 事件函数

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			switch (e.CloseReason)
			{
				case CloseReason.None:
					break;
				case CloseReason.WindowsShutDown:
					break;
				case CloseReason.MdiFormClosing:
				case CloseReason.UserClosing:
					if (DialogResult.OK == CMessageBoxPlus.Show(this, "你确定要关闭退出登录吗？", "关闭提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
					{
						if (this.IsMdiContainer)
						{
							//----为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
							this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
						}

						//---确认关闭事件
						e.Cancel = false;

						//---退出当前窗体
						this.Dispose();
					}
					else
					{
						//---取消关闭事件
						e.Cancel = true;
					}
					break;
				case CloseReason.TaskManagerClosing:
					break;
				case CloseReason.FormOwnerClosing:
					break;
				case CloseReason.ApplicationExitCall:
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 按键点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			btn.Enabled = false;
			switch (btn.Name)
			{
				//---登录系统
				case "button_LogIn":
					this.DialogResult = DialogResult.OK;
					//---注销窗体关闭事件
					this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
					//---关闭窗体
					this.Close();
					//---注销
					this.Dispose();
					break;
				//---注销系统
				case "button_LogOut":
					//---关闭窗体
					this.Close();
					//---注销
					this.Dispose();
					break;
				default:
					break;
			}
			btn.Enabled = true;
		}

		#endregion
	}
}
