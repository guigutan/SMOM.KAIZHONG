using SIE.Services;

namespace SIE.EventMessages.WorkOrders
{
    /// <summary>
    /// 是否组合板工单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultIsPanelWorkOrder))]
    public interface IIsPanelWorkOrder
    {
        /// <summary>
        /// 获取是否组合板工单
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>是/否</returns>
        bool IsPanelWorkOrder(double productId);
    }

    /// <summary>
    /// 默认实现是否组合板工单接口
    /// </summary>
    public class DefaultIsPanelWorkOrder : IIsPanelWorkOrder
    {
        /// <summary>
        /// 获取产品拼板数
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>是/否</returns>
        public bool IsPanelWorkOrder(double productId)
        {
            return false;
        }
    }
}
