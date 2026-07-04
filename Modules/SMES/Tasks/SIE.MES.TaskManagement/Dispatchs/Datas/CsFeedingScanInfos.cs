using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// CS生产报工-上料-扫描信息
    /// </summary>
    [Serializable]
    public class CsFeedingScanInfos
    {

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double ItemLabelId { get; set; }

        /// <summary>
        /// 物料标签号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal FeedingQty { get; set; }



        /// <summary>
        /// 上料时间
        /// </summary>
        public DateTime FeedingTime { get; set; }




        /// <summary>
        /// 状态
        /// </summary>
        public string ItemLabelState { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }
    }
}
