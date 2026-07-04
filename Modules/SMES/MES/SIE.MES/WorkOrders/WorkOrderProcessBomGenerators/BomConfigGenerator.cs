using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.KzItemCategorys;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.ItemProcess;
using SIE.MES.MtartProcessLookups;
using SIE.MES.ProcessProperty;
using SIE.MES.Routings.RoutingBoms;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders._Routing_;
using SIE.MES.WorkOrders.Configs;
using SIE.MES.WorkOrders.Events;
using SIE.MES.WorkOrders.Routings;
using SIE.Rbac.InvOrgs;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.Technologys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static IronPython.Modules._ast;

namespace SIE.MES.WorkOrders.WorkOrderProcessBomGenerators
{
    /// <summary>
    /// 工单工序BOM生成（使用工艺路线的工序BOM配置）
    /// </summary>
    public class BomConfigGenerator : IWoProcessBomGenerator
    {
        /// <summary>
        /// 物料数据拥有者
        /// </summary>
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 工艺路线BOM设置
        /// </summary>
        public EntityList<RoutingProcessBomConfig> routingProcessBomConfigs { get; set; }

        /// <summary>
        /// 是否参考工序BOM
        /// </summary>
        private readonly bool referenceWoBom;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_itemDataOwner">物料数据拥有者</param>
        /// <param name="routingProcessBomConfigs"></param>
        public BomConfigGenerator(ItemDataOwner _itemDataOwner, EntityList<RoutingProcessBomConfig> routingProcessBomConfigs)
        {
            itemDataOwner = _itemDataOwner;

            //工序 BOM 管理 是否考虑工单BOM 配置项
            referenceWoBom = true;
            var referenceWoBomConfigValue = ConfigService.GetConfig(new ReferenceWoBomConfig(), typeof(WorkOrder));
            if (referenceWoBomConfigValue != null)
            {
                referenceWoBom = referenceWoBomConfigValue.ReferenceWoBom;
            }
            this.routingProcessBomConfigs = routingProcessBomConfigs;
        }

        /// <summary>
        /// 生成工序Bom（取工艺路线工序BOM配置 与 工单BOM 的交集）
        /// </summary>
        /// <param name="workOrder">工单</param>        
        public void GenerateProcessBoms(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            workOrder.ProcessBomList.Clear();

            if (workOrder.VersionId == null)
            {
                return;
            }

            var itemIds = workOrder.BomList.Where(p => p.IsVritualItem == false).Select(p => p.ItemId).Distinct().ToList();
            //这边只用到库存类别
            var itemCategoryRelations = RT.Service.Resolve<ItemController>().GetItemCategoryRelationByCategoryTypes(itemIds, Items.Items.CategoryType.Item);
            //工序清单是根据工艺路线生成的
            var processIds = workOrder.RoutingProcessList.Where(p=>p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

            var itemCategoryIds = itemCategoryRelations.Where(p => p.ItemCategoryId != null).Select(p => p.ItemCategoryId.Value).Distinct().ToList();
            //根据分类和工序获取物料分类与工序关系对照表和物料
            var lookUps = RT.Service.Resolve<MtartProcessLookupsController>().GetMtartProcessLookupsByItemCategoryId(itemCategoryIds, processIds);

            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            //EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds);

            //当找得到物料得时候，优先找到物料的,找不到就直接查全部，然后再找工序的
            //var pps = processPtys.Where(f => f.CategoryItemCode == workOrder.Product.Code).ToList();
            //if (pps.Count == 0)
            //    pps = processPtys.ToList();
            var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(workOrder.ProductId);

            //根据物料ID获取单位耗用量向上取整配置表
            EntityList<SingleQtyRoundUp> singleQtyRoundUps = RT.Service.Resolve<WorkOrderController>().GetSingleQtyRoundUpsByItemIds(itemIds);

            //获取物料分类与工序关系对照表配置项
            var config = ConfigService.GetConfig(new SIE.MES.MtartProcessLookups.Configs.MtartProcessLookupConfig(), typeof(MtartProcessLookup));

            int seq = 1;
            foreach (var itemId in itemIds)
            {
                //当单机定额合计小于等于0 的时候，不生成工序BOM，因为工序BOM的单机定额必须大于等于0
                var boms = workOrder.BomList.Where(p => p.ItemId == itemId && p.IsVritualItem == false).ToList();

                //当没有对应分类的时候，就不生成工序BOM
                var itemCategoryRelation = itemCategoryRelations.FirstOrDefault(p => p.ItemId == itemId);
                if (itemCategoryRelation == null)
                    continue;

                var processCodes = new List<string>();
                if (workOrder.Type != WorkOrderType.Rework)
                {

                    var lus = new List<MtartProcessLookup>();
                    //优先找到有工艺属性分类的
                    if (kzItemCategory != null)
                    {
                        lus = lookUps.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId && p.ItemCategoryId == itemCategoryRelation.ItemCategoryId).ToList();
                    }
                    //如果找不到，那么就找那些工艺属性分类为空的数据
                    if (lus.Count == 0)
                        lus = lookUps.Where(p => p.ItemCategoryId == itemCategoryRelation.ItemCategoryId && p.KzCategoryId == null).ToList();

                    //当在物料分类与工序对照表中，没有维护数据，不生成工序BOM
                    if (lus.Count < 1)
                        continue;
                    processCodes = lus.Select(p => p.ProcessCode).Distinct().ToList();
                }
                else if (workOrder.DAUAT == "KZ04" || (workOrder.DAUAT == "KZ05" && itemId == workOrder.ProductId))
                {
                    //当工单类型为返工的时候，只有首工序才会生成工序BOM
                    //工单生成工序BOM时，如果工单类型为“KZ05”（在制返工），则只将主料（工单BOM中与工单产品编码一致的即为主料）加入工序BOM，如果工单类型为“KZ03”（库存返工），则不生成工序BOM。
                    var layout = workOrder.LayoutInfoList.Where(p => p.Factory == invOrg.ExternalId).OrderByDescending(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
                    if (layout != null)
                        processCodes.Add(layout.ProcessCode);
                }

                if (processCodes.Count == 0)
                    continue;
                //只获取当前工厂+工序
                //非返工类型,当工序对应的控制码是PP04 ，生成工序BOM的时候，不参与生成工序BOM
                var layoutInfo = workOrder.LayoutInfoList.Where(p => p.Factory == invOrg.ExternalId && processCodes.Contains(p.ProcessCode) && ((workOrder.Type != WorkOrderType.Rework && p.Steus != "PP04") || workOrder.Type == WorkOrderType.Rework)).OrderBy(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
                if (layoutInfo == null)
                    continue;

                var bom = boms.FirstOrDefault();
                var item = bom.Item;
                var routingProcess = workOrder.RoutingProcessList.FirstOrDefault(p => p.Process.Code == layoutInfo.ProcessCode);

                //校验配置的工序是否是当前工单整个工艺路线（不管工序是不是排程点，派工点）的首工序，如果是则生成BOM，不是则不生成工序BOM
                if (!config.GenrateStartProcessBom.IsNullOrEmpty() && workOrder.Type != WorkOrderType.Rework && config.GenrateStartProcessBom.Split(",").Any(p => p == routingProcess.Process.Code && routingProcess.Index != workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault().Index))
                {
                    continue;
                }

                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { routingProcess.ProcessId.Value }, workOrder.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                var processPty = pps.FirstOrDefault();

                var singleQty = boms.Sum(p => p.SingleQty);
                
                var singleQtyRoundUp = singleQtyRoundUps.FirstOrDefault(p => p.ItemId == itemId);
                //在配置表里的向上取整
                if (singleQtyRoundUp != null)
                {
                    singleQty = Math.Ceiling(singleQty);
                    if (singleQty == 0)
                        continue;
                }
                else
                {
                    //此处只有当工序属性维护中的按工序BOM生成派工单数量勾上的时候，单机定额就不按照这个计算，后面会在重新计算，所以不参与判断
                    if (singleQty <= 0 && (processPty == null || processPty.IsPbcd == null || processPty.IsPbcd == false))
                        continue;
                }

                var workOrderProcessBom = new WorkOrderProcessBom()
                {
                    UnitId = item.UnitId,
                    SingleQty = singleQty,
                    Weight = singleQty,
                    ProcessId = routingProcess.ProcessId,
                    Seq = seq++,
                    ItemId = bom.ItemId,
                    //此时未生成工单工序清单的ID
                    RoutingProcess = routingProcess,
                    //此时未生成工单的ID
                    WorkOrder = workOrder,
                    Alter = bom.Alter,
                    AlterGroup = bom.AlterGroup,
                    IsAlternative = bom.IsAlternative,
                    Werks = bom.Werks,
                    Meins = bom.Meins
                };

                var bomItem = itemDataOwner.GetItem(itemId);
                if (bomItem != null)
                {
                    workOrderProcessBom.ExtValues["ItemId_Display"] = bomItem.Code;
                    workOrderProcessBom.ItemName = bomItem.Name;
                    workOrderProcessBom.ItemSpecificationModel = bomItem.SpecificationModel;
                }

                workOrderProcessBom.ItemExtProp = bom.ItemExtProp;
                workOrderProcessBom.ItemExtPropName = bom.ItemExtPropName;
                EntityList<ItemExtBom> validItems = new EntityList<ItemExtBom>();

                validItems.AddRange(routingProcess.BomConfigList.Select(m => new ItemExtBom
                {
                    ItemId = m.ItemId,
                    ItemExtProp = m.ItemExtProp,
                    ItemExtPropName = m.ItemExtPropName
                }));

                if (!bom.IsAlternative && referenceWoBom)
                {
                    seq = AddAlternativeItem(workOrder, bom, routingProcess, seq, validItems);
                }

                //工序BOM前面会清空，记录数不会太多，所以不用担心取太多资料过来
                workOrder.ProcessBomList.Add(workOrderProcessBom);
                validItems.Clear();
            }
            //此处重新计算一下工序BOM单机定额
            foreach (var processBom in workOrder.ProcessBomList)
            {
                var singleQtyRoundUp = singleQtyRoundUps.FirstOrDefault(p => p.ItemId == processBom.ItemId);
                //在配置表里的向上取整的时候，就跳过原逻辑，因为上面已经向上取整了，不需要再跑这一步
                if (singleQtyRoundUp != null)
                {
                    continue;
                }

                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { processBom.ProcessId.Value }, workOrder.ProductId);
                //var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(workOrder.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                var processPty = pps.FirstOrDefault();
                //当工序勾选了“按工序BOM生成派工单数量”，工单工序BOM单机用量=该物料的需求量/该工序BOM需求量汇总。
                if (processPty != null && processPty.IsPbcd == true)
                {
                    //获取这个工序的所有工序BOM对应的物料
                    var biids = workOrder.ProcessBomList.Where(p => p.ProcessId == processBom.ProcessId).Select(p => p.ItemId).Distinct().ToList();
                    //查询该工序的总需求量
                    var totalRequireQty = workOrder.BomList.Where(p => biids.Contains(p.ItemId) && p.IsVritualItem == false).Sum(p => p.RequireQty);
                    //查询该工序、该物料的总需求量(工单BOM可能存在相同物料情况)
                    var requireQty = workOrder.BomList.Where(p => processBom.ItemId == p.ItemId && p.IsVritualItem == false).Sum(p => p.RequireQty);
                    if (requireQty == 0)
                    {
                        throw new ValidationException("工单BOM物料[{0}]总需求量不能为0，无法计算工序BOM单机定额".L10nFormat(processBom.Item.Code));
                    }
                    processBom.SingleQty = Math.Truncate((requireQty / totalRequireQty) * 100000000) / 100000000;
                    processBom.Weight = processBom.SingleQty;
                }
                //当组件物料类型=KZ02或KZ03(半成品类型)，分类不为110201，且单位为PCS时，且物料名称包含铜排两个字时，单位耗用量不取整，，如果物料名称不包含铜排两个字时则单位耗用量取整，
                var itemCategoryRelation = itemCategoryRelations.FirstOrDefault(p => p.ItemId == processBom.ItemId);
                if (processBom.Item.Type == ItemType.SemiFinished && itemCategoryRelation.ItemCategoryCode != "110201" && (processBom.Item.Unit.Code == "PCS" || processBom.Item.Unit.Code == "pcs") && !processBom.Item.Name.Contains("铜排"))
                {
                    processBom.SingleQty = Math.Truncate(processBom.SingleQty);
                    processBom.Weight = processBom.SingleQty;
                }
            }

            #region 旧逻辑

            //foreach (var routingProcess in workOrder.RoutingProcessList)
            //{
            //    if (!routingProcess.BomConfigList.Any())
            //    {
            //        var bomConfigs = routingProcessBomConfigs.Where(x => x.RoutingProcessId == routingProcess.RoutingProcessId).ToList();
            //        GenerateBomConfigs(bomConfigs, routingProcess);
            //    }
            //    int seq = 1;
            //    EntityList<ItemExtBom> validItems = new EntityList<ItemExtBom>();

            //    validItems.AddRange(routingProcess.BomConfigList.Select(m => new ItemExtBom
            //    {
            //        ItemId = m.ItemId,
            //        ItemExtProp = m.ItemExtProp,
            //        ItemExtPropName = m.ItemExtPropName
            //    }));

            //    var boms = workOrder.BomList
            //        .Where(p => validItems.Any(i => i.ItemId == p.Item.Id && i.ItemExtProp == p.ItemExtProp));

            //    //根据工序Id获取物料分类与工序关系对照表
            //    var itemIds = workOrder.BomList.Select(p => p.ItemId).Distinct().ToList();

            //    var ids = RT.Service.Resolve<MtartProcessLookupsController>().GetItemsByProcessIds(new List<double>() { routingProcess.ProcessId.Value }, itemIds);

            //    foreach (var id in ids)
            //    {
            //        var bom = workOrder.BomList.FirstOrDefault(p => p.ItemId == id);
            //        if (bom == null || bom.SingleQty <= 0) 
            //            continue;
            //        var item = bom.Item;
            //        var workOrderProcessBom = new WorkOrderProcessBom()
            //        {
            //            UnitId = item.UnitId,
            //            SingleQty = bom.SingleQty,
            //            ProcessId = routingProcess.ProcessId,
            //            Seq = seq++,
            //            ItemId = bom.ItemId,
            //            //此时未生成工单工序清单的ID
            //            RoutingProcess = routingProcess,
            //            //此时未生成工单的ID
            //            WorkOrder = workOrder,
            //            Alter = bom.Alter,
            //            AlterGroup = bom.AlterGroup,
            //            IsAlternative = bom.IsAlternative,
            //            Werks = bom.Werks,
            //            Meins = bom.Meins
            //        };

            //        //设置工单工序BOM扩展属性
            //        RT.Service.Resolve<ErpWorkOrderPropertyChanged>()
            //            .SetWorkOrderProcessBomExtProperty(workOrderProcessBom, bom);

            //        var bomItem = itemDataOwner.GetItem(bom.ItemId);
            //        if (bomItem != null)
            //        {
            //            workOrderProcessBom.ExtValues["ItemId_Display"] = bomItem.Code;
            //            workOrderProcessBom.ItemName = bomItem.Name;
            //            workOrderProcessBom.ItemSpecificationModel = bomItem.SpecificationModel;
            //        }

            //        workOrderProcessBom.ItemExtProp = bom.ItemExtProp;
            //        workOrderProcessBom.ItemExtPropName = bom.ItemExtPropName;

            //        if (!bom.IsAlternative && referenceWoBom)
            //        {
            //            seq = AddAlternativeItem(workOrder, bom, routingProcess, seq, validItems);
            //        }

            //        //工序BOM前面会清空，记录数不会太多，所以不用担心取太多资料过来
            //        workOrder.ProcessBomList.Add(workOrderProcessBom);
            //    }

            //    #region 旧逻辑 2025-8-25

            //    //EntityList<ProcessItem> processItems = RT.Service.Resolve<ProcessItemController>().GetProcessItemsByProcessId(new List<double>() { routingProcess.ProcessId.Value });


            //    //var itemIds = processItems.Select(p => p.ItemId).Distinct().ToList();
            //    //获取物料
            //    //var items = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
            //    //获取库存类别
            //    //var itemCategoryRelations = RT.Service.Resolve<ItemController>().GetQualityCategory(itemIds, Items.Items.CategoryType.Item);


            //    //foreach (var processItem in processItems)
            //    //{
            //    //    //物料
            //    //    var item = items.FirstOrDefault(p => p.Id == processItem.ItemId);
            //    //    //物料与分类关系
            //    //    var itemCategoryRelation = itemCategoryRelations.FirstOrDefault(p => p.ItemId == processItem.ItemId);

            //    //    var bom = workOrder.BomList.FirstOrDefault(p => p.ItemId == processItem.ItemId);
            //    //    //当这个物料在BOM中不存在的时候，就跳过，防止在功能界面上维护错或者乱维护
            //    //    if (bom == null || bom.SingleQty <= 0)
            //    //        continue;
            //    //    /*
            //    //    当关系表中只维护了工序+物料类型+MRP控制者，那么工单BOM物料的的物料类型+MRP控制者要同时满足关系表维护的数据才能生成工序BOM；
            //    //    当关系维护了工序 + 物料类型 + MRP控制者 + 物料组，那么工单BOM物料的的物料类型 + MRP控制者 + 物料组要同时满足关系表维护的数据才能生成工序BOM；
            //    //    */
            //    //    var mtartProcessLookup = mtartProcessLookups.FirstOrDefault(p => p.ProcessId == processItem.ProcessId && item.Mtart == p.Mtart && item.MrpController == p.Dispo);
            //    //    if (mtartProcessLookup == null || (mtartProcessLookup.ItemCategoryId != null && mtartProcessLookup.ItemCategoryId != itemCategoryRelation.ItemCategoryId))
            //    //        continue;                    

            //    //    //var item = itemDataOwner.GetItem(processItem.ItemId);
            //    //    //if (item == null)
            //    //    //{
            //    //    //    throw new ValidationException("物料【{0}】信息找不到".L10nFormat(processItem.Item.Code));
            //    //    //}

            //    //    //var item = bom.Item;
            //    //    var workOrderProcessBom = new WorkOrderProcessBom()
            //    //    {
            //    //        UnitId = item.UnitId,
            //    //        SingleQty = bom.SingleQty,
            //    //        ProcessId = routingProcess.ProcessId,
            //    //        Seq = seq++,
            //    //        ItemId = bom.ItemId,
            //    //        //此时未生成工单工序清单的ID
            //    //        RoutingProcess = routingProcess,
            //    //        //此时未生成工单的ID
            //    //        WorkOrder = workOrder,
            //    //        Alter = bom.Alter,
            //    //        AlterGroup = bom.AlterGroup,
            //    //        IsAlternative = bom.IsAlternative,
            //    //        Werks = bom.Werks,
            //    //        Meins = bom.Meins
            //    //    };

            //    //    //设置工单工序BOM扩展属性
            //    //    RT.Service.Resolve<ErpWorkOrderPropertyChanged>()
            //    //        .SetWorkOrderProcessBomExtProperty(workOrderProcessBom, bom);

            //    //    var bomItem = itemDataOwner.GetItem(bom.ItemId);
            //    //    if (bomItem != null)
            //    //    {
            //    //        workOrderProcessBom.ExtValues["ItemId_Display"] = bomItem.Code;
            //    //        workOrderProcessBom.ItemName = bomItem.Name;
            //    //        workOrderProcessBom.ItemSpecificationModel = bomItem.SpecificationModel;
            //    //    }

            //    //    workOrderProcessBom.ItemExtProp = bom.ItemExtProp;
            //    //    workOrderProcessBom.ItemExtPropName = bom.ItemExtPropName;

            //    //    if (!bom.IsAlternative && referenceWoBom)
            //    //    {
            //    //        seq = AddAlternativeItem(workOrder, bom, routingProcess, seq, validItems);
            //    //    }

            //    //    //工序BOM前面会清空，记录数不会太多，所以不用担心取太多资料过来
            //    //    workOrder.ProcessBomList.Add(workOrderProcessBom);
            //    //}

            //    #endregion

            //    #region 旧逻辑

            //    //foreach (var bom in boms)
            //    //{
            //    //    var item = itemDataOwner.GetItem(bom.ItemId);
            //    //    if (item == null)
            //    //    {
            //    //        throw new ValidationException("物料【{0}】信息找不到".L10nFormat(bom.ItemId));
            //    //    }

            //    //    var workOrderProcessBom = new WorkOrderProcessBom()
            //    //    {
            //    //        UnitId = item.UnitId,
            //    //        SingleQty = bom.SingleQty,
            //    //        ProcessId = routingProcess.ProcessId,
            //    //        Seq = seq++,
            //    //        ItemId = bom.ItemId,
            //    //        //此时未生成工单工序清单的ID
            //    //        RoutingProcess = routingProcess,
            //    //        //此时未生成工单的ID
            //    //        WorkOrder = workOrder,
            //    //        Alter = bom.Alter,
            //    //        AlterGroup = bom.AlterGroup,
            //    //        IsAlternative = bom.IsAlternative,
            //    //    };

            //    //    //设置工单工序BOM扩展属性
            //    //    RT.Service.Resolve<ErpWorkOrderPropertyChanged>()
            //    //        .SetWorkOrderProcessBomExtProperty(workOrderProcessBom, bom);

            //    //    var bomItem = itemDataOwner.GetItem(bom.ItemId);
            //    //    if (bomItem != null)
            //    //    {
            //    //        workOrderProcessBom.ExtValues["ItemId_Display"] = bomItem.Code;
            //    //        workOrderProcessBom.ItemName = bomItem.Name;
            //    //        workOrderProcessBom.ItemSpecificationModel = bomItem.SpecificationModel;
            //    //    }

            //    //    workOrderProcessBom.ItemExtProp = bom.ItemExtProp;
            //    //    workOrderProcessBom.ItemExtPropName = bom.ItemExtPropName;

            //    //    if (!bom.IsAlternative && referenceWoBom)
            //    //    {
            //    //        seq = AddAlternativeItem(workOrder, bom, routingProcess, seq, validItems);
            //    //    }

            //    //    //工序BOM前面会清空，记录数不会太多，所以不用担心取太多资料过来
            //    //    workOrder.ProcessBomList.Add(workOrderProcessBom);
            //    //}
            //    #endregion

            //    validItems.Clear();
            //}
            #endregion
        }

        /// <summary>
        /// 处理替代料
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="workOrderBom">工单BOM</param>
        /// <param name="workOrderRoutingProcess">工单工序</param>
        /// <param name="seq">序号</param>
        /// <param name="validItems">工艺路线配置的工序物料清单</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private int AddAlternativeItem(WorkOrder workOrder, WorkOrderBom workOrderBom,
            WorkOrderRoutingProcess workOrderRoutingProcess, int seq, EntityList<ItemExtBom> validItems)
        {
            //工单BOM的替代料
            var alternativeList = workOrder.BomList
                .Where(x => x.Alter == workOrderBom.Alter && x.IsAlternative);

            foreach (var woBom in alternativeList)
            {
                //已经配置这个料，这里就不加进替代料里了
                if (validItems.Any(x => x.ItemId == woBom.ItemId && x.ItemExtProp == woBom.ItemExtProp))
                {
                    continue;
                }

                var item = itemDataOwner.GetItem(woBom.ItemId);

                if (item == null)
                {
                    throw new ValidationException("物料【{0}】找不到".L10nFormat(woBom.ItemId));
                }

                var alternativeProcessBom = new WorkOrderProcessBom
                {
                    UnitId = item.UnitId,
                    ItemUnitName = woBom.UnitName,
                    //用量相同
                    SingleQty = workOrderBom.SingleQty,
                    Weight = workOrderBom.SingleQty,
                    Seq = seq++,
                    ItemId = woBom.ItemId,
                    //此时未生成ID
                    RoutingProcess = workOrderRoutingProcess,
                    //此时未生成ID
                    WorkOrder = workOrder,
                    ProcessId = workOrderRoutingProcess.ProcessId,
                    ItemExtProp = woBom.ItemExtProp,
                    ItemExtPropName = woBom.ItemExtPropName,
                    IsAlternative = true,
                    AlterGroup = woBom.AlterGroup,
                    Alter = woBom.Alter,
                };

                workOrder.ProcessBomList.Add(alternativeProcessBom);
            }

            return seq;
        }

        /// <summary>
        /// 生成工序BOM配置
        /// </summary>
        /// <param name="bomConfigList"></param>
        /// <param name="workOrderRoutingProcess"></param>
        private void GenerateBomConfigs(IList<RoutingProcessBomConfig> bomConfigList,
            WorkOrderRoutingProcess workOrderRoutingProcess)
        {
            //由工艺路线的工序BOM配置 生成 【工单工序清单与BOM关系】
            foreach (var bomConfig in bomConfigList)
            {
                var workOrderRoutingProcessBomConfig = new WorkOrderRoutingProcessBom
                {
                    ItemId = bomConfig.ItemId,
                    Process = workOrderRoutingProcess,
                    ItemExtProp = bomConfig.ItemExtProp,
                    ItemExtPropName = bomConfig.ItemExtPropName
                };

                //改成保存前批量获取Id
                //<code>workOrderRoutingProcessBomConfig.GenerateId();</code>

                workOrderRoutingProcess.BomConfigList.Add(workOrderRoutingProcessBomConfig);
            }
        }

    }
}
