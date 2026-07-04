using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Inv
{
    /// <summary>
    /// 流程详情里单据明细详情键值对
    /// </summary>
    public class ProcessLableData
    {
        /// <summary>
        /// 流程详情-单据明细详情里的Label值
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 流程详情-单据明细详情里的Value值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 单据明细
    /// </summary>
    public class ProcessDetailList
    {
        /// <summary>
        /// 单据明细
        /// </summary>
        public List<ProcessLableData> ProcessDetail { get; set; }
    }
}
