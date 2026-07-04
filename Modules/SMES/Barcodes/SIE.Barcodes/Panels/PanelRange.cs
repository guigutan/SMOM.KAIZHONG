using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码范围
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PanelRangeCriteria))]
    [Label("拼板码范围")]
    public partial class PanelRange : DataEntity
    {
        #region 开始拼板码号 StartNo
        /// <summary>
        /// 开始拼板码号
        /// </summary>
        [Label("开始拼板码号")]
        public static readonly Property<string> StartNoProperty = P<PanelRange>.Register(e => e.StartNo);

        /// <summary>
        /// 开始拼板码号
        /// </summary>
        public string StartNo
        {
            get { return GetProperty(StartNoProperty); }
            set { SetProperty(StartNoProperty, value); }
        }
        #endregion

        #region 结束拼板码号 EndNo
        /// <summary>
        /// 结束拼板码号
        /// </summary>
        [Label("结束拼板码号")]
        public static readonly Property<string> EndNoProperty = P<PanelRange>.Register(e => e.EndNo);

        /// <summary>
        /// 结束拼板码号
        /// </summary>
        public string EndNo
        {
            get { return GetProperty(EndNoProperty); }
            set { SetProperty(EndNoProperty, value); }
        }
        #endregion
        #region 打印数量 PrintQty
        /// <summary>
        /// 打印数量
        /// </summary>
        [MinValue(0)]
        [Label("打印数量")]
        public static readonly Property<int> PrintQtyProperty = P<PanelRange>.Register(e => e.PrintQty);

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
        public static readonly Property<int> ScrapedQtyProperty = P<PanelRange>.Register(e => e.ScrapedQty);

        /// <summary>
        /// 作废数量
        /// </summary>
        public int ScrapedQty
        {
            get { return GetProperty(ScrapedQtyProperty); }
            set { SetProperty(ScrapedQtyProperty, value); }
        }
        #endregion
        #region 领用状态 State
        /// <summary>
        /// 领用状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState> StateProperty = P<PanelRange>.Register(e => e.State);

        /// <summary>
        /// 领用状态
        /// </summary>
        public ReceiveState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 领用时间 ReceiveDate
        /// <summary>
        /// 领用时间
        /// </summary>
        [Label("领用时间")]
        public static readonly Property<DateTime?> ReceiveDateProperty = P<PanelRange>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? ReceiveDate
        {
            get { return GetProperty(ReceiveDateProperty); }
            set { SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 领用人 ReceiveBy
        /// <summary>
        /// 领用人Id
        /// </summary>
        [Label("领用人")]
        public static readonly IRefIdProperty ReceiveByIdProperty = P<PanelRange>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ReceiveByProperty = P<PanelRange>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 领用人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return GetRefEntity(ReceiveByProperty); }
            set { SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<PanelRange>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<PanelRange>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 工单号 WONo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WONoProperty = P<PanelRange>.RegisterView(e => e.WONo, p => p.WorkOrder.No);

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
        public static readonly Property<string> ProductNameProperty = P<PanelRange>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

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
    /// 拼板码范围 实体配置
    /// </summary>
    internal class PanelRangeConfig : EntityConfig<PanelRange>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_PANLE_RANGE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}