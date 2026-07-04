using SIE.Core.Items;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Labels
{
    /// <summary>
    /// 物料标签
    /// </summary>
    [RootEntity, Serializable]
    public class ItemLabel : DataEntity
    {
        /// <summary>
        /// 
        /// </summary>
        protected ItemLabel()
        {

        }

        #region 标签号(条码) No
        /// <summary>
        /// 标签号(条码)
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> NoProperty = P<ItemLabel>.Register(e => e.No);

        /// <summary>
        /// 标签号(条码)
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 生产批号 BatchNo
        /// <summary>
        /// 生产批号
        /// </summary>
        [Label("生产批号")]
        public static readonly Property<string> BatchNoProperty = P<ItemLabel>.Register(e => e.BatchNo);

        /// <summary>
        /// 生产批号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 规格 Specification
        /// <summary>
        /// 规格
        /// </summary>
        [Label("规格")]
        public static readonly Property<string> SpecificationProperty = P<ItemLabel>.Register(e => e.Specification);

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification
        {
            get { return GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 可用数量 Qty
        /// <summary>
        /// 可用数量
        /// </summary>
        [Label("可用数量")]
        public static readonly Property<decimal> QtyProperty = P<ItemLabel>.Register(e => e.Qty);

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemLabel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<LabelState> StateProperty = P<ItemLabel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public LabelState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> SupplierProperty = P<ItemLabel>.Register(e => e.Supplier);

        /// <summary>
        /// 供应商ID
        /// </summary>
        public string Supplier
        {
            get { return this.GetProperty(SupplierProperty); }
            set { this.SetProperty(SupplierProperty, value); }
        }
        #endregion
    }
}