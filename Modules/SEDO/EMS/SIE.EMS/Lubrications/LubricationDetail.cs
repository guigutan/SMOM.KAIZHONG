using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑项目
    /// </summary>
    [ChildEntity, Serializable]    
    [Label("润滑项目")]
    public partial class LubricationDetail : DataEntity
    {
        #region 周期 ProjectCycle
        /// <summary>
        /// 周期
        /// </summary>
        [Label("周期")]
        public static readonly Property<int> ProjectCycleProperty = P<LubricationDetail>.Register(e => e.ProjectCycle);

        /// <summary>
        /// 周期
        /// </summary>
        public int ProjectCycle
        {
            get { return GetProperty(ProjectCycleProperty); }
            set { SetProperty(ProjectCycleProperty, value); }
        }
        #endregion

        #region 预警期 WarningPeriod
        /// <summary>
        /// 预警期
        /// </summary>
        [Label("预警期")]
        public static readonly Property<string> WarningPeriodProperty = P<LubricationDetail>.Register(e => e.WarningPeriod);

        /// <summary>
        /// 预警期
        /// </summary>
        public string WarningPeriod
        {
            get { return GetProperty(WarningPeriodProperty); }
            set { SetProperty(WarningPeriodProperty, value); }
        }
        #endregion

        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<LubricationDetail>.Register(e => e.Part);

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
        public static readonly Property<string> ConsumableProperty = P<LubricationDetail>.Register(e => e.Consumable);

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
        public static readonly Property<string> MethodProperty = P<LubricationDetail>.Register(e => e.Method);

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
        public static readonly Property<string> StandardProperty = P<LubricationDetail>.Register(e => e.Standard);

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard
        {
            get { return GetProperty(StandardProperty); }
            set { SetProperty(StandardProperty, value); }
        }
        #endregion

        #region 加油量下限 MinValue
        /// <summary>
        /// 加油量下限
        /// </summary>
        [Label("加油量下限")]
        public static readonly Property<decimal?> MinValueProperty = P<LubricationDetail>.Register(e => e.MinValue);

        /// <summary>
        /// 加油量下限
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 加油量上限 MaxValue
        /// <summary>
        /// 加油量上限
        /// </summary>
        [Label("加油量上限")]
        public static readonly Property<decimal?> MaxValueProperty = P<LubricationDetail>.Register(e => e.MaxValue);

        /// <summary>
        /// 加油量上限
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 上次执行日期 LastDate
        /// <summary>
        /// 上次执行日期
        /// </summary>
        [Label("上次执行日期")]
        public static readonly Property<DateTime?> LastDateProperty = P<LubricationDetail>.Register(e => e.LastDate);

        /// <summary>
        /// 上次执行日期
        /// </summary>
        public DateTime? LastDate
        {
            get { return GetProperty(LastDateProperty); }
            set { SetProperty(LastDateProperty, value); }
        }
        #endregion

        #region 下次执行日期 NextDate
        /// <summary>
        /// 下次执行日期
        /// </summary>
        [Label("下次执行日期")]
        public static readonly Property<DateTime?> NextDateProperty = P<LubricationDetail>.Register(e => e.NextDate);

        /// <summary>
        /// 下次执行日期
        /// </summary>
        public DateTime? NextDate
        {
            get { return GetProperty(NextDateProperty); }
            set { SetProperty(NextDateProperty, value); }
        }
        #endregion

        #region 加油量 ActualValue
        /// <summary>
        /// 加油量
        /// </summary>
        [Label("加油量")]
        public static readonly Property<decimal?> ActualValueProperty = P<LubricationDetail>.Register(e => e.ActualValue);

        /// <summary>
        /// 加油量
        /// </summary>
        public decimal? ActualValue
        {
            get { return GetProperty(ActualValueProperty); }
            set { SetProperty(ActualValueProperty, value); }
        }
        #endregion

        #region 延期天数 DelayDays
        /// <summary>
        /// 延期天数
        /// </summary>
        [Label("延期天数")]
        public static readonly Property<int?> DelayDaysProperty = P<LubricationDetail>.Register(e => e.DelayDays);

        /// <summary>
        /// 延期天数
        /// </summary>
        public int? DelayDays
        {
            get { return GetProperty(DelayDaysProperty); }
            set { SetProperty(DelayDaysProperty, value); }
        }
        #endregion

        #region 润滑方式 LubricatingType
        /// <summary>
        /// 润滑方式
        /// </summary>
        [Label("润滑方式")]
        public static readonly Property<LubricatingType> LubricatingTypeProperty = P<LubricationDetail>.Register(e => e.LubricatingType);

        /// <summary>
        /// 润滑方式
        /// </summary>
        public LubricatingType LubricatingType
        {
            get { return GetProperty(LubricatingTypeProperty); }
            set { SetProperty(LubricatingTypeProperty, value); }
        }
        #endregion

        #region 项目 ProjectDetail
        /// <summary>
        /// 项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<LubricationDetail>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProjectDetailId
        {
            get { return (double)GetRefId(ProjectDetailIdProperty); }
            set { SetRefId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountLubricationProject> ProjectDetailProperty = P<LubricationDetail>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public EquipAccountLubricationProject ProjectDetail
        {
            get { return GetRefEntity(ProjectDetailProperty); }
            set { SetRefEntity(ProjectDetailProperty, value); }
        }
        #endregion

        #region 润滑项目 Lubrication
        /// <summary>
        /// 润滑项目Id
        /// </summary>
        public static readonly IRefIdProperty LubricationIdProperty = P<LubricationDetail>.RegisterRefId(e => e.LubricationId, ReferenceType.Parent);

        /// <summary>
        /// 润滑项目Id
        /// </summary>
        public double LubricationId
        {
            get { return (double)GetRefId(LubricationIdProperty); }
            set { SetRefId(LubricationIdProperty, value); }
        }

        /// <summary>
        /// 润滑项目
        /// </summary>
        public static readonly RefEntityProperty<Lubrication> LubricationProperty = P<LubricationDetail>.RegisterRef(e => e.Lubrication, LubricationIdProperty);

        /// <summary>
        /// 润滑项目
        /// </summary>
        public Lubrication Lubrication
        {
            get { return GetRefEntity(LubricationProperty); }
            set { SetRefEntity(LubricationProperty, value); }
        }
        #endregion
        
        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<LubricationDetail>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CycleType> CycleTypeProperty = P<LubricationDetail>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<LubricationDetail>.Register(e => e.Unit);

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
        public static readonly Property<decimal?> UseTimeProperty = P<LubricationDetail>.Register(e => e.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public decimal? UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 润滑项目 实体配置
    /// </summary>
    internal class LubricationDetailConfig : EntityConfig<LubricationDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_LUBR_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}