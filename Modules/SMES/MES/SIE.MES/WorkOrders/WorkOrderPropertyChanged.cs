using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Domain;
using SIE.Common.Sort;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Repairs;
using SIE.MES.WorkOrders.Configs;
using SIE.MES.WorkOrders.WorkOrderBomGenerators;
using SIE.MES.WorkOrders.WorkOrderPackageGenerators;
using SIE.MES.WorkOrders.WorkOrderProcessBomGenerators;
using SIE.Packages;
using SIE.Packages.Printables;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单事件变更控制器
    /// </summary>
    [IgnoreProxy]
    public class WorkOrderPropertyChanged : DomainController
    {
        /// <summary>
        /// 工单属性变更事件
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        public virtual void WorkOrderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var workOrder = sender as WorkOrder;
            if (workOrder == null)
            {
                return;
            }

            SetMesWorkOrder(workOrder, e);
            SetErpWorkOrder(workOrder, e);
        }

        /// <summary>
        /// 设置MES工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="e">属性变更参数</param>
        protected virtual void SetMesWorkOrder(WorkOrder workOrder, PropertyChangedEventArgs e)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.PropertyName == WorkOrder.ProductIdProperty.Name)
            {
                ProductChanged(workOrder);
            }

            if (e.PropertyName == WorkOrder.PlanQtyProperty.Name)
            {
                PlanQtyChanged(workOrder);
            }

            if (e.PropertyName == WorkOrder.WorkShopIdProperty.Name)
            {
                WorkShopChanged(workOrder);
            }

            BindingRoutingVersion(workOrder, e);

            if (workOrder.Version != null && e.PropertyName == WorkOrder.VersionIdProperty.Name)
            {
                RoutingVersionChanged(workOrder);
            }

            if (workOrder.ErpWorkOrder != null && e.PropertyName == WorkOrder.ErpWorkOrderIdProperty.Name)
            {
                ErpWorkOrderChanged(workOrder);
            }

            if (e.PropertyName == WorkOrder.TypeProperty.Name)
            {
                WorkOrderTypeChanged(workOrder);
            }
        }

        /// <summary>
        /// 设置ERP工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="e">属性变更参数</param>
        protected virtual void SetErpWorkOrder(WorkOrder workOrder, PropertyChangedEventArgs e)
        {
            if (workOrder.Source == SourceType.External)
            {
                BindingRoutingVersion(workOrder, e);
            }
        }

        /// <summary>
        /// 产品变更
        /// </summary>
        /// <param name="workOrder">工单</param>        
        public virtual void ProductChanged(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                return;
            }

            var productIds = new List<double> { workOrder.ProductId };
            ItemDataOwner itemDataOwner = new ItemDataOwner();
            var workOrderBomGenerator = new UseProductBomGenerator(productIds, itemDataOwner);

            workOrderBomGenerator.GenerateWorkOrderBom(workOrder);

            //工单包装规则生成器
            var woPackageGenerator = new WoPackageGenerator(
                 new EntityList<Item> { workOrder.Product });

            woPackageGenerator.GenerateWorkOrderPackageRule(workOrder);

            woPackageGenerator.GenerateProductLabelTemplate(workOrder);
        }

        /// <summary>
        /// 计划数量变更
        /// </summary>
        /// <param name="workOrder">工单</param>       
        public virtual void PlanQtyChanged(WorkOrder workOrder)
        {
            workOrder.BomList.ForEach(p =>
            {
                p.RequireQty = workOrder.PlanQty * p.SingleQty;
            });
        }

        /// <summary>
        /// 车间变更
        /// </summary>
        /// <param name="workOrder">工单</param>       
        protected virtual void WorkShopChanged(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            workOrder.Resource = null;
        }

        /// <summary>
        /// 工单类型属性变更
        /// </summary>
        /// <param name="workOrder">工单</param>     
        protected virtual void WorkOrderTypeChanged(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.PersistenceStatus == PersistenceStatus.New)
            {
                if (workOrder.Type == WorkOrderType.Rework)
                {
                    workOrder.UseOldSn = true;
                }
                else
                {
                    workOrder.UseOldSn = false;
                }
            }
        }

        /// <summary>
        /// 绑定工艺路线版本
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="e">属性变更参数</param>
        protected virtual void BindingRoutingVersion(WorkOrder workOrder, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == WorkOrder.ResourceIdProperty.Name
                || e.PropertyName == WorkOrder.ProductIdProperty.Name
                || e.PropertyName == WorkOrder.PlanBeginDateProperty.Name
                || e.PropertyName == WorkOrder.TypeProperty.Name
                || e.PropertyName == WorkOrder.ProcessTechIdProperty.Name)
            {
                BindingRoutingVersion(workOrder);
            }
        }

        /// <summary>
        /// 绑定工艺路线版本
        /// </summary>
        /// <param name="workOrder">工单</param>
        public virtual void BindingRoutingVersion(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.Product == null && workOrder.Resource == null)
            {
                return;
            }

            workOrder.Version = null;
            var versions = RT.Service.Resolve<RoutingSettingController>()
                .AutoRoutingVersions(workOrder.Type, workOrder.PlanBeginDate, workOrder.ProductId, workOrder.ResourceId, workOrder.ProcessSegmentId, workOrder.ProjectMaintainId);
            // 获取多个版本时
            workOrder.Version = versions.Count > 1 ? versions.FirstOrDefault(x => x.IsDefault == YesNo.Yes) : versions.FirstOrDefault();
            if (workOrder.Version != null)
            {
                workOrder.RoutingId = workOrder.Version.RoutingId;
            }
        }

        /// <summary>
        /// 工艺路线版本变更
        /// </summary>
        /// <param name="workOrder">工单</param>      
        public virtual void RoutingVersionChanged(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            workOrder.Layout = null;
            GenerateRoutingProcesss(workOrder);
            GenerateProcessBoms(workOrder);
            GenerateWorkOrderProcessPackingUnit(workOrder);
        }

        /// <summary>
        /// Erp工单变更
        /// </summary>
        /// <param name="workOrder">工单</param>       
        public virtual void ErpWorkOrderChanged(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.ErpWorkOrder == null)
            {
                return;
            }

            workOrder.ErpWorkOrderNo = workOrder.ErpWorkOrder.No;
            workOrder.ErpSaleOrder = workOrder.ErpWorkOrder.ErpSaleOrder;
            workOrder.ErpOrderNo = workOrder.ErpWorkOrder.ErpSaleOrder?.No;
            workOrder.SaleOrderNo = workOrder.ErpOrderNo;
            workOrder.CustomerId = workOrder.ErpWorkOrder.ErpSaleOrder?.CustomerId;
            workOrder.OrderQty = workOrder.ErpWorkOrder.PlanQty;
        }

        /// <summary>
        /// 生成工单属性值清单
        /// </summary>
        /// <param name="productBom">产品BOM</param>
        /// <param name="workOrder">工单</param>
        public virtual void GeneratePropertyValues(ProductBom productBom, WorkOrder workOrder)
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
        /// <param name="workOrder">工单</param>
        public virtual void GenerateWorkOrderBoms(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            var productIds = new List<double> { workOrder.ProductId };
            ItemDataOwner itemDataOwner = new ItemDataOwner();
            var workOrderBomGenerator = new UseProductBomGenerator(productIds, itemDataOwner);

            workOrderBomGenerator.GenerateWorkOrderBom(workOrder);
        }

        /// <summary>
        /// 设置工单bom扩展属性
        /// </summary>
        /// <param name="bom">工单BOM</param>
        /// <param name="detail">产品BOM明细</param>
        public virtual void SetWorkOrderBomExtProperty(WorkOrderBom bom, ProductBomDetail detail)
        {
        }

        /// <summary>
        /// 生成工单包装规则
        /// </summary>
        /// <param name="workOrder">工单</param>
        public virtual void GenerateWorkOrderPackageRule(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                return;
            }

            workOrder.PackageRuleDetailList.Clear();
            if (workOrder.ProductId <= 0)
            {
                return;
            }

            //工单包装规则生成器
            var woPackageGenerator = new WoPackageGenerator(new EntityList<Item> { workOrder.Product });

            woPackageGenerator.GenerateWorkOrderPackageRule(workOrder);

            GenerateWorkOrderProcessPackingUnit(workOrder);
        }

        /// <summary>
        /// 物料包装规则明细生成工单包装规则明细
        /// </summary>
        /// <param name="packageRule">物料包装规则</param>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>工单包装规则明细</returns>
        public virtual WorkOrderPackageRuleDetail[] SetPackageRuleDetail(ItemPackageRule packageRule, double workOrderId)
        {
            double index = 1;
            var rules = packageRule.ItemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).Select((f, i) =>
            {
                var rule = new WorkOrderPackageRuleDetail
                {
                    DetailId = f.Id,
                    Description = f.Description,
                    Height = f.Height,
                    IsInStockLabel = f.IsInStockLabel,
                    IsOutStockLabel = f.IsOutStockLabel,
                    IsPackage = f.IsPackage,
                    Length = f.Length,
                    NumberRuleId = f.NumberRuleId,
                    PackageUnitId = f.PackageUnitId,
                    Qty = f.Qty,
                    LevelQty = f.LevelQty,
                    Volume = f.Volume,
                    Weight = f.Weight,
                    Width = f.Width,
                    WorkOrderId = workOrderId,
                    IsPrint = f.IsPrint,
                    PrintTemplate = f.PrintTemplate
                };
                rule.GenerateId();
                //SortExtension.SetIndex(rule, (int)rule.Id);
                rule.SetIndex(index++);
                return rule;
            }).ToArray();
            return rules;
        }

        #region 生成工艺路线数据
        /// <summary>
        /// 生成工单工艺路线工序清单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="isGenChildren">是否同时生成子列表</param>
        /// <param name="routingList">工序清单</param>
        /// <remarks>BS生成工序清单的子列表不能传到前端，在保存的时候才重新生成子</remarks>
        public virtual void GenerateRoutingProcesss(WorkOrder workOrder,
            bool isGenChildren = true, EntityList<WorkOrderRoutingProcess> routingList = null)
        {
            if (workOrder == null || !workOrder.VersionId.HasValue)
            {
                return;
            }

            //工单工序BOM生成器
            WorkOrderBomSourceType sourceType = WorkOrderBomSourceType.RoutingProcessBom;
            //var workOrderConfig = ConfigService.GetConfig(new WorkOrderProcessBomSourceConfig(), typeof(WorkOrder));
            //if (workOrderConfig != null)
            //{
            //    sourceType = workOrderConfig.ProcessBomType;
            //}

            bool useRoutingBomConfig = true;
            // 如果配置工序bom来源工艺路线
            if (sourceType == WorkOrderBomSourceType.ProductRoutingVersionBom)
            {
                //如果工单配置项【工单工序BOM配置项】的【工序bom来源】配置为【产品工序BOM】时，则不获取工艺路线的【工序BOM配置】资料
                //配置项对应 WorkOrderProcessBomSourceConfig()
                //配置在工单功能 typeof(WorkOrder)
                useRoutingBomConfig = false;
            }

            WoRoutingProcessGenerator woRoutingProcessGenerator =
                new WoRoutingProcessGenerator(new List<double> { workOrder.VersionId.Value }, useRoutingBomConfig);

            woRoutingProcessGenerator
               .GenerateRoutingProcesss(workOrder, setDisplayProperty: true, isGenChildren, routingList);
        }
        #endregion

        /// <summary>
        /// 生成工序Bom（取工艺路线工序BOM配置 与 工单BOM 的交集）
        /// </summary>
        /// <param name="workOrder">工单</param>
        public virtual void GenerateProcessBoms(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                return;
            }

            workOrder.ProcessBomList.Clear();
            if (!workOrder.VersionId.HasValue)
            {
                return;
            }

            

            // 工单工序BOM生成器
            IWoProcessBomGenerator WoProcessBomGenerator;
            ItemDataOwner itemDataOwner = new ItemDataOwner();

            //工单工序BOM生成器
            WorkOrderBomSourceType sourceType = WorkOrderBomSourceType.RoutingProcessBom;
            //固定通过工艺路线去生成
            //var workOrderConfig = ConfigService.GetConfig(new WorkOrderProcessBomSourceConfig(), typeof(WorkOrder));
            //if (workOrderConfig != null)
            //{
            //    sourceType = workOrderConfig.ProcessBomType;
            //}


            // 如果配置工序bom来源工艺路线
            if (sourceType == WorkOrderBomSourceType.RoutingProcessBom)
            {
                var routingProcessIds = workOrder.RoutingProcessList.SelectMany(p => new List<double> { p.RoutingProcessId.Value }).ToList();
                var routingProcessBomConfigs = RT.Service.Resolve<RoutingController>()
                         .GetRoutingProcessBomConfigs(routingProcessIds);
                WoProcessBomGenerator = new BomConfigGenerator(itemDataOwner, routingProcessBomConfigs);
                var itemIds = routingProcessBomConfigs.Select(m => m.ItemId).Distinct().ToList();
                itemDataOwner.GetItemsAndCache(itemIds);
            }
            else
            {
                // 产品工序BOM ProductRoutingVersionBom = 1                
                var productIds = new List<double> { workOrder.ProductId };
                var versionIds = new List<double> { workOrder.VersionId.Value };
                WoProcessBomGenerator = new RoutingBomDetailGenerator(productIds, versionIds, itemDataOwner);
            }

            WoProcessBomGenerator.GenerateProcessBoms(workOrder);
        }

        /// <summary>
        /// 设置工单工序BOM扩展属性
        /// </summary>
        /// <param name="processBom">工序BOM</param>
        /// <param name="bom">工单BOM</param>
        public virtual void SetWorkOrderProcessBomExtProperty(WorkOrderProcessBom processBom, WorkOrderBom bom)
        {
        }


        /// <summary>
        /// 生成工单工序单位关系
        /// </summary>
        /// <param name="workOrder">工单</param>
        public virtual void GenerateWorkOrderProcessPackingUnit(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                return;
            }

            if (!workOrder.RoutingProcessList.Any(f => f.ProcessType == ProcessType.Packing || f.ProcessType == ProcessType.BatchPacking))
            {
                return;
            }

            var processIds = workOrder.RoutingProcessList
                .Where(f => f.ProcessType == ProcessType.Packing || f.ProcessType == ProcessType.BatchPacking)
                .Where(x => x.ProcessId.HasValue)
                .Select(x => x.ProcessId.Value)
                .Distinct().ToList();
            var processes = RT.Service.Resolve<ProcessController>().GetProcessByIds(processIds, loadViewProperty: false);

            var woProcessPackingUnitGenerator = new WoProcessPackingUnitGenerator(processes);

            woProcessPackingUnitGenerator.GenerateWorkOrderProcessPackingUnit(workOrder);

            //包装规则生成ID
            workOrder.PackageRuleDetailList.ForEach(x => x.GenerateId());

            //包装规则与工序的关系
            workOrder.PackageRuleDetailList.SelectMany(x => x.WorkOrderProcessPackingUnitList).ForEach(ppu =>
            {
                ppu.PackageRuleId = ppu.PackageRule.Id;
            });
        }

        /// <summary>
        /// 生成工艺路线布局
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void GenerateRoutingLayout(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.Version == null || workOrder.Version.Layout == null)
            {
                return;
            }

            var workOrderRoutingLayout = new WorkOrderRoutingLayout()
            {
                Layout = workOrder.Version.Layout.Layout,
            };
            workOrderRoutingLayout.GenerateId();
            workOrder.Layout = workOrderRoutingLayout;
        }


        /// <summary>
        /// 生成工单工序清单参数的下一个工序
        /// </summary>
        /// <param name="routingProcessParameterList">工艺路线工序清单参数</param>
        /// <param name="routingProcesses">工艺路线工序清单</param>
        /// <param name="workOrderRoutingProcessList">工单工艺路线工序清单</param>
        public virtual void GenerateParameterNextProcess(IEnumerable<RoutingProcessParameter> routingProcessParameterList,
            IList<RoutingProcess> routingProcesses,
            EntityList<WorkOrderRoutingProcess> workOrderRoutingProcessList)
        {
            var workOrderRoutingProcessParameterList = workOrderRoutingProcessList.SelectMany(p => p.ParameterList);

            foreach (var workOrderRoutingProcessParameter in workOrderRoutingProcessParameterList)
            {
                var routingProcessParameter = routingProcessParameterList
                    .FirstOrDefault(p => p.RuleId == workOrderRoutingProcessParameter.RuleId);

                if (routingProcessParameter == null || routingProcessParameter.NextProcessId == null)
                {
                    continue;
                }

                var routingProcess = routingProcesses
                    .FirstOrDefault(x => x.Id == routingProcessParameter.NextProcessId);
                if (routingProcess == null)
                {
                    continue;
                }

                var nextWorkOrderRoutingProcess = workOrderRoutingProcessList
                    .FirstOrDefault(p => p.ActivityId == routingProcess.ActivityId);
                if (nextWorkOrderRoutingProcess != null)
                {
                    if (nextWorkOrderRoutingProcess.Id > 0)
                    {
                        workOrderRoutingProcessParameter.NextProcessId = nextWorkOrderRoutingProcess.Id;
                    }
                    else
                    {
                        workOrderRoutingProcessParameter.NextProcess = nextWorkOrderRoutingProcess;
                    }
                }
            }
        }

    }
}
