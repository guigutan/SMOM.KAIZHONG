using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Linq;

namespace SIE.EMS.Warehouses
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class WarehouseController : DomainController
    {
        /// <summary>
        /// 查询库位
        /// </summary>
        /// <param name="warehouseId">仓库</param>
        /// <param name="p">分页信息</param>
        /// <param name="k">关键字</param>
        /// <returns></returns>
        public virtual EntityList GetStorageLocations(double warehouseId, PagingInfo p, string k)
        {
            var query = Query<StorageLocation>()
                .Where(x => x.WarehouseId == warehouseId);
            if (!k.IsNullOrEmpty())
            {
                query.Where(x => x.Code.Contains(k) || x.Name.Contains(k));
            }
            return query.ToList(p, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取有默认STAGE库位的仓库
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>仓库列表数据</returns>
        public virtual EntityList<Warehouse> GetStageLocatgionWarehouses(string keyword, PagingInfo info)
        {
            var warehouseList = Query<Warehouse>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(info);

            var storageLocationList = warehouseList.Select(p => p.Id).SplitContains(tempIds => {
                return Query<StorageLocation>().Where(p => p.Code == Warehouse.STAGE && tempIds.Contains(p.WarehouseId)).ToList(info);
            });

            foreach (var warehouse in warehouseList)
            {
                var storageLocation = storageLocationList.FirstOrDefault(p => p.WarehouseId == warehouse.Id);

                if (storageLocation != null)
                {
                    warehouse.SetScrapLocationId(storageLocation.Id);
                    warehouse.SetScrapLocationCode(storageLocation.Code);
                }
            }

            return warehouseList;
        }
    }
}
