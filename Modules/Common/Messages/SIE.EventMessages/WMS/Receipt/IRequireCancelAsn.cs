using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// MES需求取消WMSASN单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultRequireCancelAsn))]
    public interface IRequireCancelAsn
    {
        /// <summary>
        /// 更新MES入库单取消
        /// </summary>
        /// <param name="reqNos">MES入库单号集合</param>
        void UpdateAsnRequireCancelInfos(List<string> reqNos);
    }

    /// <summary>
    /// MES需求取消WMS发运单接口默认实现
    /// </summary>
    public class DefaultRequireCancelAsn : IRequireCancelAsn
    {
        /// <summary>
        /// 更新发货单需求取消
        /// </summary>
        /// <param name="reqNos">工单备料单号集合</param>
        public void UpdateAsnRequireCancelInfos(List<string> reqNos)
        {
            // Method intentionally left empty.
        }
    }
}
