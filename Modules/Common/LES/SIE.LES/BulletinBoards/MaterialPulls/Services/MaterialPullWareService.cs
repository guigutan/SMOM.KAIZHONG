using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.EventMessages.Shipment;
using SIE.LES.BulletinBoards.MaterialPulls.APIModels;
using SIE.LES.StockOrders;
using SIE.LES.StockPlans;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.BulletinBoards.MaterialPulls.Services
{
    /// <summary>
    /// 物料拉动(仓库)服务
    /// </summary>
    public class MaterialPullWareService : DomainService
    {
        /// <summary>
        /// 获取物料拉通(仓库)看板基本数据
        /// </summary>
        /// <param name="workShopIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public virtual List<MaterialPullWareInfo> GetMaterialPullWareInfos(List<double?> workShopIds, List<double?> resourceIds)
        {
            var materialPullWareList = new List<MaterialPullWareInfo>();
            // 获取状态不为完成和取消的数据
            var stockPlanList = DB.Query<StockPlan>().Where(p => workShopIds.Contains(p.EnterpriseId) && resourceIds.Contains(p.ResourceId) && p.State != ShipPlan.DeliveryState.Finished && p.State != ShipPlan.DeliveryState.Cancel).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            // 获取对应的备料单物料需求明细（有合并情况需要拆分）
            var stockNoList = new List<string>();
            foreach (var stockPlan in stockPlanList)
            {
                //是否组合
                if (!stockPlan.IsMergeIssued)
                {
                    stockNoList.Add(stockPlan.OrderNo);
                }
                else
                {
                    var stockNoGroupList = stockPlan.OrderNo.Split(';'); // 组合拆分
                    stockNoGroupList.ForEach(x =>
                    {
                        stockNoList.Add(x.Split(':')[0]);
                    });
                }
            }
            stockNoList = stockNoList.Distinct().ToList();
            // 根据备料单号获取备料单
            var stockList = stockNoList.SplitContains(tempNos =>
            {
                return DB.Query<StockOrder>().Where(p => tempNos.Contains(p.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            var stockIds = stockList.Select(p => p.Id).ToList();
            // 备料单物料需求明细
            var stockDetailList = stockIds.SplitContains(tempIds =>
            {
                return DB.Query<StockOrderDetail>().Where(p => tempIds.Contains(p.StockOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 备料计划单号集合
            var stockPlanNoList = stockPlanList.Select(p => p.No).ToList();
            // 发运单明细信息
            var shippingList = RT.Service.Resolve<IMaterialPull>().GetPickingQtySum(stockPlanNoList);
            stockPlanList.ForEach(stock =>
            {
                // 取备料单物料需求
                var earliestDemandTime = GetEarlyDemandTime(stock, stockList, stockDetailList);
                var pickQtySum = shippingList.Find(p => p.OrderNo == stock.No && p.LineNo == stock.LineNo.ToString())?.Qty;
                MaterialPullWareInfo materialPullWareInfo = new MaterialPullWareInfo
                {
                    No = stock.No,
                    ResourceId = stock.ResourceId,
                    ResourceName = stock.ResourceName,
                    ItemName = stock.ItemName,
                    DeliveryQty = stock.DeliveryQty,
                    RequireQty = stock.RequireQty,
                    PickingQty = pickQtySum??0,
                    Residue = stock.RequireQty - stock.DeliveryQty,
                    NoCreateQty = stock.NoCreateQty,
                    UnitName = stock.ItemUnitName,
                    DemandTime = earliestDemandTime,
                };
                materialPullWareList.Add(materialPullWareInfo);
            });
            // 需求时间升序 单号升序
            var materialPullWareOrderList = materialPullWareList.OrderBy(p => p.DemandTime).ThenBy(p => p.No).ToList();
            materialPullWareList = materialPullWareOrderList;
            return materialPullWareList;
        }

        /// <summary>
        /// 取备料单明细最早的需求时间
        /// </summary>
        /// <param name="stockPlan">备料计划</param>
        /// <param name="stockOrders">备料单</param>
        /// <param name="stockOrderDetails">备料单物料需求</param>
        /// <returns></returns>
        private DateTime GetEarlyDemandTime(StockPlan stockPlan, EntityList<StockOrder> stockOrders, EntityList<StockOrderDetail> stockOrderDetails)
        {
            var stockNoList = new List<string>();
            if (!stockPlan.IsMergeIssued)
            {
                stockNoList.Add(stockPlan.OrderNo);
            }
            else
            {
                var stockNoGroupList = stockPlan.OrderNo.Split(';'); // 组合拆分
                stockNoGroupList.ForEach(x =>
                {
                    stockNoList.Add(x.Split(':')[0]);
                });
            }
            var stockOrderList = stockOrders.Where(p => stockNoList.Contains(p.No)).ToList();
            var stockOrderIds = stockOrderList.Select(x => x.Id).ToList();
            var srockDetailList = stockOrderDetails.Where(p => stockOrderIds.Contains(p.StockOrderId)).ToList();
            var minNeedTime = srockDetailList.Min(p => p.DemandTime);
            return minNeedTime;
        }
    }
}
