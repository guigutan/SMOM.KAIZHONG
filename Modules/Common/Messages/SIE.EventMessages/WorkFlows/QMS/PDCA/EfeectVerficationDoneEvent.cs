using System;

namespace SIE.EventMessages.WorkFlows.QMS.PDCA
{
    /// <summary>
    /// 效果验证完成事件
    /// </summary>
    [Serializable]
    public  class EfeectVerficationDoneEvent
    {
        /// <summary>
        /// 改善单Id
        /// </summary>
        public double ImprovementId { get; set; }
    }
}
