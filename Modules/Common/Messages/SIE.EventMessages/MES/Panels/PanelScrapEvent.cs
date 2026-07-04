using System;

namespace SIE.EventMessages.MES.Panels
{
    /// <summary>
    /// 拼板码报废事件
    /// </summary>
    [Serializable]
    public class PanelScrapEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="panelCode">拼板码号</param>
        /// <param name="scrapReason">报废原因</param>
        public PanelScrapEvent(double workOrderId, string panelCode, string scrapReason)
        {
            WorkOrderId = workOrderId;
            PanelCode = panelCode;
            ScrapReason = scrapReason;
        }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 拼板码号
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason { get; set; }
    }
}