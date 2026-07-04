using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Barcodes
{
    /// <summary>
    /// 条码
    /// </summary>
    [RootEntity, Serializable]
    [Label("条码")]
    [DisplayMember(nameof(Barcode.Sn))]
    public abstract partial class Barcode : DataEntity
    {
        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<Barcode>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 是否报废 IsScraped
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrapedProperty = P<Barcode>.Register(e => e.IsScraped);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScraped
        {
            get { return GetProperty(IsScrapedProperty); }
            set { SetProperty(IsScrapedProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<Barcode>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 满箱数量 BoxesQty
        /// <summary>
        /// 满箱数量
        /// </summary>
        [Label("满箱数量")]
        public static readonly Property<decimal> BoxesQtyProperty = P<Barcode>.Register(e => e.BoxesQty);

        /// <summary>
        /// 满箱数量
        /// </summary>
        public decimal BoxesQty
        {
            get { return GetProperty(BoxesQtyProperty); }
            set { SetProperty(BoxesQtyProperty, value); }
        }
        #endregion

        #region 是否尾数 IsMantissa
        /// <summary>
        /// 是否尾数
        /// </summary>
        [Label("是否尾数")]
        public static readonly Property<bool> IsMantissaProperty = P<Barcode>.Register(e => e.IsMantissa);

        /// <summary>
        /// 是否尾数
        /// </summary>
        public bool IsMantissa
        {
            get { return GetProperty(IsMantissaProperty); }
            set { SetProperty(IsMantissaProperty, value); }
        }
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        public abstract double? WorkOrderId { get; set; }
        #endregion

        #region 是否挂起 IsPending
        /// <summary>
        /// 是否挂起
        /// </summary>
        [Label("是否挂起")]
        public static readonly Property<bool> IsPendingProperty = P<Barcode>.Register(e => e.IsPending);

        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool IsPending
        {
            get { return GetProperty(IsPendingProperty); }
            set { SetProperty(IsPendingProperty, value); }
        }
        #endregion
    }
}