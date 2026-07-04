using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.WorkOrders.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders.WorkOrderBomGenerators
{
    /// <summary>
    /// 工单BOM 创建逻辑(使用产品BOM)
    /// </summary>
    public class UseProductBomGenerator
    {
        /// <summary>
        /// 产品BOM 列表
        /// </summary>
        private readonly EntityList<ProductBom> productBoms;

        /// <summary>
        /// 产品 BOM 明细列表
        /// </summary>
        private readonly EntityList<ProductBomDetail> productBomDetails;

        /// <summary>
        /// 产品BOM替代料列表
        /// </summary>
        private readonly EntityList<ProductBomDetailAlternative> bomDetailAlternatives;
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 工单BOM 创建逻
        /// </summary>
        /// <param name="productIds">产品ID列表</param>
        /// <param name="_itemDataOwner"></param>
        public UseProductBomGenerator(List<double> productIds, ItemDataOwner _itemDataOwner)
        {
            //产品默认BOM
            productBoms = AppRuntime.Service.Resolve<ItemController>().GetDefaultProductBoms(productIds);

            var productBomIds = productBoms.Select(x => x.Id).Distinct().ToList();
            productBomDetails = AppRuntime.Service.Resolve<ItemController>().GetProductBomDetails(productBomIds);

            var detailIds = productBomDetails.Select(x => x.Id).Distinct().ToList();
            bomDetailAlternatives = RT.Service.Resolve<ProductBomController>()
                .GetProductBomDetailAlternativeList(detailIds);

            if (_itemDataOwner == null)
            {
                throw new ArgumentNullException(nameof(_itemDataOwner));
            }

            itemDataOwner = _itemDataOwner;
            var bomItemIds = productBomDetails
                .Select(x => x.ItemId).Distinct().ToList();

            itemDataOwner.GetItemsAndCache(bomItemIds);

            var altItemIds = bomDetailAlternatives
               .Select(x => x.ItemId).Distinct().ToList();
            itemDataOwner.GetItemsAndCache(altItemIds);
        }

        /// <summary>
        /// 生成工单的工单BOM
        /// </summary>        
        /// <param name="workOrder">工单</param>
        public void GenerateWorkOrderBom(WorkOrder workOrder)
        {
            ProductBom productBom = null;
            if (workOrder.ProductId != 0)
            {
                productBom = productBoms.FirstOrDefault(x => x.ProductId == workOrder.ProductId && x.ItemExtProp == workOrder.ItemExtProp && x.ProjectMaintainId == workOrder.ProjectMaintainId);
            }

            GeneratePropertyValues(productBom, workOrder);

            GenerateWorkOrderBoms(productBom, workOrder);
        }

        /// <summary>
        /// 生成工单属性值清单
        /// </summary>
        /// <param name="productBom">产品BOM</param>
        /// <param name="workOrder">工单</param>
        void GeneratePropertyValues(ProductBom productBom, WorkOrder workOrder)
        {
            if (productBom == null || workOrder == null)
            {
                return;
            }

            workOrder.ItemExtProp = productBom.ItemExtProp;
            workOrder.ItemExtPropName = productBom.ItemExtPropName;
        }

        /// <summary>
        /// 生成工单bom清单
        /// </summary>
        /// <param name="productBom">产品BOM</param>
        /// <param name="workOrder">工单</param>
        private void GenerateWorkOrderBoms(ProductBom productBom, WorkOrder workOrder)
        {
            if (productBom == null || workOrder == null)
            {
                return;
            }

            workOrder.BomList.Clear();

            //贪婪加载明细的数据
            var detailList = productBomDetails.Where(x => x.ProductBomId == productBom.Id);
            // 行号
            var lineNo = 1;
            foreach (var detail in detailList)
            {
                var item = itemDataOwner.GetItem(detail.ItemId);
                if (item == null)
                {
                    throw new ValidationException("物料【{0}】信息找不到".L10nFormat(detail.ItemId));
                }

                var bom = new WorkOrderBom
                {
                    LineNo = (lineNo++).ToString(),
                    ItemId = detail.ItemId,
                    WorkOrder = workOrder,
                    RequireQty = detail.UnitQty * workOrder.PlanQty,
                    SingleQty = detail.UnitQty,
                    IsRecoilItem = detail.IsRecoilItem.HasValue && detail.IsRecoilItem.Value,
                    IsVritualItem = detail.ItemIsVirtualPart,
                    IsByBill = false,
                    Remark = detail.Remark,
                    IsAllowEdit = detail.EnableExtendProperty
                };

                AppRuntime.Service.Resolve<ErpWorkOrderPropertyChanged>().SetWorkOrderBomExtProperty(bom, detail);

                bom.SetProperty(WorkOrderBom.ItemNameProperty, detail.ItemName);

                //生产工单BOM属性值列表
                bom.ItemExtProp = detail.ItemExtProp;
                bom.ItemExtPropName = detail.ItemExtPropName;

                workOrder.BomList.Add(bom);

                if (bomDetailAlternatives.Any(x => x.BomDetailId == detail.Id))
                {
                    //生成替代料列表
                    foreach (var alternativel in bomDetailAlternatives.Where(x => x.BomDetailId == detail.Id))
                    {
                        var altBom = new WorkOrderBom
                        {
                            ItemId = alternativel.ItemId,
                            WorkOrder = workOrder,
                            RequireQty = detail.UnitQty * workOrder.PlanQty,
                            SingleQty = detail.UnitQty,
                            IsRecoilItem = detail.IsRecoilItem.HasValue && detail.IsRecoilItem.Value,
                            IsVritualItem = detail.ItemIsVirtualPart,
                            IsByBill = false,
                            Remark = detail.Remark,
                        };

                        //生产工单BOM属性值列表
                        altBom.ItemExtProp = detail.ItemExtProp;
                        altBom.ItemExtPropName = detail.ItemExtPropName;

                        altBom.Alter = item.Code;
                        altBom.IsAlternative = true;

                        //主料的替代组也设置为主料编码
                        bom.Alter = item.Code;

                        workOrder.BomList.Add(altBom);
                    }
                }

            }
        }
    }
}
