using SIE.Common.Alert;
using SIE.Common.Sender;

namespace SIE.AbnormalInfo.Senders
{
    /// <summary>
    /// 异常管理发送参数
    /// </summary>
    public class AbnormalSendParam : ISendParam
    {
        /// <summary>
        /// 预警值
        /// </summary>
        public decimal? AlertValue { get; set; }

        /// <summary>
        /// 预警等级
        /// </summary>
        public AlertLevel? AlertLevel { get; set; }

        /// <summary>
        /// 预警配置ID
        /// </summary>
        public double AlerterId { get; set; }
    }
}
