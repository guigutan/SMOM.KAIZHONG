using SIE.Core.Common.Service;
using SIE.Items;
using SIE.LES.MaterialReceptions.Dao;
using SIE.LES.StockOrders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.Packages.ItemLabels;
using SIE.Warehouses;
using SIE.Domain;
using System.Diagnostics;

namespace SIE.LES.MaterialReceptions.Services
{
    /// <summary>
    ///物料接收验证服务
    /// </summary>
    public partial class MaterialReceptionServices : DomainService
    {
        /// <summary>
        /// 物料接收数据访问
        /// </summary>
        private readonly MaterialReceptionDao _materialReceptionDao;

        /// <summary>
        /// 接收记录数据访问
        /// </summary>
        private readonly StockOrderSnDao _stockOrderSnDao;


        /// <summary>
        /// 物料扩展属性数据访问
        /// </summary>
        private readonly ItemStockDataBaseDao _itemStockDataBaseDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialReceptionServices(StockOrderSnDao stockOrderSnDao, ItemStockDataBaseDao itemStockDataBaseDao, MaterialReceptionDao materialReceptionDao)
        {
            _stockOrderSnDao = stockOrderSnDao;
            _itemStockDataBaseDao = itemStockDataBaseDao;
            _materialReceptionDao = materialReceptionDao;
        }

        /// <summary>
        /// 验证扫描数据
        /// </summary>
        /// <param name="scanParamters"></param>
        /// <returns></returns>
        public virtual ScanParamters ScanValidate(ScanParamters scanParamters)
        {
            ObjectType objectType = 0;
            List<StockOrderSn> resultEntitys = new List<StockOrderSn>();
            if (scanParamters.Oparetion == 1)//按明细接收
            {
                resultEntitys = _stockOrderSnDao.GetStockOrderByItemCode(scanParamters.Keyword, out objectType);
            }
            if (scanParamters.Oparetion == 2)//按单号接收
            {
                resultEntitys = _stockOrderSnDao.GetStockOrderByKeAndNo(scanParamters.Keyword, scanParamters.StockOrderNo, out objectType);
            }
            scanParamters.ObjectType = objectType;

            if (!resultEntitys.Any())
            {
                scanParamters.ErroMes = "扫描内容不正确,请扫描标签号".L10N();
                scanParamters.Isvalidated = false;
                return scanParamters;
            }
            if (scanParamters.ResourcesId.HasValue)
            {//只带出当前产线得备料单
                resultEntitys = resultEntitys.Where(p => p.StockOrder.ResourceId == scanParamters.ResourcesId).ToList();
            }
            if (!resultEntitys.Any())
            {
                scanParamters.ErroMes = "扫描内容不正确,未找到相关单据".L10N();
                scanParamters.Isvalidated = false;
                return scanParamters;
            }
            var isSNManange = ValidateIsSerialNumber(resultEntitys.First().ItemId);
            scanParamters.StockOrderSnList.AddRange(resultEntitys);
            //序列号管理 校验已扫描记录是否重复
            if (isSNManange.HasValue && isSNManange.Value)
            {
                if (SNIsRepeat(scanParamters.ScanRecords, resultEntitys.First()))
                {
                    scanParamters.ErroMes = "标签已扫描成功，请勿重复扫描".L10N();
                    scanParamters.Isvalidated = false;
                    return scanParamters;
                }//判断物料标签表是否存在同序列号且数量不为0的记录
                if (ItemLabelIsRepeat(resultEntitys.First()))
                {
                    scanParamters.ErroMes = "物料标签表存在相同的序列号标签，标签以前已接收成功，请勿重复接收".L10N();
                    scanParamters.Isvalidated = false;
                    return scanParamters;
                }
            }
            else
            {//非序列号验证
                var res = NotSNValidate(resultEntitys, scanParamters);
                if (!res.Isvalidated)
                {
                    return scanParamters;
                }
                scanParamters.Isvalidated = true;
            }
            return ValidateResoureIsSame(scanParamters, resultEntitys);
        }

        /// <summary>
        /// 非序列号的验证
        /// </summary>
        /// <param name="resultEntitys"></param>
        /// <param name="scanParamters"></param>
        /// <returns></returns>
        public virtual ScanParamters NotSNValidate(List<StockOrderSn> resultEntitys, ScanParamters scanParamters)
        {
            //能否找到唯一备料单
            var stockOrderIds = resultEntitys.Select(m => m.StockOrderId).Distinct().ToList();
            //校验是否重复
            foreach (var item in resultEntitys)
            {
                if (scanParamters.ScanRecords.FindIndex(m => m.Id == item.Id && m.BillId == item.StockOrderId) >= 0)
                {
                    scanParamters.ErroMes = "列表已存在相同的扫描记录，请检查";
                    scanParamters.Isvalidated = false;
                    return scanParamters;
                }
            }

            if (stockOrderIds.Count > 1)
            {
                scanParamters.ErroMes = "不唯一".L10N();
                foreach (var item in stockOrderIds)
                {
                    var order = resultEntitys.FirstOrDefault(m => m.StockOrderId == item);
                    if (order != null)
                    {
                        scanParamters.StockOrderListForSelect.Add(
                        new Core.ApiModels.BaseDataInfo()
                        {
                            Id = order.StockOrderId,
                            Code = order.StockOrderNo,
                            Name = order.StockOrderNo
                        });
                    }
                }

                scanParamters.Isvalidated = true;
                scanParamters.NeedGotoDetail = true;
            }

            //将同标签号、同批次号、同物料号、同备料单号 的进行汇总。总扫描数量=已扫描数据+本次扫描数量。判断总扫描量是否大于待收数量
            var dic = resultEntitys.GroupBy(p => new { p.StockOrderId, p.LotNo, p.Sn, p.ItemId }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var key in dic.Keys)
            {
                var tempEntity = dic[key].First();
                var recivedQty = dic[key].Sum(m => m.Qty);
                var tobeReciveQty = dic[key].Sum(m => m.ShipQty);

                //已扫数据
                var scandList = scanParamters.ScanRecords.FindAll(m => m.BillId == tempEntity.StockOrderId && m.Label == tempEntity.Sn
                  && m.BatchNo == tempEntity.LotNo && m.ItemId == tempEntity.ItemId);
                var scanRecivedQty = scandList.Sum(m => m.Qty);
                var scanTobeReciveQty = scandList.Sum(m => m.ShipQty);
                if (recivedQty + scanRecivedQty > tobeReciveQty + scanTobeReciveQty)
                {
                    scanParamters.ErroMes = "总扫描量大于待收数量，扫描无效，请重新扫描".L10N();
                    scanParamters.Isvalidated = false;
                    return scanParamters;
                }
            }
            scanParamters.Isvalidated = true;
            return scanParamters;
        }

        /// <summary>
        /// 验证资源是否相同
        /// </summary>
        /// <param name="scanParamters"></param>
        /// <param name="resultEntitys"></param>
        /// <returns></returns>

        public virtual ScanParamters ValidateResoureIsSame(ScanParamters scanParamters, List<StockOrderSn> resultEntitys)
        {
            var stockOrder = resultEntitys.First().StockOrder;
            if (scanParamters.ResourcesId.HasValue)
            {
                var stockOrderDetial = resultEntitys.FirstOrDefault(m => m.StockOrder.ResourceId == scanParamters.ResourcesId);
                stockOrder = stockOrderDetial?.StockOrder;
                if (stockOrder == null || scanParamters.ResourcesId != stockOrder.ResourceId)
                {
                    scanParamters.ErroMes = "验证不通过！备料单生产资源与当前所选资源不一致，请先提交当前产线记录".L10N();
                    scanParamters.Isvalidated = false;
                    return scanParamters;
                }
            }
            if (stockOrder.ResourceId.HasValue)
            {
                scanParamters.ResourcesId = stockOrder.ResourceId;
                scanParamters.ResourcesName = stockOrder.Resource.Name;
            }
            scanParamters.Isvalidated = true;
            return scanParamters;
        }


        /// <summary>
        /// 验证是否是序列号管理
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual bool? ValidateIsSerialNumber(double itemId)
        {
            var itemStockDataBase = _itemStockDataBaseDao.GetItemStockDataBase(itemId);
            if (itemStockDataBase != null)
            {
                return itemStockDataBase.IsSerialNumber == true;
            }
            return false;
        }

        /// <summary>
        /// 是否标签管理
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual bool? ValidateIsLabelManage(double itemId)
        {
            var itemStockDataBase = _itemStockDataBaseDao.GetItemBaseData(itemId);
            if (itemStockDataBase != null)
            {
                return itemStockDataBase.IsLabel == true;
            }
            return false;
        }

        /// <summary>
        ///验证是否批次管理
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual bool? ValidateIsBitchManage(double itemId)
        {
            var itemStockDataBase = _itemStockDataBaseDao.GetItemStockDataBase(itemId);
            if (itemStockDataBase != null)
            {
                return itemStockDataBase.IsBatch == true;
            }
            return false;
        }


        /// <summary>
        /// 序列号是否重复
        /// </summary>
        /// <param name="scanRecords"></param>
        /// <param name="stockOrderSn"></param>
        /// <returns></returns>
        public virtual bool SNIsRepeat(List<MaterialReceptionInfo> scanRecords, StockOrderSn stockOrderSn)
        {
            return scanRecords.FindIndex(m => m.Label == stockOrderSn.Sn) >= 0;
        }

        /// <summary>
        /// 判断物料标签表是否已存在该物料标签
        /// </summary>
        /// <param name="stockOrderSn"></param>
        /// <returns></returns>
        public virtual bool ItemLabelIsRepeat(StockOrderSn stockOrderSn)
        {
            var itemLable = RT.Service.Resolve<ItemLabelController>().GetItemLabel(stockOrderSn.Sn);
            if (itemLable != null&&itemLable.SourceType!= LabelSource.Wip&&itemLable.SourceType!= LabelSource.BatchWip)
            {
                return itemLable.Qty > 0;
            }
            return false;
        }

        //按单查询
        // 依次查找备料单、发运单，判断是否找到唯一备料单 不唯一 在扫描单号页面，显示备料单
        /// <summary>
        /// 获取备料单
        /// </summary>
        /// <param name="scanParamters"></param>
        public virtual void ValidateStockOrders(ScanParamters scanParamters)
        {
            var reslist = _stockOrderSnDao.GetStockOrderByKeyWord(scanParamters.Keyword);
            var index = 0;
            if (!reslist.Any())//不存在查询对象
            {
                scanParamters.Isvalidated = false;
                scanParamters.ErroMes = "扫描内容不存在于系统，请扫描正确的备料单号或发运单号".L10N();
                return;
            }
            foreach (var order in reslist)
            {
                index++;
                var newOrderRecord = CreateReceiveInfoByOrder(order, index.ToString());
                if (newOrderRecord.StayQty > 0&& newOrderRecord.StockState== StockState.TobeReceive)//待接受数大于0
                {
                    scanParamters.NewOrderRecords.Add(newOrderRecord);
                }
                else
                {
                    scanParamters.Isvalidated = false;
                    scanParamters.ErroMes = "备料单待接收数量为0".L10N();
                    return;
                }
            }
            if (!scanParamters.NewOrderRecords.Any())
            {
                scanParamters.Isvalidated = false;
                scanParamters.ErroMes = "未找到备料单，或备料单待接收数量为0".L10N();
                return;
            }
            scanParamters.Isvalidated = true;
        }

        /// <summary>
        /// 验证改变后的备料单号并返回其明细
        /// </summary>
        /// <param name="scanParamters"></param>
        public virtual void ValidateChangedStockOrder(ScanParamters scanParamters)
        {
            scanParamters.Isvalidated = false;
            if (scanParamters.StockOrderNo.IsNullOrEmpty())
            {
                scanParamters.ErroMes = "请选择备料单".L10N();
                scanParamters.Isvalidated = false;
                return;
            }
            var orderTuply = _stockOrderSnDao.GetStockOrder(scanParamters.StockOrderNo);
            if (orderTuply == null)
            {
                scanParamters.ErroMes = "备料单不存在系统中，请检查".L10N();
                scanParamters.Isvalidated = false;
                return;
            }
            if (orderTuply.Item1.StockState != StockState.TobeReceive
                )
            {
                scanParamters.ErroMes = "备料单状态不为【待接收】,不允许接收".L10N();
                scanParamters.Isvalidated = false;
                return;
            }
            if (orderTuply.Item2.Any())
            {
                var index = 0;
                // 配置项的接收方式
                var configType = GetReceiveConfig();
                var stockOrderSnIds = orderTuply.Item2.Select(x => x.Id).Distinct().ToList();
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
                foreach (var item in orderTuply.Item2)
                {
                    index++;
                    if (item.ShipQty - item.Qty > 0)
                    {
                        var stockOrder = stockOrderList.FirstOrDefault(p => p.Id == item.StockOrderId);
                        var stockOrderDtl = stockOrderDtlList.FirstOrDefault(p => p.Id == item.StockOrderDetailId);
                        var material = itemList.FirstOrDefault(p => p.Id == item.ItemId);
                        var wip = wipList.FirstOrDefault(p => p.Id == stockOrder?.ResourceId);
                        var ware = wareList.FirstOrDefault(p => p.Id == stockOrderDtl?.WarehouseId);
                        var storage = storageList.FirstOrDefault(p => p.Id == stockOrderDtl?.StorageLocationId);
                        var unit = unitList.FirstOrDefault(p => p.Id == material?.UnitId);
                        var itemStockDataBase = itemStockList.FirstOrDefault(p => p.ItemId == item.ItemId);
                        var workOrder = workOrderList.FirstOrDefault(p => p.Id == stockOrder?.WorkOrderId);
                        scanParamters.NewRecords.Add(CreateReceiveLabelInfo(item, index.ToString(), (int)scanParamters.ObjectType, configType, stockOrder, stockOrderDtl, material, wip, ware, storage, unit, itemStockDataBase, workOrder));
                    }
                }
            }
            scanParamters.Isvalidated = true;
        }
    }
}
