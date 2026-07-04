using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件库存控制器
    /// </summary>
    public partial class StoreSummaryController : DomainController
    {
        /// <summary>
        /// 获取有可用库存的库位列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="qualityStatus">质量状态</param>
        /// <param name="sparePartId">备件Id</param>
        /// <param name="controlMethod">管控方式</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationForOutDepot(double warehouseId, QualityStatus qualityStatus, double? sparePartId = null, ControlMethod? controlMethod = null,string keyword=null,PagingInfo pageInfo = null)
        {
            EntityList<StorageLocation> storeList = new EntityList<StorageLocation>();

            var queryItem = Query<StoreSummaryLocation>()
                .Where(p => p.WarehouseId == warehouseId)
                .WhereIf(qualityStatus == QualityStatus.Good, p => p.GoodNumber > 0)
                .WhereIf(qualityStatus == QualityStatus.Defective, p => p.RotNumber > 0)
                .WhereIf(sparePartId != null, p => p.StoreSummary.SparePartId == sparePartId)
                .WhereIf(keyword.IsNotEmpty(), p=>p.StorageLocation.Code.Contains(keyword) || p.StorageLocation.Name.Contains(keyword))
                .GroupBy(p => new { p.StorageLocationId, p.StorageLocation.Code, p.StorageLocation.Name,p.StorageLocation.LibraryType, WarehouseCode = p.StorageLocation.Warehouse.Code, WarehouseName = p.StorageLocation.Warehouse.Name, AreaCode = p.StorageLocation.Area.Code, AreaName = p.StorageLocation.Area.Name, p.StorageLocation.IsFrozen })
                .Select(p => new { Id = p.StorageLocationId, Code = p.StorageLocation.Code, Name = p.StorageLocation.Name, p.StorageLocation.LibraryType, WarehouseCode = p.StorageLocation.Warehouse.Code, WarehouseName = p.StorageLocation.Warehouse.Name, AreaCode = p.StorageLocation.Area.Code, AreaName = p.StorageLocation.Area.Name, p.StorageLocation.IsFrozen,LayerNo = p.GoodNumber.SUM(), RowNo = p.RotNumber.SUM() });

            var queryLot = Query<StoreSummaryLot>()
                .Where(p => p.WarehouseId == warehouseId)
                .WhereIf(qualityStatus == QualityStatus.Good, p => p.GoodNumber > 0)
                .WhereIf(qualityStatus == QualityStatus.Defective, p => p.RotNumber > 0)
                .WhereIf(sparePartId != null, p => p.StoreSummary.SparePartId == sparePartId)
                .WhereIf(keyword.IsNotEmpty(), p => p.StorageLocation.Code.Contains(keyword) || p.StorageLocation.Name.Contains(keyword))
                .GroupBy(p => new { p.StorageLocationId, p.StorageLocation.Code, p.StorageLocation.Name, p.StorageLocation.LibraryType, WarehouseCode = p.StorageLocation.Warehouse.Code, WarehouseName = p.StorageLocation.Warehouse.Name, AreaCode = p.StorageLocation.Area.Code, AreaName = p.StorageLocation.Area.Name, p.StorageLocation.IsFrozen })
                .Select(p => new { Id = p.StorageLocationId, Code = p.StorageLocation.Code, Name = p.StorageLocation.Name, p.StorageLocation.LibraryType, WarehouseCode = p.StorageLocation.Warehouse.Code, WarehouseName = p.StorageLocation.Warehouse.Name, AreaCode = p.StorageLocation.Area.Code, AreaName = p.StorageLocation.Area.Name, p.StorageLocation.IsFrozen, LayerNo = p.GoodNumber.SUM(), RowNo = p.RotNumber.SUM() });

            var queryDetail = Query<StoreSummaryDetail>()
                .Where(p => p.WarehouseId == warehouseId)
                .WhereIf(qualityStatus == QualityStatus.Good, p => p.GoodNumber > 0)
                .WhereIf(qualityStatus == QualityStatus.Defective, p => p.RotNumber > 0)
                .WhereIf(sparePartId != null, p => p.StoreSummary.SparePartId == sparePartId)
                .WhereIf(keyword.IsNotEmpty(), p => p.StorageLocation.Code.Contains(keyword) || p.StorageLocation.Name.Contains(keyword))
                .GroupBy(p => new { p.StorageLocationId, p.StorageLocation.Code, p.StorageLocation.Name, p.StorageLocation.LibraryType, WarehouseCode = p.StorageLocation.Warehouse.Code, WarehouseName = p.StorageLocation.Warehouse.Name, AreaCode = p.StorageLocation.Area.Code, AreaName = p.StorageLocation.Area.Name, p.StorageLocation.IsFrozen })
                .Select(p => new { Id = p.StorageLocationId, Code = p.StorageLocation.Code, Name = p.StorageLocation.Name, p.StorageLocation.LibraryType, WarehouseCode = p.StorageLocation.Warehouse.Code, WarehouseName = p.StorageLocation.Warehouse.Name, AreaCode = p.StorageLocation.Area.Code, AreaName = p.StorageLocation.Area.Name, p.StorageLocation.IsFrozen, LayerNo = p.GoodNumber.SUM(), RowNo = p.RotNumber.SUM() });

            IList<StorageLocation> list = null;

            if (controlMethod == ControlMethod.ItemCode)
            {
                list = queryItem.ToList<StorageLocation>();
            }
            else if (controlMethod == ControlMethod.Batch)
            {
                list = queryLot.ToList<StorageLocation>(); 
            }
            else if (controlMethod == ControlMethod.Sn)
            {
                list = queryDetail.ToList<StorageLocation>();
            }
            else
            {
                storeList.AddRange(queryItem.ToList<StorageLocation>());
                storeList.AddRange(queryLot.ToList<StorageLocation>());
                storeList.AddRange(queryDetail.ToList<StorageLocation>());
                list = storeList.Select(p => p.Id).Distinct().SplitContains(tempIds =>
                {
                    return Query<StorageLocation>().Where(p => tempIds.Contains(p.Id))
                                                   .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                                                   .ToList(pageInfo,new EagerLoadOptions().LoadWithViewProperty());
                });
                storeList.Clear();
            }

            list.ForEach(p =>
            {
                p.RoutewayId = qualityStatus == QualityStatus.Good ? p.LayerNo : p.RowNo;
                if (sparePartId == null) 
                {
                    p.RoutewayId = null;
                }
            });

            storeList.AddRange(list);
            return storeList;
        }

        /// <summary>
        /// 获取现场入库时的库位列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="sparePartId">备件Id</param>
        /// <param name="controlMethod">管控方式</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationForSparePartStore(double warehouseId, double? sparePartId = null, ControlMethod? controlMethod = null)
        {
            EntityList<StorageLocation> storeList = new EntityList<StorageLocation>();

            var queryItem = Query<StoreSummaryLocation>()
                .Where(p => p.WarehouseId == warehouseId)
                .Where(p => p.GoodNumber > 0 || p.RotNumber > 0)
                .Where(p => p.StoreSummary.SparePartId == sparePartId)
                .GroupBy(p => new { p.StorageLocationId, p.StorageLocation.Code, p.StorageLocation.Name })
                .Select(p => new { Id = p.StorageLocationId, Code = p.StorageLocation.Code, Name = p.StorageLocation.Name});

            var queryLot = Query<StoreSummaryLot>()
                .Where(p => p.WarehouseId == warehouseId)
                .Where(p => p.GoodNumber > 0 || p.RotNumber > 0)
                .Where(p => p.StoreSummary.SparePartId == sparePartId)
                .GroupBy(p => new { p.StorageLocationId, p.StorageLocation.Code, p.StorageLocation.Name })
                .Select(p => new { Id = p.StorageLocationId, Code = p.StorageLocation.Code, Name = p.StorageLocation.Name});

            var queryDetail = Query<StoreSummaryDetail>()
                .Where(p => p.WarehouseId == warehouseId)
                .Where(p => p.GoodNumber > 0 || p.RotNumber > 0)
                .Where(p => p.StoreSummary.SparePartId == sparePartId)
                .GroupBy(p => new { p.StorageLocationId, p.StorageLocation.Code, p.StorageLocation.Name })
                .Select(p => new { Id = p.StorageLocationId, Code = p.StorageLocation.Code, Name = p.StorageLocation.Name });

            IList<StorageLocation> list = null;

            if (controlMethod == ControlMethod.ItemCode)
            {
                list = queryItem.Distinct().ToList<StorageLocation>();
            }
            else if (controlMethod == ControlMethod.Batch)
            {
                list = queryLot.Distinct().ToList<StorageLocation>();
            }
            else
            {
                list = queryDetail.Distinct().ToList<StorageLocation>();
            }

            if (sparePartId == null || !list.Any())
            {
                storeList = Query<StorageLocation>().Where(p => p.WarehouseId == warehouseId).ToList();
            }
            else 
            {
                storeList.AddRange(list);
            }

            return storeList;
        }

        /// <summary>
        /// 获取入库时的库位列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns>库位列表</returns>
        public virtual EntityList<StorageLocation> GetStorageLocationForStoreDetails(double warehouseId,string keyword,PagingInfo pageInfo)
        {
            var storeList = Query<StorageLocation>()
                            .WhereIf(keyword.IsNotEmpty(),p=>p.Code.Contains(keyword) || p.Name.Contains(keyword))
                            .Where(p=>p.WarehouseId == warehouseId)
                            .ToList(pageInfo);

            return storeList;
        }

        /// <summary>
        /// 根据序列号获取序列号明细集合
        /// </summary>
        /// <param name="CodeList"></param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryDetail> GetStoreSummaryDetails(List<string> CodeList)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            elo.LoadWith(StoreSummary.SparePartProperty);

            return CodeList.SplitContains((codes) =>
            {
                return Query<StoreSummaryDetail>().Where(p => codes.Contains(p.OrderNumberCode)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据序列号获取批次明细集合
        /// </summary>
        /// <param name="NumberList"></param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLot> GetStoreSummaryLots(List<string> NumberList)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            elo.LoadWith(StoreSummary.SparePartProperty);

            return NumberList.SplitContains((numbers) =>
            {
                return Query<StoreSummaryLot>().Where(p => numbers.Contains(p.BatchNumber)).ToList(null, elo);
            });
        }
        
    }
}
