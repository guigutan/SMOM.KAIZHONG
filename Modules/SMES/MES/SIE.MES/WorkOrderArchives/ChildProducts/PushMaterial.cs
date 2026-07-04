using SIE.Core.Items;
using SIE.Domain;
using SIE.LES.Interfaces;
using SIE.MES.BatchWIP.Products;
using SIE.MES.LoadItems;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrderArchives.Bases;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using System;
using System.Linq;

namespace SIE.MES.WorkOrderArchives.ChildProducts
{
    /// <summary>
    /// 推式物料
    /// </summary>
    public class PushMaterial : MaterialAbsBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PushMaterial(CalculateParameterInfo calculateParameterInfo)
        {
            Init(calculateParameterInfo);
            PrepareData();
        }

        #region 属性
        /// <summary>
        /// 物料标签投入工单情况
        /// </summary>
        protected EntityList<ItemLabelWorkOrder> ItemLabelWoList { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 计算满足套数
        /// </summary>
        /// <param name="availableQty">可用量</param>
        /// <param name="feedQty">已上料</param>
        /// <param name="singleQty">工单bom单位耗用量</param>
        public decimal CalculateSetQty(decimal availableQty, decimal feedQty, decimal singleQty)
        {
            return Math.Floor((availableQty + feedQty) / singleQty);
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
            var totalNeedQty = CalculateTotalNeedQty(bom.SingleQty);
            var hasCostQty = CalculateHasCostQty(CalculateParameterInfo.WorkOrder.Id, CalculateParameterInfo.WorkOrder.ProductId, itemId, itemExtprop);
            var residueNeedQty = CalculateResidueNeedQty(totalNeedQty, hasCostQty);
            var setQty = CalculateSetQty(availableQty, feedQty, bom.SingleQty);
            var shortQty = CalculateShortQty(residueNeedQty, availableQty, feedQty);
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
                TotalNeedQty = totalNeedQty,
                HasCostQty = hasCostQty,
                ResidueNeedQty = residueNeedQty,
                SetQty = setQty,
            };
            return workOrderArchiveItemShortViewModel;
        }
        #endregion

        #region 方法(接口实现)

        /// <summary>
        /// 计算已上料量
        /// </summary>
        /// <param name="itemId">物料id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateFeedQty(double itemId, string itemExtProp)
        {
            return CalculateParameterInfo.LoadItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.WorkOrderId == CalculateParameterInfo.WorkOrder.Id).Sum(p => p.Qty);

        }

        /// <summary>
        /// 计算已耗用量
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="productId">工单产品Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateHasCostQty(double woId, double productId, double itemId, string itemExtProp)
        {
            var retrospectType = GetRetrospectType(CalculateParameterInfo.WorkOrder.ProductId);
            var deductItems = WoCostList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).ToList();
            decimal countOne = deductItems.Sum(p => p.Qty);
            decimal countTwo = 0;

            if (retrospectType == RetrospectType.Single)
            {
                countTwo = SingleKeyItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).Sum(p => p.Qty);
            }
            else
            {
                countTwo = BatchKeyItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).Sum(p => p.Qty);
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
            return StockOrderDetails.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.WorkOrderId == CalculateParameterInfo.WorkOrder.Id).Sum(p => p.Qty - p.CancelQty);

        }

        /// <summary>
        /// 计算备料待接收数量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateToTakeQty(double itemId, string itemExtProp)
        {
            return StockOrderDetails.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp && p.WorkOrderId == CalculateParameterInfo.WorkOrder.Id).Sum(p => p.Qty - p.CancelQty - p.ReceiveQty);

        }

        /// <summary>
        /// 计算可用量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected override decimal CalculateUseAvailableQty(double itemId, string itemExtProp)
        {
            // 物料标签
            var labelIds = CalculateParameterInfo.ItemLabelList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).ToList().Select(p => p.Id).ToList();
            return ItemLabelWoList.Where(p => labelIds.Contains(p.ItemLabelId) && p.WorkOrderId == CalculateParameterInfo.WorkOrder.Id).ToList().Sum(p => p.Qty);
        }

        /// <summary>
        /// 获取计算数据
        /// </summary>
        protected override void PrepareData()
        {
            // 获取产品的追溯方式
            ItemBatchRules = DB.Query<ItemBatchRule>().Where(p => p.ItemId == CalculateParameterInfo.WorkOrder.ProductId).ToList().ToList();

            // 获取物料标签投入工单情况
            ItemLabelWoList = RT.Service.Resolve<ItemLabelController>().GetItemLabelWorkOrders(CalculateParameterInfo.ItemLabelList.Select(p => p.Id).ToList());

            // 获取接收仓库物料需求明细
            StockOrderDetails = RT.Service.Resolve<StockOrderController>().GetStockOrderDetailsWithWoId(CalculateParameterInfo.BomPushItemIds, CalculateParameterInfo.WorkOrder.Id);

            // 根据工单Ids获取单体关键件清单
            SingleKeyItemList = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItemByWoId(CalculateParameterInfo.WorkOrder.Id);


            // 根据工单Ids获取批次关键件清单
            BatchKeyItemList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipKeyItemByWoId(CalculateParameterInfo.WorkOrder.Id);

            // 根据工单ids获取耗用单基础数据
            WoCostList = RT.Service.Resolve<WoCostItemController>().GetBaseWoCostItemByWoId(CalculateParameterInfo.WorkOrder.Id);
        }
        #endregion

    }
}
