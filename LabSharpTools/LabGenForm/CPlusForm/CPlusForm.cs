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
	public partial class CPlusForm : Form
	{
        #region 变量定义
        /// <summary>
        /// 
        /// </summary>
        private ToolTip defaultToolTip=null;
        #endregion

        #region 属性定义
        
		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public CPlusForm():base()
		{
			InitializeComponent();
            //---判断是否为空
            if (this.defaultToolTip==null)
            {
                this.defaultToolTip = new ToolTip();
            }         
        }

        #endregion

        #region 析构函数

        /// <summary>
        /// 析构函数
        /// </summary>
        ~CPlusForm()
        {
            this.Dispose(true);
        }

        #endregion

        #region 公共函数

        #endregion

        #region 保护函数
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (disposing)
        //    {
        //        this.defaultToolTip.Dispose();
        //    }
        //   this.defaultToolTip = null;
        //}
        #endregion

        #region 私有函数

        #endregion
    }
}
