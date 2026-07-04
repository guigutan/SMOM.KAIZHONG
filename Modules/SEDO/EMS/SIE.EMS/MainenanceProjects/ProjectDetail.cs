using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Common.Entity;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
	/// 点检保养项目
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(ProjectDetailCriteria))]
    [Label("点检保养项目")]
    [DisplayMember(nameof(Name))]
    public partial class ProjectDetail : DataEntity
    {
        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        [MaxLength(26)]
        [Label("项目名称")]
        public static readonly Property<string> NameProperty = P<ProjectDetail>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<ProjectDetail>.Register(e => e.Part);

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
        public static readonly Property<string> ConsumableProperty = P<ProjectDetail>.Register(e => e.Consumable);

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
        public static readonly Property<string> MethodProperty = P<ProjectDetail>.Register(e => e.Method);

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
        public static readonly Property<string> StandardProperty = P<ProjectDetail>.Register(e => e.Standard);

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
        public static readonly Property<decimal?> MinValueProperty = P<ProjectDetail>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<ProjectDetail>.Register(e => e.MaxValue);

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
        public static readonly Property<string> UnitProperty = P<ProjectDetail>.Register(e => e.Unit);

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
        public static readonly Property<decimal?> UseTimeProperty = P<ProjectDetail>.Register(e => e.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public decimal? UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Required]
        [Label("项目类型")]
        public static readonly Property<ProjectType> ProjectTypeProperty = P<ProjectDetail>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 备件清单列表 SparePartItemList
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartItem>> SparePartItemListProperty = P<ProjectDetail>.RegisterList(e => e.SparePartItemList);
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public EntityList<SparePartItem> SparePartItemList
        {
            get { return this.GetLazyList(SparePartItemListProperty); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>        
        [Label("周期类型")]
        public static readonly Property<CycleType?> CycleTypeProperty = P<ProjectDetail>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType? CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 周期类型 CycleTypeInfo
        /// <summary>
        /// 周期类型Id
        /// </summary>
        [Label("周期类型")]
        public static readonly IRefIdProperty CycleTypeInfoIdProperty =
            P<ProjectDetail>.RegisterRefId(e => e.CycleTypeInfoId, ReferenceType.Normal);

        /// <summary>
        /// 周期类型Id
        /// </summary>
        public string CycleTypeInfoId
        {
            get { return (string)this.GetRefNullableId(CycleTypeInfoIdProperty); }
            set { this.SetRefNullableId(CycleTypeInfoIdProperty, value); }
        }

        /// <summary>
        /// 周期类型
        /// </summary>
        public static readonly RefEntityProperty<CycleTypeInfo> CycleTypeInfoProperty =
            P<ProjectDetail>.RegisterRef(e => e.CycleTypeInfo, CycleTypeInfoIdProperty);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleTypeInfo CycleTypeInfo
        {
            get { return this.GetRefEntity(CycleTypeInfoProperty); }
            set { this.SetRefEntity(CycleTypeInfoProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 点检保养项目 实体配置
    /// </summary>
    internal class ProjectDetailConfig : EntityConfig<ProjectDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PROJECT_DTL").MapAllPropertiesExcept(ProjectDetail.CycleTypeInfoIdProperty);

            Meta.EnablePhantoms();
        }
    }
}
