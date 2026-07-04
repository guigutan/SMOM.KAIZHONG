using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 下料采集:获取上料记录
    /// </summary>
    [Serializable]
    public class Pda_BlankingGetFeedRecordInfo
    {
        /// <summary>
        /// 上料记录Id
        /// </summary>
        public double RecordId { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemDesc { get; set; }

        /// <summary>
        /// 标签批次
        /// </summary>
        public string ItemLabelLot { get; set; }

        /// <summary>
        /// 上料时间
        /// </summary>
        public DateTime FeedingTime { get; set; }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal? FeedingQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? RemainingQty { get; set; }
    }
}
