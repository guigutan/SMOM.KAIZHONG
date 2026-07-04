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
    /// 反向追溯-Mes工序采集信息
    /// </summary>
    [Serializable]
    public class MesProcessCollectInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<MesProcessCollectItemInfo> Data { get; set; } = new List<MesProcessCollectItemInfo>();
    }

    /// <summary>
    /// 反向追溯-Mes工序采集详细信息
    /// </summary>
    [Serializable]
    public class MesProcessCollectItemInfo
    {
        /// <summary>
        /// 工序采集Id
        /// </summary>
        public double ReportProcessId { get; set; }

        /// <summary>
        /// 采集条码
        /// </summary>
        public string CollectSn { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public string Result { get; set; }
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
