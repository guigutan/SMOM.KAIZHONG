using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.ProductPanelQtys;
using SIE.EventMessages.WorkOrders;
using SIE.Items;
using SIE.MES.Interfaces.TaskManages;
using SIE.MES.RoutingSettings;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Events;
using SIE.MES.WorkOrders.Interfaces;
using SIE.Packages;
using SIE.Tech.Routings;
using SIE.Web.Data;
using SIE.Web.Items.ViewModels;
using SIE.Web.MES.WorkOrders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单BS数据处理
    /// </summary>  
    public class WorkOrderDataQueryer : DataQueryer
    {
        /// <summary>
        /// 工单属性变更
        /// </summary>
        readonly WorkOrderPropertyChanged proChanged = new WorkOrderPropertyChanged();

        /// <summary>
        /// 新增工单
        /// </summary>
        /// <returns>工单</returns>
        public WorkOrder GetNewWorkOrder()
        {
            return RT.Service.Resolve<WorkOrderController>().CreateWorkOrder();
        }

        /// <summary>
        /// 复制工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="oldWorkOrderId">旧工单ID</param>
        /// <returns>工单</returns>
        public WorkOrder GetCopyWorkOrder(WorkOrder workOrder, double oldWorkOrderId)
        {
            return RT.Service.Resolve<WorkOrderController>().CopyWorkOrder(workOrder, oldWorkOrderId);
        }

        /// <summary>
        /// 获取复制的工单信息
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="oldWorkOrderId">旧工单ID</param>
        /// <returns>工单信息</returns>
        public CopyWorkOrderInfo GetCopyWorkOrderInfo(WorkOrder workOrder, double oldWorkOrderId)
        {
            var info = new CopyWorkOrderInfo();
            info.CopyWorkOrder = GetCopyWorkOrder(workOrder, oldWorkOrderId);
            info.PrintTemplate = GetWorkOrderPrintTemplate(workOrder.TemplateId);
            info.BatchQty = GetWorkOrderBatch(oldWorkOrderId);
            info.ItemExtProp = workOrder.ItemExtProp;
            info.ItemExtPropName = workOrder.ItemExtPropName;
            //var propertyList = GetPropertyValueViewModel(oldWorkOrderId);
            //info.PropertyValues.AddRange(propertyList);
            return info;
        }

        #region 工单产品属性变更查询数据
        /// <summary>
        /// 工单产品属性变更查询数据
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>工单变更信息</returns>
        public ProductChangedInfo ProductChangedGetData(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            var info = new ProductChangedInfo();
            if (workOrder.ProductId != 0)
            {
                //生成工单bom
                RT.Service.Resolve<ErpWorkOrderPropertyChanged>().GenerateWorkOrderBoms(workOrder);

                //生成包装规则
                RT.Service.Resolve<ErpWorkOrderPropertyChanged>().GenerateWorkOrderPackageRule(workOrder);
            }
            //获取打印模板
            info.Template = GetTemplateByItemIdOrCustomerId(workOrder.ProductId, workOrder.CustomerId);
            //获取产品是否单体追溯
            info.IsSingle = IsSingleProduct(workOrder.ProductId);
            //获取产品对应拼板数，默认为1
            info.PanelQty = RT.Service.Resolve<IProductPanelQty>().GetPanelQty(workOrder.ProductId);
            //获取是否组合板工单
            info.IsPanelWorkOrder = RT.Service.Resolve<IIsPanelWorkOrder>().IsPanelWorkOrder(workOrder.ProductId);
            //绑定工艺路线版本
            //info.RoutingVersion = BindingRoutingVersion(workOrder);
            //工单bom和包装规则数据处理
            HandleBomAndPackData(workOrder);
            info.BomInfos.AddRange(workOrder.BomList);
            info.PackageRuleInfos.AddRange(workOrder.PackageRuleDetailList);
            //获取工单BOM属性值已修改为字符串
            //var workOrderBomPropertyList = workOrder.ItemExtPropName;   //GetBomPropertyData(workOrder);
            //info.WorkOrderBomPropertys.Add(workOrderBomPropertyList);
            return info;
        }

        /// <summary>
        /// 工单bom和包装规则数据处理(用于前端正确显示)
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void HandleBomAndPackData(WorkOrder workOrder)
        {
            var now = RF.Find<WorkOrder>().GetDbTime();
            workOrder.BomList.ForEach(p =>
            {
                p.ItemCode = p.Item.Code; p.ItemName = p.Item.Name;
                p.CreateDate = now; p.UpdateDate = now;
                p.ExtValues["ItemId_Display"]= p.Item.Code;
            });
            workOrder.PackageRuleDetailList.ForEach(p =>
            {
                p.ExtValues.Add("NumberRuleId_Display", p.NumberRule?.Name);
                p.PackageUnitName = p.PackageUnit.Name;
                p.CreateDate = now; p.UpdateDate = now;
                p.WorkOrderProcessPackingUnitList.ForEach(w => { w.UpdateDate = now; w.CreateDate = now; });
            });
        }
        #endregion

        /// <summary>
        /// 产品更改生成工单bom
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>WorkOrder</returns>
        public EntityList<WorkOrderBom> ProductChangedGetBomList(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.ProductId != 0)
            {
                RT.Service.Resolve<ErpWorkOrderPropertyChanged>().GenerateWorkOrderBoms(workOrder);
            }

            workOrder.BomList.ForEach(p =>
            {
                p.ItemCode = p.Item.Code;
                p.ItemName = p.Item.Name;
            });

            return workOrder.BomList;
        }

        /// <summary>
        /// 产品更改选择包装规则
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>WorkOrder</returns>
        public EntityList<WorkOrderPackageRuleDetail> ProductChangedGetPackRule(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.ProductId != 0)
            {
                RT.Service.Resolve<ErpWorkOrderPropertyChanged>().GenerateWorkOrderPackageRule(workOrder);
            }
            workOrder.PackageRuleDetailList.ForEach(p => p.PackageUnitName = p.PackageUnit.Name);
            return workOrder.PackageRuleDetailList;
        }

        /// <summary>
        /// BS选择包装规则
        /// </summary>
        /// <param name="packId">包装Id</param>
        /// <param name="woId">工单Id</param>
        /// <returns>包装规则明细</returns>
        public EntityList<WorkOrderPackageRuleDetail> ProductChangedGetPackRuleById(double packId, double woId)
        {
            var packRule = RF.GetById<ItemPackageRule>(packId, new EagerLoadOptions().LoadWithViewProperty());
            EntityList<WorkOrderPackageRuleDetail> ruleList = new EntityList<WorkOrderPackageRuleDetail>();
            var rules = RT.Service.Resolve<ErpWorkOrderPropertyChanged>().SetPackageRuleDetail(packRule, woId);
            ruleList.AddRange(rules);
            ruleList.ForEach(p =>
            {
                p.PackageUnitName = p.PackageUnit.Name;
                p.ExtValues.Add("NumberRuleId_Display", p.NumberRule?.Name);
                p.ExtValues.Add("PrintTemplateId_Display", p.PrintTemplate?.FileName);
            });
            return ruleList;
        }

        /// <summary>
        /// 计划数量变更
        /// </summary>
        /// <param name="workOrder">工单</param>       
        /// <returns>WorkOrder</returns>
        public EntityList<WorkOrderBom> PlanQtyChanged(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            proChanged.PlanQtyChanged(workOrder);
            return workOrder.BomList;
        }

        /// <summary>
        /// 绑定工艺路线版本
        /// </summary>
        /// <param name="workOrder">工单</param>       
        /// <returns>WorkOrder</returns>
        public RoutingVersion BindingRoutingVersion(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            proChanged.BindingRoutingVersion(workOrder);
            return workOrder.Version;
        }

        #region 工艺路线版本变更

        /// <summary>
        /// 工艺路线版本变更
        /// </summary>
        /// <param name="workOrder">工单</param>       
        /// <returns>工单与工序清单关系</returns>
        public EntityList<WorkOrderRoutingProcess> RoutingVersionChanged(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            proChanged.GenerateRoutingProcesss(workOrder, false);

            workOrder.RoutingProcessList.ForEach(x =>
            {
                if (!x.HasId)
                {
                    x.GenerateId();
                }
            });
            return workOrder.RoutingProcessList;
        }

        /// <summary>
        /// 包装规则子，选择工序后生成工序包装单位
        /// </summary>
        /// <param name="processIds">工序Id集合</param>
        /// <param name="ruleId">包装规则Id</param>
        /// <returns>工序包装单位</returns>
        public EntityList<WorkOrderProcessPackingUnit> GeneratePackUnit(List<double> processIds, double ruleId)
        {
            if (processIds is null)
            {
                throw new ArgumentNullException(nameof(processIds));
            }

            var list = new EntityList<WorkOrderProcessPackingUnit>();
            processIds.ForEach(p =>
            {
                WorkOrderProcessPackingUnit item = new WorkOrderProcessPackingUnit()
                {
                    ProcessId = p,
                    PackageRuleId = ruleId
                };
                list.Add(item);
            });
            return list;
        }

        /// <summary>
        /// 工艺路线版本变更工单工序BOM
        /// </summary>
        /// <param name="workOrder">工单</param>       
        /// <returns>工单与工序BOM关系</returns>
        public EntityList<WorkOrderProcessBom> RoutingVersionChangedProcessBom(WorkOrder workOrder)
        {
            return RT.Service.Resolve<WorkOrderController>().RoutingVersionChangedProcessBom(workOrder);
        }

        /// <summary>
        /// 工艺路线版本变更生成工单工序单位关系
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>工单包装规则列表</returns>
        public EntityList<WorkOrderPackageRuleDetail> ProcessPackingUnit(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            proChanged.GenerateWorkOrderProcessPackingUnit(workOrder);
            workOrder.PackageRuleDetailList.ForEach(p => { p.PackageUnitName = p.PackageUnit.Name; });
            return workOrder.PackageRuleDetailList;
        }

        #endregion

        /// <summary>
        /// Erp工单变更
        /// </summary>
        /// <param name="workOrder">工单</param>       
        /// <returns>WorkOrder</returns>
        public WorkOrder ErpWorkOrderChanged(WorkOrder workOrder)
        {
            proChanged.ErpWorkOrderChanged(workOrder);
            return workOrder;
        }

        /// <summary>
        /// 获取产品属性定义
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="pageIndex">分页</param>
        /// <param name="pageSize">大小</param>
        /// <param name="keyword">k</param>
        /// <returns>产品属性定义</returns>
        public EntityList GetDefinition(double? productId, int pageIndex, int pageSize, string keyword)
        {
            if (productId > 0)
            {
                var pagingInfo = new PagingInfo(pageIndex, pageSize, true);
                var result = RT.Service.Resolve<ItemController>().GetItemPropertys(productId.Value, keyword, pagingInfo).
                    Select(p => p.Definition).Distinct((x, y) => x.Name == y.Name).AsEntityList();
                return result;
            }
            return new EntityList<ItemPropertyDefinition>();
        }

        /// <summary>
        /// 获取物料属性值
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="definitionId">属性Id</param> 
        /// <param name="keyWord">关键件</param>
        /// <returns>产品属性定值</returns>
        public EntityList GetDefinitionValues(double? productId, double? definitionId, string keyWord)
        {
            if (productId > 0 && definitionId > 0)
            {
                var result = RT.Service.Resolve<ItemController>().GetItemPropertys(productId.Value, definitionId.Value);
                return result;
            }
            return new EntityList<ItemPropertyValue>();
        }

        /// <summary>
        /// 获取工艺路线版本
        /// </summary>
        /// <param name="woType">工单类型</param>
        /// <param name="planBeginDate">开始时间</param>
        /// <param name="productId">产品Id</param>
        /// <param name="resId">产线Id</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public EntityList GetRoutingVersion(int woType, DateTime planBeginDate, double productId, double resId, double? processSegmentId, double? projectId)
        {
            return RT.Service.Resolve<RoutingSettingController>().GetRoutingVersions((SIE.Core.WorkOrders.WorkOrderType)woType, planBeginDate, productId, resId, processSegmentId, projectId);
        }

        #region 属性值保存
        /// <summary>
        /// 保存工单属性值
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="proModel">属性值</param>
        /// <param name="processBomProModel">工序BOM属性值列表</param>
        /// <param name="bomProModel">BOM属性值</param>
        public void SaveWorkOrderProValue(double workOrderId, List<PropertyValueViewModel> proModel, List<PropertyValueViewModel> processBomProModel, List<PropertyValueViewModel> bomProModel)
        {
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            RF.Save(workOrder);
        }
        #endregion

        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>模板</returns>
        public SIE.Core.Items.LabelPrintTemplate GetTemplate(double workOrderId)
        {
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
            {
                return null;
            }
            return RF.GetById<SIE.Core.Items.LabelPrintTemplate>(workOrder.TemplateId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料+客户获取模板数据(可扩展获取客户的打印)
        /// </summary>
        /// <param name="productId">物料Id</param>
        /// <param name="customerId">客户Id</param>
        /// <returns>模板</returns>
        public SIE.Core.Items.LabelPrintTemplate GetTemplateByItemIdOrCustomerId(double productId, double? customerId)
        {
            var template = RT.Service.Resolve<IWorkOrderTemplate>().GetWorkOrderTemplate(productId, customerId);
            if (template != null)
            {
                template.ExtValues.Add("NumberRuleId_Display", template.NumberRule?.Name);
                template.ExtValues.Add("LabelTemplateId_Display", template.LabelTemplate?.FileName);
                template.ExtValues.Add("PackingTemplateId_Display", template.PackingTemplate?.FileName);
            }
            return template;
        }

        /// <summary>
        /// 判断产品是否批次追溯
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>bool</returns>
        public bool IsSingleProduct(double productId)
        {
            var batch = RT.Service.Resolve<ItemController>().GetBatchRule(productId);
            if (batch == null) return false;
            return batch.RetrospectType == SIE.Core.Items.RetrospectType.Single;
        }

        /// <summary>
        /// 复制工单获取原工单的打印模板设置
        /// </summary>
        /// <param name="templateId">原工单打印模板ID</param>
        /// <returns>打印模板</returns>
        public SIE.Core.Items.LabelPrintTemplate GetWorkOrderPrintTemplate(double? templateId)
        {
            var printTemplate = new SIE.Core.Items.LabelPrintTemplate();

            if (templateId != null)
            {
                printTemplate = RF.GetById<SIE.Core.Items.LabelPrintTemplate>(templateId, new EagerLoadOptions().LoadWithViewProperty());
            }

            return printTemplate;
        }

        /// <summary>
        /// 复制工单获取原工单的批次数量
        /// </summary>
        /// <param name="woId">原工单ID</param>
        /// <returns>批次数量</returns>
        public decimal GetWorkOrderBatch(double woId)
        {
            var batch = RT.Service.Resolve<SIE.Core.WorkOrders.WorkOrderController>().GetWipBatch(woId);
            if (batch == null) return 1;
            else
                return batch.Qty.Value;
        }

        #region 工单工艺路线 
        /// <summary>
        /// 获取工单工艺路线版本布局
        /// UpdateRoutingCommand.js调用
        /// </summary>
        /// <param name="layoutId">工艺路线布局id</param>
        /// <returns>布局xml</returns>
        public string GetRoutingLayout(double layoutId)
        {
            var l = RF.GetById<WorkOrderRoutingLayout>(layoutId)?.Layout;
            return l;
        }

        /// <summary>
        /// 判断工单是否存在包装规则
        /// SIE.Web.MES.WorkOrders.UpdateRoutingCommand.js 调用
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public bool IsExistPackingUnit(double workOrderId)
        {
            return RT.Service.Resolve<WorkOrderController>().IsExistPackingUnit(workOrderId);
        }

        /// <summary>
        /// 获取工单工艺路线布局
        /// SIE.Web.MES.WorkOrders.WorkOrderLifeCycle.js 调用
        /// </summary>
        /// <param name="versionId">工艺路线版本ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="fromVersion">布局是否来自版本变更</param>
        /// <returns></returns>
        public string GetWorkOrderRoutingLayout(double versionId, double workOrderId, bool fromVersion)
        {
            string layout = string.Empty;
            if (fromVersion)
                layout = RF.GetById<RoutingVersion>(versionId)?.Layout?.Layout;
            else
            {
                layout = RF.GetById<WorkOrder>(workOrderId)?.Layout?.Layout;
                if (layout.IsNullOrEmpty())
                    layout = RF.GetById<RoutingVersion>(versionId)?.Layout?.Layout;
            }
            return layout;
        }
        #endregion

        /// <summary>
        /// 获取物料追溯方式
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>物料追溯方式</returns>
        public SIE.Core.Items.RetrospectType? GetRetrospectType(double itemId)
        {
            return RT.Service.Resolve<WorkOrderController>().GetRetrospectType(itemId);
        }

        #region 任务单生成
        /// <summary>
        /// 任务单生成
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>true/false</returns>
        public bool IsReGenerateTask(WorkOrder workOrder)
        {
            if (workOrder.Id > 0)
            {
                var isGenerate = RT.Service.Resolve<ITaskManage>().IsGenerateTask();
                if (!isGenerate) return false;
                var oldWorkOrder = RF.GetById<WorkOrder>(workOrder.Id);
                if (oldWorkOrder != null)
                {
                    if (oldWorkOrder.VersionId != workOrder.VersionId || oldWorkOrder.PlanQty != workOrder.PlanQty)
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
}