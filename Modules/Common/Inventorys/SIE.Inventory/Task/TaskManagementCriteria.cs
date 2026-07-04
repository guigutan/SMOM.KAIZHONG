using SIE.Domain;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务管理查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("任务管理查询")]
    public partial class TaskManagementCriteria : Criteria
    {
        #region 初始化
        /// <summary>
        /// 任务管理查询 初始化
        /// </summary>
        public TaskManagementCriteria()
        {
            ReleaseDate = new DateRange();
            ReleaseDate.DateTimePart = DateTimePart.DateTime;
            ReleaseDate.DateRangeType = DateRangeType.Today;  //默认日期为今天
            EndDateTime = new DateRange();
            EndDateTime.DateTimePart = DateTimePart.DateTime;
            EndDateTime.DateRangeType = DateRangeType.All;
        }
        #endregion

        #region 任务号 No
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> NoProperty = P<TaskManagementCriteria>.Register(e => e.No);

        /// <summary>
        /// 任务号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 任务组号 TaskGroupNo
        /// <summary>
        /// 任务组号
        /// </summary>
        [Label("任务组号")]
        public static readonly Property<string> TaskGroupNoProperty = P<TaskManagementCriteria>.Register(e => e.TaskGroupNo);

        /// <summary>
        /// 任务组号
        /// </summary>
        public string TaskGroupNo
        {
            get { return this.GetProperty(TaskGroupNoProperty); }
            set { this.SetProperty(TaskGroupNoProperty, value); }
        }
        #endregion

        #region 释放时间 ReleaseDate
        /// <summary>
        /// 释放时间
        /// </summary>
        [Label("释放时间")]
        public static readonly Property<DateRange> ReleaseDateProperty = P<TaskManagementCriteria>.Register(e => e.ReleaseDate);

        /// <summary>
        /// 释放时间
        /// </summary>
        public DateRange ReleaseDate
        {
            get { return GetProperty(ReleaseDateProperty); }
            set { SetProperty(ReleaseDateProperty, value); }
        }
        #endregion

        #region 关联单号 BillNo
        /// <summary>
        /// 关联单号
        /// </summary>
        [Label("关联单号")]
        public static readonly Property<string> BillNoProperty = P<TaskManagementCriteria>.Register(e => e.BillNo);

        /// <summary>
        /// 关联单号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<TaskManagementCriteria>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号（ERP） TaskNo
        /// <summary>
        /// 任务号（ERP）
        /// </summary>
        [Label("任务号（ERP）")]
        public static readonly Property<string> TaskNoProperty = P<TaskManagementCriteria>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号（ERP）
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 操作类型 OperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OperationType?> OperationTypeProperty = P<TaskManagementCriteria>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType? OperationType
        {
            get { return GetProperty(OperationTypeProperty); }
            set { SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<TaskManagementCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<TaskManagementCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 建议目标库位 SuggestToLoc
        /// <summary>
        /// 建议目标库位Id
        /// </summary>
        [Label("建议目标库位")]
        public static readonly IRefIdProperty SuggestToLocIdProperty =
            P<TaskManagementCriteria>.RegisterRefId(e => e.SuggestToLocId, ReferenceType.Normal);

        /// <summary>
        /// 建议目标库位Id
        /// </summary>
        public double? SuggestToLocId
        {
            get { return (double?)this.GetRefNullableId(SuggestToLocIdProperty); }
            set { this.SetRefNullableId(SuggestToLocIdProperty, value); }
        }

        /// <summary>
        /// 建议目标库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> SuggestToLocProperty =
            P<TaskManagementCriteria>.RegisterRef(e => e.SuggestToLoc, SuggestToLocIdProperty);

        /// <summary>
        /// 建议目标库位
        /// </summary>
        public StorageLocation SuggestToLoc
        {
            get { return this.GetRefEntity(SuggestToLocProperty); }
            set { this.SetRefEntity(SuggestToLocProperty, value); }
        }
        #endregion

        #region 建议批次 Lot
        /// <summary>
        /// 建议批次
        /// </summary>
        [Label("建议批次")]
        public static readonly Property<string> LotProperty = P<TaskManagementCriteria>.Register(e => e.Lot);

        /// <summary>
        /// 建议批次
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 建议来源LPN LPN
        /// <summary>
        /// LPN
        /// </summary>
        [Label("建议来源LPN")]
        public static readonly Property<string> LPNProperty = P<TaskManagementCriteria>.Register(e => e.LPN);

        /// <summary>
        /// 建议来源LPN
        /// </summary>
        public string LPN
        {
            get { return this.GetProperty(LPNProperty); }
            set { this.SetProperty(LPNProperty, value); }
        }
        #endregion

        #region 建议目标LPN SuggestToLpn
        /// <summary>
        /// 建议目标LPN
        /// </summary>
        [Label("建议目标LPN")]
        public static readonly Property<string> SuggestToLpnProperty = P<TaskManagementCriteria>.Register(e => e.SuggestToLpn);

        /// <summary>
        /// 建议目标LPN
        /// </summary>
        public string SuggestToLpn
        {
            get { return GetProperty(SuggestToLpnProperty); }
            set { SetProperty(SuggestToLpnProperty, value); }
        }
        #endregion

        #region 实际来源LPN ActualFromLpn
        /// <summary>
        /// 实际来源LPN
        /// </summary>
        [Label("实际来源LPN")]
        public static readonly Property<string> ActualFromLpnProperty = P<TaskManagementCriteria>.Register(e => e.ActualFromLpn);

        /// <summary>
        /// 实际来源LPN
        /// </summary>
        public string ActualFromLpn
        {
            get { return GetProperty(ActualFromLpnProperty); }
            set { SetProperty(ActualFromLpnProperty, value); }
        }
        #endregion

        #region 实际目标LPN ActualToLpn
        /// <summary>
        /// 实际目标ActualToLpn
        /// </summary>
        [Label("实际目标LPN")]
        public static readonly Property<string> ActualToLpnProperty = P<TaskManagementCriteria>.Register(e => e.ActualToLpn);

        /// <summary>
        /// 实际目标ActualToLpn
        /// </summary>
        public string ActualToLpn
        {
            get { return this.GetProperty(ActualToLpnProperty); }
            set { this.SetProperty(ActualToLpnProperty, value); }
        }
        #endregion

        #region 优先级 Level
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("任务优先级")]
        public static readonly Property<TaskLevel?> LevelProperty = P<TaskManagementCriteria>.Register(e => e.Level);

        /// <summary>
        /// 优先级
        /// </summary>
        public TaskLevel? Level
        {
            get { return GetProperty(LevelProperty); }
            set { SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 来源仓库 FromWarehouse
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库")]
        public static readonly IRefIdProperty FromWarehouseIdProperty = P<TaskManagementCriteria>.RegisterRefId(e => e.FromWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double? FromWarehouseId
        {
            get { return (double?)GetRefNullableId(FromWarehouseIdProperty); }
            set { SetRefNullableId(FromWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 来源仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FromWarehouseProperty = P<TaskManagementCriteria>.RegisterRef(e => e.FromWarehouse, FromWarehouseIdProperty);

        /// <summary>
        /// 来源仓库
        /// </summary>
        public Warehouse FromWarehouse
        {
            get { return GetRefEntity(FromWarehouseProperty); }
            set { SetRefEntity(FromWarehouseProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("任务状态")]
        public static readonly Property<string> StateProperty = P<TaskManagementCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 目标仓库 ToWarehouse
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库")]
        public static readonly IRefIdProperty ToWarehouseIdProperty = P<TaskManagementCriteria>.RegisterRefId(e => e.ToWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double? ToWarehouseId
        {
            get { return (double?)GetRefNullableId(ToWarehouseIdProperty); }
            set { SetRefNullableId(ToWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> ToWarehouseProperty = P<TaskManagementCriteria>.RegisterRef(e => e.ToWarehouse, ToWarehouseIdProperty);

        /// <summary>
        /// 目标仓库
        /// </summary>
        public Warehouse ToWarehouse
        {
            get { return GetRefEntity(ToWarehouseProperty); }
            set { SetRefEntity(ToWarehouseProperty, value); }
        }
        #endregion

        #region 事务类型 TransactionType
        /// <summary>
        /// 事务类型
        /// </summary>
        [Label("事务类型")]
        public static readonly Property<TransactionType?> TransactionTypeProperty = P<TaskManagementCriteria>.Register(e => e.TransactionType);

        /// <summary>
        /// 事务类型
        /// </summary>
        public TransactionType? TransactionType
        {
            get { return GetProperty(TransactionTypeProperty); }
            set { SetProperty(TransactionTypeProperty, value); }
        }
        #endregion

        #region 实际操作人 ActualOperator
        /// <summary>
        /// 实际操作人Id
        /// </summary>
        [Label("实际操作人")]
        public static readonly IRefIdProperty ActualOperatorIdProperty =
            P<TaskManagementCriteria>.RegisterRefId(e => e.ActualOperatorId, ReferenceType.Normal);

        /// <summary>
        /// 实际操作人Id
        /// </summary>
        public double? ActualOperatorId
        {
            get { return (double?)this.GetRefNullableId(ActualOperatorIdProperty); }
            set { this.SetRefNullableId(ActualOperatorIdProperty, value); }
        }

        /// <summary>
        /// 实际操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ActualOperatorProperty =
            P<TaskManagementCriteria>.RegisterRef(e => e.ActualOperator, ActualOperatorIdProperty);

        /// <summary>
        /// 实际操作人
        /// </summary>
        public Employee ActualOperator
        {
            get { return this.GetRefEntity(ActualOperatorProperty); }
            set { this.SetRefEntity(ActualOperatorProperty, value); }
        }
        #endregion

        #region 执行结束时间 ReleaseDate
        /// <summary>
        /// 执行结束时间
        /// </summary>
        [Label("执行结束时间")]
        public static readonly Property<DateRange> EndDateTimeProperty = P<TaskManagementCriteria>.Register(e => e.EndDateTime);

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateRange EndDateTime
        {
            get { return GetProperty(EndDateTimeProperty); }
            set { SetProperty(EndDateTimeProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<TaskManagementCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateByIdProperty = P<TaskManagementCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreateById
        {
            get { return (double?)GetRefNullableId(CreateByIdProperty); }
            set { SetRefNullableId(CreateByIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateByProperty = P<TaskManagementCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return GetRefEntity(CreateByProperty); }
            set { SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<TaskController>().GetTaskManagements(this);
        }
    }
}
