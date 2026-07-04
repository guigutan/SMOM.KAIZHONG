using Irony.Parsing.Construction;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.LES.StockOrders.Dao;
using SIE.Warehouses;
using SIE.Web.LES.StockOrders.WorkOrders;
using System;
using System.ArrayExtensions;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.StockOrders.Service
{
    public class StockOrderDetailService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台维护数据访问
        /// </summary>
        private readonly StockOrderDetailDao _stockOrderDetailDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StockOrderDetailService(StockOrderDetailDao stockOrderDetail)
        {
            _stockOrderDetailDao = stockOrderDetail;
        }
        #endregion

        /// <summary>
        /// 更新物料需求明细
        /// </summary>
        /// <param name="stockOrderDetail">物料需求明细</param>
        /// <param name="Qty">发运数量</param>
        public virtual void UpdateStockDetailQty(StockOrderDetail stockOrderDetail, decimal Qty)
        {
            _stockOrderDetailDao.UpdateStockDetailQty(stockOrderDetail, Qty);
        }

        /// <summary>
        /// 获取物料需求明细
        /// </summary>
        /// <param name="billId">备料单Id</param>
        /// <returns>物料需求明细</returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailList(double billId)
        {
            return _stockOrderDetailDao.GetStockOrderDetailList(billId);
        }

        /// <summary>
        /// 获取物料需求明细
        /// </summary>
        /// <param name="dtlIds">明细Id</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetails(List<double> dtlIds)
        {
            return _stockOrderDetailDao.GetStockOrderDetails(dtlIds);
        }

        /// <summary>
        /// 获取备料单明细
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="lineNos">行号</param>
        /// <returns>明细</returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetails(string billNo, List<string> lineNos)
        {
            return _stockOrderDetailDao.GetStockOrderDetails(billNo, lineNos);
        }

        /// <summary>
        /// 获取已备料数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>已备料数量</returns>
        public virtual decimal GetStockDetailStockQty(double workOrderId, double itemId)
        {
            return _stockOrderDetailDao.GetStockDetailStockQty(workOrderId, itemId);
        }

        /// <summary>
        /// 修改需求明细状态
        /// </summary>
        public virtual void UpdateStockDetailState(StockOrderDetail stockOrderDetail)
        {
            _stockOrderDetailDao.UpdateStockDetailState(stockOrderDetail);
        }

        /// <summary>
        /// 备料计划强制完成更新取消数
        /// </summary>
        public virtual void UpdateDtlCancelQtyByPlans(string OrderNo, string OrderLineNo, decimal cancelQty)
        {
            _stockOrderDetailDao.UpdateDtlCancelQtyByPlans(OrderNo, OrderLineNo, cancelQty);
        }

        /// <summary>
        /// 备料计划强制完成更新明细状态
        /// </summary>
        /// <param name="stockOrderDetail"></param>
        public virtual void UpdateDtlCancelStateByPlans(StockOrderDetail stockOrderDetail)
        {
            _stockOrderDetailDao.UpdateDtlCancelStateByPlans(stockOrderDetail);
        }

        /// <summary>
        /// 备料计划创建发运单更新备料明细为拣配中
        /// </summary>
        /// <param name="OrderNo">备料单号</param>
        /// <param name="OrderLineNo">备料明细行号</param>  
        /// <param name="stockState">状态</param>
        public virtual void UpdateDtlStateByPlans(string OrderNo, string OrderLineNo, StockState stockState)
        {
            _stockOrderDetailDao.UpdateDtlStateByPlans(OrderNo, OrderLineNo, stockState);
        }

        /// <summary>
        /// 和并下发的备料计划创建发运单更新备料明细为拣配中
        /// </summary>
        /// <param name="OrderNo">备料单号</param>
        /// <param name="stockState">状态</param>
        public virtual List<string> UpdateDtlStateByMergeNo(string OrderNo, StockState stockState)
        {
            //拆分合并单号
            var Nos = new List<string>();
            string[] stockOrder = OrderNo.Split(';');
            stockOrder.ForEach(p =>
            {
                string[] order = p.Split(':');
                string[] detail = order[1].Split(',');
                detail.ForEach(p =>
                {
                    _stockOrderDetailDao.UpdateDtlStateByPlans(order[0], p, stockState);
                });
                Nos.Add(order[0]);
            });
            return Nos;
        }

        /// <summary>
        /// 计算本次备料量
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        private EntityList<StockOrderDetail> GetSameWoDetails(double woId)
        {
            // 找出同工单且备料明细状态不为待提交、已撤回、已关闭的数据
            return _stockOrderDetailDao.GetSameWoDetails(woId);
        }

        /// <summary>
        /// 批量添加备料单明细
        /// </summary>
        /// <param name="itemList">物料列表</param>
        /// <param name="resourceId">资源</param>
        /// <param name="woId">工单</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> MultiAddDetail(EntityList<StockOrderItemViewModel> itemList, double? resourceId, double? woId)
        {
            EntityList<StockOrderDetail> stockOrderDetails = new EntityList<StockOrderDetail>();
            // 产线线边仓
            LinesideWarehouse lineWare = null;
            // 收发控制
            EntityList<BaseItemIoLimit> itemIoLimits = null;
            if (resourceId != null)
            {
                lineWare = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(resourceId.Value);
            }
            // 本次备料量
            EntityList<StockOrderDetail> oldStockOrderDetails = new EntityList<StockOrderDetail>();
            // 是否限制最高存量
            var isLimit = false;
            // 物料ids
            var itemIds = itemList.Select(p => p.Id).ToList();
            if (woId != null)
            {
                oldStockOrderDetails = GetSameWoDetails(woId.Value);
                isLimit = RT.Service.Resolve<StockOrderService>().GetLimitedMaximumStock(); // 推式限制最高库存
            }
            if (lineWare != null)
            {
                itemIoLimits = RT.Service.Resolve<BaseItemExtController>().GetBaseItemIoLimit(new List<double> { lineWare.WarehouseId }, itemIds);
            }
            // 是否启动手动接收
            var receiveType = RT.Service.Resolve<StockOrderService>().GetReceiveType();
            
            foreach (var item in itemList)
            {
                StockOrderDetail stockOrderDetail = new StockOrderDetail
                {
                    IsManualRec = receiveType == StockReceiveType.Hand,
                    ItemId = item.Id,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    ConsumeMode = item.ConsumeMode,
                    ItemExtProp = item.ItemExtProp,
                    ItemExtPropName = item.ItemExtPropName,
                    WoTotalQty = item.WorkOrderQty,
                    IsEnableItemExtProp = item.IsEnableItemExtProp,
                };
                if (lineWare != null)
                {
                    stockOrderDetail.WarehouseId = lineWare.WarehouseId;
                    stockOrderDetail.WareName = lineWare.WarehouseName;
                    stockOrderDetail.StorageLocationId = lineWare.StorageLocationId;
                    stockOrderDetail.StorName = lineWare.LocaltionName;
                }
                if (woId != null)
                {
                    decimal hasQty = oldStockOrderDetails.Where(x => x.ItemId == stockOrderDetail.ItemId && x.ItemExtProp == stockOrderDetail.ItemExtProp).Sum(x => x.Qty - x.CancelQty);
                    stockOrderDetail.Qty = stockOrderDetail.WoTotalQty - hasQty >= 0 ? stockOrderDetail.WoTotalQty - hasQty : 0;
                    if (isLimit && itemIoLimits != null)
                    {
                        var itemIo = itemIoLimits.FirstOrDefault(p => p.ItemId == item.Id);
                        decimal? limitQty = itemIo != null ? itemIo.MaxStockQty : 0;
                        stockOrderDetail.Qty = stockOrderDetail.Qty <= limitQty.Value ? stockOrderDetail.Qty : limitQty.Value;
                    }
                }
                stockOrderDetails.Add(stockOrderDetail);
            }
            return stockOrderDetails;
        }

        /// <summary>
        /// 批量添加备料单明细
        /// </summary>
        /// <param name="resourceId">资源</param>
        /// <param name="woId">工单</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> MultiAddPushDetail(double? resourceId, double? woId)
        {
            EntityList<StockOrderDetail> stockOrderDetails = new EntityList<StockOrderDetail>();
            // 产线线边仓
            LinesideWarehouse lineWare = null;
            // 收发控制
            EntityList<BaseItemIoLimit> itemIoLimits = null;
            if (resourceId != null)
            {
                lineWare = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(resourceId.Value);
            }
            // 本次备料量
            EntityList<StockOrderDetail> oldStockOrderDetails = new EntityList<StockOrderDetail>();
            // 是否限制最高存量
            var isLimit = false;
            
            if (woId != null)
            {
                oldStockOrderDetails = GetSameWoDetails(woId.Value);
                isLimit = RT.Service.Resolve<StockOrderService>().GetLimitedMaximumStock(); // 推式限制最高库存
            }
            // 是否启动手动接收
            var receiveType = RT.Service.Resolve<StockOrderService>().GetReceiveType();
            var itemList = RT.Service.Resolve<StockOrderService>().GetStockOrderItemViewModels(new StockOrderItemViewModelCriteria
            {
                ConsumeMode = ConsumeMode.Push,
                StockType = PrepareItemType.Push,
                WoId = woId,
            });
            // 物料ids
            var itemIds = itemList.Select(p => p.Id).ToList();
            if (lineWare != null)
            {
                itemIoLimits = RT.Service.Resolve<BaseItemExtController>().GetBaseItemIoLimit(new List<double> { lineWare.WarehouseId }, itemIds);
            }
            foreach (var item in itemList)
            {
                StockOrderDetail stockOrderDetail = new StockOrderDetail
                {
                    IsManualRec = receiveType == StockReceiveType.Hand,
                    ItemId = item.Id,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    ConsumeMode = item.ConsumeMode,
                    ItemExtProp = item.ItemExtProp,
                    ItemExtPropName = item.ItemExtPropName,
                    WoTotalQty = item.WorkOrderQty,
                    IsEnableItemExtProp = item.IsEnableItemExtProp,
                };
                if (lineWare != null)
                {
                    stockOrderDetail.WarehouseId = lineWare.WarehouseId;
                    stockOrderDetail.WareName = lineWare.WarehouseName;
                    stockOrderDetail.StorageLocationId = lineWare.StorageLocationId;
                    stockOrderDetail.StorName = lineWare.LocaltionName;
                }
                if (woId != null)
                {
                    decimal hasQty = oldStockOrderDetails.Where(x => x.ItemId == stockOrderDetail.ItemId && x.ItemExtProp == stockOrderDetail.ItemExtProp).Sum(x => x.Qty - x.CancelQty);
                    stockOrderDetail.Qty = stockOrderDetail.WoTotalQty - hasQty >= 0 ? stockOrderDetail.WoTotalQty - hasQty : 0;
                    if (isLimit && itemIoLimits != null)
                    {
                        var itemIo = itemIoLimits.FirstOrDefault(p => p.ItemId == item.Id);
                        decimal? limitQty = itemIo != null ? itemIo.MaxStockQty : 0;
                        stockOrderDetail.Qty = stockOrderDetail.Qty <= limitQty.Value ? stockOrderDetail.Qty : limitQty.Value;
                    }
                }
                stockOrderDetails.Add(stockOrderDetail);
            }
            return stockOrderDetails;
        }
    }
}
