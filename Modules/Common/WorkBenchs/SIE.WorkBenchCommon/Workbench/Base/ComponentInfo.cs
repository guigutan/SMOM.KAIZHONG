using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
	/// <summary>
	/// 组件信息
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("组件信息")]
	public partial class ComponentInfo : DataEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<ComponentInfo>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 描述 Description
		/// <summary>
		/// 描述
		/// </summary>
		[Label("描述")]
		public static readonly Property<string> DescriptionProperty = P<ComponentInfo>.Register(e => e.Description);

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get { return GetProperty(DescriptionProperty); }
			set { SetProperty(DescriptionProperty, value); }
		}
		#endregion

		#region 组件内容 Content
		/// <summary>
		/// 组件内容
		/// </summary>
		[Label("组件内容")]
		public static readonly Property<string> ContentProperty = P<ComponentInfo>.Register(e => e.Content);

		/// <summary>
		/// 组件内容
		/// </summary>
		public string Content
		{
			get { return GetProperty(ContentProperty); }
			set { SetProperty(ContentProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 组件信息 实体配置
	/// </summary>
	internal class ComponentInfoConfig : EntityConfig<ComponentInfo>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("wb_component").MapAllProperties();
			Meta.Property(ComponentInfo.ContentProperty).MapColumn().HasLength("MAX");
			Meta.EnablePhantoms();
			Meta.DisableInvOrg();
		}
	}

}
