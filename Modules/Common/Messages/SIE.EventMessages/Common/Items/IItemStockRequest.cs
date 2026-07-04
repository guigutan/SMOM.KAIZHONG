using System.Collections.Generic;

namespace SIE.EventMessages.Common.Items
{
    /// <summary>
    /// WMS库存资料接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitItemStockRequest))]
    public interface IItemStockRequest
    {
        /// <summary>
        /// 获取物料库存资料
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <returns>物料库存资料</returns>
        List<ItemStockBaseData> GetItemStockBaseDatas(List<double> itemIds);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefalitItemStockRequest : IItemStockRequest
    {
        /// <summary>
        /// 获取物料库存资料
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <returns>物料库存资料</returns>
        public List<ItemStockBaseData> GetItemStockBaseDatas(List<double> itemIds)
        {
            return new List<ItemStockBaseData>();
        }
    }
}
