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
    /// 过程追溯-产品维修记录追溯
    /// </summary>
    [Serializable]
    public class TraceInfoForProductRepair
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<TraceItemInfoForProductRepair> Data { get; set; } = new List<TraceItemInfoForProductRepair>();
    }

    /// <summary>
    /// 过程追溯-产品维修记录详细信息
    /// </summary>
    [Serializable]
    public class TraceItemInfoForProductRepair
    {
        /// <summary>
        /// 维修表Id
        /// </summary>
        public double WipProductReportRepairId { get; set; }

        /// <summary>
        /// 维修类型
        /// </summary>
        public string RepairType { get; set; }

        /// <summary>
        /// 维修人
        /// </summary>
        public string RepairBy { get; set; }

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? RepairTime { get; set; }

        /// <summary>
        /// 维修工序
        /// </summary>
        public string RepairProcess { get; set; }

        /// <summary>
        /// 维修工位
        /// </summary>
        public string RepairStation { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDes { get; set; }

        /// <summary>
        /// 缺陷备注
        /// </summary>
        public string DefectRemark { get; set; }


    }
}
