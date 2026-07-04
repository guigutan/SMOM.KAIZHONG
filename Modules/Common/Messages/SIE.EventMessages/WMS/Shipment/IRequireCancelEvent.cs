using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// MES需求取消WMS发运单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultRequireCancelEvent))]
    public interface IRequireCancelEvent
    {
        /// <summary>
        /// 更新发货单需求取消
        /// </summary>
        /// <param name="reqNos">工单备料单号集合</param>
        void UpdateSoRequireCancelInfos(List<string> reqNos);
    }

    /// <summary>
    /// MES需求取消WMS发运单接口默认实现
    /// </summary>
    public class DefaultRequireCancelEvent : IRequireCancelEvent
    {
        /// <summary>
        /// 更新发货单需求取消
        /// </summary>
        /// <param name="reqNos">工单备料单号集合</param>
        public void UpdateSoRequireCancelInfos(List<string> reqNos)
        {
            // Method intentionally left empty.
        }
    }
}
