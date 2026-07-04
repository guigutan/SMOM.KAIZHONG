using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 序列号明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("序列号明细")]
    public partial class SparePartReceiveSn : DataEntity
    {
        #region 序列号编码 Sn
        /// <summary>
        /// 序列号编码
        /// </summary>
        [Label("序列号编码")]
        [Required]
        public static readonly Property<string> SnProperty = P<SparePartReceiveSn>.Register(e => e.Sn);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<SparePartReceiveSn>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 备件接收明细 SparePartReceiveDetail
        /// <summary>
        /// 备件接收明细Id
        /// </summary>
        public static readonly IRefIdProperty SparePartReceiveDetailIdProperty = P<SparePartReceiveSn>.RegisterRefId(e => e.SparePartReceiveDetailId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<SparePartReceiveDetail> SparePartReceiveDetailProperty = P<SparePartReceiveSn>.RegisterRef(e => e.SparePartReceiveDetail, SparePartReceiveDetailIdProperty);

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
        public static readonly Property<int> LineNoProperty = P<SparePartReceiveSn>.RegisterView(e => e.LineNo, p => p.SparePartReceiveDetail.LineNo);

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
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartReceiveSn>.RegisterView(e => e.PurchaseOrderNo, p => p.SparePartReceiveDetail.PurchaseOrder.OrderNo);

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
        public static readonly Property<int?> PurchaseOrderItemLineNoProperty = P<SparePartReceiveSn>.RegisterView(e => e.PurchaseOrderItemLineNo, p => p.SparePartReceiveDetail.PurchaseOrderItem.LineNo);

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
        public static readonly Property<string> SupplierCodeProperty = P<SparePartReceiveSn>.RegisterView(e => e.SupplierCode, p => p.SparePartReceiveDetail.Supplier.Code);

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
        public static readonly Property<string> SupplierNameProperty = P<SparePartReceiveSn>.RegisterView(e => e.SupplierName, p => p.SparePartReceiveDetail.Supplier.Name);

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
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<SparePartReceiveSn>.RegisterView(e => e.PurchaseObjectType, p => p.SparePartReceiveDetail.PurchaseOrder.PurchaseObjectType);

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
        public static readonly Property<string> SparePartCodeProperty = P<SparePartReceiveSn>.RegisterView(e => e.SparePartCode, p => p.SparePartReceiveDetail.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<SparePartReceiveSn>.RegisterView(e => e.SparePartName, p => p.SparePartReceiveDetail.SparePart.SparePartName);

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
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartReceiveSn>.RegisterView(e => e.ControlMethod, p => p.SparePartReceiveDetail.SparePart.ControlMethod);

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
    /// 序列号明细 实体配置
    /// </summary>
    internal class SparePartReceiveSnConfig : EntityConfig<SparePartReceiveSn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_RECV_SN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}