using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备型号润滑项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账润滑项目")]
    public class EquipAccountLubricationProjectBase : DataEntity
    {
        #region 润滑项目 ProjectDetail
        /// <summary>
        /// 润滑项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<EquipAccountLubricationProjectBase>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

        /// <summary>
        /// 润滑项目Id
        /// </summary>
        public double ProjectDetailId
        {
            get { return (double)GetRefId(ProjectDetailIdProperty); }
            set { SetRefId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 润滑项目
        /// </summary>
        public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<EquipAccountLubricationProjectBase>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 润滑项目
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
        public static readonly IRefIdProperty DepartmentIdProperty = P<EquipAccountLubricationProjectBase>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<EquipAccountLubricationProjectBase>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 责任部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<EquipAccountLubricationProjectBase>.RegisterView(e => e.ProjectName, p => p.ProjectDetail.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion 

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<ProjectType> ProjectTypeProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.ProjectType);

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
        public static readonly Property<CycleType> CycleTypeProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion 


        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.Part);

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
        public static readonly Property<string> ConsumableProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.Consumable);

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
        public static readonly Property<string> MethodProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.Method);

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
        public static readonly Property<string> StandardProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.Standard);

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
        public static readonly Property<decimal?> MinValueProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.MaxValue);

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
        public static readonly Property<string> UnitProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.Unit);

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
        public static readonly Property<decimal?> UseTimeProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public decimal? UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 部门名称 DepartmentNameView
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DepartmentNameViewProperty = P<EquipAccountLubricationProjectBase>.RegisterView(e => e.DepartmentNameView, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentNameView
        {
            get { return this.GetProperty(DepartmentNameViewProperty); }
            set { SetProperty(DepartmentNameViewProperty, value); }
        }
        #endregion

        #region 周期 ProjectCycle
        /// <summary>
        /// 周期
        /// </summary>
        [Label("周期")]
        public static readonly Property<int?> ProjectCycleProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.ProjectCycle);

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
        public static readonly Property<int?> WarningPeriodProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.WarningPeriod);

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
        public static readonly Property<LubricatingType?> LubricatingTypeProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.LubricatingType);

        /// <summary>
        /// 润滑方式
        /// </summary>
        public LubricatingType? LubricatingType
        {
            get { return GetProperty(LubricatingTypeProperty); }
            set { SetProperty(LubricatingTypeProperty, value); }
        }
        #endregion

        #region 上次润滑日期 LastDate
        /// <summary>
        /// 上次润滑日期
        /// </summary>
        [Label("上次润滑日期")]
        public static readonly Property<DateTime?> LastDateProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.LastDate);

        /// <summary>
        /// 上次润滑日期
        /// </summary>
        public DateTime? LastDate
        {
            get { return GetProperty(LastDateProperty); }
            set { SetProperty(LastDateProperty, value); }
        }
        #endregion

        #region 下次润滑日期 NextDate
        /// <summary>
        /// 下次润滑日期
        /// </summary>
        [Label("下次润滑日期")]
        public static readonly Property<DateTime?> NextDateProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.NextDate);

        /// <summary>
        /// 下次润滑日期
        /// </summary>
        public DateTime? NextDate
        {
            get { return GetProperty(NextDateProperty); }
            set { SetProperty(NextDateProperty, value); }
        }
        #endregion

        #region 是否未提交 NotSubmit
        /// <summary>
        /// 是否未提交
        /// </summary>
        [Label("是否未提交")]
        public static readonly Property<bool?> NotSubmitProperty = P<EquipAccountLubricationProjectBase>.Register(e => e.NotSubmit);

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

    /// <summary>
    /// 设备台账校验项目 实体配置
    /// </summary>
    internal class EquipAccountLubricationProjectBaseConfig : EntityConfig<EquipAccountLubricationProjectBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ACCOUNT_LUBRICAT_PRJ").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
