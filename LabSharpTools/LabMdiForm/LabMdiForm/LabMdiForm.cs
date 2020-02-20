using Harry.LabTools.LabControlPlus;
using Harry.LabTools.LabGenForm;
using Harry.LabTools.LabGenFunc;
using Harry.LabTools.LabMcuForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabMdiForm
{
	public partial class LabMdiForm : Form
	{
		#region 变量定义

		#endregion

		#region 属性定义

		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public LabMdiForm()
		{
			InitializeComponent();
			//---注册船体加载事件
			this.Load += new System.EventHandler(this.Form_Load);
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
		/// 船体初始化函数
		/// </summary>
		private void StartupInit()
		{
			//---事件注册
			this.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);
			//---退出操作
			this.ToolStripMenuItem_ExitTools.Click += new EventHandler(this.ToolStripMenuItem_Click);
			//---调用计算器
			this.ToolStripMenuItem_CalcTools.Click += new EventHandler(this.ToolStripMenuItem_Click);
			//---调用并嵌套记事本应用
			this.ToolStripMenuItem_NoteTools.Click += new EventHandler(this.ToolStripMenuItem_Click);
			//---在线下载器
			this.ToolStripMenuItem_OnlineAVRDownTools.Click += new EventHandler(this.ToolStripMenuItem_Click);

		}


		/// <summary>
		/// AVR8Bit的MCU的烧录器
		/// </summary>

		private void FormCMcuFormAVR8BitsForm_Init()
		{
			//---遍历窗体
			for (int i = 0; i < this.MdiChildren.Length; i++)
			{
				if (this.MdiChildren[i] is CMcuFormAVR8BitsForm)
				{
					if (this.MdiChildren[i].WindowState == FormWindowState.Minimized)
					{
						this.MdiChildren[i].WindowState = FormWindowState.Normal;
					}
					this.MdiChildren[i].Activate();
					return;
				}
			}
			CMcuFormAVR8BitsForm newForm = new CMcuFormAVR8BitsForm();
			//---判断窗体高度是否合适
			if (this.Height <= newForm.Height)
			{
				this.Height = newForm.Height + 100;
			}
			else if ((this.Height - newForm.Height) < 100)
			{
				this.Height = newForm.Height + 100;
			}
			//---判断窗体宽度是否合适
			if (this.Width <= newForm.Width)
			{
				this.Width = newForm.Width + 100;
			}
			else if ((this.Width - newForm.Width) < 100)
			{
				this.Width = newForm.Width + 100;
			}
			//---设置父窗体的最小值
			this.MinimumSize = this.Size;
			//---设置MDI的父窗体
			newForm.MdiParent = this;
			//---船体居中显示
			newForm.StartPosition = FormStartPosition.CenterScreen;
			//---显示船体
			newForm.Show();
		}

		#endregion

		#region 事件函数
		/// 窗体加载事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void Form_Load(object sender, System.EventArgs e)
		{
			this.StartupInit();
		}

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
					if (DialogResult.OK == CMessageBoxPlus.Show(this, "你确定要关闭应用程序吗？", "关闭提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
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
		/// 菜单单击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
			tsm.Enabled = false;
			int i = 0;
			string exePatch = null;
			switch (tsm.Name)
			{
				//---退出操作
				case "ToolStripMenuItem_ExitTools":
					if (DialogResult.OK == CMessageBoxPlus.Show(this, "你确定要关闭应用程序吗？", "关闭提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
					{
						if (this.IsMdiContainer)
						{
							//----为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
							this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
						}
						//---退出当前窗体
						this.Dispose();
					}
					break;
				//---调用计算器，只能调用，不能嵌套
				case "ToolStripMenuItem_CalcTools":
					System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
					Info.FileName = "calc.exe ";//"calc.exe"为计算器，"notepad.exe"为记事本
					System.Diagnostics.Process Proc = System.Diagnostics.Process.Start(Info);
					break;
				//---调用文本编辑器
				case "ToolStripMenuItem_NoteTools":
					for (i = 0; i < this.MdiChildren.Length; i++)
					{
						if (this.MdiChildren[i].Text == "记事本")
						{
							//---判断当前窗体是否处于最小化状态，
							if (this.MdiChildren[i].WindowState == FormWindowState.Minimized)
							{
								this.MdiChildren[i].WindowState = FormWindowState.Normal;
							}
							this.MdiChildren[i].Activate();
							return;
						}
					}
					exePatch = CGenFuncEXE.GenFuncGetExeNamePatch("notepad++.exe");
					//---检查应用是否存在
					if (exePatch == null)
					{
						exePatch = @"C:\Windows\system32\notepad.exe";
					}
					//---将外部应用嵌套当前窗体
					CEmbeddedForm txtForm = new CEmbeddedForm(exePatch, "记事本");
					txtForm.MdiParent = this;
					txtForm.StartPosition = FormStartPosition.CenterScreen;
					txtForm.Show();
					txtForm.Focus();
					//txtForm.StartProcess(@"C:\Windows\system32\notepad.exe");
					break;
				//---AVR的8Bit的MCU的在线下载器
				case "ToolStripMenuItem_OnlineAVRDownTools":
					this.FormCMcuFormAVR8BitsForm_Init();
					break;
				default:
					break;
			}
			tsm.Enabled = true;
		}
		#endregion
	}
}
