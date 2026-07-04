using SIE.Common.Configs;
using SIE.Domain;
using SIE.Warehouses.Common;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses
{
    /// <summary>
    /// 物料扩展控制器
    /// </summary>
    public class BaseItemExtController : DomainController
    {
        /// <summary>
        /// 获取物料收发控制信息
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns>物料收发控制</returns>
        public virtual EntityList<BaseItemIoLimit> GetItemIOLimit(double itemId)
        {
            return Query<BaseItemIoLimit>().Where(p => p.ItemId == itemId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料收发控制信息
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>物料收发控制</returns>
        public virtual BaseItemIoLimit GetItemIOLimit(double itemId, double warehouseId)
        {
            return Query<BaseItemIoLimit>().Where(p => p.ItemId == itemId && p.WarehouseId == warehouseId).FirstOrDefault();
        }

        /// <summary>
        /// 获取收发控制
        /// </summary>
        /// <param name="warehouseIds">仓库IDs</param>
        /// <returns>收发控制</returns>
        public virtual EntityList<BaseItemIoLimit> GetBaseItemIoLimit(List<double> warehouseIds)
        {
            return Query<BaseItemIoLimit>().Where(p => warehouseIds.Contains(p.WarehouseId)).ToList();
        }

        /// <summary>
        /// 获取收发控制
        /// </summary>
        /// <param name="warehouseIds">仓库IDs</param>
        /// <param name="itemIds"></param>
        /// <returns>收发控制</returns>
        public virtual EntityList<BaseItemIoLimit> GetBaseItemIoLimit(List<double> warehouseIds, List<double> itemIds)
        {
            return warehouseIds.SplitContains(tempWhIds =>
            {
                return itemIds.SplitContains(tempItemIds =>
                {
                    return Query<BaseItemIoLimit>()
                        .Where(p => tempWhIds.Contains(p.WarehouseId)
                            && tempItemIds.Contains(p.ItemId))
                        .ToList();

                });
            });
        }
    }
}
