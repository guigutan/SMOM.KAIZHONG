using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料
    /// </summary>
    [RootEntity, Serializable]
    [Label("上料")]
    public class LoadItemViewModel : ViewModel
    {
        #region LoadItem 下料
        /// <summary>
        /// 下料Id
        /// </summary>
        [Label("下料")]
        public static readonly IRefIdProperty LoadItemIdProperty =
            P<LoadItemViewModel>.RegisterRefId(e => e.LoadItemId, ReferenceType.Normal);

        /// <summary>
        /// 下料Id
        /// </summary>
        public double LoadItemId
        {
            get { return (double)this.GetRefId(LoadItemIdProperty); }
            set { this.SetRefId(LoadItemIdProperty, value); }
        }

        /// <summary>
        /// 下料
        /// </summary>
        public static readonly RefEntityProperty<LoadItem> LoadItemProperty =
            P<LoadItemViewModel>.RegisterRef(e => e.LoadItem, LoadItemIdProperty);

        /// <summary>
        /// 下料
        /// </summary>
        public LoadItem LoadItem
        {
            get { return this.GetRefEntity(LoadItemProperty); }
            set { this.SetRefEntity(LoadItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<LoadItemViewModel>.RegisterView(e => e.ItemCode, p => p.LoadItem.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<LoadItemViewModel>.RegisterView(e => e.ItemName, p => p.LoadItem.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 标签号 SourceCode
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> SourceCodeProperty = P<LoadItemViewModel>.RegisterView(e => e.SourceCode, p => p.LoadItem.SourceCode);

        /// <summary>
        /// 标签号
        /// </summary>
        public string SourceCode
        {
            get { return this.GetProperty(SourceCodeProperty); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<SIE.MES.SingleLabels.LoadItemSourceType> SourceTypeProperty = P<LoadItemViewModel>.RegisterView(e => e.SourceType, p => p.LoadItem.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public SIE.MES.SingleLabels.LoadItemSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
        }
        #endregion

        #region 上料数量 LoadQty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Label("上料数量")]
        public static readonly Property<decimal> LoadQtyProperty = P<LoadItemViewModel>.RegisterView(e => e.LoadQty, p => p.LoadItem.LoadQty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadQty
        {
            get { return this.GetProperty(LoadQtyProperty); }
        }
        #endregion

        #region 剩余数量 Qty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> QtyProperty = P<LoadItemViewModel>.RegisterView(e => e.Qty, p => p.LoadItem.Qty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
        }
        #endregion

        #region 上料时间 CreateDate
        /// <summary>
        /// 上料时间
        /// </summary>
        [Label("上料时间")]
        public static readonly Property<DateTime> CreateDateProperty = P<LoadItemViewModel>.RegisterView(e => e.CreateDate, p => p.LoadItem.CreateDate);

        /// <summary>
        /// 上料时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
        }
        #endregion

    }
}
