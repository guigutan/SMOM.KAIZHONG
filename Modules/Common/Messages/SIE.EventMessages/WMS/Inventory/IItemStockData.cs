using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 判断物料是否位置跟踪序列号物料
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIItemStockDataInterface))]
    public interface IItemStockData
    {
        /// <summary>
        /// 判断物料是否位置跟踪序列号物料
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        bool CheckItemsIsLocation(List<double> itemIds);
    }

    class DefaultIItemStockDataInterface : IItemStockData
    {
        /// <summary>
        /// 判断物料是否位置跟踪序列号物料
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public bool CheckItemsIsLocation(List<double> itemIds)
        {
            return false;
        }
    }
}
