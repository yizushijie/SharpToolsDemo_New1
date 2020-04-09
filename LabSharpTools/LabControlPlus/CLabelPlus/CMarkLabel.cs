using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabControlPlus
{
	#region 关键字

	/// <summary>
	/// 标记关键字
	/// </summary>
	public partial class CMarkKeyWord
	{
		/// <summary>
		/// 关键字-字符串
		/// </summary>
		public string KeyWordText
		{
			get;
			set;
		} = string.Empty;
		/// <summary>
		/// 关键字凸显颜色
		/// </summary>
		public Color KeyWordColor 
		{ 
			get;
			set;
		} = Color.Black;
	}
	#endregion


	#region 带关键字的标签

	/// <summary>
	/// 
	/// </summary>
	public partial class CMarkLabel : Label
	{
		#region 变量定义

		#endregion

		#region 属性定义
		/// <summary>
		/// 
		/// </summary>
		[Browsable(true), Category("自定义属性"), Description("关键字标记数组")]
		public CMarkKeyWord[] mMarkKeyWords 
		{ 
			get; 
			set; 
		} = new CMarkKeyWord[0];
		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public CMarkLabel():base()
		{
			
		}

		#endregion

		#region 公共函数

		#endregion

		#region 保护函数

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			TextRenderer.DrawText(e.Graphics, this.Text, this.Font, new Point(0, 0), this.ForeColor);
			if (this.Text.Length <= 0)
			{
				return;
			}
			//---依次将每一个关键字重新覆盖绘制一次, // 遍历访问每一个关键字
			for (int i = 0; i < mMarkKeyWords.Length; i++)
			{
				string keyWord = this.mMarkKeyWords[i].KeyWordText;
				Color keyColor = this.mMarkKeyWords[i].KeyWordColor;
				//---当前关键字的测量宽度
				int keyWidth = TextRenderer.MeasureText(keyWord, this.Font).Width;
				//---第一次出现该关键字的位置
				int IndexOffset = this.Text.IndexOf(keyWord); 
				while (IndexOffset >= 0)
				{
					//---该关键字前面的所有字符
					string StrFront = this.Text.Substring(0, IndexOffset);
					if (StrFront.Length > 0)
					{
						int strWidth = TextRenderer.MeasureText(StrFront + keyWord, this.Font).Width;
						//---计算好关键字的位置，然后重新覆盖绘制。
						TextRenderer.DrawText(e.Graphics, keyWord, Font, new Point((strWidth - keyWidth), 0), keyColor);
					}
					else
					{
						//---使用该关键字指定的颜色，重绘该关键字
						TextRenderer.DrawText(e.Graphics, keyWord, Font, new Point(0, 0), keyColor);
					}
					//---判断条件，然后退出该关键字循环
					IndexOffset += keyWord.Length;
					if (IndexOffset >= Text.Length)
					{
						break;
					}
					//---提取关键字后面的所有字符
					string StrBehind = Text.Substring(IndexOffset);
					if (StrBehind.Length <= 0)
					{
						break;
					}
					int index2 = StrBehind.IndexOf(keyWord);
					if (index2 < 0)
					{
						break;
					}
					//---当前关键字，下一个位置
					IndexOffset += index2; 
				}
			}
		}

		#endregion

		#region 私有函数

		#endregion

		#region 事件函数

		#endregion

	}
	#endregion

}
