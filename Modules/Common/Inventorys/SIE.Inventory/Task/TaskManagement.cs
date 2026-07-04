using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.Task.Configs;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务管理
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(TaskManagementCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [EntityWithConfig(typeof(TaskParameterConfig))]
    [Label("任务管理")]
    [DisplayMember(nameof(No))]
    public partial class TaskManagement : DataEntity
    {
        #region 任务号 No
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        [NotDuplicate]
        [Required]
        public static readonly Property<string> NoProperty = P<TaskManagement>.Register(e => e.No);

        /// <summary>
        /// 任务号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 任务描述 StateDesc
        /// <summary>
        /// 任务描述
        /// </summary>
        [MaxLength(4000)]
        [Label("任务描述")]
        public static readonly Property<string> StateDescProperty = P<TaskManagement>.Register(e => e.StateDesc);

        /// <summary>
        /// 任务描述
        /// </summary>
        public string StateDesc
        {
            get { return GetProperty(StateDescProperty); }
            set { SetProperty(StateDescProperty, value); }
        }
        #endregion

        #region 释放时间 ReleaseDate
        /// <summary>
        /// 释放时间
        /// </summary>
        [Label("释放时间")]
        public static readonly Property<DateTime?> ReleaseDateProperty = P<TaskManagement>.Register(e => e.ReleaseDate);

        /// <summary>
        /// 释放时间
        /// </summary>
        public DateTime? ReleaseDate
        {
            get { return GetProperty(ReleaseDateProperty); }
            set { SetProperty(ReleaseDateProperty, value); }
        }
        #endregion

        #region 操作要求 OperationRequires
        /// <summary>
        /// 操作要求
        /// </summary>
        [MaxLength(4000)]
        [Label("操作要求")]
        public static readonly Property<string> OperationRequiresProperty = P<TaskManagement>.Register(e => e.OperationRequires);

        /// <summary>
        /// 操作要求
        /// </summary>
        public string OperationRequires
        {
            get { return GetProperty(OperationRequiresProperty); }
            set { SetProperty(OperationRequiresProperty, value); }
        }
        #endregion

        #region 执行开始时间 BeginDate
        /// <summary>
        /// 执行开始时间
        /// </summary>
        [Label("执行开始时间")]
        public static readonly Property<DateTime?> BeginDateProperty = P<TaskManagement>.Register(e => e.BeginDate);

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime? BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 执行结束时间 EndDate
        /// <summary>
        /// 执行结束时间
        /// </summary>
        [Label("执行结束时间")]
        public static readonly Property<DateTime?> EndDateProperty = P<TaskManagement>.Register(e => e.EndDate);

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 时长（分钟） LengthTime
        /// <summary>
        /// 时长（分钟）
        /// </summary>
        [Label("时长（分钟）")]
        public static readonly Property<decimal?> LengthTimeProperty = P<TaskManagement>.Register(e => e.LengthTime);

        /// <summary>
        /// 时长（分钟）
        /// </summary>
        public decimal? LengthTime
        {
            get { return GetProperty(LengthTimeProperty); }
            set { SetProperty(LengthTimeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<TaskManagement>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 实际数量 ActualQty
        /// <summary>
        /// 实际数量
        /// </summary>
        [Label("实际数量")]
        public static readonly Property<decimal?> ActualQtyProperty = P<TaskManagement>.Register(e => e.ActualQty);

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal? ActualQty
        {
            get { return GetProperty(ActualQtyProperty); }
            set { SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<TaskManagement>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 单据ID BillId
        /// <summary>
        /// 单据ID
        /// </summary>
        [Label("单据ID")]
        public static readonly Property<double> BillIdProperty = P<TaskManagement>.Register(e => e.BillId);

        /// <summary>
        /// 单据ID
        /// </summary>
        public double BillId
        {
            get { return GetProperty(BillIdProperty); }
            set { SetProperty(BillIdProperty, value); }
        }
        #endregion

        #region 单据明细号 BillDtlNo
        /// <summary>
        /// 单据明细号
        /// </summary>
        [Label("单据明细号")]
        public static readonly Property<string> BillDtlNoProperty = P<TaskManagement>.Register(e => e.BillDtlNo);

        /// <summary>
        /// 单据明细号
        /// </summary>
        public string BillDtlNo
        {
            get { return GetProperty(BillDtlNoProperty); }
            set { SetProperty(BillDtlNoProperty, value); }
        }
        #endregion

        #region 单据明细ID BillDtlId
        /// <summary>
        /// 单据明细ID
        /// </summary>
        [Label("单据明细ID")]
        public static readonly Property<double> BillDtlIdProperty = P<TaskManagement>.Register(e => e.BillDtlId);

        /// <summary>
        /// 单据明细ID
        /// </summary>
        public double BillDtlId
        {
            get { return GetProperty(BillDtlIdProperty); }
            set { SetProperty(BillDtlIdProperty, value); }
        }
        #endregion

        #region 二级单据明细号 SecondBillDtlNo
        /// <summary>
        /// 二级单据明细号 (记录LPN或者分配ID)
        /// </summary>
        [Label("二级单据明细号")]
        public static readonly Property<string> SecondBillDtlNoProperty = P<TaskManagement>.Register(e => e.SecondBillDtlNo);

        /// <summary>
        /// 二级单据明细号
        /// </summary>
        public string SecondBillDtlNo
        {
            get { return GetProperty(SecondBillDtlNoProperty); }
            set { SetProperty(SecondBillDtlNoProperty, value); }
        }
        #endregion

        #region 关闭时间 CloseDate
        /// <summary>
        /// 关闭时间
        /// </summary>
        [Label("关闭时间")]
        public static readonly Property<DateTime?> CloseDateProperty = P<TaskManagement>.Register(e => e.CloseDate);

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseDate
        {
            get { return GetProperty(CloseDateProperty); }
            set { SetProperty(CloseDateProperty, value); }
        }
        #endregion

        #region 建议来源LPN SuggestFromLpn
        /// <summary>
        /// 建议来源LPN
        /// </summary>
        [Label("建议来源LPN")]
        public static readonly Property<string> SuggestFromLpnProperty = P<TaskManagement>.Register(e => e.SuggestFromLpn);

        /// <summary>
        /// 建议来源LPN
        /// </summary>
        public string SuggestFromLpn
        {
            get { return GetProperty(SuggestFromLpnProperty); }
            set { SetProperty(SuggestFromLpnProperty, value); }
        }
        #endregion

        #region 建议目标LPN SuggestToLpn
        /// <summary>
        /// 建议目标LPN
        /// </summary>
        [Label("建议目标LPN")]
        public static readonly Property<string> SuggestToLpnProperty = P<TaskManagement>.Register(e => e.SuggestToLpn);

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
        public static readonly Property<string> ActualFromLpnProperty = P<TaskManagement>.Register(e => e.ActualFromLpn);

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
        /// 实际目标LPN
        /// </summary>
        [Label("实际目标LPN")]
        public static readonly Property<string> ActualToLpnProperty = P<TaskManagement>.Register(e => e.ActualToLpn);

        /// <summary>
        /// 实际目标LPN
        /// </summary>
        public string ActualToLpn
        {
            get { return GetProperty(ActualToLpnProperty); }
            set { SetProperty(ActualToLpnProperty, value); }
        }
        #endregion

        #region 冻结时间 FrozenDate
        /// <summary>
        /// 冻结时间
        /// </summary>
        [Label("冻结时间")]
        public static readonly Property<DateTime?> FrozenDateProperty = P<TaskManagement>.Register(e => e.FrozenDate);

        /// <summary>
        /// 冻结时间
        /// </summary>
        public DateTime? FrozenDate
        {
            get { return GetProperty(FrozenDateProperty); }
            set { SetProperty(FrozenDateProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<TaskManagement>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 库存任务号 TaskNo
        /// <summary>
        /// 库存任务号
        /// </summary>
        [Label("库存任务号")]
        public static readonly Property<string> TaskNoProperty = P<TaskManagement>.Register(e => e.TaskNo);

        /// <summary>
        /// 库存任务号
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
        public static readonly Property<OperationType> OperationTypeProperty = P<TaskManagement>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType
        {
            get { return GetProperty(OperationTypeProperty); }
            set { SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 建议目标库位 SuggestToLoc
        /// <summary>
        /// 建议目标库位Id
        /// </summary>
        [Label("建议目标库位")]
        public static readonly IRefIdProperty SuggestToLocIdProperty = P<TaskManagement>.RegisterRefId(e => e.SuggestToLocId, ReferenceType.Normal);

        /// <summary>
        /// 建议目标库位Id
        /// </summary>
        public double? SuggestToLocId
        {
            get { return (double?)GetRefNullableId(SuggestToLocIdProperty); }
            set { SetRefNullableId(SuggestToLocIdProperty, value); }
        }

        /// <summary>
        /// 建议目标库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> SuggestToLocProperty = P<TaskManagement>.RegisterRef(e => e.SuggestToLoc, SuggestToLocIdProperty);

        /// <summary>
        /// 建议目标库位
        /// </summary>
        public StorageLocation SuggestToLoc
        {
            get { return GetRefEntity(SuggestToLocProperty); }
            set { SetRefEntity(SuggestToLocProperty, value); }
        }
        #endregion

        #region 实际目标库位 ActualToLoc
        /// <summary>
        /// 实际目标库位Id
        /// </summary>
        [Label("实际目标库位")]
        public static readonly IRefIdProperty ActualToLocIdProperty = P<TaskManagement>.RegisterRefId(e => e.ActualToLocId, ReferenceType.Normal);

        /// <summary>
        /// 实际目标库位Id
        /// </summary>
        public double? ActualToLocId
        {
            get { return (double?)GetRefNullableId(ActualToLocIdProperty); }
            set { SetRefNullableId(ActualToLocIdProperty, value); }
        }

        /// <summary>
        /// 实际目标库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> ActualToLocProperty = P<TaskManagement>.RegisterRef(e => e.ActualToLoc, ActualToLocIdProperty);

        /// <summary>
        /// 实际目标库位
        /// </summary>
        public StorageLocation ActualToLoc
        {
            get { return GetRefEntity(ActualToLocProperty); }
            set { SetRefEntity(ActualToLocProperty, value); }
        }
        #endregion

        #region 建议批次 SuggestLot
        /// <summary>
        /// 建议批次Id
        /// </summary>
        [Label("建议批次")]
        public static readonly IRefIdProperty SuggestLotIdProperty = P<TaskManagement>.RegisterRefId(e => e.SuggestLotId, ReferenceType.Normal);

        /// <summary>
        /// 建议批次Id
        /// </summary>
        public double? SuggestLotId
        {
            get { return (double?)GetRefNullableId(SuggestLotIdProperty); }
            set { SetRefNullableId(SuggestLotIdProperty, value); }
        }

        /// <summary>
        /// 建议批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> SuggestLotProperty = P<TaskManagement>.RegisterRef(e => e.SuggestLot, SuggestLotIdProperty);

        /// <summary>
        /// 建议批次
        /// </summary>
        public Lot SuggestLot
        {
            get { return GetRefEntity(SuggestLotProperty); }
            set { SetRefEntity(SuggestLotProperty, value); }
        }
        #endregion

        #region 建议来源库位 SuggestFromLoc
        /// <summary>
        /// 建议来源库位Id
        /// </summary>
        [Label("建议来源库位")]
        public static readonly IRefIdProperty SuggestFromLocIdProperty = P<TaskManagement>.RegisterRefId(e => e.SuggestFromLocId, ReferenceType.Normal);

        /// <summary>
        /// 建议来源库位Id
        /// </summary>
        public double? SuggestFromLocId
        {
            get { return (double?)GetRefNullableId(SuggestFromLocIdProperty); }
            set { SetRefNullableId(SuggestFromLocIdProperty, value); }
        }

        /// <summary>
        /// 建议来源库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> SuggestFromLocProperty = P<TaskManagement>.RegisterRef(e => e.SuggestFromLoc, SuggestFromLocIdProperty);

        /// <summary>
        /// 建议来源库位
        /// </summary>
        public StorageLocation SuggestFromLoc
        {
            get { return GetRefEntity(SuggestFromLocProperty); }
            set { SetRefEntity(SuggestFromLocProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<TaskManagement>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<TaskManagement>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 单据大类 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType> OrderTypeProperty = P<TaskManagement>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 单据小类 Transaction
        /// <summary>
        /// 单据小类
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty =
            P<TaskManagement>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
        /// 单据小类
        /// </summary>
        public double? TransactionId
        {
            get { return (double?)this.GetRefNullableId(TransactionIdProperty); }
            set { this.SetRefNullableId(TransactionIdProperty, value); }
        }

        /// <summary>
        /// 单据小类
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty =
            P<TaskManagement>.RegisterRef(e => e.Transaction, TransactionIdProperty);

        /// <summary>
        /// 单据小类
        /// </summary>
        public Transaction Transaction
        {
            get { return this.GetRefEntity(TransactionProperty); }
            set { this.SetRefEntity(TransactionProperty, value); }
        }
        #endregion

        #region 优先级 Level
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("任务优先级")]
        public static readonly Property<TaskLevel> LevelProperty = P<TaskManagement>.Register(e => e.Level);

        /// <summary>
        /// 优先级
        /// </summary>
        public TaskLevel Level
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
        public static readonly IRefIdProperty FromWarehouseIdProperty = P<TaskManagement>.RegisterRefId(e => e.FromWarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> FromWarehouseProperty = P<TaskManagement>.RegisterRef(e => e.FromWarehouse, FromWarehouseIdProperty);

        /// <summary>
        /// 来源仓库
        /// </summary>
        public Warehouse FromWarehouse
        {
            get { return GetRefEntity(FromWarehouseProperty); }
            set { SetRefEntity(FromWarehouseProperty, value); }
        }
        #endregion

        #region 实际操作人列表 ActualOperatorList
        /// <summary>
        /// 实际操作人列表
        /// </summary>
        [Label("实际操作人")]
        public static readonly ListProperty<EntityList<ActualOperator>> ActualOperatorListProperty = P<TaskManagement>.RegisterList(e => e.ActualOperatorList);

        /// <summary>
        /// 实际操作人列表
        /// </summary>
        public EntityList<ActualOperator> ActualOperatorList
        {
            get { return this.GetLazyList(ActualOperatorListProperty); }
        }
        #endregion

        #region 实际来源库位 ActualFromLoc
        /// <summary>
        /// 实际来源库位Id
        /// </summary>
        [Label("实际来源库位")]
        public static readonly IRefIdProperty ActualFromLocIdProperty = P<TaskManagement>.RegisterRefId(e => e.ActualFromLocId, ReferenceType.Normal);

        /// <summary>
        /// 实际来源库位Id
        /// </summary>
        public double? ActualFromLocId
        {
            get { return (double?)GetRefNullableId(ActualFromLocIdProperty); }
            set { SetRefNullableId(ActualFromLocIdProperty, value); }
        }

        /// <summary>
        /// 实际来源库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> ActualFromLocProperty = P<TaskManagement>.RegisterRef(e => e.ActualFromLoc, ActualFromLocIdProperty);

        /// <summary>
        /// 实际来源库位
        /// </summary>
        public StorageLocation ActualFromLoc
        {
            get { return GetRefEntity(ActualFromLocProperty); }
            set { SetRefEntity(ActualFromLocProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("任务状态")]
        public static readonly Property<TaskState> StateProperty = P<TaskManagement>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public TaskState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 任务操作人列表 OperatorList
        /// <summary>
        /// 任务操作人列表
        /// </summary>
        [Label("任务操作人")]
        public static readonly ListProperty<EntityList<Operator>> OperatorListProperty = P<TaskManagement>.RegisterList(e => e.OperatorList);

        /// <summary>
        /// 任务操作人列表
        /// </summary>
        public EntityList<Operator> OperatorList
        {
            get { return this.GetLazyList(OperatorListProperty); }
        }
        #endregion

        #region 目标仓库 ToWarehouse
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库")]
        public static readonly IRefIdProperty ToWarehouseIdProperty = P<TaskManagement>.RegisterRefId(e => e.ToWarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> ToWarehouseProperty = P<TaskManagement>.RegisterRef(e => e.ToWarehouse, ToWarehouseIdProperty);

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
        public static readonly Property<TransactionType> TransactionTypeProperty = P<TaskManagement>.Register(e => e.TransactionType);

        /// <summary>
        /// 事务类型
        /// </summary>
        public TransactionType TransactionType
        {
            get { return GetProperty(TransactionTypeProperty); }
            set { SetProperty(TransactionTypeProperty, value); }
        }
        #endregion

        #region 关闭人 CloseBy
        /// <summary>
        /// 关闭人Id
        /// </summary>
        [Label("关闭人")]
        public static readonly IRefIdProperty CloseByIdProperty = P<TaskManagement>.RegisterRefId(e => e.CloseById, ReferenceType.Normal);

        /// <summary>
        /// 关闭人Id
        /// </summary>
        public double? CloseById
        {
            get { return (double?)GetRefNullableId(CloseByIdProperty); }
            set { SetRefNullableId(CloseByIdProperty, value); }
        }

        /// <summary>
        /// 关闭人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CloseByProperty = P<TaskManagement>.RegisterRef(e => e.CloseBy, CloseByIdProperty);

        /// <summary>
        /// 关闭人
        /// </summary>
        public Employee CloseBy
        {
            get { return GetRefEntity(CloseByProperty); }
            set { SetRefEntity(CloseByProperty, value); }
        }
        #endregion

        #region 实际批次 ActualLot
        /// <summary>
        /// 实际批次Id
        /// </summary>
        [Label("实际批次")]
        public static readonly IRefIdProperty ActualLotIdProperty = P<TaskManagement>.RegisterRefId(e => e.ActualLotId, ReferenceType.Normal);

        /// <summary>
        /// 实际批次Id
        /// </summary>
        public double? ActualLotId
        {
            get { return (double?)GetRefNullableId(ActualLotIdProperty); }
            set { SetRefNullableId(ActualLotIdProperty, value); }
        }

        /// <summary>
        /// 实际批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> ActualLotProperty = P<TaskManagement>.RegisterRef(e => e.ActualLot, ActualLotIdProperty);

        /// <summary>
        /// 实际批次
        /// </summary>
        public Lot ActualLot
        {
            get { return GetRefEntity(ActualLotProperty); }
            set { SetRefEntity(ActualLotProperty, value); }
        }
        #endregion

        #region 冻结人 FrozenBy
        /// <summary>
        /// 冻结人Id
        /// </summary>
        [Label("冻结人")]
        public static readonly IRefIdProperty FrozenByIdProperty = P<TaskManagement>.RegisterRefId(e => e.FrozenById, ReferenceType.Normal);

        /// <summary>
        /// 冻结人Id
        /// </summary>
        public double? FrozenById
        {
            get { return (double?)GetRefNullableId(FrozenByIdProperty); }
            set { SetRefNullableId(FrozenByIdProperty, value); }
        }

        /// <summary>
        /// 冻结人
        /// </summary>
        public static readonly RefEntityProperty<Employee> FrozenByProperty = P<TaskManagement>.RegisterRef(e => e.FrozenBy, FrozenByIdProperty);

        /// <summary>
        /// 冻结人
        /// </summary>
        public Employee FrozenBy
        {
            get { return GetRefEntity(FrozenByProperty); }
            set { SetRefEntity(FrozenByProperty, value); }
        }
        #endregion

        #region 来源ID（产生任务的数据源ID） SourceId
        /// <summary>
        /// 来源ID（产生任务的数据源ID）
        /// </summary>
        [Label("来源ID（产生任务的数据源ID）")]
        public static readonly Property<double> TaskSourceIdProperty = P<TaskManagement>.Register(e => e.TaskSourceId);

        /// <summary>
        /// 来源ID（产生任务的数据源ID）
        /// </summary>
        public double TaskSourceId
        {
            get { return GetProperty(TaskSourceIdProperty); }
            set { SetProperty(TaskSourceIdProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<TaskManagement>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return this.GetProperty(StorerCodeProperty); }
            set { this.SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 任务领取人 GetBy
        /// <summary>
        /// 任务领取人
        /// </summary>
        [Label("任务领取人")]
        public static readonly IRefIdProperty GetByIdProperty =
            P<TaskManagement>.RegisterRefId(e => e.GetById, ReferenceType.Normal);

        /// <summary>
        /// 任务领取人
        /// </summary>
        public double? GetById
        {
            get { return (double?)this.GetRefId(GetByIdProperty); }
            set { this.SetRefId(GetByIdProperty, value); }
        }

        /// <summary>
        /// 任务领取人
        /// </summary>
        public static readonly RefEntityProperty<Employee> GetByProperty =
            P<TaskManagement>.RegisterRef(e => e.GetBy, GetByIdProperty);

        /// <summary>
        /// 任务领取人
        /// </summary>
        public Employee GetBy
        {
            get { return this.GetRefEntity(GetByProperty); }
            set { this.SetRefEntity(GetByProperty, value); }
        }
        #endregion

        #region 来源库区 FromArea
        /// <summary>
        /// 来源库区Id
        /// </summary>
        [Label("来源库区")]
        public static readonly IRefIdProperty FromAreaIdProperty = P<TaskManagement>.RegisterRefId(e => e.FromAreaId, ReferenceType.Normal);

        /// <summary>
        /// 来源库区Id
        /// </summary>
        public double? FromAreaId
        {
            get { return (double?)GetRefNullableId(FromAreaIdProperty); }
            set { SetRefNullableId(FromAreaIdProperty, value); }
        }

        /// <summary>
        /// 来源库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> FromAreaProperty = P<TaskManagement>.RegisterRef(e => e.FromArea, FromAreaIdProperty);

        /// <summary>
        /// 来源库区
        /// </summary>
        public StorageArea FromArea
        {
            get { return GetRefEntity(FromAreaProperty); }
            set { SetRefEntity(FromAreaProperty, value); }
        }
        #endregion

        #region 目标库区 ToArea
        /// <summary>
        /// 目标库区Id
        /// </summary>
        [Label("目标库区")]
        public static readonly IRefIdProperty ToAreaIdProperty = P<TaskManagement>.RegisterRefId(e => e.ToAreaId, ReferenceType.Normal);

        /// <summary>
        /// 目标库区Id
        /// </summary>
        public double? ToAreaId
        {
            get { return (double?)GetRefNullableId(ToAreaIdProperty); }
            set { SetRefNullableId(ToAreaIdProperty, value); }
        }

        /// <summary>
        /// 目标库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> ToAreaProperty = P<TaskManagement>.RegisterRef(e => e.ToArea, ToAreaIdProperty);

        /// <summary>
        /// 目标库区
        /// </summary>
        public StorageArea ToArea
        {
            get { return GetRefEntity(ToAreaProperty); }
            set { SetRefEntity(ToAreaProperty, value); }
        }
        #endregion

        #region 任务组 TaskGroup
        /// <summary>
        /// 任务组Id
        /// </summary>
        [Label("任务组")]
        public static readonly IRefIdProperty TaskGroupIdProperty =
            P<TaskManagement>.RegisterRefId(e => e.TaskGroupId, ReferenceType.Normal);

        /// <summary>
        /// 任务组Id
        /// </summary>
        public double? TaskGroupId
        {
            get { return (double?)this.GetRefNullableId(TaskGroupIdProperty); }
            set { this.SetRefNullableId(TaskGroupIdProperty, value); }
        }

        /// <summary>
        /// 任务组
        /// </summary>
        public static readonly RefEntityProperty<TaskGroup> TaskGroupProperty =
            P<TaskManagement>.RegisterRef(e => e.TaskGroup, TaskGroupIdProperty);

        /// <summary>
        /// 任务组
        /// </summary>
        public TaskGroup TaskGroup
        {
            get { return this.GetRefEntity(TaskGroupProperty); }
            set { this.SetRefEntity(TaskGroupProperty, value); }
        }
        #endregion

        #region 播种类型 SowType
        /// <summary>
        /// 播种类型
        /// </summary>
        [Label("播种方式")]
        public static readonly Property<SowType?> SowTypeProperty = P<TaskManagement>.Register(e => e.SowType);

        /// <summary>
        /// 播种类型
        /// </summary>
        public SowType? SowType
        {
            get { return GetProperty(SowTypeProperty); }
            set { SetProperty(SowTypeProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<TaskManagement>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<TaskManagement>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState?> OnhandStateProperty = P<TaskManagement>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? OnhandState
        {
            get { return this.GetProperty(OnhandStateProperty); }
            set { this.SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 建议来源站台 SuggestFromStation
        /// <summary>
        /// 建议来源站台
        /// </summary>
        [Label("建议来源站台")]
        public static readonly Property<string> SuggestFromStationProperty = P<TaskManagement>.Register(e => e.SuggestFromStation);

        /// <summary>
        /// 建议来源站台
        /// </summary>
        public string SuggestFromStation
        {
            get { return this.GetProperty(SuggestFromStationProperty); }
            set { this.SetProperty(SuggestFromStationProperty, value); }
        }
        #endregion

        #region 实际来源站台 ActualFromStation
        /// <summary>
        /// 实际来源站台
        /// </summary>
        [Label("实际来源站台")]
        public static readonly Property<string> ActualFromStationProperty = P<TaskManagement>.Register(e => e.ActualFromStation);

        /// <summary>
        /// 实际来源站台
        /// </summary>
        public string ActualFromStation
        {
            get { return this.GetProperty(ActualFromStationProperty); }
            set { this.SetProperty(ActualFromStationProperty, value); }
        }
        #endregion

        #region 建议目标站台 SuggestToStation
        /// <summary>
        /// 建议目标站台
        /// </summary>
        [Label("建议目标站台")]
        public static readonly Property<string> SuggestToStationProperty = P<TaskManagement>.Register(e => e.SuggestToStation);

        /// <summary>
        /// 建议目标站台
        /// </summary>
        public string SuggestToStation
        {
            get { return this.GetProperty(SuggestToStationProperty); }
            set { this.SetProperty(SuggestToStationProperty, value); }
        }
        #endregion

        #region 实际目标站台 ActualToStaion
        /// <summary>
        /// 实际目标站台
        /// </summary>
        [Label("实际目标站台")]
        public static readonly Property<string> ActualToStaionProperty = P<TaskManagement>.Register(e => e.ActualToStaion);

        /// <summary>
        /// 实际目标站台
        /// </summary>
        public string ActualToStaion
        {
            get { return this.GetProperty(ActualToStaionProperty); }
            set { this.SetProperty(ActualToStaionProperty, value); }
        }
        #endregion

        #region 关联单号 RelationOrderNo
        /// <summary>
        /// 关联单号
        /// </summary>
        [Label("关联单号")]
        public static readonly Property<string> RelationOrderNoProperty = P<TaskManagement>.Register(e => e.RelationOrderNo);

        /// <summary>
        /// 关联单号
        /// </summary>
        public string RelationOrderNo
        {
            get { return this.GetProperty(RelationOrderNoProperty); }
            set { this.SetProperty(RelationOrderNoProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<TaskManagement>.RegisterView(e => e.ItemCode, e => e.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<TaskManagement>.RegisterView(e => e.ItemName, e => e.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<TaskManagement>.RegisterView(e => e.ItemSpecificationModel, e => e.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<TaskManagement>.RegisterView(e => e.ItemUnitName, e => e.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { this.SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 来源库区 FromAreaCode
        /// <summary>
        /// 来源库区
        /// </summary>
        [Label("来源库区")]
        public static readonly Property<string> FromAreaCodeProperty = P<TaskManagement>.RegisterView(e => e.FromAreaCode, p => p.FromArea.Code);

        /// <summary>
        /// 来源库区
        /// </summary>
        public string FromAreaCode
        {
            get { return this.GetProperty(FromAreaCodeProperty); }
        }
        #endregion

        #region 建议批次 SuggestLotCode
        /// <summary>
        /// 建议批次
        /// </summary>
        [Label("建议批次")]
        public static readonly Property<string> SuggestLotCodeProperty = P<TaskManagement>.RegisterView(e => e.SuggestLotCode, p => p.SuggestLot.Code);

        /// <summary>
        /// 建议批次
        /// </summary>
        public string SuggestLotCode
        {
            get { return this.GetProperty(SuggestLotCodeProperty); }
        }
        #endregion

        #region 建议来源库位编码 SuggestFromLocCode
        /// <summary>
        /// 建议来源库位编码
        /// </summary>
        [Label("建议来源库位编码")]
        public static readonly Property<string> SuggestFromLocCodeProperty = P<TaskManagement>.RegisterView(e => e.SuggestFromLocCode, p => p.SuggestFromLoc.Code);

        /// <summary>
        /// 建议来源库位编码
        /// </summary>
        public string SuggestFromLocCode
        {
            get { return this.GetProperty(SuggestFromLocCodeProperty); }
        }
        #endregion

        #region 建议来源库位名称 SuggestFromLocName
        /// <summary>
        /// 建议来源库位名称
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> SuggestFromLocNameProperty = P<TaskManagement>.RegisterView(e => e.SuggestFromLocName, p => p.SuggestFromLoc.Name);

        /// <summary>
        /// 建议来源库位名称
        /// </summary>
        public string SuggestFromLocName
        {
            get { return this.GetProperty(SuggestFromLocNameProperty); }
        }
        #endregion

        #region 实际来源库位编码 ActualFromLocCode
        /// <summary>
        /// 实际来源库位编码
        /// </summary>
        [Label("实际来源库位编码")]
        public static readonly Property<string> ActualFromLocCodeProperty = P<TaskManagement>.RegisterView(e => e.ActualFromLocCode, p => p.ActualFromLoc.Code);

        /// <summary>
        /// 实际来源库位编码
        /// </summary>
        public string ActualFromLocCode
        {
            get { return this.GetProperty(ActualFromLocCodeProperty); }
        }
        #endregion

        #region 实际来源库位名称 ActualFromLocName
        /// <summary>
        /// 
        /// </summary>
        [Label("实际来源库位名称")]
        public static readonly Property<string> ActualFromLocNameProperty = P<TaskManagement>.RegisterView(e => e.ActualFromLocName, p => p.ActualFromLoc.Name);

        /// <summary>
        /// 
        /// </summary>
        public string ActualFromLocName
        {
            get { return this.GetProperty(ActualFromLocNameProperty); }
        }
        #endregion

        #region 建议目标库位编码 SuggestToLocCode
        /// <summary>
        /// 建议目标库位编码
        /// </summary>
        [Label("建议目标库位编码")]
        public static readonly Property<string> SuggestToLocCodeProperty = P<TaskManagement>.RegisterView(e => e.SuggestToLocCode, p => p.SuggestToLoc.Code);

        /// <summary>
        /// 建议目标库位编码
        /// </summary>
        public string SuggestToLocCode
        {
            get { return this.GetProperty(SuggestToLocCodeProperty); }
        }
        #endregion

        #region 建议目标库位名称 SuggestToLocName
        /// <summary>
        /// 建议目标库位名称
        /// </summary>
        [Label("建议目标库位名称")]
        public static readonly Property<string> SuggestToLocNameProperty = P<TaskManagement>.RegisterView(e => e.SuggestToLocName, p => p.SuggestToLoc.Name);

        /// <summary>
        /// 建议目标库位名称
        /// </summary>
        public string SuggestToLocName
        {
            get { return this.GetProperty(SuggestToLocNameProperty); }
        }
        #endregion

        #region 实际目标库位编码 ActualToLocCode
        /// <summary>
        /// 实际目标库位编码
        /// </summary>
        [Label("实际目标库位编码")]
        public static readonly Property<string> ActualToLocCodeProperty = P<TaskManagement>.RegisterView(e => e.ActualToLocCode, p => p.ActualToLoc.Code);

        /// <summary>
        /// 实际目标库位编码
        /// </summary>
        public string ActualToLocCode
        {
            get { return this.GetProperty(ActualToLocCodeProperty); }
        }
        #endregion

        #region 实际目标库位名称 ActualToLocName
        /// <summary>
        /// 实际目标库位名称
        /// </summary>
        [Label("实际目标库位名称")]
        public static readonly Property<string> ActualToLocNameProperty = P<TaskManagement>.RegisterView(e => e.ActualToLocName, p => p.ActualToLoc.Name);

        /// <summary>
        /// 实际目标库位名称
        /// </summary>
        public string ActualToLocName
        {
            get { return this.GetProperty(ActualToLocNameProperty); }
        }
        #endregion

        #region 单据小类名称 TransactionName
        /// <summary>
        /// 单据小类名称
        /// </summary>
        [Label("单据小类名称")]
        public static readonly Property<string> TransactionNameProperty = P<TaskManagement>.RegisterView(e => e.TransactionName, p => p.Transaction.Name);

        /// <summary>
        /// 单据小类名称
        /// </summary>
        public string TransactionName
        {
            get { return this.GetProperty(TransactionNameProperty); }
        }
        #endregion

        #region 任务组号 TaskGroupNo
        /// <summary>
        /// 任务组号
        /// </summary>
        [Label("任务组号")]
        public static readonly Property<string> TaskGroupNoProperty = P<TaskManagement>.RegisterView(e => e.TaskGroupNo, p => p.TaskGroup.No);

        /// <summary>
        /// 任务组号
        /// </summary>
        public string TaskGroupNo
        {
            get { return this.GetProperty(TaskGroupNoProperty); }
        }
        #endregion

        #region 来源仓库编码 SourceWarehoueCode
        /// <summary>
        /// 来源仓库编码
        /// </summary>
        [Label("来源仓库编码")]
        public static readonly Property<string> SourceWarehoueCodeProperty = P<TaskManagement>.RegisterView(e => e.SourceWarehoueCode, p => p.FromWarehouse.Code);

        /// <summary>
        /// 来源仓库编码
        /// </summary>
        public string SourceWarehoueCode
        {
            get { return this.GetProperty(SourceWarehoueCodeProperty); }
        }
        #endregion

        #region 来源仓库名称 SourceWarehouseName
        /// <summary>
        /// 来源仓库名称
        /// </summary>
        [Label("来源仓库名称")]
        public static readonly Property<string> SourceWarehouseNameProperty = P<TaskManagement>.RegisterView(e => e.SourceWarehouseName, p => p.FromWarehouse.Name);

        /// <summary>
        /// 来源仓库名称
        /// </summary>
        public string SourceWarehouseName
        {
            get { return this.GetProperty(SourceWarehouseNameProperty); }
        }
        #endregion

        #region 目标仓库编码 ToWarehouseCode
        /// <summary>
        /// 目标仓库编码
        /// </summary>
        [Label("目标仓库编码")]
        public static readonly Property<string> ToWarehouseCodeProperty = P<TaskManagement>.RegisterView(e => e.ToWarehouseCode, p => p.ToWarehouse.Code);

        /// <summary>
        /// 目标仓库编码
        /// </summary>
        public string ToWarehouseCode
        {
            get { return this.GetProperty(ToWarehouseCodeProperty); }
        }
        #endregion

        #region 目标仓库 ToWarehouseName
        /// <summary>
        /// 目标仓库
        /// </summary>
        [Label("目标仓库名称")]
        public static readonly Property<string> ToWarehouseNameProperty = P<TaskManagement>.RegisterView(e => e.ToWarehouseName, p => p.ToWarehouse.Name);

        /// <summary>
        /// 目标仓库
        /// </summary>
        public string ToWarehouseName
        {
            get { return this.GetProperty(ToWarehouseNameProperty); }
        }
        #endregion

        #region 建议来源库位库区是否允许人工上架 FromIsAllowManualGrounding
        /// <summary>
        /// 建议来源库位库区是否允许人工上架
        /// </summary>
        [Label("建议来源库位库区是否允许人工上架")]
        public static readonly Property<bool> FromIsAllowManualGroundingProperty = P<TaskManagement>.RegisterView(e => e.FromIsAllowManualGrounding, p => p.SuggestFromLoc.Area.IsAllowManualGrounding);

        /// <summary>
        /// 建议来源库位库区是否允许人工上架
        /// </summary>
        public bool FromIsAllowManualGrounding
        {
            get { return this.GetProperty(FromIsAllowManualGroundingProperty); }
        }
        #endregion

        #region 建议目标库位库区是否允许人工上架 ToIsAllowManualGrounding
        /// <summary>
        /// 建议目标库位库区是否允许人工上架
        /// </summary>
        [Label("建议目标库位库区是否允许人工上架")]
        public static readonly Property<bool> ToIsAllowManualGroundingProperty = P<TaskManagement>.RegisterView(e => e.ToIsAllowManualGrounding, p => p.SuggestToLoc.Area.IsAllowManualGrounding);

        /// <summary>
        /// 建议目标库位库区是否允许人工上架
        /// </summary>
        public bool ToIsAllowManualGrounding
        {
            get { return this.GetProperty(ToIsAllowManualGroundingProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 任务管理 实体配置
    /// </summary>
    internal class TaskManagementConfig : EntityConfig<TaskManagement>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_MANAGEMENT").MapAllProperties();
            Meta.Property(TaskManagement.StateDescProperty).ColumnMeta.HasLength(4000);
            Meta.Property(TaskManagement.OperationRequiresProperty).ColumnMeta.HasLength(4000);
            Meta.Property(TaskManagement.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(TaskManagement.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.IndexGroupOnProperties(TaskManagement.TaskSourceIdProperty, TaskManagement.BillDtlIdProperty);
            Meta.IndexGroupOnProperties(TaskManagement.FromWarehouseIdProperty, TaskManagement.EndDateProperty);
            Meta.Property(TaskManagement.SuggestToLpnProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}