using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InventoryPlanCriteria))]
    [Label("盘点计划")]
    [DisplayMember(nameof(PlanNo))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "盘点计划单号配置项", "盘点计划单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class InventoryPlan : DataEntity
    {
        /// <summary>
        /// 盘点类型快码组
        /// </summary>
        public const string InventoryTypeCatalog = "INVENTORY_TYPE_CATALOG";

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<InventoryPlan>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<InventoryPlan>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 计划单号 PlanNo
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> PlanNoProperty = P<InventoryPlan>.Register(e => e.PlanNo);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo
        {
            get { return GetProperty(PlanNoProperty); }
            set { SetProperty(PlanNoProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryPlan>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 固定资产 IsAsset
        /// <summary>
        /// 固定资产
        /// </summary>
        [Label("固定资产")]
        public static readonly Property<bool> IsAssetProperty = P<InventoryPlan>.Register(e => e.IsAsset);

        /// <summary>
        /// 固定资产
        /// </summary>
        public bool IsAsset
        {
            get { return GetProperty(IsAssetProperty); }
            set { SetProperty(IsAssetProperty, value); }
        }
        #endregion

        #region 申请日期 ApplyDate
        /// <summary>
        /// 申请日期
        /// </summary>
        [Label("申请日期")]
        public static readonly Property<DateTime> ApplyDateProperty = P<InventoryPlan>.Register(e => e.ApplyDate);

        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 盘点进度 Percentage
        /// <summary>
        /// 盘点进度
        /// </summary>
        [Label("盘点进度")]
        public static readonly Property<decimal> PercentageProperty = P<InventoryPlan>.Register(e => e.Percentage);

        /// <summary>
        /// 盘点进度
        /// </summary>
        public decimal Percentage
        {
            get { return GetProperty(PercentageProperty); }
            set { SetProperty(PercentageProperty, value); }
        }
        #endregion

        #region 盘点类型 InventoryType
        /// <summary>
        /// 盘点类型
        /// </summary>
        [Label("盘点类型")]
        [Required]
        public static readonly Property<string> InventoryTypeProperty = P<InventoryPlan>.Register(e => e.InventoryType);

        /// <summary>
        /// 盘点类型
        /// </summary>
        public string InventoryType
        {
            get { return this.GetProperty(InventoryTypeProperty); }
            set { this.SetProperty(InventoryTypeProperty, value); }
        }
        #endregion

        #region 执行类型 InventoryExecuteType
        /// <summary>
        /// 执行类型
        /// </summary>
        [Label("执行类型")]
        public static readonly Property<InventoryExecuteType> InventoryExecuteTypeProperty = P<InventoryPlan>.Register(e => e.InventoryExecuteType);

        /// <summary>
        /// 执行类型
        /// </summary>
        public InventoryExecuteType InventoryExecuteType
        {
            get { return this.GetProperty(InventoryExecuteTypeProperty); }
            set { this.SetProperty(InventoryExecuteTypeProperty, value); }
        }
        #endregion

        #region 盘点说明 Remark
        /// <summary>
        /// 盘点说明
        /// </summary>
        [MaxLength(1000)]
        [Required]
        [Label("盘点说明")]
        public static readonly Property<string> RemarkProperty = P<InventoryPlan>.Register(e => e.Remark);

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 计划完成日期 PlanEndDate
        /// <summary>
        /// 计划完成日期
        /// </summary>
        [Label("计划完成日期")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<InventoryPlan>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划完成日期
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 强制拍照 NeedPhoto
        /// <summary>
        /// 强制拍照
        /// </summary>
        [Label("强制拍照")]
        public static readonly Property<bool> NeedPhotoProperty = P<InventoryPlan>.Register(e => e.NeedPhoto);

        /// <summary>
        /// 强制拍照
        /// </summary>
        public bool NeedPhoto
        {
            get { return GetProperty(NeedPhotoProperty); }
            set { SetProperty(NeedPhotoProperty, value); }
        }
        #endregion

        #region 图片样例 PhotoFilePath
        /// <summary>
        /// 图片样例
        /// </summary>
        [Label("图片样例")]
        [MaxLength(1000)]
        public static readonly Property<string> PhotoFilePathProperty = P<InventoryPlan>.Register(e => e.PhotoFilePath);

        /// <summary>
        /// 图片样例
        /// </summary>
        public string PhotoFilePath
        {
            get { return GetProperty(PhotoFilePathProperty); }
            set { SetProperty(PhotoFilePathProperty, value); }
        }
        #endregion

        #region 盘点责任人 Responsible
        /// <summary>
        /// 盘点责任人Id
        /// </summary>
        [Label("盘点责任人")]
        public static readonly IRefIdProperty ResponsibleIdProperty = P<InventoryPlan>.RegisterRefId(e => e.ResponsibleId, ReferenceType.Normal);

        /// <summary>
        /// 盘点责任人Id
        /// </summary>
        public double? ResponsibleId
        {
            get { return (double?)GetRefNullableId(ResponsibleIdProperty); }
            set { SetRefNullableId(ResponsibleIdProperty, value); }
        }

        /// <summary>
        /// 盘点责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ResponsibleProperty = P<InventoryPlan>.RegisterRef(e => e.Responsible, ResponsibleIdProperty);

        /// <summary>
        /// 盘点责任人
        /// </summary>
        public Employee Responsible
        {
            get { return GetRefEntity(ResponsibleProperty); }
            set { SetRefEntity(ResponsibleProperty, value); }
        }
        #endregion

        #region 资产对象 InventoryAssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        public static readonly Property<InventoryAssetObject> InventoryAssetObjectProperty = P<InventoryPlan>.Register(e => e.InventoryAssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public InventoryAssetObject InventoryAssetObject
        {
            get { return GetProperty(InventoryAssetObjectProperty); }
            set { SetProperty(InventoryAssetObjectProperty, value); }
        }
        #endregion

        #region 盘点人员清单 InventoryCounterList
        /// <summary>
        /// 盘点人员清单
        /// </summary>
        [Label("盘点人员清单")]
        public static readonly ListProperty<EntityList<InventoryCounter>> InventoryCounterListProperty = P<InventoryPlan>.RegisterList(e => e.InventoryCounterList);
        /// <summary>
        /// 盘点人员清单
        /// </summary>
        public EntityList<InventoryCounter> InventoryCounterList
        {
            get { return this.GetLazyList(InventoryCounterListProperty); }
        }
        #endregion

        #region 盘点人 InventoryFixtureCounterList
        /// <summary>
        /// 盘点人
        /// </summary>
        [Label("盘点人")]
        public static readonly ListProperty<EntityList<InventoryFixtureCounter>> InventoryFixtureCounterListProperty = P<InventoryPlan>.RegisterList(e => e.InventoryFixtureCounterList);
        /// <summary>
        /// 盘点人
        /// </summary>
        public EntityList<InventoryFixtureCounter> InventoryFixtureCounterList
        {
            get { return this.GetLazyList(InventoryFixtureCounterListProperty); }
        }
        #endregion

        #region 盘点进度百分比 PercentageString
        /// <summary>
        /// 盘点进度百分比
        /// </summary>
        [Label("盘点进度")]
        public static readonly Property<string> PercentageStringProperty = P<InventoryPlan>.RegisterReadOnly(
            e => e.PercentageString, e => e.GetPercentageString(), PercentageProperty);
        /// <summary>
        /// 盘点进度百分比
        /// </summary>

        public string PercentageString
        {
            get { return this.GetProperty(PercentageStringProperty); }
        }
        private string GetPercentageString()
        {
            return Percentage == 0 ? "0" : Percentage + "%";
        }
        #endregion

        #region 关闭原因 CloseRemark
        /// <summary>
        /// 关闭原因
        /// </summary>
        [Label("关闭原因")]
        [MaxLength(1000)]
        public static readonly Property<string> CloseRemarkProperty = P<InventoryPlan>.Register(e => e.CloseRemark);

        /// <summary>
        /// 关闭原因
        /// </summary>
        public string CloseRemark
        {
            get { return this.GetProperty(CloseRemarkProperty); }
            set { this.SetProperty(CloseRemarkProperty, value); }
        }
        #endregion

        #region 是否保存 IsSave
        /// <summary>
        /// 是否保存 (校验主表数据是否保存)
        /// </summary>
        [Label("是否保存")]
        public static readonly Property<bool?> IsSaveProperty = P<InventoryPlan>.Register(e => e.IsSave);

        /// <summary>
        /// 是否保存
        /// </summary>
        public bool? IsSave
        {
            get { return this.GetProperty(IsSaveProperty); }
            set { this.SetProperty(IsSaveProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 盘点计划 实体配置
    /// </summary>
    internal class InventoryPlanConfig : EntityConfig<InventoryPlan>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_PLAN").MapAllProperties();
            Meta.Property(InventoryPlan.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlan.CloseRemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryPlan.PhotoFilePathProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}