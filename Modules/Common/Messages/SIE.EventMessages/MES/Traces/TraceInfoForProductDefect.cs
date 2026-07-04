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
    public class TraceInfoForProductDefect
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<TraceItemInfoForProductDefect> Data { get; set; } = new List<TraceItemInfoForProductDefect>();
    }

    /// <summary>
    /// 过程追溯-关联产品追溯详细信息
    /// </summary>
    [Serializable]
    public class TraceItemInfoForProductDefect
    {

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription { get; set; }

        /// <summary>
        /// 检验项描述
        /// </summary>
        public string InspItemName { get; set; }

        /// <summary>
        /// 板号
        /// </summary>
        public int? BoardNo { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? FixedDate { get; set; }

        /// <summary>
        /// 是否误判
        /// </summary>
        public bool IsMisjudgment { get; set; }

        /// <summary>
        /// 维修人
        /// </summary>
        public string FixedBy { get; set; }


    }
}
