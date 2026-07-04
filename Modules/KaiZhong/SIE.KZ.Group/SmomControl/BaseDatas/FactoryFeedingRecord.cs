using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Group.SmomControl.BaseDatas
{
    /// <summary>
    /// 工厂上料记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("工厂上料记录")]
    public class FactoryFeedingRecord : FactoryBase
    {
        #region 机台号 ResourceCode
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceCodeProperty = P<FactoryFeedingRecord>.Register(e => e.ResourceCode);

        /// <summary>
        /// 机台号
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 机台名称 ResourceName
        /// <summary>
        /// 机台名称
        /// </summary>
        [Label("机台名称")]
        public static readonly Property<string> ResourceNameProperty = P<FactoryFeedingRecord>.Register(e => e.ResourceName);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 上料标签 FeedingItemLabel
        /// <summary>
        /// 上料标签
        /// </summary>
        [Label("上料标签")]
        public static readonly Property<string> FeedingItemLabelProperty = P<FactoryFeedingRecord>.Register(e => e.FeedingItemLabel);

        /// <summary>
        /// 上料标签
        /// </summary>
        public string FeedingItemLabel
        {
            get { return this.GetProperty(FeedingItemLabelProperty); }
            set { this.SetProperty(FeedingItemLabelProperty, value); }
        }
        #endregion

        #region 上料数量 FeedingQty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Label("上料数量")]
        public static readonly Property<decimal?> FeedingQtyProperty = P<FactoryFeedingRecord>.Register(e => e.FeedingQty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal? FeedingQty
        {
            get { return this.GetProperty(FeedingQtyProperty); }
            set { this.SetProperty(FeedingQtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainingQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal?> RemainingQtyProperty = P<FactoryFeedingRecord>.Register(e => e.RemainingQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
            set { this.SetProperty(RemainingQtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FactoryFeedingRecord>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FactoryFeedingRecord>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<FactoryFeedingRecord>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<FactoryFeedingRecord>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

    }

    internal class FactoryFeedingRecordConfig : EntityConfig<FactoryFeedingRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("FACTORY_FEEDING_RECORD_V").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisablePhantoms();
        }
    }
}
