using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.RunStandards
{
	/// <summary>
	/// 维修标准
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("维修标准")]
	public partial class RunStandardProject : DataEntity
	{
		#region 部位 Part
		/// <summary>
		/// 部位
		/// </summary>
		[Label("部位")]
		public static readonly Property<string> PartProperty = P<RunStandardProject>.Register(e => e.Part);

		/// <summary>
		/// 部位
		/// </summary>
		public string Part
		{
			get { return GetProperty(PartProperty); }
			set { SetProperty(PartProperty, value); }
		}
		#endregion

		#region 项目耗材 Consumable
		/// <summary>
		/// 项目耗材
		/// </summary>
		[Label("项目耗材")]
		public static readonly Property<string> ConsumableProperty = P<RunStandardProject>.Register(e => e.Consumable);

		/// <summary>
		/// 项目耗材
		/// </summary>
		public string Consumable
		{
			get { return GetProperty(ConsumableProperty); }
			set { SetProperty(ConsumableProperty, value); }
		}
		#endregion

		#region 操作方法 Method
		/// <summary>
		/// 操作方法
		/// </summary>
		[Label("操作方法")]
		public static readonly Property<string> MethodProperty = P<RunStandardProject>.Register(e => e.Method);

		/// <summary>
		/// 操作方法
		/// </summary>
		public string Method
		{
			get { return GetProperty(MethodProperty); }
			set { SetProperty(MethodProperty, value); }
		}
		#endregion

		#region 标准 Standard
		/// <summary>
		/// 标准
		/// </summary>
		[Label("标准")]
		public static readonly Property<string> StandardProperty = P<RunStandardProject>.Register(e => e.Standard);

		/// <summary>
		/// 标准
		/// </summary>
		public string Standard
		{
			get { return GetProperty(StandardProperty); }
			set { SetProperty(StandardProperty, value); }
		}
		#endregion

		#region 最小值 MinValue
		/// <summary>
		/// 最小值
		/// </summary>
		[Label("最小值")]
		public static readonly Property<decimal?> MinValueProperty = P<RunStandardProject>.Register(e => e.MinValue);

		/// <summary>
		/// 最小值
		/// </summary>
		public decimal? MinValue
		{
			get { return GetProperty(MinValueProperty); }
			set { SetProperty(MinValueProperty, value); }
		}
		#endregion

		#region 最大值 MaxValue
		/// <summary>
		/// 最大值
		/// </summary>
		[Label("最大值")]
		public static readonly Property<decimal?> MaxValueProperty = P<RunStandardProject>.Register(e => e.MaxValue);

		/// <summary>
		/// 最大值
		/// </summary>
		public decimal? MaxValue
		{
			get { return GetProperty(MaxValueProperty); }
			set { SetProperty(MaxValueProperty, value); }
		}
		#endregion

		#region 单位 Unit
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<string> UnitProperty = P<RunStandardProject>.Register(e => e.Unit);

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit
		{
			get { return GetProperty(UnitProperty); }
			set { SetProperty(UnitProperty, value); }
		}
		#endregion

		#region 用时(分钟) UseTime
		/// <summary>
		/// 用时(分钟)
		/// </summary>
		[Label("用时(分钟)")]
		public static readonly Property<string> UseTimeProperty = P<RunStandardProject>.Register(e => e.UseTime);

		/// <summary>
		/// 用时(分钟)
		/// </summary>
		public string UseTime
		{
			get { return GetProperty(UseTimeProperty); }
			set { SetProperty(UseTimeProperty, value); }
		}
		#endregion

		#region 维修项目 ProjectDetail
		/// <summary>
		/// 维修项目Id
		/// </summary>
		[Label("维修项目")]
		public static readonly IRefIdProperty ProjectDetailIdProperty = P<RunStandardProject>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

		/// <summary>
		/// 维修项目Id
		/// </summary>
		public double ProjectDetailId
		{
			get { return (double)GetRefId(ProjectDetailIdProperty); }
			set { SetRefId(ProjectDetailIdProperty, value); }
		}

		/// <summary>
		/// 维修项目
		/// </summary>
		public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<RunStandardProject>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

		/// <summary>
		/// 维修项目
		/// </summary>
		public ProjectDetail ProjectDetail
		{
			get { return GetRefEntity(ProjectDetailProperty); }
			set { SetRefEntity(ProjectDetailProperty, value); }
		}
		#endregion

		#region 责任部门 Department
		/// <summary>
		/// 责任部门Id
		/// </summary>
		[Label("责任部门")]
		public static readonly IRefIdProperty DepartmentIdProperty = P<RunStandardProject>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 责任部门Id
		/// </summary>
		public double? DepartmentId
		{
			get { return (double?)GetRefNullableId(DepartmentIdProperty); }
			set { SetRefNullableId(DepartmentIdProperty, value); }
		}

		/// <summary>
		/// 责任部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<RunStandardProject>.RegisterRef(e => e.Department, DepartmentIdProperty);

		/// <summary>
		/// 责任部门
		/// </summary>
		public Enterprise Department
		{
			get { return GetRefEntity(DepartmentProperty); }
			set { SetRefEntity(DepartmentProperty, value); }
		}
		#endregion

		#region 设备运行定标 RunStandard
		/// <summary>
		/// 设备运行定标Id
		/// </summary>
		[Label("设备运行定标")]
		public static readonly IRefIdProperty RunStandardIdProperty = P<RunStandardProject>.RegisterRefId(e => e.RunStandardId, ReferenceType.Parent);

		/// <summary>
		/// 设备运行定标Id
		/// </summary>
		public double RunStandardId
		{
			get { return (double)GetRefId(RunStandardIdProperty); }
			set { SetRefId(RunStandardIdProperty, value); }
		}

		/// <summary>
		/// 设备运行定标
		/// </summary>
		public static readonly RefEntityProperty<RunStandard> RunStandardProperty = P<RunStandardProject>.RegisterRef(e => e.RunStandard, RunStandardIdProperty);

		/// <summary>
		/// 设备运行定标
		/// </summary>
		public RunStandard RunStandard
		{
			get { return GetRefEntity(RunStandardProperty); }
			set { SetRefEntity(RunStandardProperty, value); }
		}
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<RunStandardProject>.RegisterView(e => e.ProjectName, p => p.ProjectDetail.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

    }

	/// <summary>
	/// 维修标准 实体配置
	/// </summary>
	internal class RunStandardProjectConfig : EntityConfig<RunStandardProject>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_RUN_STD_PRJ").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}