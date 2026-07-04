using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Lubrications.ViewModels
{
    /// <summary>
    /// 检验规程的检验项目的点检保养项目ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SelLubricationDetailCriteria))]
    public class SelLubricationDetailViewModel : ViewModel
    {
        #region 项目id ProjectDetailId
        /// <summary>
        /// 项目id
        /// </summary>
        [Label("项目id")]
        public static readonly Property<double> ProjectDetailIdProperty = P<SelLubricationDetailViewModel>.Register(e => e.ProjectDetailId);

        /// <summary>
        /// 项目id
        /// </summary>
        public double ProjectDetailId
        {
            get { return GetProperty(ProjectDetailIdProperty); }
            set { SetProperty(ProjectDetailIdProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<SelLubricationDetailViewModel>.Register(e => e.ProjectName);

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
        public static readonly Property<string> PartProperty = P<SelLubricationDetailViewModel>.Register(e => e.Part);

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
        public static readonly Property<string> ConsumableProperty = P<SelLubricationDetailViewModel>.Register(e => e.Consumable);

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
        public static readonly Property<string> MethodProperty = P<SelLubricationDetailViewModel>.Register(e => e.Method);

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
        public static readonly Property<string> StandardProperty = P<SelLubricationDetailViewModel>.Register(e => e.Standard);

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
        public static readonly Property<decimal?> MinValueProperty = P<SelLubricationDetailViewModel>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<SelLubricationDetailViewModel>.Register(e => e.MaxValue);

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
        public static readonly Property<string> UnitProperty = P<SelLubricationDetailViewModel>.Register(e => e.Unit);

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
        public static readonly Property<decimal?> UseTimeProperty = P<SelLubricationDetailViewModel>.Register(e => e.UseTime);

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
        [Label("项目类型")]
        public static readonly Property<ProjectType> ProjectTypeProperty = P<SelLubricationDetailViewModel>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>        
        [Label("周期类型")]
        public static readonly Property<CycleType?> CycleTypeProperty = P<SelLubricationDetailViewModel>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType? CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 周期 ProjectCycle
        /// <summary>
        /// 周期
        /// </summary>
        [Label("周期")]
        public static readonly Property<int?> ProjectCycleProperty = P<SelLubricationDetailViewModel>.Register(e => e.ProjectCycle);

        /// <summary>
        /// 周期
        /// </summary>
        public int? ProjectCycle
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
        public static readonly Property<int?> WarningPeriodProperty = P<SelLubricationDetailViewModel>.Register(e => e.WarningPeriod);

        /// <summary>
        /// 预警期
        /// </summary>
        public int? WarningPeriod
        {
            get { return GetProperty(WarningPeriodProperty); }
            set { SetProperty(WarningPeriodProperty, value); }
        }
        #endregion

        #region 润滑方式 LubricatingType
        /// <summary>
        /// 润滑方式
        /// </summary>
        [Label("润滑方式")]
        public static readonly Property<LubricatingType?> LubricatingTypeProperty = P<SelLubricationDetailViewModel>.Register(e => e.LubricatingType);

        /// <summary>
        /// 润滑方式
        /// </summary>
        public LubricatingType? LubricatingType
        {
            get { return GetProperty(LubricatingTypeProperty); }
            set { SetProperty(LubricatingTypeProperty, value); }
        }
        #endregion

        #region 是否未提交 NotSubmit
        /// <summary>
        /// 是否未提交
        /// </summary>
        [Label("是否未提交")]
        public static readonly Property<bool?> NotSubmitProperty = P<SelLubricationDetailViewModel>.Register(e => e.NotSubmit);

        /// <summary>
        /// 是否未提交
        /// </summary>
        public bool? NotSubmit
        {
            get { return GetProperty(NotSubmitProperty); }
            set { SetProperty(NotSubmitProperty, value); }
        }
        #endregion
    }
}
