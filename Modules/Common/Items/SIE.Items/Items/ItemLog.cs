using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料更新日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料更新日志")]
    [DisplayMember(nameof(OperatDescription))]
    public partial class ItemLog : DataEntity
    {
        #region 时间/日期 OperatDate
        /// <summary>
        /// 时间/日期
        /// </summary>
        [Label("时间/日期")]
        public static readonly Property<DateTime> OperatDateProperty = P<ItemLog>.Register(e => e.OperatDate);

        /// <summary>
        /// 时间/日期
        /// </summary>
        public DateTime OperatDate
        {
            get { return GetProperty(OperatDateProperty); }
            set { SetProperty(OperatDateProperty, value); }
        }
        #endregion

        #region 操作类型 OperatType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<string> OperatTypeProperty = P<ItemLog>.Register(e => e.OperatType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperatType
        {
            get { return GetProperty(OperatTypeProperty); }
            set { SetProperty(OperatTypeProperty, value); }
        }
        #endregion

        #region 内容描述 OperatDescription
        /// <summary>
        /// 内容描述
        /// </summary>
        [Label("内容描述")]
        public static readonly Property<string> OperatDescriptionProperty = P<ItemLog>.Register(e => e.OperatDescription);

        /// <summary>
        /// 内容描述
        /// </summary>
        public string OperatDescription
        {
            get { return GetProperty(OperatDescriptionProperty); }
            set { SetProperty(OperatDescriptionProperty, value); }
        }
        #endregion

        #region 物料更新日志 Item
        /// <summary>
        /// 物料更新日志Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料更新日志Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料更新日志
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemLog>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料更新日志
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料更新日志 实体配置
    /// </summary>
    internal class ItemLogConfig : EntityConfig<ItemLog>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}