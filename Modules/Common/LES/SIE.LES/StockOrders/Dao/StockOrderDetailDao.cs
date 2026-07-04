using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.LES.StockPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.StockOrders.Dao
{
    public class StockOrderDetailDao : BaseDao<StockOrderDetail>
    {
        /// <summary>
        /// 通过备料计划单号获取备料需求明细
        /// </summary>
        /// <param name="StockNo">备料单号</param>
        /// <param name="ItemId">物料ID</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailByStockNo(string StockNo, double ItemId)
        {
            var query = Query();
            query.LeftJoin<StockPlan>((x, y) => x.StockOrder.No == y.No && y.No == StockNo && x.ItemId == ItemId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更新物料需求明细
        /// </summary>
        /// <param name="stockOrderDetail">物料需求明细</param>
        /// <param name="Qty">发运数量</param>
        public virtual void UpdateStockDetailQty(StockOrderDetail stockOrderDetail, decimal Qty)
        {
            stockOrderDetail.ShipQty = stockOrderDetail.ShipQty + Qty;
            if (!stockOrderDetail.IsManualRec)
            {
                stockOrderDetail.ReceiveQty = stockOrderDetail.ReceiveQty + Qty;
            }
            //RF.Save(stockOrderDetail);
        }

        /// <summary>
        /// 获取物料需求明细
        /// </summary>
        /// <param name="billId">备料单Id</param>
        /// <returns>物料需求明细</returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailList(double billId)
        {
            return Query().Where(p => p.StockOrderId == billId).ToList();
        }

        /// <summary>
        /// 获取需求明细
        /// </summary>
        /// <param name="ids">需求明细ID集合</param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetails(List<double> ids)
        {
            return Query().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取需求明细物料为未接收的
        /// </summary>
        /// <returns>需求明细列表</returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetails(List<double> itemIds, PrepareItemType prepareItemType)
        {
            return itemIds.SplitContains(tempIds =>
            {
                return Query().Join<StockOrder>((x, y) => x.StockOrderId == y.Id
                        && y.StockType == prepareItemType)
                    .Where(x => tempIds.Contains(x.ItemId)
                        && (x.StockState == StockState.Audit
                            || x.StockState == StockState.Submitted
                            || x.StockState == StockState.PickStocking
                            || x.StockState == StockState.Issued
                            || x.StockState == StockState.TobeReceive))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取备料单明细
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="lineNos">行号</param>
        /// <returns>明细</returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetails(string billNo, List<string> lineNos)
        {
            return Query().Where(f => f.StockOrder.No == billNo && lineNos.Contains(f.LineNo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取需求明细物料为未接收的
        /// </summary>
        /// <returns>需求明细列表</returns>
        public virtual EntityList<StockOrderDetail> GetPushStockOrderDetails(List<double> woIds)
        {
            return woIds.Cast<double?>().SplitContains(tempWoIds =>
            {
                return Query()
                    .Where(x => tempWoIds.Contains(x.StockOrder.WorkOrderId)
                        && x.Item.ConsumeMode == Items.ConsumeMode.Push)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取已备料数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>已备料数量</returns>
        public virtual decimal GetStockDetailStockQty(double workOrderId, double itemId)
        {
            decimal stockQty = 0;
            var query = Query().Where(p => p.ItemId == itemId);
            query.Exists<StockOrder>((x, y) => y.Where(p => p.Id == x.StockOrderId && p.WorkOrderId == workOrderId && p.StockState != StockState.Created && p.StockState != StockState.ReCall && p.StockState != StockState.Closed));
            stockQty = query.ToList().Sum(p => p.Qty - p.CancelQty);
            return stockQty;
        }

        /// <summary>
        /// 通过备料计划相关单号查找备料明细增加取消数
        /// </summary>
        /// <param name="OrderNo">相关单号</param>
        /// <param name="OrderLineNo">相关订单行号</param>
        /// <param name="CancelQty">取消数量</param>
        public virtual void UpdateDtlCancelQtyByPlans(string OrderNo, string OrderLineNo, decimal CancelQty)
        {
            var stockDetail = Query().Where(p => p.StockOrder.No == OrderNo && p.LineNo == OrderLineNo).ToList(null, null).FirstOrDefault();
            stockDetail.CancelQty = stockDetail.CancelQty + CancelQty;
            UpdateDtlCancelStateByPlans(stockDetail);
            RF.Save(stockDetail);

        }

        /// <summary>
        /// 备料计划强制完成
        /// </summary>
        /// <param name="stockOrderDetail"></param>
        public virtual void UpdateDtlCancelStateByPlans(StockOrderDetail stockOrderDetail)
        {
            if (stockOrderDetail == null)
            {
                return;
            }
            if (stockOrderDetail.UnFinishQty == stockOrderDetail.ReceiveQty && stockOrderDetail.ReceiveQty == 0)
            {
                stockOrderDetail.StockState  = StockState.Closed;
            }
            else if(stockOrderDetail.UnFinishQty != 0)
            {
                if (stockOrderDetail.ReceiveQty < stockOrderDetail.ShipQty || (stockOrderDetail.ReceiveQty == stockOrderDetail.ShipQty && stockOrderDetail.ShipQty > 0))
                {
                    stockOrderDetail.StockState = StockState.TobeReceive;
                }
                else if (stockOrderDetail.ReceiveQty == stockOrderDetail.ShipQty && stockOrderDetail.ShipQty == 0)
                {
                    stockOrderDetail.StockState = StockState.PickStocking;
                }
            }
            else if (stockOrderDetail.UnFinishQty == 0 && stockOrderDetail.ReceiveQty != 0)
            {
                stockOrderDetail.StockState = StockState.Received;
            }
        }

        /// <summary>
        /// 更新状态物料需求明细状态
        /// </summary>
        /// <param name="stockOrderDetail">物料需求明细</param>
        public virtual void UpdateStockDetailState(StockOrderDetail stockOrderDetail)
        {
            if (stockOrderDetail.Qty != stockOrderDetail.CancelQty &&
                stockOrderDetail.Qty - stockOrderDetail.CancelQty > stockOrderDetail.Qty
                && stockOrderDetail.ShipQty == stockOrderDetail.ReceiveQty
                )
            {
                stockOrderDetail.StockState = StockState.PickStocking;
            }
            if (stockOrderDetail.Qty != stockOrderDetail.CancelQty && stockOrderDetail.ShipQty > stockOrderDetail.ReceiveQty)
            {
                stockOrderDetail.StockState = StockState.TobeReceive;
            }
            //if (stockOrderDetail.Qty != stockOrderDetail.CancelQty && stockOrderDetail.ReceiveQty > 0 && stockOrderDetail.ReceiveQty != stockOrderDetail.Qty - stockOrderDetail.CancelQty)
            //{
            //    stockOrderDetail.StockState = StockState.PartReceived;
            //}

            if (stockOrderDetail.Qty != stockOrderDetail.CancelQty && stockOrderDetail.Qty - stockOrderDetail.CancelQty <= stockOrderDetail.ShipQty//超发 ShipQty 会>  Qty-CancelQty
            && stockOrderDetail.ShipQty == stockOrderDetail.ReceiveQty)
            {
                stockOrderDetail.StockState = StockState.Received;
            }
            if (stockOrderDetail.CancelQty > 0 && stockOrderDetail.Qty == stockOrderDetail.CancelQty)
            {
                stockOrderDetail.StockState = StockState.Closed;
            }
        }

        /// <summary>
        /// 根据单号和行号更新备料明细状态
        /// </summary>
        /// <param name="orderNo">备料单号</param>
        /// <param name="orderLineNo">备料明细行号</param>
        /// <param name="stockState">明细状态</param>
        public virtual void UpdateDtlStateByPlans(string orderNo, string orderLineNo, StockState stockState)
        {
            var data = Query().Join<StockOrder>((x, y) => x.StockOrderId == y.Id && y.No == orderNo).Where(p => p.LineNo == orderLineNo).FirstOrDefault();
            if (data != null)
            {
                data.StockState = stockState;
                RF.Save(data);
            }
        }

        /// <summary>
        /// 找出同工单且备料明细状态不为待提交、已撤回、已关闭的数据
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetSameWoDetails(double woId)
        {
            return DB.Query<StockOrderDetail>().Join<StockOrder>((x, y) => x.StockOrderId == y.Id && y.WorkOrderId == woId)
                .Where(x => x.StockState != StockState.Created
                    && x.StockState != StockState.ReCall
                    && x.StockState != StockState.Closed).ToList();
        }
    }
}
