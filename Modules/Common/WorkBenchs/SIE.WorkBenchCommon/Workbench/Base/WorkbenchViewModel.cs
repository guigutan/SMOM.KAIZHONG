using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
	/// <summary>
	/// 添加工作台ViewModel
	/// </summary>
	[RootEntity, Serializable]
	[DisplayMember(nameof(WorkbenchViewModel.Name))]
	public class WorkbenchViewModel: ViewModel
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<WorkbenchViewModel>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return this.GetProperty(CodeProperty); }
			set { this.SetProperty(CodeProperty, value); }
		}
		#endregion


		#region 名称 Name
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<WorkbenchViewModel>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return this.GetProperty(NameProperty); }
			set { this.SetProperty(NameProperty, value); }
		}
		#endregion

		#region 描述 Description
		/// <summary>
		/// 描述
		/// </summary>
		[Label("描述")]
		public static readonly Property<string> DescriptionProperty = P<WorkbenchViewModel>.Register(e => e.Description);

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get { return this.GetProperty(DescriptionProperty); }
			set { this.SetProperty(DescriptionProperty, value); }
		}
		#endregion

		#region 布局 LayoutCode
		/// <summary>
		/// 布局
		/// </summary>
		[Required]
		[Label("布局")]
		public static readonly Property<string> LayoutCodeProperty = P<WorkbenchViewModel>.Register(e => e.LayoutCode);

		/// <summary>
		/// 布局
		/// </summary>
		public string LayoutCode
		{
			get { return this.GetProperty(LayoutCodeProperty); }
			set { this.SetProperty(LayoutCodeProperty, value); }
		}
		#endregion

		#region Token Token
		/// <summary>
		/// Token
		/// </summary>
		[Label("Token")]
		public static readonly Property<string> TokenProperty = P<WorkbenchViewModel>.Register(e => e.Token);

		/// <summary>
		/// Token
		/// </summary>
		public string Token
		{
			get { return this.GetProperty(TokenProperty); }
			set { this.SetProperty(TokenProperty, value); }
		}
		#endregion

		#region 组件内容 ComponentContent
		/// <summary>
		/// 组件内容
		/// </summary>
		[Label("组件内容")]
		public static readonly Property<string> ComponentContentProperty = P<WorkbenchViewModel>.Register(e => e.ComponentContent);

		/// <summary>
		/// 组件内容
		/// </summary>
		public string ComponentContent
		{
			get { return this.GetProperty(ComponentContentProperty); }
			set { this.SetProperty(ComponentContentProperty, value); }
		}
		#endregion

		#region 是否新数据 IsNewData
		/// <summary>
		/// 是否新数据
		/// </summary>
		[Label("是否新数据")]
		public static readonly Property<bool> IsNewDataProperty = P<WorkbenchViewModel>.Register(e => e.IsNewData);

		/// <summary>
		/// 是否新数据
		/// </summary>
		public bool IsNewData
		{
			get { return this.GetProperty(IsNewDataProperty); }
			set { this.SetProperty(IsNewDataProperty, value); }
		}
		#endregion

		#region 组件脚本 ComponentScript
		/// <summary>
		/// 组件脚本
		/// </summary>
		[Label("组件脚本")]
		public static readonly Property<string> ComponentScriptProperty = P<WorkbenchViewModel>.Register(e => e.ComponentScript);

		/// <summary>
		/// 组件脚本
		/// </summary>
		public string ComponentScript
		{
			get { return this.GetProperty(ComponentScriptProperty); }
			set { this.SetProperty(ComponentScriptProperty, value); }
		}
		#endregion

	
	}
}
