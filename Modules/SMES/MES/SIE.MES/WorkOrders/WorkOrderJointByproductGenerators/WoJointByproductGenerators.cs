using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.MES.Interfaces.ApsTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WorkOrders.WorkOrderJointByproductGenerators
{
    /// <summary>
    /// 工单联副产品生成
    /// </summary>
    public class WoJointByproductGenerators
    {  /// <summary>
       /// 物料数据拥有者
       /// </summary>
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_releasePlanDatas"></param>
        /// <param name="_itemDataOwner"></param>
        public WoJointByproductGenerators(IReadOnlyList<ReleasePlanData> _releasePlanDatas, ItemDataOwner _itemDataOwner)
        {
            if (_itemDataOwner == null)
            {
                throw new ArgumentNullException(nameof(_itemDataOwner));
            }

            itemDataOwner = _itemDataOwner;
            IEnumerable<ReleasePlanDetail> releasePlanDetails = _releasePlanDatas.SelectMany(x => x.Details);
            var jointByProductsIds = releasePlanDetails
                .SelectMany(x => x.JointByProducts).Where(m=>m.ItemId.HasValue).Select(x => x.ItemId.Value).Distinct().ToList();
            var itemCodes = releasePlanDetails
               .SelectMany(x => x.JointByProducts).Select(x => x.ItemCode).Distinct().ToList();
            if (jointByProductsIds.Any())
            {
                itemDataOwner.GetItemsAndCache(jointByProductsIds);
            }
            else { //没使用ItemId 则尝试用Code找
                itemDataOwner.GetItemsAndCacheByCode(itemCodes);
            }
        }

        /// <summary>
        /// 创建工单联副产品
        /// </summary>
        /// <param name="data">ERP工单数据</param> 
        /// <param name="workOrder">工单</param>
        public void GenerateWorkOrderJointByproduct(ReleasePlanDetail data, WorkOrder workOrder)
        {
            if (data == null)
            {
                return;
            }
            int rowNumber = 10;
            foreach (var group in data.JointByProducts.GroupBy(p => p.ItemId))
            {
                var itemId = group.Key;

                if (itemId.HasValue)
                {
                    if (!this.itemDataOwner.ExistsItem(itemId.Value))
                    {
                        throw new EntityNotFoundException(typeof(Item), itemId);
                    }
                }

                var outputProducts = group.ToList();
                var outputProduct = outputProducts.FirstOrDefault();
                if (outputProduct == null)
                {
                    continue;
                }
                var workOrderOutputProduct = new WorkOrderOutputProduct()
                {
                    ItemId = itemId.Value,
                    WorkOrder = workOrder,
                    Qty = outputProduct.Qty,
                    OutputListType = outputProduct.OutPutType == 0 ? OutputListType.ByProducts : OutputListType.JointProducts,
                    RowNumber = rowNumber.ToString(),
                    ItemExtProp = outputProduct.ItemExtProp,
                    ItemExtPropName = outputProduct.ItemExtPropName,
                };
                rowNumber += 10;

                ////生成工单联副产品列表
                workOrder.WorkOrderOutputProductList.Add(workOrderOutputProduct);
            }
        }

    }
}
