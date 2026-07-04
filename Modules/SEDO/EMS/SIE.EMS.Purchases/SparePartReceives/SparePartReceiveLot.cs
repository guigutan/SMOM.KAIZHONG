using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 批次明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次明细")]
    public partial class SparePartReceiveLot : DataEntity
    {
        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> LotNoProperty = P<SparePartReceiveLot>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<int> QtyProperty = P<SparePartReceiveLot>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 备件接收明细 SparePartReceiveDetail
        /// <summary>
        /// 备件接收明细Id
        /// </summary>
        public static readonly IRefIdProperty SparePartReceiveDetailIdProperty = P<SparePartReceiveLot>.RegisterRefId(e => e.SparePartReceiveDetailId, ReferenceType.Parent);

        /// <summary>
        /// 备件接收明细Id
        /// </summary>
        public double SparePartReceiveDetailId
        {
            get { return (double)GetRefId(SparePartReceiveDetailIdProperty); }
            set { SetRefId(SparePartReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件接收明细
        /// </summary>
        public static readonly RefEntityProperty<SparePartReceiveDetail> SparePartReceiveDetailProperty = P<SparePartReceiveLot>.RegisterRef(e => e.SparePartReceiveDetail, SparePartReceiveDetailIdProperty);

        /// <summary>
        /// 备件接收明细
        /// </summary>
        public SparePartReceiveDetail SparePartReceiveDetail
        {
            get { return GetRefEntity(SparePartReceiveDetailProperty); }
            set { SetRefEntity(SparePartReceiveDetailProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<SparePartReceiveLot>.RegisterView(e => e.LineNo, p => p.SparePartReceiveDetail.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartReceiveLot>.RegisterView(e => e.PurchaseOrderNo, p => p.SparePartReceiveDetail.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
        }
        #endregion

        #region 采购单行号 PurchaseOrderItemLineNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<int?> PurchaseOrderItemLineNoProperty = P<SparePartReceiveLot>.RegisterView(e => e.PurchaseOrderItemLineNo, p => p.SparePartReceiveDetail.PurchaseOrderItem.LineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public int? PurchaseOrderItemLineNo
        {
            get { return this.GetProperty(PurchaseOrderItemLineNoProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<SparePartReceiveLot>.RegisterView(e => e.SupplierCode, p => p.SparePartReceiveDetail.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<SparePartReceiveLot>.RegisterView(e => e.SupplierName, p => p.SparePartReceiveDetail.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<SparePartReceiveLot>.RegisterView(e => e.PurchaseObjectType, p => p.SparePartReceiveDetail.PurchaseOrder.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType? PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartReceiveLot>.RegisterView(e => e.SparePartCode, p => p.SparePartReceiveDetail.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartReceiveLot>.RegisterView(e => e.SparePartName, p => p.SparePartReceiveDetail.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartReceiveLot>.RegisterView(e => e.ControlMethod, p => p.SparePartReceiveDetail.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 批次明细 实体配置
    /// </summary>
    internal class SparePartReceiveLotConfig : EntityConfig<SparePartReceiveLot>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_RECV_LOT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}