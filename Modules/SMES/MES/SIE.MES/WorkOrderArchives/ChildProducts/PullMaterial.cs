using SIE.Core.Items;
using SIE.Domain;
using SIE.LES.Interfaces;
using SIE.LES.LinesideWarehouses;
using SIE.MES.BatchWIP.Products;
using SIE.MES.LoadItems;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrderArchives.Bases;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Models;
using SIE.Packages.ItemLabels.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrderArchives.ChildProducts
{
    /// <summary>
    /// 拉式产品
    /// </summary>
    public class PullMaterial : MaterialAbsBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PullMaterial(CalculateParameterInfo calculateParameterInfo)
        {
            Init(calculateParameterInfo);
            PrepareData();
        }

        #region 属性
        /// <summary>
        /// 物料标签
        /// </summary>
        protected List<ItemLabelBaseData> ItemLabels { get; set; }

        /// <summary>
        /// 同产线工单
        /// </summary>
        protected List<WorkOrderBaseData> SameResourceWo { get; set; }

        /// <summary>
        /// 同产线工单工单bom
        /// </summary>
        protected EntityList<WorkOrderBom> SamWoBomList { get; set; }

        /// <summary>
        /// 同资源下的工单Ids
        /// </summary>
        protected List<double> SameWoIds { get; set; }

        /// <summary>
        /// 同工单下的产品Ids
        /// </summary>
        protected List<double> SameProductIds { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 计算相同线边仓工单剩余需求数量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        public decimal CalculateSameWoNeedQty(double itemId, string itemExtProp)
        {
            decimal sameNeedQty = 0;
            var itemWoBomList = SamWoBomList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).ToList();
            foreach (var p in itemWoBomList)
            {
                var wo = SameResourceWo.FirstOrDefault(q => q.Id == p.WorkOrderId);
                if (wo == null)
                {
                    continue;
                }
                var woNeedQty = p.SingleQty * wo.PlanQty;
                var woCostQty = CalculateHasCostQty(wo.Id, wo.ProductId, p.ItemId, p.ItemExtProp);
                sameNeedQty += woNeedQty - woCostQty;
            }
            return sameNeedQty;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="bom">工单Bom</param>
        /// <returns></returns>
        public WorkOrderArchiveItemShortViewModel ReturnViewModel(WorkOrderBom bom)
        {
            var itemId = bom.ItemId;
            var itemExtprop = bom.ItemExtProp;
            var availableQty = CalculateUseAvailableQty(itemId, itemExtprop);
            var feedQty = CalculateFeedQty(itemId, itemExtprop);
            var stockOrderQty = CalculateStockOrderQty(itemId, itemExtprop);
            var toTakeQty = CalculateToTakeQty(itemId, itemExtprop);
            var sameNeedQty = CalculateSameWoNeedQty(itemId, itemExtprop);
            var totalNeedQty = CalculateTotalNeedQty(bom.SingleQty);
            var hasCostQty = CalculateHasCostQty(CalculateParameterInfo.WorkOrder.Id, CalculateParameterInfo.WorkOrder.ProductId, itemId, itemExtprop);
            var residueNeedQty = CalculateResidueNeedQty(totalNeedQty, hasCostQty);
            var shortQty = CalculateShortQty(sameNeedQty, availableQty, feedQty);
            WorkOrderArchiveItemShortViewModel workOrderArchiveItemShortViewModel = new WorkOrderArchiveItemShortViewModel()
            {
                ShortQty = shortQty,
                ItemCode = bom.ItemCode,
                ItemName = bom.ItemName,
                ItemExtPropName = bom.ItemExtPropName,
                ConsumeModel = bom.ItemConsumeMode,
                SingleQty = bom.SingleQty,
                UnitName = bom.UnitName,
                AvailableQty = availableQty,
                FeedQty = feedQty,
                StockOrderQty = stockOrderQty,
                ToTakeQty = toTakeQty,
                SameNeedQty = sameNeedQty,
                TotalNeedQty = totalNeedQty,
                HasCostQty = hasCostQty,
                ResidueNeedQty = residueNeedQty,
            };
            return workOrderArchiveItemShortViewModel;
        }
        #endregion

        #region 方法(接口实现)

        /// <summary>
        /// 计算已上料量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateFeedQty(double itemId, string itemExtProp)
        {
            return CalculateParameterInfo.LoadItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.ResourceId == CalculateParameterInfo.LineWare.WipResouceId).Sum(p => p.Qty);
        }

        /// <summary>
        /// 计算工单已耗用数量
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="productId">工单产品Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateHasCostQty(double woId, double productId, double itemId, string itemExtProp)
        {
            // 追溯方式
            var retrospectType = GetRetrospectType(productId);
            // 工单耗用
            var woCostList = WoCostList.Where(p => p.WorkOrderId == woId).ToList();
            var singleWipList = SingleKeyItemList.Where(p => p.WoOrderId == woId).ToList();
            var batchWipList = BatchKeyItemList.Where(p => p.WoOrderId == woId).ToList();

            decimal countOne = woCostList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).Sum(p => p.Qty);
            decimal countTwo = 0;

            if (retrospectType == RetrospectType.Single)
            {
                countTwo = singleWipList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).Sum(p => p.Qty);
            }
            else
            {
                countTwo = batchWipList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).Sum(p => p.Qty);
            }
            return countOne + countTwo;
        }

        /// <summary>
        /// 计算已建备料量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateStockOrderQty(double itemId, string itemExtProp)
        {
            return StockOrderDetails.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.WarehouseId == CalculateParameterInfo.LineWare.WarehouseId).Sum(p => p.Qty - p.CancelQty);
        }

        /// <summary>
        /// 计算备料待接收量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateToTakeQty(double itemId, string itemExtProp)
        {
            return StockOrderDetails.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.WarehouseId == CalculateParameterInfo.LineWare.WarehouseId).Sum(p => p.Qty - p.CancelQty - p.ReceiveQty);

        }

        /// <summary>
        /// 计算可用量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateUseAvailableQty(double itemId, string itemExtProp)
        {
            return CalculateParameterInfo.ItemLabelList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.WarehouseId == CalculateParameterInfo.LineWare.WarehouseId).Sum(p => p.Qty);

        }

        /// <summary>
        /// 准备计算数据
        /// </summary>
        protected override void PrepareData()
        {
            // 备料需求明细
            StockOrderDetails = RT.Service.Resolve<StockOrderController>().GetStockOrderDetailsWithWareId(CalculateParameterInfo.BomPullItemIds, CalculateParameterInfo.LineWare.WarehouseId);

            // 获取物料标签
            ItemLabels = CalculateParameterInfo.ItemLabelList.Where(p => p.WarehouseId == CalculateParameterInfo.LineWare.WarehouseId).ToList();

            // 同线边仓资源
            var sameResourceIds = RT.Service.Resolve<LinesideWarehouseController>().GetSameWareResourceIds(CalculateParameterInfo.LineWare);
            // 这些资源下发放生产中的工单
            SameResourceWo = RT.Service.Resolve<WorkOrderController>().GetSameResourceBaseWorkOrders(sameResourceIds);
            SameWoIds = SameResourceWo.Select(p => p.Id).Distinct().ToList();
            SameProductIds = SameResourceWo.Select(p => p.ProductId).Distinct().ToList();
            // 这些资源下发放生产中的工单工单bom
            SamWoBomList = RT.Service.Resolve<WorkOrderController>().GetWorkOrderBomsByWoIds(SameWoIds);

            // 获取产品的追溯方式
            List<ItemBatchRule> itemBatchRules = new List<ItemBatchRule>();
            // 工单产品追溯规则
            SameProductIds.SplitDataExecute(tempIds =>
            {
                var list = DB.Query<ItemBatchRule>().Where(p => tempIds.Contains(p.ItemId)).ToList();
                itemBatchRules.AddRange(list);
            });
            ItemBatchRules = itemBatchRules;

            // 根据工单Ids获取单体关键件清单
            SingleKeyItemList = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItemByWoIds(SameWoIds);


            // 根据工单Ids获取批次关键件清单
            BatchKeyItemList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipKeyItemByWoIds(SameWoIds);

            // 根据工单ids获取耗用单基础数据
            WoCostList = RT.Service.Resolve<WoCostItemController>().GetBaseWoCostItemByWoIds(SameWoIds);
        }
        #endregion

    }
}
