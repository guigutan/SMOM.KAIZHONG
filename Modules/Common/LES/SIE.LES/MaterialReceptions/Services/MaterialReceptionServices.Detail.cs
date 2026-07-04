using SIE.Core.Common.Service;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Items;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.MaterialReceptions.Enums;
using SIE.LES.StockOrders;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SIE.LES.MaterialReceptions.Services
{
    public partial class MaterialReceptionServices : DomainService
    {
        /// <summary>
        /// 扫描
        /// </summary>
        /// <param name="scanParamters"></param>
        /// <returns></returns>
        public virtual ScanParamters ScanByDetail(ScanParamters scanParamters)
        {
            var validateResult = RT.Service.Resolve<MaterialReceptionServices>().ScanValidate(scanParamters);
            if (validateResult.Isvalidated)
            {
                var index = !scanParamters.ScanRecords.Any() ? 0 : scanParamters.ScanRecords.Max(m => Convert.ToInt32(m.Index));
                var configType = GetReceiveConfig();
                var stockOrderSnIds = scanParamters.StockOrderSnList.Select(x => x.Id).Distinct().ToList();
                // 备料单接收记录
                var stockOrderSnList = _materialReceptionDao.GetStockOrderSns(stockOrderSnIds);
                var stockOrderIds = stockOrderSnList.Select(p => p.StockOrderId).Distinct().ToList();
                var stockOrderDtlIds = stockOrderSnList.Select(p => p.StockOrderDetailId).Distinct().ToList();
                // 备料单
                var stockOrderList = _materialReceptionDao.GetStockOrders(stockOrderIds);
                // 备料单物料需求明细
                var stockOrderDtlList = _materialReceptionDao.GetStockOrderDetails(stockOrderDtlIds);
                var itemIds = stockOrderSnList.Select(p => p.ItemId).Distinct().ToList();
                // 物料
                var itemList = _materialReceptionDao.GetItems(itemIds);
                var wipIds = stockOrderList.Select(p => p.ResourceId).Distinct().ToList();
                // 资源
                var wipList = _materialReceptionDao.GetResources(wipIds);
                var workOrderIds = stockOrderList.Select(p => p.WorkOrderId).Distinct().ToList();
                // 工单
                var workOrderList = _materialReceptionDao.GetWorkOrders(workOrderIds);
                var wareIds = stockOrderDtlList.Select(p => p.WarehouseId).Distinct().ToList();
                // 仓库
                var wareList = _materialReceptionDao.GetWarehouses(wareIds);
                var storageIds = stockOrderDtlList.Select(p => p.StorageLocationId).Distinct().ToList();
                // 库位
                var storageList = _materialReceptionDao.GetStorageLocations(storageIds);
                var unitIds = itemList.Select(p => p.UnitId).Distinct().ToList();
                // 单位
                var unitList = _materialReceptionDao.GetUnits(unitIds);
                // 管控方式
                var itemStockList = _materialReceptionDao.GetItemStockDataBases(itemIds);
                scanParamters.StockOrderSnList.ForEach(stockOrderSn =>
                {
                    index++;
                    var stockOrder = stockOrderList.FirstOrDefault(p => p.Id == stockOrderSn.StockOrderId);
                    var stockOrderDtl = stockOrderDtlList.FirstOrDefault(p => p.Id == stockOrderSn.StockOrderDetailId);
                    var material = itemList.FirstOrDefault(p => p.Id == stockOrderSn.ItemId);
                    var wip = wipList.FirstOrDefault(p => p.Id == stockOrder?.ResourceId);
                    var ware = wareList.FirstOrDefault(p => p.Id == stockOrderDtl?.WarehouseId);
                    var storage = storageList.FirstOrDefault(p => p.Id == stockOrderDtl?.StorageLocationId);
                    var unit = unitList.FirstOrDefault(p => p.Id == material?.UnitId);
                    var itemStockDataBase = itemStockList.FirstOrDefault(p => p.ItemId == stockOrderSn.ItemId);
                    var workOrder = workOrderList.FirstOrDefault(p => p.Id == stockOrder?.WorkOrderId);
                    validateResult.NewRecords.Add(CreateReceiveLabelInfo(stockOrderSn, index.ToString(), (int)validateResult.ObjectType, configType, stockOrder, stockOrderDtl, material, wip, ware, storage, unit, itemStockDataBase, workOrder));
                });
            }
            return validateResult;
        }


        /// <summary>
        /// 添加新记录
        /// </summary>
        /// <param name="stockOrderSn"></param>
        /// <param name="index"></param>
        /// <param name="objectType"></param>
        /// <param name="configType"></param>
        /// <param name="stockOrder"></param>
        /// <param name="stockOrderDtl"></param>
        /// <param name="item"></param>
        /// <param name="wip"></param>
        /// <param name="ware"></param>
        /// <param name="storage"></param>
        /// <param name="unit"></param>
        /// <param name="itemStockDataBase"></param>
        /// <param name="workOrder"></param>
        /// <returns></returns>
        private MaterialReceptionInfo CreateReceiveLabelInfo(StockOrderSn stockOrderSn, string index, int objectType, ConfigValues configType, StockOrder stockOrder, StockOrderDetail stockOrderDtl, Items.Item item,
            Resource wip, Warehouse ware, StorageLocation storage, Unit unit, ItemStockDataBase itemStockDataBase, WorkOrder workOrder)
        {
            MaterialReceptionInfo receive = new MaterialReceptionInfo();
            receive.Id = stockOrderSn.Id;
            receive.Label = stockOrderSn.Sn;
            receive.ItemId = stockOrderSn.ItemId;
            receive.ItemCode = stockOrderSn.ItemCode;
            receive.ItemName = stockOrderSn.ItemName;
            receive.ItemExtProp = stockOrderDtl.ItemExtProp;
            receive.ItemExtPropName = stockOrderDtl.ItemExtPropName;
            receive.SpecificationModel = item.SpecificationModel;
            receive.FactoryId = stockOrder.FactoryId;
            if (workOrder != null)
            {
                receive.WorkOrderId = workOrder.Id;
                receive.WorkOrderNo = workOrder.No;
            }
            receive.BillId = stockOrder.Id;
            receive.BillNo = stockOrder.No;
            receive.BillDtlId = stockOrderDtl.Id;
            receive.Qty = configType == Enums.ConfigValues.AllAccept ? stockOrderSn.ShipQty - stockOrderSn.Qty : 0;
            receive.StayQty = stockOrderSn.ShipQty - stockOrderSn.Qty;
            receive.BatchNo = stockOrderSn.LotNo;
            if (wip != null)
            {
                receive.ResourceId = wip.Id;
                receive.ResourceName = wip.Name;
            }
            if (storage != null)
            {
                receive.ReceiveStorageLocationId = storage.Id;
                receive.ReceiveStorageLocationName = storage.Name;
            }
            if (ware != null)
            {
                receive.ReceiveWarehouseId = ware.Id;
                receive.ReceiveWarehouseName = ware.Name;
            }
            receive.SoNo = stockOrderSn.SoNo;
            receive.LineNo = stockOrderSn.LineNo;
            receive.SoLineNo = stockOrderSn.SoLineNo;
            receive.UnitName = unit.Name;
            receive.ItemType = item.Type.ToLabel();
            receive.ReceiveBy = RT.IdentityId;
            receive.ShipQty = stockOrderSn.ShipQty - stockOrderSn.Qty;
            receive.Index = index;
            receive.ScanType = objectType < 3 ? 0 : 1;
            receive.ReceiveRowNumber = stockOrderDtl.LineNo;
            receive.StockStateDisplay = stockOrderDtl.StockState.ToLabel();
            receive.IsSerialNumberControl = itemStockDataBase.IsSerialNumber??false;
            return receive;
        }


        /// <summary>
        /// 根据订单创建接收信息
        /// </summary>
        /// <param name="order"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private MaterialReceptionInfo CreateReceiveInfoByOrder(StockOrder order, string index)
        {
            MaterialReceptionInfo receive = new MaterialReceptionInfo();
            var stockOrder = order;
            receive.FactoryId = stockOrder.FactoryId;
            receive.WorkOrderId = stockOrder.WorkOrderId ?? 0;
            receive.WorkOrderNo = stockOrder.WorkOrder?.No;
            receive.BillId = stockOrder.Id;
            receive.BillNo = stockOrder.No;
            receive.Index = index;
            receive.ResourceId = stockOrder.ResourceId ?? 0;
            receive.ResourceName = stockOrder.Resource?.Name;
            receive.ReceiveBy = RT.IdentityId;
            receive.StockState = order.StockState;
            receive.Factory = order.FactoryName;
            receive.StockStateDisplay = order.StockState.ToLabel();
            receive.StayQty = stockOrder.StockOrderDetailList.Sum(x => x.UnFinishQty);
            return receive;
        }

    }
}
