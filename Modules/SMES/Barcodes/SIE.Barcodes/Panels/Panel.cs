using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码
    /// </summary>
    [RootEntity, Serializable]
    [Label("拼板码")]
    public partial class Panel : DataEntity
    {
        public Panel()
        {
            IsPending = false;
        }

        #region 拼板码号 Code
        /// <summary>
        /// 拼板码号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("拼板码号")]
        public static readonly Property<string> CodeProperty = P<Panel>.Register(e => e.Code);

        /// <summary>
        /// 拼板码号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 打印日期 PrintDate
        /// <summary>
        /// 打印日期
        /// </summary>
        [Label("打印日期")]
        public static readonly Property<DateTime> PrintDateProperty = P<Panel>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime PrintDate
        {
            get { return GetProperty(PrintDateProperty); }
            set { SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 打印次数 PrintQty
        /// <summary>
        /// 打印次数
        /// </summary>
        [Label("打印次数")]
        public static readonly Property<decimal> PrintQtyProperty = P<Panel>.Register(e => e.PrintQty);

        /// <summary>
        /// 打印次数
        /// </summary>
        public decimal PrintQty
        {
            get { return GetProperty(PrintQtyProperty); }
            set { SetProperty(PrintQtyProperty, value); }
        }
        #endregion

        //#region 数量 Qty
        ///// <summary>
        ///// 数量
        ///// </summary>
        //[Label("数量")]
        //public static readonly Property<int> QtyProperty = P<Panel>.Register(e => e.Qty);

        ///// <summary>
        ///// 数量
        ///// </summary>
        //public int Qty
        //{
        //    get { return GetProperty(QtyProperty); }
        //    set { SetProperty(QtyProperty, value); }
        //}
        //#endregion

        #region 叉板数 ForkPlateQty
        /// <summary>
        /// 叉板数
        /// </summary>
        [MinValue(0)]
        [Label("叉板数")]
        public static readonly Property<int> ForkPlateQtyProperty = P<Panel>.Register(e => e.ForkPlateQty);

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty
        {
            get { return GetProperty(ForkPlateQtyProperty); }
            set { SetProperty(ForkPlateQtyProperty, value); }
        }
        #endregion

        #region 叉板 ForkPlate
        /// <summary>
        /// 叉板，多个叉板使用空格隔开
        /// </summary>
        [Label("叉板")]
        public static readonly Property<string> ForkPlateProperty = P<Panel>.Register(e => e.ForkPlate);

        /// <summary>
        /// 叉板
        /// </summary>
        public string ForkPlate
        {
            get { return GetProperty(ForkPlateProperty); }
            set { SetProperty(ForkPlateProperty, value); }
        }
        #endregion

        #region 绑定数量 BindQty
        /// <summary>
        /// 绑定数量
        /// </summary>
        [Label("绑定数量")]
        public static readonly Property<int> BindQtyProperty = P<Panel>.Register(e => e.BindQty);

        /// <summary>
        /// 绑定数量
        /// </summary>
        public int BindQty
        {
            get { return GetProperty(BindQtyProperty); }
            set { SetProperty(BindQtyProperty, value); }
        }
        #endregion

        #region 是否报废 IsScrap
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrapProperty = P<Panel>.Register(e => e.IsScrap);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrap
        {
            get { return GetProperty(IsScrapProperty); }
            set { SetProperty(IsScrapProperty, value); }
        }
        #endregion

        #region 是否挂起 IsPending
        /// <summary>
        /// 是否挂起
        /// </summary>
        [Label("是否挂起")]
        public static readonly Property<bool?> IsPendingProperty = P<Panel>.Register(e => e.IsPending);

        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool? IsPending
        {
            get { return GetProperty(IsPendingProperty); }
            set { SetProperty(IsPendingProperty, value); }
        }
        #endregion

        #region 报废原因 ScrapReason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ScrapReasonProperty = P<Panel>.Register(e => e.ScrapReason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason
        {
            get { return GetProperty(ScrapReasonProperty); }
            set { SetProperty(ScrapReasonProperty, value); }
        }
        #endregion

        #region 条码范围 Range
        /// <summary>
        /// 条码范围Id
        /// </summary>
        [Label("条码范围")]
        public static readonly IRefIdProperty RangeIdProperty = P<Panel>.RegisterRefId(e => e.RangeId, ReferenceType.Normal);

        /// <summary>
        /// 条码范围Id
        /// </summary>
        public double RangeId
        {
            get { return (double)GetRefId(RangeIdProperty); }
            set { SetRefId(RangeIdProperty, value); }
        }

        /// <summary>
        /// 条码范围
        /// </summary>
        public static readonly RefEntityProperty<PanelRange> RangeProperty = P<Panel>.RegisterRef(e => e.Range, RangeIdProperty);

        /// <summary>
        /// 条码范围
        /// </summary>
        public PanelRange Range
        {
            get { return GetRefEntity(RangeProperty); }
            set { SetRefEntity(RangeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<Panel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<Panel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<PanelState> StateProperty = P<Panel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public PanelState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 打印人 PrintBy
        /// <summary>
        /// 打印人Id
        /// </summary>
        [Label("打印人")]
        public static readonly IRefIdProperty PrintByIdProperty = P<Panel>.RegisterRefId(e => e.PrintById, ReferenceType.Normal);

        /// <summary>
        /// 打印人Id
        /// </summary>
        public double PrintById
        {
            get { return (double)GetRefId(PrintByIdProperty); }
            set { SetRefId(PrintByIdProperty, value); }
        }

        /// <summary>
        /// 打印人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrintByProperty = P<Panel>.RegisterRef(e => e.PrintBy, PrintByIdProperty);

        /// <summary>
        /// 打印人
        /// </summary>
        public Employee PrintBy
        {
            get { return GetRefEntity(PrintByProperty); }
            set { SetRefEntity(PrintByProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 开始条码 StartSn
        /// <summary>
        /// 开始条码
        /// </summary>
        [Label("开始条码")]
        public static readonly Property<string> StartSnProperty = P<Panel>.RegisterView(e => e.StartSn, p => p.Range.StartNo);

        /// <summary>
        /// 开始条码
        /// </summary>
        public string StartSn
        {
            get { return this.GetProperty(StartSnProperty); }
        }
        #endregion

        #region 结束条码 EndSn
        /// <summary>
        /// 结束条码
        /// </summary>
        [Label("结束条码")]
        public static readonly Property<string> EndSnProperty = P<Panel>.RegisterView(e => e.EndSn, p => p.Range.EndNo);

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get { return this.GetProperty(EndSnProperty); }
        }
        #endregion

        #region 工单号 WONo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WONoProperty = P<Panel>.RegisterView(e => e.WONo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WONo
        {
            get { return this.GetProperty(WONoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 拼板码 实体配置
    /// </summary>
    internal class PanelConfig : EntityConfig<Panel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_PANEL").MapAllProperties();
            Meta.Property(Panel.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(Panel.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}