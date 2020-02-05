using Harry.LabTools.LabControlPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabGenForm
{
	public partial class CEmbeddedForm : Form
	{
		#region 变量定义
		/// <summary>
		/// 应用程序的路劲
		/// </summary>
		private string defaultProcessPatch = null;

		#endregion

		#region 属性定义
		/// <summary>
		/// 获取当前进程的名称
		/// </summary>
		public virtual string mProcessName
		{
			get
			{
				return this.Text;
			}
		}

		/// <summary>
		/// 获取当前进程的路劲
		/// </summary>
		public virtual string mProcessPatch
		{
			get
			{
				return this.defaultProcessPatch;
			}
		}

		#endregion

		#region 构造函数
		/// <summary>
		/// 
		/// </summary>
		public CEmbeddedForm()
		{
			InitializeComponent();
			this.Load += new EventHandler(this.CEmbeddedForm_Load);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="exeName">嵌入程序的名称</param>
		public CEmbeddedForm(string exeName)
		{
			InitializeComponent();
			this.Load += new EventHandler(this.CEmbeddedForm_Load);
			this.Text = exeName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="exePatch">嵌入程序的路径</param>
		/// <param name="exeName">嵌入程序的名称</param>
		public CEmbeddedForm(string exePatch, string exeName)
		{
			InitializeComponent();
			this.Load += new EventHandler(this.CEmbeddedForm_Load);
			this.Text = exeName;
			this.StartProcess(exePatch);
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
		/// 窗体初始化函数
		/// </summary>
		private void StartupInit()
		{
			//---事件注册
			this.FormClosing += new FormClosingEventHandler(this.CEmbeddedForm_FormClosing);
		}

		/// <summary>
		/// 启动应用程序的进程
		/// </summary>
		/// <param name="exePath"></param>
		public void StartProcess(string exePath)
		{
			this.defaultProcessPatch = exePath;
			this.cPanelEx_EmbeddedForm.EmbeddedProcess(exePath);
		}
		#endregion

		#region 事件函数
		/// <summary>
		/// 窗体加载事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CEmbeddedForm_Load(object sender, System.EventArgs e)
		{
			this.StartupInit();
		}
		/// <summary>
		/// 窗体关闭事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CEmbeddedForm_FormClosing(object sender, FormClosingEventArgs e)
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
							this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.CEmbeddedForm_FormClosing);
						}
						//---结束进程
						this.cPanelEx_EmbeddedForm.KillProcess();
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
		#endregion

	}
}
