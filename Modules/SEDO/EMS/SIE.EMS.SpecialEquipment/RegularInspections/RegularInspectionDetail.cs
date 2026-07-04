using SIE.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 检验明细
    /// </summary>
    [ChildEntity, Serializable]    
    [Label("检验明细")]
    public partial class RegularInspectionDetail : DataEntity
    {
        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<RegularInspectionDetail>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<RegularInspectionDetail>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<RegularInspectionDetail>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 定检检验数据列表 RegularInspectionValueList
        /// <summary>
        /// 定检检验数据列表
        /// </summary>
        public static readonly ListProperty<EntityList<RegularInspectionValue>> RegularInspectionValueListProperty = P<RegularInspectionDetail>.RegisterList(e => e.RegularInspectionValueList);
        /// <summary>
        /// 定检检验数据列表
        /// </summary>
        public EntityList<RegularInspectionValue> RegularInspectionValueList
        {
            get { return this.GetLazyList(RegularInspectionValueListProperty); }
        }
        #endregion

        #region 点检保养项目 ProjectDetail
        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<RegularInspectionDetail>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId
        {
            get { return (double)GetRefId(ProjectDetailIdProperty); }
            set { SetRefId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<RegularInspectionDetail>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public ProjectDetail ProjectDetail
        {
            get { return GetRefEntity(ProjectDetailProperty); }
            set { SetRefEntity(ProjectDetailProperty, value); }
        }
        #endregion

        #region 特种设备定检 RegularInspection
        /// <summary>
        /// 特种设备定检Id
        /// </summary>
        public static readonly IRefIdProperty RegularInspectionIdProperty = P<RegularInspectionDetail>.RegisterRefId(e => e.RegularInspectionId, ReferenceType.Parent);

        /// <summary>
        /// 特种设备定检Id
        /// </summary>
        public double RegularInspectionId
        {
            get { return (double)GetRefId(RegularInspectionIdProperty); }
            set { SetRefId(RegularInspectionIdProperty, value); }
        }

        /// <summary>
        /// 特种设备定检
        /// </summary>
        public static readonly RefEntityProperty<RegularInspection> RegularInspectionProperty = P<RegularInspectionDetail>.RegisterRef(e => e.RegularInspection, RegularInspectionIdProperty);

        /// <summary>
        /// 特种设备定检
        /// </summary>
        public RegularInspection RegularInspection
        {
            get { return GetRefEntity(RegularInspectionProperty); }
            set { SetRefEntity(RegularInspectionProperty, value); }
        }
        #endregion

        #region 检验人 Inspector
        /// <summary>
        /// 检验人Id
        /// </summary>
        public static readonly IRefIdProperty InspectorIdProperty = P<RegularInspectionDetail>.RegisterRefId(e => e.InspectorId, ReferenceType.Normal);

        /// <summary>
        /// 检验人Id
        /// </summary>
        public double? InspectorId
        {
            get { return (double?)GetRefNullableId(InspectorIdProperty); }
            set { SetRefNullableId(InspectorIdProperty, value); }
        }

        /// <summary>
        /// 检验人
        /// </summary>
        public static readonly RefEntityProperty<Employee> InspectorProperty = P<RegularInspectionDetail>.RegisterRef(e => e.Inspector, InspectorIdProperty);

        /// <summary>
        /// 检验人
        /// </summary>
        public Employee Inspector
        {
            get { return GetRefEntity(InspectorProperty); }
            set { SetRefEntity(InspectorProperty, value); }
        }
        #endregion

        #region 检验时间 InspectionDateTime
        /// <summary>
        /// 检验时间
        /// </summary>
        [Label("检验时间")]
        public static readonly Property<DateTime?> InspectionDateTimeProperty = P<RegularInspectionDetail>.Register(e => e.InspectionDateTime);

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectionDateTime
        {
            get { return GetProperty(InspectionDateTimeProperty); }
            set { SetProperty(InspectionDateTimeProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<RegularInspectionDetail>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<RegularInspectionDetail>.Register(e => e.Part);

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
        public static readonly Property<string> ConsumableProperty = P<RegularInspectionDetail>.Register(e => e.Consumable);

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
        public static readonly Property<string> MethodProperty = P<RegularInspectionDetail>.Register(e => e.Method);

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method
        {
            get { return GetProperty(MethodProperty); }
            set { SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<RegularInspectionDetail>.Register(e => e.Unit);

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
        public static readonly Property<string> UseTimeProperty = P<RegularInspectionDetail>.Register(e => e.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public string UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CycleType> CycleTypeProperty = P<RegularInspectionDetail>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 标准 Standard
        /// <summary>
        /// 标准
        /// </summary>
        [Label("标准")]
        public static readonly Property<string> StandardProperty = P<RegularInspectionDetail>.Register(e => e.Standard);

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard
        {
            get { return GetProperty(StandardProperty); }
            set { SetProperty(StandardProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 检验明细 实体配置
    /// </summary>
    internal class RegularInspectionDetailConfig : EntityConfig<RegularInspectionDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_REG_INS_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}