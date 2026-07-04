using SIE.Core.Items;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档与适用产品对应关系
    /// </summary>
    [Serializable, ChildEntity]
    [Label("文档集与产品关系")]
    [DisplayMember(nameof(DocumentCollectionItem.Id))]
    public class DocumentCollectionItem : DataEntity
    {
        #region 文档集合 DocumentCollection
        /// <summary>
        /// 文档集合ID
        /// </summary>
        public static readonly IRefIdProperty DocumentCollectionIdProperty = P<DocumentCollectionItem>.RegisterRefId(e => e.DocumentCollectionId, ReferenceType.Parent);

        /// <summary>
        /// 文档集合ID
        /// </summary>
        public double DocumentCollectionId
        {
            get { return (double)this.GetRefId(DocumentCollectionIdProperty); }
            set { this.SetRefId(DocumentCollectionIdProperty, value); }
        }

        /// <summary>
        /// 文档集合
        /// </summary>
        public static readonly RefEntityProperty<DocumentCollection> DocumentCollectionProperty = P<DocumentCollectionItem>.RegisterRef(e => e.DocumentCollection, DocumentCollectionIdProperty);

        /// <summary>
        /// 文档集合
        /// </summary>
        public DocumentCollection DocumentCollection
        {
            get { return this.GetRefEntity(DocumentCollectionProperty); }
            set { this.SetRefEntity(DocumentCollectionProperty, value); }
        }
        #endregion

        #region 适用产品 Item
        /// <summary>
        /// 适用产品ID
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<DocumentCollectionItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 适用产品ID
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 适用产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<DocumentCollectionItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 适用产品
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<DocumentCollectionItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<DocumentCollectionItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 文档与适用物料对应关系 配置
    /// </summary>
    internal class DocumentCollectionItemConfig : EntityConfig<DocumentCollectionItem>
    {
        /// <summary>
        /// 数据库映射配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ESOP_DOC_SET_ITEM").MapAllProperties();
            Meta.Property(DocumentCollectionItem.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}