using SIE.Core.Common.Service;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.LES.BulletinBoards.MaterialPulls.APIModels;
using SIE.LES.LinesideWarehouses;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.BulletinBoards.MaterialPulls.Services
{
    /// <summary>
    /// 线边库存水位看板服务类
    /// </summary>
    public class InventoryStageService : DomainService
    {
        /// <summary>
        /// 获取推式拉式水位信息
        /// </summary>
        /// <param name="resourceIds">所选资源</param>
        /// <param name="rate">安全水位预警比例</param>
        /// <returns></returns>
        public virtual List<InventoryStageInfo> GetInventoryStageList(List<double?> resourceIds, decimal? rate)
        {
            if (rate == null || rate == 0)
            {
                rate = 0;
            }
            var inventoryStageList = new List<InventoryStageInfo>();
            // 资源
            var resources = resourceIds.SplitContains(tempIds =>
            {
                return DB.Query<WipResource>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 线边仓
            var lineWares = resourceIds.SplitContains(tempIds =>
            {
                return DB.Query<LinesideWarehouse>().Where(p => tempIds.Contains(p.WipResouceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            var lineWareIds = lineWares.Select(p => (double?)p.WarehouseId).ToList();
            // lpn库存
            var lotLpnOnhandList = lineWareIds.SplitContains(tempIds =>
            {
                return DB.Query<LotLpnOnhand>().Where(p => tempIds.Contains(p.WarehouseId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 拉式
            var pullList = GetPullList(lineWareIds, lotLpnOnhandList, lineWares, rate.Value);
            // 推式
            var pushList = GetPushList(lotLpnOnhandList, resourceIds, resources, lineWares);
            inventoryStageList.AddRange(pullList);
            inventoryStageList.AddRange(pushList);
            inventoryStageList = inventoryStageList.OrderBy(p => p.ResourceName).OrderByDescending(p => p.LowestStage != null ? p.LowestStage - p.NowStage : 0).OrderByDescending(p => p.HightestStage != null ? p.HightestStage - p.NowStage : 0).ToList();
            return inventoryStageList;
        }

        /// <summary>
        /// 拉式
        /// </summary>
        /// <param name="lineWareIds">线边仓Ids</param>
        /// <param name="lotLpnOnhandList">lpn库存</param>
        /// <param name="lineWares">线边仓集合</param>
        /// <param name="rate">安全水位预警比例</param>
        /// <returns></returns>

        private List<InventoryStageInfo> GetPullList(List<double?> lineWareIds, EntityList<LotLpnOnhand> lotLpnOnhandList, EntityList<LinesideWarehouse> lineWares, decimal rate)
        {
            var inventoryStageInfoList = new List<InventoryStageInfo>();
            rate /= 100;
            // 备料模式-拉式
            var prePullList = lineWareIds.SplitContains(tempIds =>
            {
                return DB.Query<PrepareItemPull>().Where(p => tempIds.Contains(p.WarehouseId) && p.TriggerType == Commons.TriggerMode.BelowSafe).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 物料ids
            var prePullItemIds = prePullList.Select(p => p.ItemId).ToList();
            var itemList = prePullItemIds.SplitContains(tempIds =>
            {
                return DB.Query<Items.Item>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 物料单位
            var itemUnitList = new List<ItemUnitInfo>();
            itemList.ForEach(item => {
                ItemUnitInfo itemUnitInfo = new ItemUnitInfo
                {
                    ItemId = item.Id,
                    UnitName = item.UnitName,
                };
                itemUnitList.Add(itemUnitInfo);
            });
            prePullList.ForEach(prePull =>
            {
                var pullWareId = prePull.WarehouseId;
                var pullItemId = prePull.ItemId;
                var pullItemExt = prePull.ItemExtProp;
                var lowSafe = prePull.LowestStage;
                decimal nowStage = 0;
                nowStage = lotLpnOnhandList.Where(p => p.WarehouseId == pullWareId && p.ItemId == pullItemId && p.ItemExtProp == pullItemExt && p.State == OnhandState.Ok).Sum(p => p.Qty);
                if (nowStage < lowSafe * (1 + rate))
                {
                    InventoryStageInfo inventoryStageInfo = new InventoryStageInfo
                    {
                        ResourceName = lineWares.FirstOrDefault(p => p.WarehouseId == pullWareId)?.WipResouceName,
                        ItemName = prePull.ItemName,
                        ItemExtProName = prePull.ItemExtPropName,
                        UnitName = itemUnitList.FirstOrDefault(p => p.ItemId == pullItemId)?.UnitName,
                        LowestStage = prePull.LowestStage,
                        NowStage = nowStage,
                    };
                    inventoryStageInfoList.Add(inventoryStageInfo);
                }
            });
            return inventoryStageInfoList;
        }

        /// <summary>
        /// 推式
        /// </summary>
        /// <param name="lotLpnOnhandList">lpn库存集合</param>
        /// <param name="resourceIds">资源Ids</param>
        /// <param name="wipResources">资源集合</param>
        /// <param name="lineWares">线边仓集合</param>
        /// <returns></returns>
        private List<InventoryStageInfo> GetPushList(EntityList<LotLpnOnhand> lotLpnOnhandList, List<double?> resourceIds, EntityList<WipResource> wipResources, EntityList<LinesideWarehouse> lineWares)
        {
            var inventoryStageInfoList = new List<InventoryStageInfo>();
            // 生产中的在制工单信息
            var woOrderList = RT.Service.Resolve<IWorkOrderQuery>().GetWipWorkOrderIds(resourceIds);
            // 产品ids
            var woProductIds = woOrderList.Select(p => (double?)p.ProductId).ToList();
            // 工单Ids
            var woOrderIds = woOrderList.Select(p => p.WorkOrderId).ToList();

            // 工单bom
            var woOrderBomList = woOrderIds.SplitContains(tempIds =>
            {
                return DB.Query<WorkOrderBom>().Where(p => tempIds.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 工序bom
            var woProBomList = RT.Service.Resolve<IWorkOrderQuery>().GetWoProcessBomInfos(woOrderIds);

            // 备料推式
            var prePushList = resourceIds.SplitContains(tempIds =>
            {
                return DB.Query<PrepareItemPush>().Where(p => tempIds.Contains(p.WipResourceId) && p.TriggerType == Commons.TriggerMode.InvBelowSafeLevelToBeat).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            
            // 备料物料
            var prePushItemIds = prePushList.Select(p => p.ItemId).ToList();
            prePushItemIds.AddRange(woProductIds);
            var prePushItemList = prePushItemIds.SplitContains(tempIds =>
            {
                return DB.Query<Items.Item>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            // 单位
            var itemList = prePushItemIds.SplitContains(tempIds =>
            {
                return DB.Query<Items.Item>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 物料单位
            var itemUnitList = new List<ItemUnitInfo>();
            itemList.ForEach(item => {
                ItemUnitInfo itemUnitInfo = new ItemUnitInfo
                {
                    ItemId = item.Id,
                    UnitName = item.UnitName,
                };
                itemUnitList.Add(itemUnitInfo);
            });

            // 物料追溯规则
            var itemBatchRuleList = prePushItemIds.SplitContains(tempIds =>
            {
                return DB.Query<ItemBatchRule>().Where(p => tempIds.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            // 工单耗用单
            var woCostItemList = RT.Service.Resolve<IWorkOrderQuery>().GetWoOrderCostInfos(woOrderIds);

            // 单体追溯关键件
            var wipKeySingleList = RT.Service.Resolve<IWorkOrderQuery>().GetSingleWipProductKeyItems(woOrderIds);

            // 批次追溯关键件
            var wipKeyBatchList = RT.Service.Resolve<IWorkOrderQuery>().GetBatchWipProductKeyItems(woOrderIds);

            // 产线产能资源工时节拍
            var proModelLineCapList = resourceIds.SplitContains(tempIds =>
            {
                return DB.Query<ProductModelLineCapacity>().Where(p => tempIds.Contains(p.ResourceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            prePushList.ForEach(prePush =>
            {
                var pushItemId = prePush.ItemId;
                var pushItemExt = prePush.ItemExtProp;
                var pushResourceId = prePush.WipResourceId;
                var pushWareId = lineWares.FirstOrDefault(p => p.WipResouceId == pushResourceId)?.WarehouseId;
                // 该物料资源下的工单
                var woOrderWipIds = woOrderList.Where(p => p.ResourceId == pushResourceId).Select(p => p.WorkOrderId).ToList();
                // 最短满足时间
                var pushSatisTime = prePush.SatisfactionTime;
                // 产品机型Id（工单产品）
                var workOrderId = woOrderBomList.FirstOrDefault(p => p.ItemId == pushItemId)?.WorkOrderId;
                var workOrderProductId = woOrderList.Find(p => p.WorkOrderId == workOrderId)?.ProductId;
                var pushProModelId = prePushItemList.FirstOrDefault(p => p.Id == workOrderProductId)?.ModelId;
                // 节拍
                var bat = proModelLineCapList.FirstOrDefault(p => p.ProductModelId == pushProModelId && p.ResourceId == pushResourceId)?.WorkingHours;
                decimal nowStage = 0;
                nowStage = lotLpnOnhandList.Where(p => p.WarehouseId == pushWareId && p.ItemId == pushItemId && p.ItemExtProp == pushItemExt && p.State == OnhandState.Ok).Sum(p => p.Qty);
                if (nowStage < pushSatisTime * bat && pushItemId != 0 && pushItemId != null &&  bat != null)
                {
                    // 物料所有对应工单剩余需求
                    var woNeed = CalculateWoNeed(woOrderWipIds, pushItemId.Value, pushItemExt, woOrderBomList, woProBomList, itemBatchRuleList, woCostItemList, wipKeySingleList, wipKeyBatchList);
                    // 物料对应剩余耗用时间
                    var lastCostTime = CalculateLastCostTime(woOrderWipIds, bat.Value, nowStage, pushItemId.Value, pushItemExt, woOrderBomList, woProBomList, itemBatchRuleList, woCostItemList, wipKeySingleList, wipKeyBatchList);
                    InventoryStageInfo inventoryStageInfo = new InventoryStageInfo
                    {
                        ResourceName = wipResources.FirstOrDefault(p => p.Id == pushResourceId)?.Name,
                        ItemName = prePush.ItemName,
                        ItemExtProName = prePush.ItemExtPropName,
                        UnitName = itemUnitList.Find(p => p.ItemId == pushItemId)?.UnitName,
                        NowStage = nowStage,
                        WoNeed = woNeed,
                        HightestStage = pushSatisTime * bat ?? 0,
                        LastCostTime = lastCostTime,
                    };
                    inventoryStageInfoList.Add(inventoryStageInfo);
                }
            });
            return inventoryStageInfoList;
        }

        /// <summary>
        /// 计算物料所有对应工单剩余需求
        /// </summary>
        /// <param name="woOrderIds">对应资源下的工单Ids</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtPro">物料拓展属性</param>
        /// <param name="woOrderBomList">工单bom</param>
        /// <param name="woProBomList">物料拓展属性</param>
        /// <param name="itemBatchRuleList">物料追溯规则</param>
        /// <param name="woCostItemList">工单耗用单</param>
        /// <param name="wipKeySingleList">单体追溯关键件</param>
        /// <param name="wipKeyBatchList">批次追溯关键件</param>
        /// <returns></returns>
        private decimal CalculateWoNeed(List<double> woOrderIds, double itemId, string itemExtPro, EntityList<WorkOrderBom> woOrderBomList, List<WoProcessBomInfo> woProBomList,
            EntityList<ItemBatchRule> itemBatchRuleList, List<WoOrderCostInfo> woCostItemList, List<WipProductKeyItem> wipKeySingleList, List<WipProductKeyItem> wipKeyBatchList)
        {
            decimal woNeed = 0;
            decimal cost = 0;
            // 物料对应多个工单需求量汇总
            var bomRequireQty = woOrderBomList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtPro && woOrderIds.Contains(p.WorkOrderId)).Sum(p => p.RequireQty);
            // 总耗用量(无工序bom取耗用， 有工序bom取关键件清单之和)
            var itemProBom = woProBomList.Where(p => p.ItemId == itemId && p.ItemExtPro == itemExtPro && woOrderIds.Contains(p.WoOrderId)).ToList();
            if (!itemProBom.Any())
            {
                // 无工序bom取耗用
                cost = woCostItemList.Where(p => p.ItemId == itemId && p.ItemExtPro == itemExtPro && woOrderIds.Contains(p.WoOrderId)).Sum(p => p.Qty);
            }
            else
            {
                var retrospectType = itemBatchRuleList.FirstOrDefault(p => p.ItemId == itemId)?.RetrospectType;
                if (retrospectType == RetrospectType.Single)
                {
                    cost = wipKeySingleList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtPro && woOrderIds.Contains(p.WoOrderId)).Sum(p => p.Qty);
                }
                else
                {
                    cost = wipKeyBatchList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtPro && woOrderIds.Contains(p.WoOrderId)).Sum(p => p.Qty);
                }
            }
            woNeed = bomRequireQty - cost;
            return woNeed > 0 ? woNeed : 0;
        }

        /// <summary>
        /// 计算物料对应剩余耗用时间
        /// </summary>
        /// <param name="woOrderIds">对应资源下的工单Ids</param>
        /// <param name="bat">节拍</param>
        /// <param name="nowStage">现有量</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtPro">物料拓展属性</param>
        /// <param name="woOrderBomList">工单bom</param>
        /// <param name="woProBomList">物料拓展属性</param>
        /// <param name="itemBatchRuleList">物料追溯规则</param>
        /// <param name="woCostItemList">工单耗用单</param>
        /// <param name="wipKeySingleList">单体追溯关键件</param>
        /// <param name="wipKeyBatchList">批次追溯关键件</param>
        /// <returns></returns>
        private decimal CalculateLastCostTime(List<double> woOrderIds, decimal bat, decimal nowStage, double itemId, string itemExtPro, EntityList<WorkOrderBom> woOrderBomList, List<WoProcessBomInfo> woProBomList, EntityList<ItemBatchRule> itemBatchRuleList, List<WoOrderCostInfo> woCostItemList, List<WipProductKeyItem> wipKeySingleList, List<WipProductKeyItem> wipKeyBatchList)
        {
            if (woOrderIds.Any())
            {
                // S=剩余耗用时间
                decimal lastCostTime = 0;
                //C=所有工单每小时BOM用量汇总
                decimal totalBomUse = 0;
                //Z=工单剩余需求，B=工单每小时bom用量,(Z/B)min=剩余耗时最短的工单时间
                List<RequireUseInfo> requireUseInfos = new List<RequireUseInfo>();
                // 现有量X = nowStage
                woOrderIds.ForEach(woOrderId =>
                {
                    var woNeed = CalculateWoNeed(new List<double> { woOrderId }, itemId, itemExtPro, woOrderBomList, woProBomList, itemBatchRuleList, woCostItemList, wipKeySingleList, wipKeyBatchList);
                    var woOrderBom = woOrderBomList.FirstOrDefault(p => p.WorkOrderId == woOrderId && p.ItemId == itemId && p.ItemExtProp == itemExtPro);
                    if (woOrderBom != null)
                    {
                        var bomUseHour = woOrderBom.SingleQty * bat;
                        requireUseInfos.Add(new RequireUseInfo { WoNeed = woNeed, BomUseHour = bomUseHour, Remainder = woNeed / bomUseHour });
                        totalBomUse += bomUseHour;
                    }
                });
                // Z升序排序
                requireUseInfos = requireUseInfos.OrderBy(p => p.WoNeed).ToList();
                // 最小下标
                int index = 0;
                // Z最小值
                var minRemainder = requireUseInfos[index];
                // 判断X与Y、(Z)min大小
                bool whileFlag = true;
                while (whileFlag)
                {
                    // Y = 剩余耗时最短的工单时间对应所有工单的用量
                    var remainderTotalUse = minRemainder.Remainder * totalBomUse;
                    // X>=Y时，X=X-Y，S=S+(Z/B)min，C = C - B，(Z / B)min更新，循环；
                    if (nowStage >= remainderTotalUse && totalBomUse != 0)
                    {
                        nowStage -= remainderTotalUse;
                        lastCostTime += minRemainder.Remainder;
                        totalBomUse -= minRemainder.BomUseHour;
                        index++;
                        if (index < requireUseInfos.Count)
                        {
                            minRemainder = requireUseInfos[index];
                        }
                    }
                    // (Z)min<=X<Y时，X=X-(Z)min，S=S+(Z/B)min，C=C-B，(Z/B)min更新，循环；
                    else if (minRemainder.WoNeed <= nowStage && totalBomUse != 0)
                    {
                        nowStage -= minRemainder.WoNeed;
                        lastCostTime += minRemainder.Remainder;
                        totalBomUse -= minRemainder.BomUseHour;
                        index++;
                        if (index < requireUseInfos.Count)
                        {
                            minRemainder = requireUseInfos[index];
                        }
                    }
                    // 0<X<(Z)min时，X=X-X，S=S+(X/B)min,结束；
                    else if (nowStage > 0 && nowStage < minRemainder.WoNeed && totalBomUse != 0)
                    {
                        lastCostTime += (nowStage / minRemainder.BomUseHour);
                        nowStage = 0;
                        whileFlag = false;
                    }
                    // X=0 || C=0时，S=S，结束；
                    else
                    {
                        whileFlag = false;
                    }
                }
                return lastCostTime;
            }
            else
            {
                return 0;
            }
        }

    }
}
