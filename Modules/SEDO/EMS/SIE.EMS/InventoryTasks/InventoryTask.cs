using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InventoryTaskCriteria))]
    [Label("盘点任务")]
    [DisplayMember(nameof(TaskNo))]
    [EntityWithConfig(typeof(NoConfig), "盘点任务单号配置项", "盘点任务单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class InventoryTask : DataEntity
    {
        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> TaskNoProperty = P<InventoryTask>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 固定资产 IsAsset
        /// <summary>
        /// 固定资产
        /// </summary>
        [Label("固定资产")]
        public static readonly Property<bool> IsAssetProperty = P<InventoryTask>.Register(e => e.IsAsset);

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
        public static readonly Property<DateTime> ApplyDateProperty = P<InventoryTask>.Register(e => e.ApplyDate);

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
        public static readonly Property<decimal> PercentageProperty = P<InventoryTask>.Register(e => e.Percentage);

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
        public static readonly Property<string> InventoryTypeProperty = P<InventoryTask>.Register(e => e.InventoryType);

        /// <summary>
        /// 盘点类型
        /// </summary>
        public string InventoryType
        {
            get { return GetProperty(InventoryTypeProperty); }
            set { SetProperty(InventoryTypeProperty, value); }
        }
        #endregion

        #region 盘点说明 Remark
        /// <summary>
        /// 盘点说明
        /// </summary>
        [MaxLength(1000)]
        [Label("盘点说明")]
        public static readonly Property<string> RemarkProperty = P<InventoryTask>.Register(e => e.Remark);

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
        public static readonly Property<DateTime> PlanEndDateProperty = P<InventoryTask>.Register(e => e.PlanEndDate);

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
        public static readonly Property<bool> NeedPhotoProperty = P<InventoryTask>.Register(e => e.NeedPhoto);

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
        public static readonly Property<string> PhotoFilePathProperty = P<InventoryTask>.Register(e => e.PhotoFilePath);

        /// <summary>
        /// 图片样例
        /// </summary>
        public string PhotoFilePath
        {
            get { return GetProperty(PhotoFilePathProperty); }
            set { SetProperty(PhotoFilePathProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<InventoryTask>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<InventoryTask>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty = P<InventoryTask>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Normal);

        /// <summary>
        /// 盘点计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return (double)GetRefId(InventoryPlanIdProperty); }
            set { SetRefId(InventoryPlanIdProperty, value); }
        }

        /// <summary>
        /// 盘点计划
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty = P<InventoryTask>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return GetRefEntity(InventoryPlanProperty); }
            set { SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDept
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty ManageDeptIdProperty = P<InventoryTask>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? ManageDeptId
        {
            get { return (double?)GetRefNullableId(ManageDeptIdProperty); }
            set { SetRefNullableId(ManageDeptIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<InventoryTask>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise ManageDept
        {
            get { return GetRefEntity(ManageDeptProperty); }
            set { SetRefEntity(ManageDeptProperty, value); }
        }
        #endregion

        #region 盘点状态 InventoryTaskStatus
        /// <summary>
        /// 盘点状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<InventoryTaskStatus> InventoryTaskStatusProperty = P<InventoryTask>.Register(e => e.InventoryTaskStatus);

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus
        {
            get { return GetProperty(InventoryTaskStatusProperty); }
            set { SetProperty(InventoryTaskStatusProperty, value); }
        }
        #endregion

        #region 盘点责任人 Responsible
        /// <summary>
        /// 盘点责任人Id
        /// </summary>
        [Label("盘点责任人")]
        public static readonly IRefIdProperty ResponsibleIdProperty = P<InventoryTask>.RegisterRefId(e => e.ResponsibleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ResponsibleProperty = P<InventoryTask>.RegisterRef(e => e.Responsible, ResponsibleIdProperty);

        /// <summary>
        /// 盘点责任人
        /// </summary>
        public Employee Responsible
        {
            get { return GetRefEntity(ResponsibleProperty); }
            set { SetRefEntity(ResponsibleProperty, value); }
        }
        #endregion

        #region 执行类型 InventoryExecuteType
        /// <summary>
        /// 执行类型
        /// </summary>
        [Label("执行类型")]
        public static readonly Property<InventoryExecuteType> InventoryExecuteTypeProperty = P<InventoryTask>.Register(e => e.InventoryExecuteType);

        /// <summary>
        /// 执行类型
        /// </summary>
        public InventoryExecuteType InventoryExecuteType
        {
            get { return GetProperty(InventoryExecuteTypeProperty); }
            set { SetProperty(InventoryExecuteTypeProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryTask>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 设备清单 InventoryTaskEquipmentList
        /// <summary>
        /// 设备清单
        /// </summary>
        [Label("设备清单")]
        public static readonly ListProperty<EntityList<InventoryTaskEquipment>> InventoryTaskEquipmentListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskEquipmentList);
        /// <summary>
        /// 设备清单
        /// </summary>
        public EntityList<InventoryTaskEquipment> InventoryTaskEquipmentList
        {
            get { return this.GetLazyList(InventoryTaskEquipmentListProperty); }
        }
        #endregion

        #region 盘点人清单 InventoryTaskCounterList
        /// <summary>
        /// 盘点人清单
        /// </summary>
        [Label("盘点人清单")]
        public static readonly ListProperty<EntityList<InventoryTaskCounter>> InventoryTaskCounterListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskCounterList);
        /// <summary>
        /// 盘点人清单
        /// </summary>
        public EntityList<InventoryTaskCounter> InventoryTaskCounterList
        {
            get { return this.GetLazyList(InventoryTaskCounterListProperty); }
        }
        #endregion

        #region 原因分析 InventoryCauseList
        /// <summary>
        /// 原因分析
        /// </summary>
        [Label("原因分析")]
        public static readonly ListProperty<EntityList<InventoryCause>> InventoryCauseListProperty = P<InventoryTask>.RegisterList(e => e.InventoryCauseList);
        /// <summary>
        /// 原因分析
        /// </summary>
        public EntityList<InventoryCause> InventoryCauseList
        {
            get { return this.GetLazyList(InventoryCauseListProperty); }
        }
        #endregion

        #region 编码明细 InventoryTaskFixtureEncodeList
        /// <summary>
        /// 编码明细
        /// </summary>
        [Label("编码明细")]
        public static readonly ListProperty<EntityList<InventoryTaskFixtureEncode>> InventoryTaskFixtureEncodeListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskFixtureEncodeList);
        /// <summary>
        /// 编码明细
        /// </summary>
        public EntityList<InventoryTaskFixtureEncode> InventoryTaskFixtureEncodeList
        {
            get { return this.GetLazyList(InventoryTaskFixtureEncodeListProperty); }
        }
        #endregion

        #region 序列号明细 InventoryTaskFixtureIdAccountList
        /// <summary>
        /// 序列号明细
        /// </summary>
        [Label("序列号明细")]
        public static readonly ListProperty<EntityList<InventoryTaskFixtureIdAccount>> InventoryTaskFixtureIdAccountListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskFixtureIdAccountList);
        /// <summary>
        /// 序列号明细
        /// </summary>
        public EntityList<InventoryTaskFixtureIdAccount> InventoryTaskFixtureIdAccountList
        {
            get { return this.GetLazyList(InventoryTaskFixtureIdAccountListProperty); }
        }
        #endregion

        #region 盘点人 InventoryTaskFixtureCounterList
        /// <summary>
        /// 盘点人
        /// </summary>
        [Label("盘点人")]
        public static readonly ListProperty<EntityList<InventoryTaskFixtureCounter>> InventoryTaskFixtureCounterListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskFixtureCounterList);
        /// <summary>
        /// 盘点人
        /// </summary>
        public EntityList<InventoryTaskFixtureCounter> InventoryTaskFixtureCounterList
        {
            get { return this.GetLazyList(InventoryTaskFixtureCounterListProperty); }
        }
        #endregion

        #region 盘点进度 PercentageString
        /// <summary>
        /// 盘点进度
        /// </summary>
        [Label("盘点进度")]
        public static readonly Property<string> PercentageStringProperty = P<InventoryTask>.RegisterReadOnly(
            e => e.PercentageString, e => e.GetPercentageString(), PercentageProperty);
        /// <summary>
        /// 盘点进度
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
        public static readonly Property<string> CloseRemarkProperty = P<InventoryTask>.Register(e => e.CloseRemark);

        /// <summary>
        /// 关闭原因
        /// </summary>
        public string CloseRemark
        {
            get { return this.GetProperty(CloseRemarkProperty); }
            set { this.SetProperty(CloseRemarkProperty, value); }
        }
        #endregion

        #region 备件汇总 InventoryTaskSparePartList
        /// <summary>
        /// 备件汇总
        /// </summary>
        [Label("备件汇总")]
        public static readonly ListProperty<EntityList<InventoryTaskSparePart>> InventoryTaskSparePartListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskSparePartList);
        /// <summary>
        /// 备件汇总
        /// </summary>
        public EntityList<InventoryTaskSparePart> InventoryTaskSparePartList
        {
            get { return this.GetLazyList(InventoryTaskSparePartListProperty); }
        }
        #endregion

        #region 盘点人 InventoryTaskSparePartCounterList
        /// <summary>
        /// 盘点人
        /// </summary>
        [Label("盘点人")]
        public static readonly ListProperty<EntityList<InventoryTaskSparePartCounter>> InventoryTaskSparePartCounterListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskSparePartCounterList);
        /// <summary>
        /// 盘点人
        /// </summary>
        public EntityList<InventoryTaskSparePartCounter> InventoryTaskSparePartCounterList
        {
            get { return this.GetLazyList(InventoryTaskSparePartCounterListProperty); }
        }
        #endregion

        #region 备件清单 InventoryTaskSparePartDetailList
        /// <summary>
        /// 备件清单
        /// </summary>
        [Label("备件清单")]
        public static readonly ListProperty<EntityList<InventoryTaskSparePartDetail>> InventoryTaskSparePartDetailListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskSparePartDetailList);
        /// <summary>
        /// 备件清单
        /// </summary>
        public EntityList<InventoryTaskSparePartDetail> InventoryTaskSparePartDetailList
        {
            get { return this.GetLazyList(InventoryTaskSparePartDetailListProperty); }
        }
        #endregion

        #region 查询序列号 FixtureCodeSnNotMap
        /// <summary>
        /// 查询序列号
        /// </summary>
        [Label("查询序列号")]
        public static readonly Property<string> FixtureCodeSnNotMapProperty = P<InventoryTask>.Register(e => e.FixtureCodeSnNotMap);

        /// <summary>
        /// 查询序列号
        /// </summary>
        public string FixtureCodeSnNotMap
        {
            get { return this.GetProperty(FixtureCodeSnNotMapProperty); }
            set { this.SetProperty(FixtureCodeSnNotMapProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<InventoryTask>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlans.Warehouse> WarehouseProperty =
            P<InventoryTask>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public InventoryPlans.Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 资产对象 InventoryAssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        public static readonly Property<InventoryAssetObject> InventoryAssetObjectProperty = P<InventoryTask>.RegisterView(e => e.InventoryAssetObject, p => p.InventoryPlan.InventoryAssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public InventoryAssetObject InventoryAssetObject
        {
            get { return this.GetProperty(InventoryAssetObjectProperty); }
        }
        #endregion

        #region 管理部门名称 ManageDeptName
        /// <summary>
        /// 管理部门名称
        /// </summary>
        [Label("管理部门名称")]
        public static readonly Property<string> ManageDeptNameProperty = P<InventoryTask>.RegisterView(e => e.ManageDeptName, p => p.ManageDept.Name);

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string ManageDeptName
        {
            get { return this.GetProperty(ManageDeptNameProperty); }
        }
        #endregion

        #region 管理部门编码 ManageDeptCode
        /// <summary>
        /// 管理部门编码
        /// </summary>
        [Label("管理部门编码")]
        public static readonly Property<string> ManageDeptCodeProperty = P<InventoryTask>.RegisterView(e => e.ManageDeptCode, p => p.ManageDept.Code);

        /// <summary>
        /// 管理部门编码
        /// </summary>
        public string ManageDeptCode
        {
            get { return this.GetProperty(ManageDeptCodeProperty); }
        }
        #endregion

        #region 备件盘点仓库 WarehouseName
        /// <summary>
        /// 备件盘点仓库
        /// </summary>
        [Label("备件盘点仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<InventoryTask>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 备件盘点仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #endregion


        #region 原因分析 FixtureCauseAnalysis
        /// <summary>
        /// 原因分析
        /// </summary>
        [Label("原因分析")]
        [MaxLength(2000)]
        public static readonly Property<string> FixtureCauseAnalysisProperty = P<InventoryTask>.Register(e => e.FixtureCauseAnalysis);

        /// <summary>
        /// 原因分析
        /// </summary>
        public string FixtureCauseAnalysis
        {
            get { return this.GetProperty(FixtureCauseAnalysisProperty); }
            set { this.SetProperty(FixtureCauseAnalysisProperty, value); }
        }
        #endregion

        #region 改善措施 ImprovementMeasures
        /// <summary>
        /// 改善措施
        /// </summary>
        [Label("改善措施")]
        [MaxLength(2000)]
        public static readonly Property<string> FixtureImprovementProperty = P<InventoryTask>.Register(e => e.FixtureImprovement);

        /// <summary>
        /// 改善措施
        /// </summary>
        public string FixtureImprovement
        {
            get { return this.GetProperty(FixtureImprovementProperty); }
            set { this.SetProperty(FixtureImprovementProperty, value); }
        }
        #endregion
       
        #region 盘点差异 InventoryTaskSparePartDiffList
        /// <summary>
        /// 盘点差异
        /// </summary>
        [Label("盘点差异")]
        public static readonly ListProperty<EntityList<InventoryTaskSparePartDiff>> InventoryTaskSparePartDiffListProperty = P<InventoryTask>.RegisterList(e => e.InventoryTaskSparePartDiffList);
        /// <summary>
        /// 盘点差异
        /// </summary>
        public EntityList<InventoryTaskSparePartDiff> InventoryTaskSparePartDiffList
        {
            get { return this.GetLazyList(InventoryTaskSparePartDiffListProperty); }
        }
        #endregion

        #region 不影射数据库字段
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<InventoryTask>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件明细关键字 SparePartDetailKeyWord
        /// <summary>
        /// 备件明细关键字
        /// </summary>
        [Label("备件明细关键字")]
        public static readonly Property<string> SparePartDetailKeyWordProperty = P<InventoryTask>.Register(e => e.SparePartDetailKeyWord);

        /// <summary>
        /// 备件明细关键字
        /// </summary>
        public string SparePartDetailKeyWord
        {
            get { return this.GetProperty(SparePartDetailKeyWordProperty); }
            set { this.SetProperty(SparePartDetailKeyWordProperty, value); }
        }
        #endregion

        #region 盘点结果 InventorySeachResult
        /// <summary>
        /// 盘点结果
        /// </summary>
        [Label("盘点结果")]
        public static readonly Property<InventorySeachResult?> InventorySeachResultProperty = P<InventoryTask>.Register(e => e.InventorySeachResult);

        /// <summary>
        /// 盘点结果
        /// </summary>
        public InventorySeachResult? InventorySeachResult
        {
            get { return this.GetProperty(InventorySeachResultProperty); }
            set { this.SetProperty(InventorySeachResultProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 盘点任务 实体配置
    /// </summary>
    internal class InventoryTaskConfig : EntityConfig<InventoryTask>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_TASK").MapAllProperties();
            Meta.Property(InventoryTask.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.Property(InventoryTask.CloseRemarkProperty).ColumnMeta.HasLength(2000);
            Meta.Property(InventoryTask.FixtureCodeSnNotMapProperty).DontMapColumn();
            Meta.Property(InventoryTask.InventorySeachResultProperty).DontMapColumn();
            Meta.Property(InventoryTask.FixtureCauseAnalysisProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryTask.FixtureImprovementProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryTask.PhotoFilePathProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryTask.SparePartCodeProperty).DontMapColumn();
            Meta.Property(InventoryTask.SparePartDetailKeyWordProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}