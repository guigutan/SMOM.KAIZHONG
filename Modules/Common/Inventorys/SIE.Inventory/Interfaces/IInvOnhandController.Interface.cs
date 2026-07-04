using SIE.Core.Common;
using SIE.Domain;
using SIE.EventMessages.WMS.Inventory;
using SIE.Inventory.Onhands;
using SIE.Warehouses;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Interfaces
{
    /// <summary>
    /// 库存接口实现
    /// </summary>
    public class IInvOnhandController : IGetLotLpnOnhand
    {
        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="onhandParams">参数</param>
        /// <returns>可用数</returns>
        public virtual decimal GetLotLpnOnhandQty(LotLpnOnhandParams onhandParams)
        {
            if (onhandParams == null || !onhandParams.WarehouseId.HasValue || !onhandParams.ItemId.HasValue)
            {
                return 0;
            }

            var onhands = RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhands(onhandParams.WarehouseId.Value, onhandParams.ItemId.Value, string.Empty, string.Empty, string.Empty, null);
            return onhands.Sum(p => p.AvailableQty);
        }

        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>可用数</returns>
        public virtual List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(double warehouseId, List<double> itemIds)
        {
            var lotLpnOnhands = RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhandByItemIds(warehouseId, itemIds);
            return lotLpnOnhands.Select(x => new LotLpnOnhandDataInfo()
            {
                WarehouseId = x.WarehouseId,
                ItemId = x.ItemId,
                ItemExtProp = x.ItemExtProp,                
                AvailableQty = x.AvailableQty
            }).ToList();
        }

        /// <summary>
        /// 获取库存可用数
        /// </summary>
        /// <param name="warehouseIds">仓库IDs</param>
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>可用数</returns>
        public virtual List<LotLpnOnhandDataInfo> GetLotLpnOnhandByItemIds(List<double> warehouseIds, List<double> itemIds)
        {
            return RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhandByItemIds(warehouseIds, itemIds);
        }

        /// <summary>
        /// 该仓库下是否存在现有量大于0的库存数据
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public virtual bool IsWareHouseHasOnHand(double warehouseId)
        {
            var onhands = RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhandDatas(warehouseId, "");
            if (onhands.Any(p => p.Qty > 0))
            {
                return true;
            }
            return false;
        }
    }
}
