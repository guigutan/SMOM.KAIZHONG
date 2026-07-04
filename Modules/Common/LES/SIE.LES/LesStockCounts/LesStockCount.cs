using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Inventory.Task;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LesStockCountCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [Label("线边仓盘点单")]
    public partial class LesStockCount : DataEntity
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("单号")]
        [MaxLength(80)]
        public static readonly Property<string> NoProperty = P<LesStockCount>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<LesStockCount>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 来源单号 SourceBillNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceBillNoProperty = P<LesStockCount>.Register(e => e.SourceBillNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceBillNo
        {
            get { return GetProperty(SourceBillNoProperty); }
            set { SetProperty(SourceBillNoProperty, value); }
        }
        #endregion

        #region 任务优先级 TaskLevel
        /// <summary>
        /// 任务优先级
        /// </summary>
        [Label("任务优先级")]
        public static readonly Property<TaskLevel?> TaskLevelProperty = P<LesStockCount>.Register(e => e.TaskLevel);

        /// <summary>
        /// 任务优先级
        /// </summary>
        public TaskLevel? TaskLevel
        {
            get { return GetProperty(TaskLevelProperty); }
            set { SetProperty(TaskLevelProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> SourceTypeProperty = P<LesStockCount>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<LesCountState> StateProperty = P<LesStockCount>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public LesCountState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 盘点范围列表 LesStockCountRangeList
        /// <summary>
        /// 盘点范围列表
        /// </summary>
        [Label("盘点范围")]
        public static readonly ListProperty<EntityList<LesStockCountRange>> LesStockCountRangeListProperty = P<LesStockCount>.RegisterList(e => e.LesStockCountRangeList);
        /// <summary>
        /// 盘点范围列表
        /// </summary>
        public EntityList<LesStockCountRange> LesStockCountRangeList
        {
            get { return this.GetLazyList(LesStockCountRangeListProperty); }
        }
        #endregion

        #region 盘点结果 LesStockCountResult
        /// <summary>
        /// 盘点结果
        /// </summary>
        [Label("盘点结果")]
        public static readonly Property<LesStockCountResult?> StockCountResultProperty = P<LesStockCount>.Register(e => e.LesStockCountResult);

        /// <summary>
        /// 盘点结果
        /// </summary>
        public LesStockCountResult? LesStockCountResult
        {
            get { return GetProperty(StockCountResultProperty); }
            set { SetProperty(StockCountResultProperty, value); }
        }
        #endregion

        #region 指派操作员 Operator
        /// <summary>
        /// 指派操作员Id
        /// </summary>
        [Label("指派操作员")]
        public static readonly IRefIdProperty OperatorIdProperty = P<LesStockCount>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 指派操作员Id
        /// </summary>
        public double? OperatorId
        {
            get { return (double?)GetRefNullableId(OperatorIdProperty); }
            set { SetRefNullableId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 指派操作员
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty = P<LesStockCount>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 指派操作员
        /// </summary>
        public Employee Operator
        {
            get { return GetRefEntity(OperatorProperty); }
            set { SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 指派操作员 OperatorName
        /// <summary>
        /// 指派操作员
        /// </summary>
        [Label("指派操作员")]
        public static readonly Property<string> OperatorNameProperty = P<LesStockCount>.RegisterView(e => e.OperatorName, p => p.Operator.Name);

        /// <summary>
        /// 指派操作员
        /// </summary>
        public string OperatorName
        {
            get { return this.GetProperty(OperatorNameProperty); }
        }
        #endregion

        #region 审核人 AuditBy
        /// <summary>
        /// 审核人Id
        /// </summary>
        public static readonly IRefIdProperty AuditByIdProperty = P<LesStockCount>.RegisterRefId(e => e.AuditById, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double? AuditById
        {
            get { return (double?)GetRefNullableId(AuditByIdProperty); }
            set { SetRefNullableId(AuditByIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AuditByProperty = P<LesStockCount>.RegisterRef(e => e.AuditBy, AuditByIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee AuditBy
        {
            get { return GetRefEntity(AuditByProperty); }
            set { SetRefEntity(AuditByProperty, value); }
        }
        #endregion

        #region 审核人名称 AuditByName
        /// <summary>
        /// 审核人名称
        /// </summary>
        [Label("审核人")]
        public static readonly Property<string> AuditByNameProperty = P<LesStockCount>.RegisterView(e => e.AuditByName, p => p.AuditBy.Name);

        /// <summary>
        /// 审核人名称
        /// </summary>
        public string AuditByName
        {
            get { return this.GetProperty(AuditByNameProperty); }
        }
        #endregion

        #region 审核时间 AuditDate
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> AuditDateProperty = P<LesStockCount>.Register(e => e.AuditDate);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate
        {
            get { return GetProperty(AuditDateProperty); }
            set { SetProperty(AuditDateProperty, value); }
        }
        #endregion

        #region 盘点单明细列表 LesStockCountDetailList
        /// <summary>
        /// 盘点单明细列表
        /// </summary>
        [Label("盘点明细")]
        public static readonly ListProperty<EntityList<LesStockCountDetail>> StockCountDetailListProperty = P<LesStockCount>.RegisterList(e => e.LesStockCountDetailList);


        /// <summary>
        /// 盘点单明细列表
        /// </summary>
        public EntityList<LesStockCountDetail> LesStockCountDetailList
        {
            get { return this.GetLazyList(StockCountDetailListProperty); }
        }
        #endregion

        #region 盘点单工单调账记录列表 LesStockCountWorkOrderList
        /// <summary>
        /// 盘点单工单调账记录列表
        /// </summary>
        [Label("工单调账记录")]
        public static readonly ListProperty<EntityList<LesStockCountWorkOrder>> LesStockCountWorkOrderListProperty = P<LesStockCount>.RegisterList(e => e.LesStockCountWorkOrderList);


        /// <summary>
        /// 盘点单工单调账记录列表
        /// </summary>
        public EntityList<LesStockCountWorkOrder> LesStockCountWorkOrderList
        {
            get { return this.GetLazyList(LesStockCountWorkOrderListProperty); }
        }
        #endregion

    }
    /// <summary>
    /// 盘点单 实体配置
    /// </summary>
    internal class LesStockCountConfig : EntityConfig<LesStockCount>
    {
        /// <summary>
        /// 摘要:子类重写此方法，并完成对 Meta 属性的配置。 注意： * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("LES_STOCK_COUNT").MapAllProperties();                   
            Meta.EnablePhantoms();
        }
    }
}