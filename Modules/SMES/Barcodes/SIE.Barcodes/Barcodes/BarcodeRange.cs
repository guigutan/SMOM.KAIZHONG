using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码领用
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BarcodeRangeCriteria))]
    [Label("条码领用")]
    [DisplayMember(nameof(BarcodeRange.Id))]
    public partial class BarcodeRange : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeRange()
        {
            State = ReceiveState.NoReceive;
            PrintQty = 0;
            ScrapedQty = 0;
        }

        #region 打印数量 PrintQty
        /// <summary>
        /// 打印数量
        /// </summary>
        [MinValue(0)]
        [Label("打印数量")]
        public static readonly Property<int> PrintQtyProperty = P<BarcodeRange>.Register(e => e.PrintQty);

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty
        {
            get { return GetProperty(PrintQtyProperty); }
            set { SetProperty(PrintQtyProperty, value); }
        }
        #endregion

        #region 作废数量 ScrapedQty
        /// <summary>
        /// 作废数量
        /// </summary>
        [MinValue(0)]
        [Label("作废数量")]
        public static readonly Property<int> ScrapedQtyProperty = P<BarcodeRange>.Register(e => e.ScrapedQty);

        /// <summary>
        /// 作废数量
        /// </summary>
        public int ScrapedQty
        {
            get { return GetProperty(ScrapedQtyProperty); }
            set { SetProperty(ScrapedQtyProperty, value); }
        }
        #endregion

        #region 领用时间 ReceiveDate
        /// <summary>
        /// 领用时间
        /// </summary>
        [Label("领用时间")]
        public static readonly Property<DateTime?> ReceiveDateProperty = P<BarcodeRange>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? ReceiveDate
        {
            get { return GetProperty(ReceiveDateProperty); }
            set { SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 开始条码 StartSn
        /// <summary>
        /// 开始条码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("开始条码")]
        public static readonly Property<string> StartSnProperty = P<BarcodeRange>.Register(e => e.StartSn);

        /// <summary>
        /// 开始条码
        /// </summary>
        public string StartSn
        {
            get { return GetProperty(StartSnProperty); }
            set { SetProperty(StartSnProperty, value); }
        }
        #endregion

        #region 结束条码 EndSn
        /// <summary>
        /// 结束条码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("结束条码")]
        public static readonly Property<string> EndSnProperty = P<BarcodeRange>.Register(e => e.EndSn);

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get { return GetProperty(EndSnProperty); }
            set { SetProperty(EndSnProperty, value); }
        }
        #endregion

        #region 领用人 ReceiveBy
        /// <summary>
        /// 领用人Id
        /// </summary>
        [Label("领用人")]
        public static readonly IRefIdProperty ReceiveByIdProperty = P<BarcodeRange>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 领用人Id
        /// </summary>
        public double? ReceiveById
        {
            get { return (double?)GetRefNullableId(ReceiveByIdProperty); }
            set { SetRefNullableId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 领用人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiveByProperty = P<BarcodeRange>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 领用人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return GetRefEntity(ReceiveByProperty); }
            set { SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 编码规则 Rule
        /// <summary>
        /// 编码规则Id
        /// </summary> 
        [Label("编码规则")]
        public static readonly IRefIdProperty RuleIdProperty = P<BarcodeRange>.RegisterRefId(e => e.RuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double? RuleId
        {
            get { return (double?)GetRefNullableId(RuleIdProperty); }
            set { SetRefNullableId(RuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> RuleProperty = P<BarcodeRange>.RegisterRef(e => e.Rule, RuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule Rule
        {
            get { return GetRefEntity(RuleProperty); }
            set { SetRefEntity(RuleProperty, value); }
        }
        #endregion

        #region 领用状态 State
        /// <summary>
        /// 领用状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState> StateProperty = P<BarcodeRange>.Register(e => e.State);

        /// <summary>
        /// 领用状态
        /// </summary>
        public ReceiveState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Required]
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BarcodeRange>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BarcodeRange>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary> 
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<BarcodeRange>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double?)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty =
            P<BarcodeRange>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 工单号 WONo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WONoProperty = P<BarcodeRange>.RegisterView(e => e.WONo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WONo
        {
            get { return this.GetProperty(WONoProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<BarcodeRange>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 条码领用 实体配置
    /// </summary>
    internal class BarcodeRangeConfig : EntityConfig<BarcodeRange>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_RANGE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}