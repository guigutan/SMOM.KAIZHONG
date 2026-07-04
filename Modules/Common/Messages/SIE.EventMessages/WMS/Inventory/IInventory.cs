using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 获取库存操作数据
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIInventoryInterface))]
    public interface IInventory
    {
        /// <summary>
        /// 获取库存调拨数据
        /// </summary>
        /// <param name="itemIds">物料Id</param>
        /// <param name="warehouseIds"></param>
        /// <returns></returns>
        List<AnalysOnhandData> GetAllocateAnalysOnhandDatas(List<double> itemIds, List<double> warehouseIds);
    }

    class DefalitIInventoryInterface : IInventory
    {
        public List<AnalysOnhandData> GetAllocateAnalysOnhandDatas(List<double> itemIds, List<double> warehouseIds)
        {
            return new List<AnalysOnhandData>();
        }
    }
}
