using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartReceives.ViewModels
{
    /// <summary>
    /// 备件接收条码模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("备件接收条码模型")]
    public class SparePartLotSnViewModel : ViewModel
    {
        #region 接收行号 ReceiveLineNo
        /// <summary>
        /// 接收行号
        /// </summary>
        [Label("接收行号")]
        public static readonly Property<int> ReceiveLineNoProperty = P<SparePartLotSnViewModel>.Register(e => e.ReceiveLineNo);

        /// <summary>
        /// 接收行号
        /// </summary>
        public int ReceiveLineNo
        {
            get { return this.GetProperty(ReceiveLineNoProperty); }
            set { this.SetProperty(ReceiveLineNoProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartLotSnViewModel>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 采购单行号 OrderItemNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<int?> OrderItemNoProperty = P<SparePartLotSnViewModel>.Register(e => e.OrderItemNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public int? OrderItemNo
        {
            get { return this.GetProperty(OrderItemNoProperty); }
            set { this.SetProperty(OrderItemNoProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartLotSnViewModel>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartLotSnViewModel>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<SparePartLotSnViewModel>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return this.GetProperty(LotNoProperty); }
            set { this.SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<int> QtyProperty = P<SparePartLotSnViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 序列号编码 Sn
        /// <summary>
        /// 序列号编码
        /// </summary>
        [Label("序列号编码")]
        public static readonly Property<string> SnProperty = P<SparePartLotSnViewModel>.Register(e => e.Sn);

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
        public static readonly Property<string> OriginalSnProperty = P<SparePartLotSnViewModel>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion
    }
}
