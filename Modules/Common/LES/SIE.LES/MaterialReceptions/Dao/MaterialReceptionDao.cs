using NPOI.Util;
using SIE.Core.Common.Dao;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.LES.StockOrders;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.MaterialReceptions.Dao
{
    /// <summary>
    /// 物料接收dao
    /// </summary>
    public class MaterialReceptionDao : BaseDao<MaterialReception>
    {
        /// <summary>
        /// 查询获取物料接收信息
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<MaterialReception> GetMaterialReceptions(MaterialReceptionCriterial criteria)
        {
            var query = Query();
            if (criteria == null)
            {
                throw new ValidationException("物料接收查询实体数据异常!".L10N());
            }
            if (!criteria.StockOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.StockOrder.No.Contains(criteria.StockOrderNo));
            }
            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State.Value);
            }
            if (!criteria.ItemCode.IsNullOrEmpty())
            {
                query.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            }
            if (!criteria.ItemName.IsNullOrEmpty())
            {
                query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            }
            if (!criteria.LabelNo.IsNullOrEmpty())
            {
                query.Where(p => p.LabelNo.Contains(criteria.LabelNo));
            }
            if (!criteria.LotNo.IsNullOrEmpty())
            {
                query.Where(p => p.LotNo.Contains(criteria.LotNo));
            }
            if (criteria.WarehouseId != 0 && criteria.WarehouseId != null)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
            }
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.ResourceId != 0 && criteria.ResourceId != null)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId.Value);
            }
            if (!criteria.SoNo.IsNullOrEmpty())
            {
                query.Where(p => p.SoNo.Contains(criteria.SoNo));
            }
            if (criteria.ReceiverId != 0 && criteria.ReceiverId != null)
            {
                query.Where(p => p.ReceiverId == criteria.ReceiverId);
            }
            if (criteria.ReceiverTime.BeginValue.HasValue)
            {
                query.Where(p => p.ReceiveTime >= criteria.ReceiverTime.BeginValue.Value);
            }
            if (criteria.ReceiverTime.EndValue.HasValue)
            {
                query.Where(p => p.ReceiveTime <= criteria.ReceiverTime.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据备料单Id获取备料单
        /// </summary>
        /// <param name="stockOrderIds"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrder> GetStockOrders(List<double> stockOrderIds)
        {
            var stockOrder = stockOrderIds.SplitContains(tempIds =>
            {
                return DB.Query<StockOrder>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return stockOrder;
        }

        /// <summary>
        /// 获取接收人
        /// </summary>
        /// <param name="receiverIds"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmployees(List<double?> receiverIds)
        {
            var employeeList = receiverIds.SplitContains(tempIds =>
            {
                return DB.Query<Employee>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return employeeList;
        }

        /// <summary>
        /// 获取接收明细
        /// </summary>
        /// <param name="detailIds"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetails(List<double> detailIds)
        {
            var detailList = detailIds.SplitContains(tempIds =>
            {
                return DB.Query<StockOrderDetail>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return detailList;
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="itemsIds"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItems(List<double> itemsIds)
        {
            var itemList = itemsIds.SplitContains(tempIds =>
            {
                return DB.Query<Item>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return itemList;
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="ordersIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(List<double?> ordersIds)
        {
            var workOrderList = ordersIds.SplitContains(tempIds =>
            {
                return DB.Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return workOrderList;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="rescourceIds"></param>
        /// <returns></returns>
        public virtual EntityList<Resource> GetResources(List<double?> rescourceIds)
        {
            var resourceList = rescourceIds.SplitContains(tempIds =>
            {
                return DB.Query<Resource>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return resourceList;
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="warehouseIds"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetWarehouses(List<double?> warehouseIds)
        {
            var wareList = warehouseIds.SplitContains(tempIds =>
            {
                return DB.Query<Warehouse>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return wareList;
        }

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(List<double?> locationIds)
        {
            var storageList = locationIds.SplitContains(tempIds =>
            {
                return DB.Query<StorageLocation>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return storageList;
        }

        /// <summary>
        /// 获取备料单sn
        /// </summary>
        /// <param name="stockOrderSnIds"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderSn> GetStockOrderSns(List<double> stockOrderSnIds)
        {
            var snList = stockOrderSnIds.SplitContains(tempIds =>
            {
                return DB.Query<StockOrderSn>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return snList;
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="unitIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<Unit> GetUnits(List<double?> unitIds)
        {
            var unitList = unitIds.SplitContains(tempIds =>
            {
                return DB.Query<Unit>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return unitList;
        }

        /// <summary>
        /// 获取管控方式
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<ItemStockDataBase> GetItemStockDataBases(List<double> itemIds)
        {
            var itemstockList = itemIds.SplitContains(tempIds =>
            {
                return DB.Query<ItemStockDataBase>().Where(p => tempIds.Contains(p.ItemId)).ToList();
            });
            return itemstockList;
        }
    }
}
