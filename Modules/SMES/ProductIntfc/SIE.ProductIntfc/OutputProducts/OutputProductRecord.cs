using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 副产品记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("副产品记录")]
    public partial class OutputProductRecord : DataEntity
    {
        #region 入库工单 OutputProduct
        /// <summary>
        /// 入库工单Id
        /// </summary>
        public static readonly IRefIdProperty StorageWorkOrderIdProperty = P<OutputProductRecord>.RegisterRefId(e => e.StorageWorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 入库工单Id
        /// </summary>
        public double StorageWorkOrderId
        {
            get { return (double)GetRefId(StorageWorkOrderIdProperty); }
            set { SetRefId(StorageWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 入库工单
        /// </summary>
        public static readonly RefEntityProperty<OutputProduct> StorageWorkOrderProperty = P<OutputProductRecord>.RegisterRef(e => e.StorageWorkOrder, StorageWorkOrderIdProperty);

        /// <summary>
        /// 入库工单
        /// </summary>
        public OutputProduct StorageWorkOrder
        {
            get { return GetRefEntity(StorageWorkOrderProperty); }
            set { SetRefEntity(StorageWorkOrderProperty, value); }
        }
        #endregion

        #region 产出类型 OutPutType
        /// <summary>
        /// 产出类型
        /// </summary>
        [Label("产出类型")]
        public static readonly Property<OutputListType> OutPutTypeProperty = P<OutputProductRecord>.Register(e => e.OutPutType);

        /// <summary>
        /// 产出类型
        /// </summary>
        public OutputListType OutPutType
        {
            get { return this.GetProperty(OutPutTypeProperty); }
            set { this.SetProperty(OutPutTypeProperty, value); }
        }
        #endregion
                
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<OutputProductRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<OutputProductRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<OutputProductRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 生产批次 Lot
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> LotProperty = P<OutputProductRecord>.Register(e => e.Lot);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 已收货数量 SubmitQty
        /// <summary>
        /// 已收货数量
        /// </summary>
        [Label("已收货数量")]
        public static readonly Property<decimal> SubmitQtyProperty = P<OutputProductRecord>.Register(e => e.SubmitQty);

        /// <summary>
        /// 已收货数量
        /// </summary>
        public decimal SubmitQty
        {
            get { return this.GetProperty(SubmitQtyProperty); }
            set { this.SetProperty(SubmitQtyProperty, value); }
        }
        #endregion

        #region 是否已上传 UploadFlag
        /// <summary>
        /// 是否已上传
        /// </summary>
        [Label("是否已上传")]
        public static readonly Property<bool?> UploadFlagProperty = P<OutputProductRecord>.Register(e => e.UploadFlag);

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool? UploadFlag
        {
            get { return this.GetProperty(UploadFlagProperty); }
            set { this.SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 上传结果 UploadResult
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<OutputProductRecord>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion

        #region 返回物料凭证 Mblnr
        /// <summary>
        /// 返回物料凭证
        /// </summary>
        [Label("返回物料凭证")]
        public static readonly Property<string> MblnrProperty = P<OutputProductRecord>.Register(e => e.Mblnr);

        /// <summary>
        /// 返回物料凭证
        /// </summary>
        public string Mblnr
        {
            get { return this.GetProperty(MblnrProperty); }
            set { this.SetProperty(MblnrProperty, value); }
        }
        #endregion

        #region 返回物料凭证年度 Mjahr
        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        [Label("返回物料凭证年度")]
        public static readonly Property<string> MjahrProperty = P<OutputProductRecord>.Register(e => e.Mjahr);

        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        public string Mjahr
        {
            get { return this.GetProperty(MjahrProperty); }
            set { this.SetProperty(MjahrProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<OutputProductRecord>.RegisterView(e => e.WorkOrderNo, p => p.StorageWorkOrder.No);

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
        public static readonly Property<string> ProductCodeProperty = P<OutputProductRecord>.RegisterView(e => e.ProductCode, p => p.StorageWorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<OutputProductRecord>.RegisterView(e => e.ProductName, p => p.StorageWorkOrder.Product.Name);

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
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<OutputProductRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

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
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<OutputProductRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ShortDescriptionProperty = P<OutputProductRecord>.RegisterView(e => e.ShortDescription, p => p.Item.ShortDescription);

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

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class OutputProductRecordEntityConfig : EntityConfig<OutputProductRecord>
    {
        /// <summary>
        /// 数据表Config
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("OUTPUT_PRO_DTL_RECORD").MapAllProperties();
            Meta.Property(OutputProductRecord.SubmitQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}