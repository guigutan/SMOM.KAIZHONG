using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 发运单明细中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("发运单明细中间表")]
    public partial class ShippingOrderDetailInf : DownloadBaseEntity
    {
        #region 发运单单号 ShippingOrderNo
        /// <summary>
        /// 发运单单号
        /// </summary>
        [Label("发运单单号")]
        public static readonly Property<string> ShippingOrderNoProperty = P<ShippingOrderDetailInf>.Register(e => e.ShippingOrderNo);

        /// <summary>
        /// 发运单单号
        /// </summary>
        public string ShippingOrderNo
        {
            get { return GetProperty(ShippingOrderNoProperty); }
            set { SetProperty(ShippingOrderNoProperty, value); }
        }
        #endregion

        #region 预期数量 ExpectQty
        /// <summary>
        /// 预期数量
        /// </summary>
        [Label("预期数量")]
        public static readonly Property<decimal> ExpectQtyProperty = P<ShippingOrderDetailInf>.Register(e => e.ExpectQty);

        /// <summary>
        /// 预期数量
        /// </summary>
        public decimal ExpectQty
        {
            get { return GetProperty(ExpectQtyProperty); }
            set { SetProperty(ExpectQtyProperty, value); }
        }
        #endregion

        #region 相关单号 OrderNo
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> OrderNoProperty = P<ShippingOrderDetailInf>.Register(e => e.OrderNo);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo
        {
            get { return GetProperty(OrderNoProperty); }
            set { SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ShippingOrderDetailInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 指定库位 AppointStorageLocation
        /// <summary>
        /// 指定库位
        /// </summary>
        [Label("指定库位")]
        public static readonly Property<string> AppointStorageLocationProperty = P<ShippingOrderDetailInf>.Register(e => e.AppointStorageLocation);

        /// <summary>
        /// 指定库位
        /// </summary>
        public string AppointStorageLocation
        {
            get { return GetProperty(AppointStorageLocationProperty); }
            set { SetProperty(AppointStorageLocationProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ShippingOrderDetailInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 制单日期 BillDate
        /// <summary>
        /// 制单日期
        /// </summary>
        [Label("制单日期")]
        public static readonly Property<DateTime> BillDateProperty = P<ShippingOrderDetailInf>.Register(e => e.BillDate);

        /// <summary>
        /// 制单日期
        /// </summary>
        public DateTime BillDate
        {
            get { return GetProperty(BillDateProperty); }
            set { SetProperty(BillDateProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<ShippingOrderDetailInf>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 需求日期 RequestDate
        /// <summary>
        /// 需求日期
        /// </summary>
        [Label("需求日期")]
        public static readonly Property<DateTime?> RequestDateProperty = P<ShippingOrderDetailInf>.Register(e => e.RequestDate);

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate
        {
            get { return GetProperty(RequestDateProperty); }
            set { SetProperty(RequestDateProperty, value); }
        }
        #endregion

        #region 物料单位 ItemUnit
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("物料单位")]
        public static readonly Property<string> ItemUnitProperty = P<ShippingOrderDetailInf>.Register(e => e.ItemUnit);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit
        {
            get { return GetProperty(ItemUnitProperty); }
            set { SetProperty(ItemUnitProperty, value); }
        }
        #endregion

        #region 订单状态 OrderState
        /// <summary>
        /// 订单状态
        /// </summary>
        [Label("订单状态")]
        public static readonly Property<int> OrderStateProperty = P<ShippingOrderDetailInf>.Register(e => e.OrderState);

        /// <summary>
        /// 单据状态
        /// </summary>
        public int OrderState
        {
            get { return this.GetProperty(OrderStateProperty); }
            set { this.SetProperty(OrderStateProperty, value); }
        }
        #endregion

        #region 采购单号 PoNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PoNoProperty = P<ShippingOrderDetailInf>.Register(e => e.PoNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo
        {
            get { return this.GetProperty(PoNoProperty); }
            set { this.SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购订单行号 PoDetailLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单行号")]
        public static readonly Property<string> PoDetailLineNoProperty = P<ShippingOrderDetailInf>.Register(e => e.PoDetailLineNo);

        /// <summary>
        /// 采购订单行
        /// </summary>
        public string PoDetailLineNo
        {
            get { return this.GetProperty(PoDetailLineNoProperty); }
            set { this.SetProperty(PoDetailLineNoProperty, value); }
        }
        #endregion

        #region 相关发票 InvoiceNo
        /// <summary>
        /// 相关发票
        /// </summary>
        [Label("相关发票")]
        public static readonly Property<string> InvoiceNoProperty = P<ShippingOrderDetailInf>.Register(e => e.InvoiceNo);

        /// <summary>
        /// 相关发票
        /// </summary>
        public string InvoiceNo
        {
            get { return this.GetProperty(InvoiceNoProperty); }
            set { this.SetProperty(InvoiceNoProperty, value); }
        }
        #endregion

        #region 体积(CM³) Volume
        /// <summary>
        /// 体积(CM³)
        /// </summary>
        [Label("体积(CM³)")]
        public static readonly Property<decimal?> VolumeProperty = P<ShippingOrderDetailInf>.Register(e => e.Volume);

        /// <summary>
        /// 体积
        /// </summary>
        public decimal? Volume
        {
            get { return this.GetProperty(VolumeProperty); }
            set { this.SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 净重(G) Weight
        /// <summary>
        /// 净重(G)
        /// </summary>
        [Label("净重(G)")]
        public static readonly Property<decimal?> WeightProperty = P<ShippingOrderDetailInf>.Register(e => e.Weight);

        /// <summary>
        /// 净重(G)
        /// </summary>
        public decimal? Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 金额 Amount
        /// <summary>
        /// 金额
        /// </summary>
        [Label("金额")]
        public static readonly Property<decimal?> AmountProperty = P<ShippingOrderDetailInf>.Register(e => e.Amount);

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount
        {
            get { return this.GetProperty(AmountProperty); }
            set { this.SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 指定LPN AppointLpn
        /// <summary>
        /// 指定LPN
        /// </summary>
        [Label("指定LPN")]
        public static readonly Property<string> AppointLpnProperty = P<ShippingOrderDetailInf>.Register(e => e.AppointLpn);

        /// <summary>
        /// 指定LPN
        /// </summary>
        public string AppointLpn
        {
            get { return this.GetProperty(AppointLpnProperty); }
            set { this.SetProperty(AppointLpnProperty, value); }
        }
        #endregion

        #region 指定批次 AppointLotCode
        /// <summary>
        /// 指定批次
        /// </summary>
        [Label("指定批次")]
        public static readonly Property<string> AppointLotCodeProperty = P<ShippingOrderDetailInf>.Register(e => e.AppointLotCode);

        /// <summary>
        /// 指定批次
        /// </summary>
        public string AppointLotCode
        {
            get { return this.GetProperty(AppointLotCodeProperty); }
            set { this.SetProperty(AppointLotCodeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 发运单明细中间表 实体配置
    /// </summary>
    internal class ShippingOrderDetailInfConfig : EntityConfig<ShippingOrderDetailInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_SHIPPING_ORDER_DTL").MapAllProperties();
            Meta.Property(ShippingOrderDetailInf.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}