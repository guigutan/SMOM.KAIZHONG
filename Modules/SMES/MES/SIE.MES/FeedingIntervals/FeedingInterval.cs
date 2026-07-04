using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.FeedingIntervals
{

    /// <summary>
    /// 上料间隔时间
    /// </summary>
    [Label("上料间隔时间")]
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FeedingIntervalCriteria))]    
    public partial class FeedingInterval : DataEntity
    {
      
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码Id")]
        public static readonly IRefIdProperty ItemIdProperty = P<FeedingInterval>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);
        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<FeedingInterval>.RegisterRef(e => e.Item, ItemIdProperty);
        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }


        #region 视图属性
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FeedingInterval>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
      
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FeedingInterval>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }

        #endregion
















        /// <summary>
        /// 时间间隔（秒）
        /// </summary>
        [Label("时间间隔（秒）")]
        public static readonly Property<int> IntervalSecondProperty = P<FeedingInterval>.Register(e => e.IntervalSecond);
        /// <summary>
        /// 时间间隔
        /// </summary>
        public int IntervalSecond
        {
            get { return this.GetProperty(IntervalSecondProperty); }
            set { this.SetProperty(IntervalSecondProperty, value); }
        }




        internal class FeedingIntervalConfig : EntityConfig<FeedingInterval>
        {
            /// <summary>
            /// 配置数据库映射
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.MapTable("FEEDING_INTERVAL").MapAllProperties();              
                Meta.EnablePhantoms();
                Meta.EnableInvOrg();
            }
        }

    }
}
