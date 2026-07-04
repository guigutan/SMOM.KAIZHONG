using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 余料称重记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("余料称重记录查询实体")]
    public class ScrapWeighingRecordCriteria : Criteria
    {
        #region 物料标签号 Sn
        /// <summary>
        /// 物料标签号
        /// </summary>
        [Label("物料标签号")]
        public static readonly Property<string> SnProperty = P<ScrapWeighingRecordCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 物料标签号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 物料批次号 Lot
        /// <summary>
        /// 物料批次号
        /// </summary>
        [Label("物料批次号")]
        public static readonly Property<string> LotProperty = P<ScrapWeighingRecordCriteria>.Register(e => e.Lot);

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ScrapWeighingRecordCriteria>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<ScrapWeighingRecordCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FeedingRecordController>().CriteriaScrapWeighingRecords(this);
        }

    }
}
