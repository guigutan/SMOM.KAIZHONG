using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 余料称重扫描标签信息
    /// </summary>
    [Serializable]
    public class PdaSWRecordScanInfo
    {
        /// <summary>
        /// 物料标签号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double ItemLabelId { get; set; }

        /// <summary>
        /// 上料记录Id
        /// </summary>
        public double FeedingRecordId { get; set; }

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string ItemLabelState { get; set; }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal FeedingQty { get; set; }

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal BlankingQty { get; set; }

        /// <summary>
        /// 理论剩余数量
        /// </summary>
        public decimal RemainingQty { get; set; }

        /// <summary>
        /// 实际重量数量(前端填写)
        /// </summary>
        public decimal? ActualQty { get; set; }
    }
}
