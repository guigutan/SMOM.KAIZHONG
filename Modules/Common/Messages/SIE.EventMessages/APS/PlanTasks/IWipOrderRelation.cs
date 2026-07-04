using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 在制工单与生产订单关系
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyWipOrderRelation))]
    public interface IWipOrderRelation
    {
        /// <summary>
        /// 根据工单编号获取在制工单与生产订单关系
        /// </summary>
        /// <param name="woNos">工单编号</param>
        /// <returns>返回在制工单与生产订单关系</returns>
        List<WipOrderInfo> GetWipOrderRelations(List<string> woNos);
    }

    /// <summary>
    /// 工艺定额接口默认实现
    /// </summary>
    public class EmptyWipOrderRelation : IWipOrderRelation
    {
        /// <summary>
        /// 根据工单编号获取在制工单与生产订单关系
        /// </summary>
        /// <param name="woNos">工单编号</param>
        /// <returns>返回在制工单与生产订单关系</returns>
        public List<WipOrderInfo> GetWipOrderRelations(List<string> woNos)
        {
            return new List<WipOrderInfo>();
        }
    }

    /// <summary>
    /// 在制工单与生产订单关系
    /// </summary>
    [Serializable]
    public class WipOrderInfo
    {
        /// <summary>
        /// 在制工单编号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 生产订单
        /// </summary>
        public string OrderCode { get; set; }
    }
}