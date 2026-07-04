using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Projects
{
    /// <summary>
    /// 点检项目
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("点检项目")]
    public partial class CheckProject : DataEntity
    {
        #region 点检计划 CheckPlan
        /// <summary>
        /// 点检计划Id
        /// </summary>
        [Label("点检计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty = P<CheckProject>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Parent);

        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)GetRefId(CheckPlanIdProperty); }
            set { SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 点检计划
        /// </summary>
        public static readonly RefEntityProperty<CheckPlan> CheckPlanProperty = P<CheckProject>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 点检计划
        /// </summary>
        public CheckPlan CheckPlan
        {
            get { return GetRefEntity(CheckPlanProperty); }
            set { SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<CheckProject>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 固定资产Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 固定资产
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<CheckProject>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 固定资产
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备点检项目 EquipCheckProject
        /// <summary>
        /// 设备点检项目Id
        /// </summary>
        [Label("设备点检项目")]
        public static readonly IRefIdProperty EquipCheckProjectIdProperty =
            P<CheckProject>.RegisterRefId(e => e.EquipCheckProjectId, ReferenceType.Normal);

        /// <summary>
        /// 设备点检项目Id
        /// </summary>
        public double? EquipCheckProjectId
        {
            get { return (double?)this.GetRefNullableId(EquipCheckProjectIdProperty); }
            set { this.SetRefNullableId(EquipCheckProjectIdProperty, value); }
        }

        /// <summary>
        /// 设备点检项目
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountCheckProject> EquipCheckProjectProperty =
            P<CheckProject>.RegisterRef(e => e.EquipCheckProject, EquipCheckProjectIdProperty);

        /// <summary>
        /// 设备点检项目
        /// </summary>
        public EquipAccountCheckProject EquipCheckProject
        {
            get { return this.GetRefEntity(EquipCheckProjectProperty); }
            set { this.SetRefEntity(EquipCheckProjectProperty, value); }
        }
        #endregion

        #region 设备物联参数 EquipPhysicalUnion
        /// <summary>
        /// 设备物联参数Id
        /// </summary>
        [Label("设备物联参数")]
        public static readonly IRefIdProperty EquipPhysicalUnionIdProperty =
            P<CheckProject>.RegisterRefId(e => e.EquipPhysicalUnionId, ReferenceType.Normal);

        /// <summary>
        /// 设备物联参数Id
        /// </summary>
        public double? EquipPhysicalUnionId
        {
            get { return (double?)this.GetRefNullableId(EquipPhysicalUnionIdProperty); }
            set { this.SetRefNullableId(EquipPhysicalUnionIdProperty, value); }
        }

        /// <summary>
        /// 设备物联参数
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountPhysicalUnion> EquipPhysicalUnionProperty =
            P<CheckProject>.RegisterRef(e => e.EquipPhysicalUnion, EquipPhysicalUnionIdProperty);

        /// <summary>
        /// 设备物联参数
        /// </summary>
        public EquipAccountPhysicalUnion EquipPhysicalUnion
        {
            get { return this.GetRefEntity(EquipPhysicalUnionProperty); }
            set { this.SetRefEntity(EquipPhysicalUnionProperty, value); }
        }
        #endregion

        #region 参数编码 ParaCode
        /// <summary>
        /// 参数编码
        /// </summary>
        [Label("参数编码")]
        public static readonly Property<string> ParaCodeProperty = P<CheckProject>.Register(e => e.ParaCode);

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParaCode
        {
            get { return this.GetProperty(ParaCodeProperty); }
            set { this.SetProperty(ParaCodeProperty, value); }
        }
        #endregion

        #region 参数名称 ParaName
        /// <summary>
        /// 参数名称
        /// </summary>
        [Label("参数名称")]
        public static readonly Property<string> ParaNameProperty = P<CheckProject>.Register(e => e.ParaName);

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParaName
        {
            get { return this.GetProperty(ParaNameProperty); }
            set { this.SetProperty(ParaNameProperty, value); }
        }
        #endregion

        #region 设备参数 EquipParamSource
        /// <summary>
        /// 设备参数
        /// </summary>
        [Label("设备参数")]
        public static readonly Property<EquipParamSource> EquipParamSourceProperty = P<CheckProject>.Register(e => e.EquipParamSource);

        /// <summary>
        /// 设备参数
        /// </summary>
        public EquipParamSource EquipParamSource
        {
            get { return this.GetProperty(EquipParamSourceProperty); }
            set { this.SetProperty(EquipParamSourceProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [MaxLength(26)]
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<CheckProject>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 项目分类 Category
        /// <summary>
        /// 项目分类
        /// </summary>
        [Label("项目分类")]
        public static readonly Property<string> CategoryProperty = P<CheckProject>.Register(e => e.Category);
        /// <summary>
        /// 项目分类
        /// </summary>
        public string Category
        {
            get { return GetProperty(CategoryProperty); }
            set { SetProperty(CategoryProperty, value); }
        }
        #endregion
        
        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<CheckProject>.Register(e => e.Part);

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
        public static readonly Property<string> ProjectConsumableProperty = P<CheckProject>.Register(e => e.ProjectConsumable);

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
        public static readonly Property<string> MethodProperty = P<CheckProject>.Register(e => e.Method);

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
        public static readonly Property<string> StandardProperty = P<CheckProject>.Register(e => e.Standard);

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
        public static readonly Property<decimal?> MinValueProperty = P<CheckProject>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<CheckProject>.Register(e => e.MaxValue);

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
        public static readonly Property<string> UnitProperty = P<CheckProject>.Register(e => e.Unit);

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
        public static readonly Property<decimal?> UseTimeProperty = P<CheckProject>.Register(e => e.UseTime);

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
        public static readonly Property<ProjectType> ProjectTypeProperty = P<CheckProject>.Register(e => e.ProjectType);

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
        [Required]
        [Label("周期类型")]
        public static readonly Property<CycleType> CycleTypeProperty = P<CheckProject>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 点检状态 ExeState
        /// <summary>
        /// 点检状态
        /// </summary>
        [Label("点检状态")]
        public static readonly Property<CheckExeState> ExeStateProperty = P<CheckProject>.Register(e => e.ExeState);

        /// <summary>
        /// 点检状态
        /// </summary>
        public CheckExeState ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 点检结果 CheckResult
        /// <summary>
        /// 点检结果
        /// </summary>
        [Label("点检结果")]
        public static readonly Property<CheckMaintainResult?> CheckResultProperty = P<CheckProject>.Register(e => e.CheckResult);

        /// <summary>
        /// 点检结果
        /// </summary>
        public CheckMaintainResult? CheckResult
        {
            get { return GetProperty(CheckResultProperty); }
            set { SetProperty(CheckResultProperty, value); }
        }
        #endregion

        #region 实际值 ActualValue
        /// <summary>
        /// 实际值
        /// </summary>
        [Label("实际值")]
        public static readonly Property<decimal?> ActualValueProperty = P<CheckProject>.Register(e => e.ActualValue);

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue
        {
            get { return GetProperty(ActualValueProperty); }
            set { SetProperty(ActualValueProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<CheckProject>.Register(e => e.DefectDesc);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
            set { this.SetProperty(DefectDescProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片Id
        /// </summary>
        public static readonly IRefIdProperty ProjectPhotoIdProperty = P<CheckProject>.RegisterRefId(e => e.ProjectPhotoId, ReferenceType.Normal);

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
            P<CheckProject>.RegisterRef(e => e.ProjectPhoto, ProjectPhotoIdProperty);

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
        public static readonly Property<string> RemarkProperty = P<CheckProject>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 AccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> AccountCodeProperty = P<CheckProject>.RegisterView(e => e.AccountCode, p => p.EquipAccount.Code);

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
        public static readonly Property<string> AccountNameProperty = P<CheckProject>.RegisterView(e => e.AccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AccountName
        {
            get { return GetProperty(AccountNameProperty); }
        }
        #endregion

        #region MyRegion


        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<CheckProject>.RegisterView(e => e.Photo, p => p.ProjectPhoto.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 点检项目 实体配置
    /// </summary>
    internal class CheckProjectConfig : EntityConfig<CheckProject>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CHECK_PROJECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}