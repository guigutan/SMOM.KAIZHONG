using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.ShipPlan;
using SIE.Warehouses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.MES.AutoJoinLineWarehouse
{
    /// <summary>
    ///半成品自动加入线边仓
    /// </summary>
    public class AutoJoinLineWarehouseController : DomainController
    {

        /// <summary>
        /// 半成品自动加入线边仓
        /// </summary>
        private void AutoJoinLineWarehouseForSingle()
        {
            //单体自动加入线边仓
            using (DataAuth.DataAuths.LoadAll())
            {
                var wipProductVersionList = Query<WipProductVersion>().Where(m => (m.IsJoinLineWarehouse == 0) && m.IsFinish).ToList(null, new EagerLoadOptions().LoadWith(WipProductVersion.WorkOrderProperty));
                var rules = Query<AssignWarehouseRule>().Where(m => m.OrderType == Core.Enums.OrderType.AutoJoinLineWarehouse && m.ItemType == Items.ItemType.SemiFinished).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (rules.Any() && wipProductVersionList.Any())
                {
                    var itemIds = wipProductVersionList.Select(m => m.WorkOrder.ProductId);

                    //取出所有产品的物料-库存分类
                    var itemCategoryRelations = itemIds.SplitContains(ids =>
                    {
                        return Query<ItemCategoryRelation>().Where(m => ids.Contains(m.ItemId) && m.Type == Items.Items.CategoryType.Item).ToList(null, new EagerLoadOptions().LoadWith(ItemCategoryRelation.ItemCategoryProperty));
                    });

                    var hasResource = rules.Where(m => m.ResourceId.HasValue && m.ResourceId > 0).ToList();
                    var noResource = rules.Where(m => !m.ResourceId.HasValue || m.ResourceId <= 0).ToList();

                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        foreach (var wip in wipProductVersionList)
                        {
                            var itemCategoryRelation = itemCategoryRelations.FirstOrDefault(m => m.ItemId == wip.Product.ItemId);
                            if (itemCategoryRelation == null)
                            {
                                throw new ValidationException("请维护【单据类型=自动入库线边仓】+【基本分类=半成品】分配仓库规则的【库存分类】属性!".L10N());
                            }
                            if (hasResource.Any())
                            {
                                //库存分类与物料的库存类别一致
                                var hasResourceRules = hasResource.Where(m => m.ResourceId == wip.WorkOrder.ResourceId && m.ItemCategoryId == itemCategoryRelation.ItemCategoryId).ToList();
                                if (hasResourceRules.Count > 1)//如果资源相同的还是有多条，取优先级最小的
                                {
                                    var hasResourceRule = hasResourceRules.FirstOrDefault(m => m.Priority == hasResourceRules.Min(k => k.Priority));
                                    //hasResourceRule 取到资源匹配 优先级最小
                                    InvokeWMSInterface(wip, hasResourceRule);
                                    wip.IsJoinLineWarehouse = 1;
                                }
                                if (hasResourceRules.Count == 1)
                                {
                                    InvokeWMSInterface(wip, hasResourceRules.First());
                                    wip.IsJoinLineWarehouse = 1;
                                }
                            }
                            if (wip.IsJoinLineWarehouse == 0)//有资源的不存在 找无资源的
                            {
                                var noResourceRules = noResource.Where(m => m.ItemCategoryId == itemCategoryRelation.ItemCategoryId).ToList();
                                if (noResourceRules.Count > 1)
                                {
                                    var noResourceRule = noResourceRules.FirstOrDefault(m => m.Priority == noResourceRules.Min(k => k.Priority));
                                    //noResourceRule 取到资源匹配 优先级最小
                                    InvokeWMSInterface(wip, noResourceRule);
                                    wip.IsJoinLineWarehouse = 1;
                                }
                                else if (noResourceRules.Count == 1)
                                {
                                    InvokeWMSInterface(wip, noResourceRules.First());
                                    wip.IsJoinLineWarehouse = 1;
                                }
                                else
                                {
                                    //todo 未匹配任何数据
                                    wip.IsJoinLineWarehouse = -1;
                                }
                            }
                            RF.Save(wip);
                        }
                        tran.Complete();
                    }
                }
            }
        }


        /// <summary>
        /// 批次半成品自动加入线边仓
        /// </summary>
        private void AutoJoinLineWarehouseForBatch()
        {
            //单体自动加入线边仓

            using (DataAuth.DataAuths.LoadAll())
            {
                var wipProductVersionList = Query<BatchWipProductVersion>().Where(m => (m.IsJoinLineWarehouse == 0 && m.IsFinish)).ToList(null, new EagerLoadOptions().LoadWith(BatchWipProductVersion.WorkOrderProperty));
                var rules = Query<AssignWarehouseRule>().Where(m => m.OrderType == Core.Enums.OrderType.AutoJoinLineWarehouse && m.ItemType == Items.ItemType.SemiFinished).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (rules.Any() && wipProductVersionList.Any())
                {
                    var itemIds = wipProductVersionList.Select(m => m.Product.ItemId).ToList();

                    //取出所有产品的物料-库存分类
                    var itemCategoryRelations = itemIds.SplitContains(ids =>
                    {
                        return Query<ItemCategoryRelation>().Where(m => ids.Contains(m.ItemId) && m.Type == Items.Items.CategoryType.Item).ToList(null, new EagerLoadOptions().LoadWith(ItemCategoryRelation.ItemCategoryProperty));
                    });

                    var hasResource = rules.Where(m => m.ResourceId.HasValue && m.ResourceId > 0).ToList();
                    var noResource = rules.Where(m => !m.ResourceId.HasValue || m.ResourceId <= 0).ToList();
                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        foreach (var wip in wipProductVersionList)
                        {
                            var itemCategoryRelation = itemCategoryRelations.FirstOrDefault(m => m.ItemId == wip.Product.ItemId);
                            if (itemCategoryRelation == null)
                            {
                                throw new ValidationException("请维护【单据类型=自动入库线边仓】+【基本分类=半成品】分配仓库规则的【库存分类】属性!".L10N());
                            }
                            //库存分类与物料的库存类别一致
                            if (hasResource.Any())
                            {
                                var hasResourceRules = hasResource.Where(m => m.ResourceId == wip.CurrentProcess.ResourceId && m.ItemCategoryId == itemCategoryRelation.ItemCategoryId).ToList();
                                if (hasResourceRules.Count > 1)//如果资源相同的还是有多条，取优先级最小的
                                {
                                    var hasResourceRule = hasResourceRules.FirstOrDefault(m => m.Priority == hasResourceRules.Min(k => k.Priority));
                                    InvokeWMSInterfaceBatch(wip, hasResourceRule);
                                    wip.IsJoinLineWarehouse = 1;
                                }
                                if (hasResourceRules.Count == 1)
                                {
                                    InvokeWMSInterfaceBatch(wip, hasResourceRules.First());
                                    wip.IsJoinLineWarehouse = 1;
                                }
                            }
                            if (wip.IsJoinLineWarehouse == 0)//有资源的不存在 找无资源的
                            {
                                var noResourceRules = noResource.Where(m => m.ItemCategoryId == itemCategoryRelation.ItemCategoryId).ToList();
                                if (noResourceRules.Count > 1)
                                {
                                    var noResourceRule = noResourceRules.FirstOrDefault(m => m.Priority == noResourceRules.Min(k => k.Priority));
                                    InvokeWMSInterfaceBatch(wip, noResourceRule);
                                    //noResourceRule 取到资源匹配 优先级最小
                                    wip.IsJoinLineWarehouse = 1;
                                }
                                else if (noResourceRules.Count == 1)
                                {
                                    InvokeWMSInterfaceBatch(wip, noResourceRules.First());
                                    wip.IsJoinLineWarehouse = 1;
                                }
                                else
                                {  //匹配不到
                                   //to do
                                    wip.IsJoinLineWarehouse = -1;
                                }
                            }
                            RF.Save(wip);
                        }
                        tran.Complete();
                    }
                }

            }
        }

        /// <summary>
        /// 批次调用WMS接口
        /// </summary>
        /// <param name="wip"></param>
        /// <param name="hasResourceRule"></param>
        private void InvokeWMSInterfaceBatch(BatchWipProductVersion wip, AssignWarehouseRule hasResourceRule)
        {
            MesUpdateOnhandData mesUpdateOnhandData = new MesUpdateOnhandData();
            mesUpdateOnhandData.EnterpriseId = wip.WorkOrder.WorkShopId;
            mesUpdateOnhandData.WoId = wip.WorkOrderId;
            mesUpdateOnhandData.WoNo = wip.WorkOrderNo;
            mesUpdateOnhandData.OpType = 4;
            mesUpdateOnhandData.WarehouseId = hasResourceRule.WarehouseId;

            var packageRuleDetail = wip.WorkOrder.PackageRuleDetailList.FirstOrDefault();
            if (packageRuleDetail != null)
            {
                mesUpdateOnhandData.ItemPackRuleDetailId = packageRuleDetail.DetailId;
            }
            mesUpdateOnhandData.LabelDatas.Add(new MesLabelData()
            {
                ItemExtProp = wip.WorkOrder.ItemExtProp,
                ItemExtPropName = wip.WorkOrder.ItemExtPropName,
                ProjectNo = wip.WorkOrder.ProjectMaintainCode,
                ItemId = wip.WorkOrder.ProductId,
                LabelNo = wip.BatchNo,
                LotCode = wip.BatchNo,
                Qty = wip.Qty,
                WorkOrderNo = wip.WorkOrderNo,
                WorkOrderId = wip.WorkOrderId,
            });
            RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(mesUpdateOnhandData);
            //更新物料标签库存位置
            var stage = RT.Service.Resolve<WarehouseController>().GetStageStorageLocation(hasResourceRule.WarehouseId);
            var label = Query<ItemLabel>().Where(p => p.Label == wip.BatchNo && p.Lot == wip.BatchNo && p.ItemId == wip.WorkOrder.ProductId && p.ItemExtProp == wip.WorkOrder.ItemExtProp).FirstOrDefault();
            if (label != null)
            {
                label.StorageLocationId = stage.Id;
                label.WarehouseId = hasResourceRule.WarehouseId;
                RF.Save(label);
            }

        }

        /// <summary>
        /// 调用WMS接口
        /// </summary>
        /// <param name="wip"></param>
        /// <param name="hasResourceRule"></param>
        private void InvokeWMSInterface(WipProductVersion wip, AssignWarehouseRule hasResourceRule)
        {
            MesUpdateOnhandData mesUpdateOnhandData = new MesUpdateOnhandData();
            mesUpdateOnhandData.EnterpriseId = wip.WorkOrder.WorkShopId;
            mesUpdateOnhandData.WoId = wip.WorkOrderId;
            mesUpdateOnhandData.WoNo = wip.WorkOrder.No;
            mesUpdateOnhandData.OpType = 4;
            mesUpdateOnhandData.WarehouseId = hasResourceRule.WarehouseId;
            var packageRuleDetail = wip.WorkOrder.PackageRuleDetailList.FirstOrDefault(m => m.IsMasterUnit);
            if (packageRuleDetail != null)
            {
                mesUpdateOnhandData.ItemPackRuleDetailId = packageRuleDetail.DetailId;
            }
            mesUpdateOnhandData.LabelDatas.Add(new MesLabelData()
            {
                ItemExtProp = wip.WorkOrder.ItemExtProp,
                ItemExtPropName = wip.WorkOrder.ItemExtPropName,
                ProjectNo = wip.WorkOrder.ProjectMaintainCode,
                ItemId = wip.WorkOrder.ProductId,
                LabelNo = wip.Sn,
                Qty = 1,
                WorkOrderId = wip.WorkOrderId,
                WorkOrderNo = wip.WorkOrderNo
            });
            RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(mesUpdateOnhandData);
            //更新物料标签库存位置
            var stage = RT.Service.Resolve<WarehouseController>().GetStageStorageLocation(hasResourceRule.WarehouseId);
            var label = Query<ItemLabel>().Where(p => p.Label == wip.Sn && p.ItemId == wip.WorkOrder.ProductId && p.ItemExtProp == wip.WorkOrder.ItemExtProp).FirstOrDefault();
            if (label != null)
            {
                label.StorageLocationId = stage.Id;
                label.WarehouseId = hasResourceRule.WarehouseId;
                RF.Save(label);
            }
        }


        /// <summary>
        /// 分别两条线程执行单体与批次
        /// </summary>
        public virtual void AutoJoinLineWarehouse()
        {
            //AutoJoinLineWarehouseForSingle();
            //AutoJoinLineWarehouseForBatch();

            Task.Factory.StartNew(() => AutoJoinLineWarehouseForSingle());

            Task.Factory.StartNew(() => AutoJoinLineWarehouseForBatch());

        }
    }
}
