using SIE;
using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.MES.Interfaces.ApsTasks;
using System.Collections.Generic;
using System.Linq;
using System;
using SIE.Domain.Validation;

namespace SIE.MES.WorkOrders.WorkOrderBomGenerators
{
    /// <summary>
    /// 工单BOM逻辑（使用接口传过来的数据）
    /// </summary>
    public class WorkBomUseSourceDataGenerator
    {
        /// <summary>
        /// 物料数据拥有者
        /// </summary>
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_releasePlanDatas"></param>
        /// <param name="_itemDataOwner"></param>
        public WorkBomUseSourceDataGenerator(IReadOnlyList<ReleasePlanData> _releasePlanDatas, ItemDataOwner _itemDataOwner)
        {
            if (_itemDataOwner == null)
            {
                throw new ArgumentNullException(nameof(_itemDataOwner));
            }

            itemDataOwner = _itemDataOwner;
            IEnumerable<ReleasePlanDetail> releasePlanDetails = _releasePlanDatas.SelectMany(x => x.Details);
            var bomItemIds = releasePlanDetails
                .SelectMany(x => x.BomDetails).Select(x => x.ItemId).Distinct().ToList();

            itemDataOwner.GetItemsAndCache(bomItemIds);

            var mainItemIds = releasePlanDetails
               .SelectMany(x => x.BomDetails)
               .Where(x => x.MainItemId.HasValue).Select(x => x.MainItemId.Value).Distinct().ToList();
            itemDataOwner.GetItemsAndCache(mainItemIds);
        }

        /// <summary>
        /// 创建工单BOM
        /// </summary>
        /// <param name="data">ERP工单数据</param> 
        /// <param name="workOrder">工单</param>
        public void GenerateWorkOrderBom(ReleasePlanDetail data, WorkOrder workOrder)
        {
            if (data == null)
            {
                return;
            }

            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            foreach (var group in data.BomDetails.GroupBy(p => p.ItemId))
            {
                var itemId = group.Key;

                if (!this.itemDataOwner.ExistsItem(itemId))
                {
                    throw new EntityNotFoundException(typeof(Item), itemId);
                }

                var boms = group.ToList();
                decimal singleQty = boms.Sum(p => p.SingleQty);
                var bom = boms.FirstOrDefault();
                if (bom == null)
                {
                    continue;
                }

                var item = itemDataOwner.GetItem(bom.ItemId);
                if (item == null)
                {
                    throw new ValidationException("物料【{0}】信息找不到".L10nFormat(bom.ItemId));
                }

                var workOrderBom = new WorkOrderBom()
                {
                    ItemId = itemId,
                    WorkOrder = workOrder,
                    SingleQty = singleQty,
                    RequireQty = bom.RequireQty,
                    IsRecoilItem = bom.IsRecoilItem,
                    IsVritualItem = bom.IsVritualItem,
                    IsByBill = bom.IsByBill,
                    MainMaterialId = bom.MainItemId,
                    Remark = bom.Remark,
                    AlterGroup = bom.CombinationGroup,
                    ItemExtProp = bom.ItemExtProp,
                    ItemExtPropName = bom.ItemExtPropName,
                };

                if (bom.MainItemId.HasValue)
                {
                    
                    if (bom.MainItemId.Value== bom.ItemId)
                    {
                        workOrderBom.Alter = item.Code;
                    }
                    else
                    {
                        var mainItem = itemDataOwner.GetItem(bom.MainItemId.Value);
                        if (mainItem == null)
                        {
                            throw new ValidationException("物料【{0}】信息找不到".L10nFormat(bom.MainItemId));
                        }

                        //APS传过来是主料ID，转换为替代组，
                        //替代组为主料的物料编码
                        workOrderBom.Alter = mainItem.Code;
                        //默认非替代料，当物料与主料物料不同时，说明是替代料
                        workOrderBom.IsAlternative = true;
                    }
                }

                ////生产工单BOM属性值列表
                workOrderBom.ItemExtProp = bom.ItemExtProp;
                workOrderBom.ItemExtPropName = bom.ItemExtPropName;
                AppRuntime.Service.Resolve<TaskReleaseExtensionController>().WorkOrderBomExtensionAssign(workOrderBom, bom);
                workOrder.BomList.Add(workOrderBom);
            }
        }

    }
}
