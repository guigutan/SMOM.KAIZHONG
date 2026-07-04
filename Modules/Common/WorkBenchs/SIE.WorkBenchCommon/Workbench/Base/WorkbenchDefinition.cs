using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
	/// <summary>
	/// 工作台定义
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("工作台定义")]
	public partial class WorkbenchDefinition : DataEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<WorkbenchDefinition>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 名称 Name
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<WorkbenchDefinition>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 描述 Description
		/// <summary>
		/// 描述
		/// </summary>
		[Label("描述")]
		public static readonly Property<string> DescriptionProperty = P<WorkbenchDefinition>.Register(e => e.Description);

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get { return GetProperty(DescriptionProperty); }
			set { SetProperty(DescriptionProperty, value); }
		}
		#endregion

		#region 布局编码 LayoutCode
		/// <summary>
		/// 布局编码
		/// </summary>
		[Required]
		[Label("布局编码")]
		public static readonly Property<string> LayoutCodeProperty = P<WorkbenchDefinition>.Register(e => e.LayoutCode);

		/// <summary>
		/// 布局编码
		/// </summary>
		public string LayoutCode
		{
			get { return GetProperty(LayoutCodeProperty); }
			set { SetProperty(LayoutCodeProperty, value); }
		}
		#endregion

		#region 组件配置内容 ComponentContent
		/// <summary>
		/// 组件配置内容
		/// </summary>
		[Required]
		[Label("组件配置内容")]
		public static readonly Property<string> ComponentContentProperty = P<WorkbenchDefinition>.Register(e => e.ComponentContent);

		/// <summary>
		/// 组件配置内容
		/// </summary>
		public string ComponentContent
		{
			get { return GetProperty(ComponentContentProperty); }
			set { SetProperty(ComponentContentProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工作台定义 实体配置
	/// </summary>
	internal class WorkbenchDefinitionConfig : EntityConfig<WorkbenchDefinition>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("wb_workbench").MapAllProperties();
			Meta.Property(WorkbenchDefinition.ComponentContentProperty).MapColumn().HasLength("MAX");
			Meta.EnablePhantoms();
			Meta.DisableInvOrg();
		}
	}

}
