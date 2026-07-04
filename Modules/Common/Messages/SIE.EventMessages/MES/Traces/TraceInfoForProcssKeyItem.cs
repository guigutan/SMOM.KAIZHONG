using SIE.Domain;
using SIE.EventMessages.WMS.Traces;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 过程追溯-采集记录追溯
    /// </summary>
    [Serializable]
    public class TraceInfoForProcssKeyItem
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<TraceItemInfoForProcssKeyItem> Data { get; set; } = new List<TraceItemInfoForProcssKeyItem>();
    }

    /// <summary>
    /// 过程追溯-关联产品追溯详细信息
    /// </summary>
    [Serializable]
    public class TraceItemInfoForProcssKeyItem
    {
        /// <summary>
        /// 采集条码
        /// </summary>
        public string CollectSn { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 使用数量
        /// </summary>
        public decimal Qty { get; set; }
      
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectTime { get; set; }
       
        /// <summary>
        /// 操作人
        /// </summary>
        public string CollectBy { get; set; }
     

    }
}
