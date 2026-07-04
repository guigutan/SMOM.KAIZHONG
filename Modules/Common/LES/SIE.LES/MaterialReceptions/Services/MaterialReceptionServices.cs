using Castle.Components.DictionaryAdapter;
using SIE.Common.Sender;
using SIE.Core.Common.Service;
using SIE.Core.WorkOrders;
using SIE.Data.DbMigration.Model;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.MaterialReceptions.Dao;
using SIE.LES.MaterialReceptions.Enums;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.LES.StockOrders;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.MaterialReceptions.Services
{
    public partial class MaterialReceptionServices : DomainService
    {
        /// <summary>
        /// 查询获取物料接收数据
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReception> GetMaterialReceptions(MaterialReceptionCriterial criterial)
        {
            return _materialReceptionDao.GetMaterialReceptions(criterial);
        }

        /// <summary>
        /// 获取明细行
        /// </summary>
        /// <param name="stockOrderSns"></param>
        /// <param name="scanType"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceptionAddViewModel> GetMaterialReceptionByScanType(List<StockOrderSn> stockOrderSns, int scanType)
        {
            ConfigValues receiveType = GetReceiveConfig();
            var materialReceptions = new EntityList<MaterialReceptionAddViewModel>();
            var stockOrderIds = stockOrderSns.Select(p => p.StockOrderId).ToList();
            var stockOrderList = _materialReceptionDao.GetStockOrders(stockOrderIds);
            var workOrderIds = stockOrderList.Select(p => p.WorkOrderId).ToList();
            var workOrderList = _materialReceptionDao.GetWorkOrders(workOrderIds);
            var resourceIds = stockOrderList.Select(p => p.ResourceId).ToList();
            var resourceList = _materialReceptionDao.GetResources(resourceIds);
            var receiverIds = stockOrderSns.Select(p => p.ReceiveById).ToList();
            var receiverList = _materialReceptionDao.GetEmployees(receiverIds);
            var detailIds = stockOrderSns.Select(p => p.StockOrderDetailId).ToList();
            var detailList = _materialReceptionDao.GetStockOrderDetails(detailIds);
            var warehouseIds = detailList.Select(p => p.WarehouseId).ToList();
            var storageLocationIds = detailList.Select(p => p.StorageLocationId).ToList();
            var warehouseList = _materialReceptionDao.GetWarehouses(warehouseIds);
            var storageList = _materialReceptionDao.GetStorageLocations(storageLocationIds);
            var itemIds = stockOrderSns.Select(p => p.ItemId).ToList();
            var itemList = _materialReceptionDao.GetItems(itemIds);
            foreach (var sn in stockOrderSns)
            {
                if (sn.ShipQty - sn.Qty <= 0)
                {
                    continue;
                }
                var stockOrder = stockOrderList.FirstOrDefault(p => p.Id == sn.StockOrderId);
                var item = itemList.FirstOrDefault(p => p.Id == sn.ItemId);
                var detail = detailList.FirstOrDefault(p => p.Id == sn.StockOrderDetailId);
                var receiver = receiverList.FirstOrDefault(p => p.Id == sn.ReceiveById);
                if (stockOrder != null && item != null && detail != null)
                {
                    var materialReception = new MaterialReceptionAddViewModel
                    {
                        DetailId = sn.Id,
                        StockOrderId = stockOrder.Id,
                        StockOrderNo = stockOrder.No,
                        LineNo = sn.LineNo,
                        State = sn.State,
                        ItemId = item.Id,
                        ItemCode = item.Code,
                        ItemName = item.Name,
                        ItemExtProp = detailList.FirstOrDefault(p => p.Id == sn.StockOrderDetailId)?.ItemExtProp,
                        ItemExtPropName = detailList.FirstOrDefault(p => p.Id == sn.StockOrderDetailId)?.ItemExtPropName,
                        ShipQty = sn.ShipQty,
                        LabelNo = sn.Sn,
                        LotNo = sn.LotNo,
                        Qty = receiveType == ConfigValues.AllAccept ? sn.ShipQty - sn.Qty : 0,
                        StayQty = sn.ShipQty - sn.Qty,
                        WorkOrderId = stockOrder.WorkOrderId,
                        WorkOrderNo = workOrderList.FirstOrDefault(p => p.Id == stockOrder.WorkOrderId)?.No,
                        ResourceId = stockOrder.ResourceId,
                        ResourceName = resourceList.FirstOrDefault(p => p.Id == stockOrder.ResourceId)?.Name,
                        WarehouseId = detail.WarehouseId,
                        WarehouseName = warehouseList.FirstOrDefault(p => p.Id == detail.WarehouseId)?.Name,
                        StorageLocationId = detail.StorageLocationId,
                        StorageLocationName = storageList.FirstOrDefault(p => p.Id == detail.StorageLocationId)?.Name,
                        SoNo = sn.SoNo,
                        SoLineNo = sn.SoLineNo,
                        IsManualRec = detail.IsManualRec,
                        ReceiverId = receiver?.Id,
                        ReceiverName = receiver?.Name,
                        ReceiverTime = sn.ReceiveTime,
                        StockOrderDetailId = sn.StockOrderDetailId,
                        FactoryId = stockOrder.FactoryId,
                        StockOrderState = stockOrder.StockState,
                    };
                    materialReceptions.Add(materialReception);
                }

            }
            return materialReceptions;
        }

        /// <summary>
        /// viewmodel转成apiInfo(包装调用方法)
        /// </summary>
        /// <param name="scanRecords"></param>
        /// <returns></returns>
        public virtual List<MaterialReceptionInfo> ViewModelToInfo(List<MaterialReceptionAddViewModel> scanRecords)
        {
            if (scanRecords.Count == 0)
            {
                return new List<MaterialReceptionInfo>();
            }
            var scanList = new List<MaterialReceptionInfo>();
            scanRecords.ForEach(scanRecord =>
            {
                var info = new MaterialReceptionInfo
                {
                    Id = scanRecord.DetailId,
                    LineNo = scanRecord.LineNo,
                    Label = scanRecord.LabelNo,
                    BatchNo = scanRecord.LotNo,
                    ItemId = scanRecord.ItemId,
                    ItemCode = scanRecord.ItemCode,
                    ItemName = scanRecord.ItemName,
                    ItemExtProp = scanRecord.ItemExtProp,
                    ItemExtPropName = scanRecord.ItemExtPropName,
                    DetailState = scanRecord.State.Value,
                    FactoryId = scanRecord.FactoryId,
                    WorkOrderId = scanRecord.WorkOrderId,
                    BillId = scanRecord.StockOrderId,
                    BillDtlId = scanRecord.StockOrderDetailId,
                    Qty = scanRecord.Qty,
                    ShipQty = scanRecord.ShipQty,
                    ResourceId = scanRecord.ResourceId,
                    ReceiveWarehouseId = scanRecord.WarehouseId,
                    ReceiveWarehouseName = scanRecord.WarehouseName,
                    ReceiveStorageLocationId = scanRecord.StorageLocationId,
                    ReceiveStorageLocationName = scanRecord.StorageLocationName,
                    SoNo = scanRecord.SoNo,
                    SoLineNo = scanRecord.SoLineNo,
                    StockState = scanRecord.StockOrderState,
                    IsManualRec = scanRecord.IsManualRec,

                    ReceiveBy = scanRecord.ReceiverId ?? 0,
                    ReceiveTime = scanRecord.ReceiverTime,
                };
                if (info.Qty < 0)
                {
                    throw new ValidationException("接收数量不能小于0！".L10N());
                }
                scanList.Add(info);
            });
            return scanList;
        }

        /// <summary>
        /// info转viewmodel(包装调用方法)
        /// </summary>
        /// <param name="newRecords"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceptionAddViewModel> InfoToViewModel(List<MaterialReceptionInfo> newRecords)
        {
            var snList = new EntityList<MaterialReceptionAddViewModel>();
            newRecords.ForEach(record =>
            {
                var sn = new MaterialReceptionAddViewModel
                {
                    StockOrderId = record.BillId,
                    StockOrderNo = record.BillNo,
                    DetailId = record.Id,
                    StockOrderDetailId = record.BillDtlId,
                    LineNo = record.LineNo,
                    State = record.DetailState,
                    ItemId = record.ItemId,
                    ItemCode = record.ItemCode,
                    ItemName = record.ItemName,
                    ItemExtProp = record.ItemExtProp,
                    ItemExtPropName = record.ItemExtPropName,
                    LabelNo = record.Label,
                    LotNo = record.BatchNo,
                    Qty = record.Qty,
                    ShipQty = record.ShipQty,
                    StayQty = record.StayQty,
                    WarehouseId = record.ReceiveWarehouseId,
                    WarehouseName = record.ReceiveWarehouseName,
                    StorageLocationId = record.ReceiveStorageLocationId,
                    StorageLocationName = record.ReceiveStorageLocationName,
                    WorkOrderId = record.WorkOrderId ?? 0,
                    WorkOrderNo = record.WorkOrderNo,
                    ResourceId = record.ResourceId ?? 0,
                    ResourceName = record.ResourceName,
                    SoNo = record.SoNo,
                    SoLineNo = record.SoLineNo,
                    IsManualRec = record.IsManualRec,
                    ReceiverId = record.ReceiveBy,
                    ReceiverTime = record.ReceiveTime,
                    FactoryId = record.FactoryId,
                };
                snList.Add(sn);
            });
            return snList;
        }
    }
}
