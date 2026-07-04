using System;
using System.Collections.Generic;

namespace SIE.Kit.EventMessages.FactoryConfirms
{
    /// <summary>
    /// 生成订单评审接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalutCreateOrderReviewTask))]
    public interface ICreateOrderReview
    {
        /// <summary>
        /// 生成订单评审
        /// </summary>
        string CreateOrderReview(List<double> ids);
    }

    /// <summary> 
    /// 接口实现
    /// </summary>
    public class DefalutCreateOrderReviewTask : ICreateOrderReview
    {
        /// <summary>
        /// 生成订单评审
        /// </summary>
        public string CreateOrderReview(List<double> ids)
        {
            throw new NotImplementedException();
        }
    }
}
