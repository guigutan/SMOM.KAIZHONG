using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.MES.FeedingIntervals
{
    /// <summary>
    /// 上料间隔时间查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("上料间隔时间查询实体")]
    public class FeedingIntervalCriteria : Criteria
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<FeedingIntervalCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<FeedingIntervalCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FeedingIntervalController>().Fetch(this);
        }


    }
}
