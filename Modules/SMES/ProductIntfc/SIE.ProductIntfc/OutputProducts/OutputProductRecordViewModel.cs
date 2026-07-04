using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 副产品记录ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("副产品记录ViewModel")]
    public partial class OutputProductRecordViewModel : ViewModel
    {
        #region 入库工单 OutputProduct
        /// <summary>
        /// 入库工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<OutputProductRecordViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 入库工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 入库工单
        /// </summary>
        public static readonly RefEntityProperty<OutputProduct> WorkOrderProperty = P<OutputProductRecordViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 入库工单
        /// </summary>
        public OutputProduct WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
                        
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<OutputProductRecordViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<OutputProductRecordViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 需求量 Qty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> QtyProperty = P<OutputProductRecordViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 已收货数量 SubmitQty
        /// <summary>
        /// 已收货数量
        /// </summary>
        [Label("已收货数量")]
        public static readonly Property<decimal> SubmitQtyProperty = P<OutputProductRecordViewModel>.Register(e => e.SubmitQty);

        /// <summary>
        /// 已收货数量
        /// </summary>
        public decimal SubmitQty
        {
            get { return this.GetProperty(SubmitQtyProperty); }
            set { this.SetProperty(SubmitQtyProperty, value); }
        }
        #endregion

        #region 副产品重量 InputQty
        /// <summary>
        /// 副产品重量
        /// </summary>
        [Label("副产品重量")]
        public static readonly Property<decimal> InputQtyProperty = P<OutputProductRecordViewModel>.Register(e => e.InputQty);

        /// <summary>
        /// 副产品重量
        /// </summary>
        public decimal InputQty
        {
            get { return this.GetProperty(InputQtyProperty); }
            set { this.SetProperty(InputQtyProperty, value); }
        }
        #endregion

        #region 总重量 TotalQty
        /// <summary>
        /// 总重量
        /// </summary>
        [Label("总重量")]
        public static readonly Property<decimal> TotalQtyProperty = P<OutputProductRecordViewModel>.Register(e => e.TotalQty);

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalQty
        {
            get { return this.GetProperty(TotalQtyProperty); }
            set { this.SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<OutputProductRecordViewModel>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }

        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<OutputProductRecordViewModel>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }

        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<OutputProductRecordViewModel>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }

        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("副产品名称")]
        public static readonly Property<string> ItemNameProperty = P<OutputProductRecordViewModel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion
        
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("副产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<OutputProductRecordViewModel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<OutputProductRecordViewModel>.RegisterView(e => e.ShortDescription, p => p.Item.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #endregion
    }

}