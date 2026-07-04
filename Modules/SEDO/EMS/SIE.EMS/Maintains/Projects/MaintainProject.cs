using SIE.Domain;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Projects
{
    /// <summary>
    /// 保养项目
    /// </summary>
    [ChildEntity, Serializable]
    [Label("保养项目")]
    public partial class MaintainProject : DataEntity
    {
        #region 保养计划 MaintainPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty MaintainPlanIdProperty = P<MaintainProject>.RegisterRefId(e => e.MaintainPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double MaintainPlanId
        {
            get { return (double)GetRefId(MaintainPlanIdProperty); }
            set { SetRefId(MaintainPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainPlan> MaintainPlanProperty = P<MaintainProject>.RegisterRef(e => e.MaintainPlan, MaintainPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainPlan MaintainPlan
        {
            get { return GetRefEntity(MaintainPlanProperty); }
            set { SetRefEntity(MaintainPlanProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<MaintainProject>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty
            = P<MaintainProject>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备保养项目 EquipMaintainProject
        /// <summary>
        /// 设备保养项目Id
        /// </summary>
        [Label("设备保养项目")]
        public static readonly IRefIdProperty EquipMaintainProjectIdProperty =
            P<MaintainProject>.RegisterRefId(e => e.EquipMaintainProjectId, ReferenceType.Normal);

        /// <summary>
        /// 设备保养项目Id
        /// </summary>
        public double? EquipMaintainProjectId
        {
            get { return (double?)this.GetRefNullableId(EquipMaintainProjectIdProperty); }
            set { this.SetRefNullableId(EquipMaintainProjectIdProperty, value); }
        }

        /// <summary>
        /// 设备保养项目
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountMaintainProject> EquipMaintainProjectProperty =
            P<MaintainProject>.RegisterRef(e => e.EquipMaintainProject, EquipMaintainProjectIdProperty);

        /// <summary>
        /// 设备保养项目
        /// </summary>
        public EquipAccountMaintainProject EquipMaintainProject
        {
            get { return this.GetRefEntity(EquipMaintainProjectProperty); }
            set { this.SetRefEntity(EquipMaintainProjectProperty, value); }
        }
        #endregion

        #region 保养状态 ExeState
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintExeState> ExeStateProperty = P<MaintainProject>.Register(e => e.ExeState);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintExeState ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 测量值 Measure
        /// <summary>
        /// 测量值
        /// </summary>
        [Label("测量值")]
        public static readonly Property<decimal?> MeasureProperty = P<MaintainProject>.Register(e => e.Measure);

        /// <summary>
        /// 测量值
        /// </summary>
        public decimal? Measure
        {
            get { return GetProperty(MeasureProperty); }
            set { SetProperty(MeasureProperty, value); }
        }
        #endregion

        #region 保养结果 MaintainResult
        /// <summary>
        /// 保养结果
        /// </summary>
        [Label("保养结果")]
        public static readonly Property<CheckMaintainResult?> MaintainResultProperty = P<MaintainProject>.Register(e => e.MaintainResult);

        /// <summary>
        /// 保养结果
        /// </summary>
        public CheckMaintainResult? MaintainResult
        {
            get { return GetProperty(MaintainResultProperty); }
            set { SetProperty(MaintainResultProperty, value); }
        }
        #endregion

        #region 缺陷描述 Defect
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectProperty = P<MaintainProject>.Register(e => e.Defect);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string Defect
        {
            get { return GetProperty(DefectProperty); }
            set { SetProperty(DefectProperty, value); }
        }
        #endregion

        #region 实际值 ActualValue
        /// <summary>
        /// 实际值
        /// </summary>
        [Label("实际值")]
        public static readonly Property<decimal?> ActualValueProperty = P<MaintainProject>.Register(e => e.ActualValue);

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue
        {
            get { return GetProperty(ActualValueProperty); }
            set { SetProperty(ActualValueProperty, value); }
        }
        #endregion

        #region 图片 ProjectPhoto
        /// <summary>
        /// 图片Id
        /// </summary>
        [Label("图片")]
        public static readonly IRefIdProperty ProjectPhotoIdProperty =
            P<MaintainProject>.RegisterRefId(e => e.ProjectPhotoId, ReferenceType.Normal);

        /// <summary>
        /// 图片Id
        /// </summary>
        public double? ProjectPhotoId
        {
            get { return (double?)this.GetRefNullableId(ProjectPhotoIdProperty); }
            set { this.SetRefNullableId(ProjectPhotoIdProperty, value); }
        }

        /// <summary>
        /// 图片
        /// </summary>
        public static readonly RefEntityProperty<ProjectPhoto> ProjectPhotoProperty =
            P<MaintainProject>.RegisterRef(e => e.ProjectPhoto, ProjectPhotoIdProperty);

        /// <summary>
        /// 图片
        /// </summary>
        public ProjectPhoto ProjectPhoto
        {
            get { return this.GetRefEntity(ProjectPhotoProperty); }
            set { this.SetRefEntity(ProjectPhotoProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MaintainProject>.Register(e => e.Remark);

        /// <summary>
        /// 点检状态
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<ProjectType> ProjectTypeProperty = P<MaintainProject>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<MaintainProject>.Register(e => e.ProjectName);

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
        public static readonly Property<CycleType> CycleTypeProperty = P<MaintainProject>.Register(e => e.CycleType);

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
        public static readonly Property<string> PartProperty = P<MaintainProject>.Register(e => e.Part);

        /// <summary>
        /// 部位
        /// </summary>
        public string Part
        {
            get { return GetProperty(PartProperty); }
            set { SetProperty(PartProperty, value); }
        }
        #endregion

        #region 项目耗材 ProjectConsumable
        /// <summary>
        /// 项目耗材
        /// </summary>
        [Label("项目耗材")]
        public static readonly Property<string> ProjectConsumableProperty = P<MaintainProject>.Register(e => e.ProjectConsumable);

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string ProjectConsumable
        {
            get { return GetProperty(ProjectConsumableProperty); }
            set { SetProperty(ProjectConsumableProperty, value); }
        }
        #endregion

        #region 操作方法 Method
        /// <summary>
        /// 操作方法
        /// </summary>
        [Label("操作方法")]
        public static readonly Property<string> MethodProperty = P<MaintainProject>.Register(e => e.Method);

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
        public static readonly Property<string> StandardProperty = P<MaintainProject>.Register(e => e.Standard);

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
        public static readonly Property<decimal?> MinValueProperty = P<MaintainProject>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<MaintainProject>.Register(e => e.MaxValue);

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
        public static readonly Property<string> UnitProperty = P<MaintainProject>.Register(e => e.Unit);

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
        public static readonly Property<decimal?> UseTimeProperty = P<MaintainProject>.Register(e => e.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public decimal? UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 视图属性 
        #region 设备编码 AccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> AccountCodeProperty = P<MaintainProject>.RegisterView(e => e.AccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string AccountCode
        {
            get { return GetProperty(AccountCodeProperty); }
        }
        #endregion

        #region 设备名称 AccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> AccountNameProperty = P<MaintainProject>.RegisterView(e => e.AccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AccountName
        {
            get { return GetProperty(AccountNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 保养项目 实体配置
    /// </summary>
    internal class MaintainProjectConfig : EntityConfig<MaintainProject>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_PROJECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}