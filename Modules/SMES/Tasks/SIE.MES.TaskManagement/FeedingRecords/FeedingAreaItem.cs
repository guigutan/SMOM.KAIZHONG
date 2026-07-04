using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 供料区物料
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Id))]
    [Label("供料区物料")]
    public class FeedingAreaItem : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FeedingAreaItem()
        {
        }

        #region 供料区 FeedingArea
        /// <summary>
        /// 供料区Id
        /// </summary>
        [Label("供料区")]
        public static readonly IRefIdProperty FeedingAreaIdProperty =
            P<FeedingAreaItem>.RegisterRefId(e => e.FeedingAreaId, ReferenceType.Parent);

        /// <summary>
        /// 供料区Id
        /// </summary>
        public double FeedingAreaId
        {
            get { return (double)this.GetRefId(FeedingAreaIdProperty); }
            set { this.SetRefId(FeedingAreaIdProperty, value); }
        }

        /// <summary>
        /// 供料区
        /// </summary>
        public static readonly RefEntityProperty<FeedingArea> FeedingAreaProperty =
            P<FeedingAreaItem>.RegisterRef(e => e.FeedingArea, FeedingAreaIdProperty);

        /// <summary>
        /// 供料区
        /// </summary>
        public FeedingArea FeedingArea
        {
            get { return this.GetRefEntity(FeedingAreaProperty); }
            set { this.SetRefEntity(FeedingAreaProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<FeedingAreaItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<FeedingAreaItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FeedingAreaItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<FeedingAreaItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion


        #endregion
    }

    internal class FeedingAreaItemEntityConfig : EntityConfig<FeedingAreaItem>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FEEDING_AREA_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
