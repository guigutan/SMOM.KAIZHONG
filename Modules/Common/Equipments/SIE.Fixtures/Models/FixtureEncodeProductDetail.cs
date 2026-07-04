using SIE;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
    /// 工治具编码（产品清单）
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具编码（产品清单）")]
    public partial class FixtureEncodeProductDetail : DataEntity
    {
        #region 需求数 DemandQuantity
        /// <summary>
        /// 需求数
        /// </summary>
        [Label("需求数")]
        public static readonly Property<int> DemandQuantityProperty = P<FixtureEncodeProductDetail>.Register(e => e.DemandQuantity);

        /// <summary>
        /// 需求数
        /// </summary>
        public int DemandQuantity
        {
            get { return GetProperty(DemandQuantityProperty); }
            set { SetProperty(DemandQuantityProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegment
        /// <summary>
        /// 工段Id
        /// </summary>
        public static readonly IRefIdProperty ProcessSegmentIdProperty = P<FixtureEncodeProductDetail>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)GetRefNullableId(ProcessSegmentIdProperty); }
            set { SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<FixtureEncodeProductDetail>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return GetRefEntity(ProcessSegmentProperty); }
            set { SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 工艺面 Deck
        /// <summary>
        /// 工艺面
        /// </summary>
        [Label("工艺面")]
        public static readonly Property<Deck?> DeckProperty = P<FixtureEncodeProductDetail>.Register(e => e.Deck);

        /// <summary>
        /// 工艺面
        /// </summary>
        public Deck? Deck
        {
            get { return GetProperty(DeckProperty); }
            set { SetProperty(DeckProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Required]
        public static readonly IRefIdProperty ItemIdProperty = P<FixtureEncodeProductDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<FixtureEncodeProductDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureEncodeProductDetail>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Parent);

        /// <summary>
        ///工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureEncodeProductDetail>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FixtureEncodeProductDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

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

    /// <summary>
    /// 工治具编码（产品清单） 实体配置
    /// </summary>
    internal class FixtureEncodeProductDetailConfig : EntityConfig<FixtureEncodeProductDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CODE_PRO").MapAllProperties();
            Meta.EnablePhantoms();
        }

    }
}