using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.AdvancePlans
{
    /// <summary>
    /// 订单负载明细接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyOrderLoading))]
    public interface IOrderLoading
    {
        /// <summary>
        /// 获取制程订单范围内的订单负载明细字典
        /// </summary>
        /// <param name="pTOIds">制程订单ID列表</param>
        /// <returns>订单负载明细表字典。key1：制程工艺单ID；key2：负载日期；value：数量</returns>
        Dictionary<double, Dictionary<DateTime, decimal>> GetOrderLoadingDiCByPTOIds(List<double> pTOIds);
    }

    /// <summary>
    /// 订单负载明细接口默认实现
    /// </summary>
    public class EmptyOrderLoading : IOrderLoading
    {
        /// <summary>
        /// 获取制程订单范围内的订单负载明细字典
        /// </summary>
        /// <param name="pTOIds">制程订单ID列表</param>
        /// <returns>订单负载明细表字典。key1：制程工艺单ID；key2：负载日期；value：数量</returns>
        public Dictionary<double, Dictionary<DateTime, decimal>> GetOrderLoadingDiCByPTOIds(List<double> pTOIds)
        {
            return new Dictionary<double, Dictionary<DateTime, decimal>>();
        }
    }
}
