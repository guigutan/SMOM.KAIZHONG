using System;
using System.Collections.Generic;

namespace SIE.EventMessages.RealTimeInventory
{
    /// <summary>
    /// 查询实时库存信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultRealTimeInventoryInterface))]
    public interface IRealTimeInventory
    {
        /// <summary>
        /// 获取实时库存信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID列表</param>
        /// <param name="itemIds">物料ID列表</param>
        /// <param name="date">日期</param>
        /// <returns>实时库存信息</returns>
        List<RealTimeInvInfo> GetRealTimeInvInfos(List<double> warehouseIds, List<double> itemIds, DateTime? date);
    }

    /// <summary>
    /// 查询实时库存信息接口默认实现
    /// </summary>
    class DefaultRealTimeInventoryInterface : IRealTimeInventory
    {
        /// <summary>
        /// 获取实时库存信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID列表</param>
        /// <param name="itemIds">物料ID列表</param>
        /// <param name="date">日期</param>
        /// <returns>实时库存信息</returns>
        public List<RealTimeInvInfo> GetRealTimeInvInfos(List<double> warehouseIds, List<double> itemIds, DateTime? date)
        {
            return new List<RealTimeInvInfo>();
        }
    }
}
