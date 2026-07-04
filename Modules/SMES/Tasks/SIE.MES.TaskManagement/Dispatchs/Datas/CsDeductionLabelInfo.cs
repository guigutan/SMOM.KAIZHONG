using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 标签信息
    /// </summary>
    [Serializable]
    public class CsDeductionLabelInfo
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public string Sn { get; set; }

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
        /// 物料总数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainingQty { get; set; }

        /// <summary>
        /// 上料记录
        /// </summary>
        public double RecordId { get; set; }

        
    }
}
