using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Inventory
{
    /// <summary>
    /// 获取库存操作数据
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIGetLotLpnOnhand))]
    public interface IGetLotLpnOnhand
    {
        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="onhandParams">参数</param>
        /// <returns>库存数据</returns>
        decimal GetLotLpnOnhandQty(LotLpnOnhandParams onhandParams);

        /// <summary>
        /// 该仓库下是否存在现有量大于0的库存数据
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        bool IsWareHouseHasOnHand(double warehouseId);

        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>可用数</returns>
        List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(double warehouseId, List<double> itemIds);

        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseIds">仓库IDs</param>
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>可用数</returns>
        List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(List<double> warehouseIds, List<double> itemIds);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    class DefalitIGetLotLpnOnhand : IGetLotLpnOnhand
    {
        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>可用数</returns>
        public List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(double warehouseId, List<double> itemIds)
        {
            return new List<LotLpnOnhandDataInfo>();
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="onhandParams"></param>
        /// <returns></returns>
        public decimal GetLotLpnOnhandQty(LotLpnOnhandParams onhandParams)
        {
            return 0;
        }

        /// <summary>
        /// 该仓库下是否存在现有量大于0的库存数据
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public bool IsWareHouseHasOnHand(double warehouseId)
        {
            return false;
        }

        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseIds">仓库IDs</param>
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>可用数</returns>
        public List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(List<double> warehouseIds, List<double> itemIds)
        {
            return new List<LotLpnOnhandDataInfo>();
        }
    }
}
