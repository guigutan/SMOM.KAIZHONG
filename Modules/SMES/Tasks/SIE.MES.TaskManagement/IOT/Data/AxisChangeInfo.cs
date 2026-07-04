using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.IOT.Data
{
    /// <summary>
    /// 换轴信息
    /// </summary>
    [Serializable]
    public class AxisChangeInfo
    {
        /// <summary>
        /// IOT实体
        /// </summary>
        public string IotEntity { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }
        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 换轴标识
        /// </summary>
        public bool ChangeFlag { get; set; }

        /// <summary>
        /// 换轴米数
        /// </summary>
        public decimal? AxisQty { get; set; }

        /// <summary>
        /// 计米数量
        /// </summary>
        public decimal? MeterCount { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? CollectionTime { get; set; }

    }
}
