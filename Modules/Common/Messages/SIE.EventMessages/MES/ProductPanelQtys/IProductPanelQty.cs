using SIE.Services;

namespace SIE.EventMessages.ProductPanelQtys
{
    /// <summary>
    /// 产品拼板数接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultProductPanelQty))]
    public interface IProductPanelQty
    {
        /// <summary>
        /// 获取产品拼板数
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>拼板数</returns>
        int GetPanelQty(double productId);
    }

    /// <summary>
    /// 默认实现产品拼板数接口
    /// </summary>
    public class DefaultProductPanelQty : IProductPanelQty
    {
        /// <summary>
        /// 获取产品拼板数
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>拼板数</returns>
        public int GetPanelQty(double productId)
        {
            return 1;
        }
    }
}
