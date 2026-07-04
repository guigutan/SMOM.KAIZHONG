using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.ProjectMaintains
{
    /// <summary>
    /// 项目维护
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("项目维护")]
	[DisplayMember(nameof(ProjectMaintain.Code))]
	public partial class ProjectMaintain : DataEntity, IStateEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<ProjectMaintain>.Register(e => e.Code);

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
		public static readonly Property<string> NameProperty = P<ProjectMaintain>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 描述 Desc
		/// <summary>
		/// 描述
		/// </summary>
		[Label("描述")]
		public static readonly Property<string> DescProperty = P<ProjectMaintain>.Register(e => e.Desc);

		/// <summary>
		/// 描述
		/// </summary>
		public string Desc
		{
			get { return GetProperty(DescProperty); }
			set { SetProperty(DescProperty, value); }
		}
		#endregion

		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<State> StateProperty = P<ProjectMaintain>.Register(e => e.State);
		/// <summary>
		/// 状态
		/// </summary>
		public State State
		{
			get { return this.GetProperty(StateProperty); }
			set { this.SetProperty(StateProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 项目维护 实体配置
	/// </summary>
	internal class ProjectMaintainConfig : EntityConfig<ProjectMaintain>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("PROJECT_MAINTAIN").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}