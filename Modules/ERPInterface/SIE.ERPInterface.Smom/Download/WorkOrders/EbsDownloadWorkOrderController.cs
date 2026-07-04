using Microsoft.Scripting.Utils;
using NPOI.SS.Formula.Functions;
using SIE.Barcodes.Panels;
using SIE.Common;
using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.SmomOrder;
using SIE.ERPInterface.Common.Enums;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.MES;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.RoutingSettings;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Routings;
using SIE.MES.WorkOrders.WorkOrderPackageGenerators;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessTechs;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.WorkOrders
{
    /// <summary>
    /// ebs工单保存
    /// </summary>
    public class EbsDownloadWorkOrderController : DomainController
    {
        /// <summary>
        /// 通用控制器
        /// </summary>
        private CommonController _commonController = RT.Service.Resolve<CommonController>();

        /// <summary>
        /// 从API下载工单到业务表
        /// </summary>
        /// <param name="workOrderDatas"></param>
        /// <param name="extentInvOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadWorkOrderToBusiness(List<EbsWorkOrderData> workOrderDatas, string extentInvOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.EbsApiSaveBusinessData<EbsWorkOrderData>(
                workOrderDatas,
                p => this.SaveWorkOrders(p),
                JobType.WorkOrder,
                extentInvOrg);
        }

        /// <summary>
        /// ERP保存工单数据
        /// </summary>
        /// <param name="erpInfoDatas">ERP工单数据集合</param>
        /// <returns>错误信息列表</returns>
        public virtual List<ErpErrorData> SaveWorkOrders(List<EbsWorkOrderData> erpInfoDatas)
        {
            List<ErpErrorData> res = new List<ErpErrorData>();
            var workOrderController = RT.Service.Resolve<WorkOrderController>();
            Dictionary<string, WorkOrder> dicWorkOrder;
            Dictionary<double, List<WorkOrderBom>> dicBom;
            Dictionary<string, Item> dicItem;
            Dictionary<string, Enterprise> dicFactory;
            Dictionary<string, Enterprise> dicWorkshop;
            Dictionary<string, WipResource> dicResource;
            Employee erpEmployee;

            // 生成的工单
            EntityList<WorkOrder> erpInsertWorkOrderList = new EntityList<WorkOrder>();
            // 生成的工单bom
            EntityList<WorkOrderBom> erpInsertWorkOrderBomList = new EntityList<WorkOrderBom>();

            // 修改的工单
            EntityList<WorkOrder> erpEditWorkOrderList = new EntityList<WorkOrder>();
            // 修改的工单bom
            EntityList<WorkOrderBom> erpEditWorkOrderBomList = new EntityList<WorkOrderBom>();

            // 工单打印模板
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates = new EntityList<Core.Items.LabelPrintTemplate>();

            // 工艺路线布局
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts = new EntityList<WorkOrderRoutingLayout>();

            //1 获取相关联数据
            GetWorkOrderRelatedDatas(erpInfoDatas, out dicWorkOrder, out dicBom, out dicItem, out dicFactory, out dicWorkshop, out dicResource, out erpEmployee);
            //2 获取工艺路线
            List<ReleasePlanData> _releasePlanDatas = EbsChangeToTask(erpInfoDatas, dicItem);
            ItemDataOwner itemDataOwner = new ItemDataOwner();
            WoRoutingGenerator woRoutingGenerator = new WoRoutingGenerator(_releasePlanDatas, itemDataOwner);

            //3 获取产品包装规则
            EntityList<Item> woProduct = new EntityList<Item>();
            woProduct.AddRange(dicItem.Values);
            WoPackageGenerator woPackageGenerator = new WoPackageGenerator(woProduct);

            //4 获取制单时间
            DateTime? makeDate = RF.Find<WorkOrder>().GetDbTime();

            foreach(var data in erpInfoDatas)
            {
                try
                {
                    //2 验证数据合理性
                    WorkOrder workOrder; Item product; Enterprise factory; Enterprise workShop; WipResource resource;
                    ValidateWorkOrderData(data, dicWorkOrder, dicItem, dicFactory, dicWorkshop, dicResource, out workOrder, out product, out factory, out workShop, out resource);
                    if (workOrder == null)
                    {
                        // 新增工单
                        workOrder = CreateWorkOrder(data, dicItem, makeDate.Value, product, factory, workShop, resource, erpEmployee, woPackageGenerator, labelPrintTemplates, woRoutingGenerator, workOrderRoutingLayouts);
                        erpInsertWorkOrderList.Add(workOrder);
                        erpInsertWorkOrderBomList.AddRange(workOrder.BomList);
                    }
                    else
                    {
                        string errMsg;
                        // 修改校验
                        if (!WorkOrderCanEdit(workOrder, data, product, out errMsg))
                            throw new ValidationException("{0}".L10nFormat(errMsg));
                        if (!dicBom.TryGetValue(workOrder.Id, out List<WorkOrderBom> existBoms))
                            existBoms = new List<WorkOrderBom>();
                        List<WorkOrderBom> insertBoms = new List<WorkOrderBom>();
                        // 修改工单
                        UpdateWorkOrder(data, existBoms, insertBoms, dicItem, workOrder);
                        erpEditWorkOrderList.Add(workOrder);
                        erpEditWorkOrderBomList.AddRange(existBoms);
                        erpInsertWorkOrderBomList.AddRange(insertBoms);
                    }
                    res.Add(new ErpErrorData()
                    {
                        ErrMsg = string.Empty,
                        Infkey = data.ErpDetailId,
                        IsChild = false,
                        IsSuccess = true,
                    });
                }
                catch (Exception exc)
                {
                    res.Add(new ErpErrorData()
                    {
                        ErrMsg = exc.Message,
                        Infkey = data.ErpDetailId,
                        IsChild = false,
                        IsSuccess = false,
                    });
                }
            }

            // 批量保存工单、工单bom、工单打印模板、工单工序清单
            BatchSave(workOrderRoutingLayouts, labelPrintTemplates, erpInsertWorkOrderList, erpInsertWorkOrderBomList, erpEditWorkOrderList, erpEditWorkOrderBomList, makeDate);

            return res;
        }

        /// <summary>
        /// 生成工单日志生成工单日志
        /// </summary>
        /// <param name="erpInsertWorkOrderList">新增工单</param>
        /// <param name="erpEditWorkOrderList">修改工单</param>
        /// <param name="dateTime">日志时间</param>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveWoLogs(EntityList<WorkOrder> erpInsertWorkOrderList, EntityList<WorkOrder> erpEditWorkOrderList, DateTime? dateTime)
        {
            dateTime ??= DateTime.Now;
            EntityList<WorkOrderLog> workOrderLogs = new EntityList< WorkOrderLog>();
            foreach (var newWo in  erpInsertWorkOrderList)
            {
                WorkOrderLog log = RT.Service.Resolve<WorkOrderController>()
                           .CreateWorkOrderLog(newWo.Id, WorkOrderLogType.Create, "EBS新增", dateTime.Value);
                workOrderLogs.Add(log);
            }
            foreach (var editWo in erpEditWorkOrderList)
            {
                WorkOrderLog log = RT.Service.Resolve<WorkOrderController>()
                           .CreateWorkOrderLog(editWo.Id, WorkOrderLogType.Other, "EBS修改", dateTime.Value);
                workOrderLogs.Add(log);
            }
            RF.BatchInsert(workOrderLogs);
        }

        /// <summary>
        /// 使用工艺路线生成器做数据转换
        /// </summary>
        /// <param name="erpInfoDatas">传入信息</param>
        /// <param name="dicItem">物料字典</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<ReleasePlanData> EbsChangeToTask(List<EbsWorkOrderData> erpInfoDatas, Dictionary<string, Item> dicItem)
        {
            ReleasePlanData releasePlanData = new ReleasePlanData();
            foreach (var data in erpInfoDatas)
            {
                if (!dicItem.TryGetValue(data.ProductCode, out Item item))
                {
                    continue;
                }
                ReleasePlanDetail releasePlanDetail = new ReleasePlanDetail
                {
                    ItemId = item.Id,
                };
                releasePlanData.Details.Add(releasePlanDetail);
            }
            return new List<ReleasePlanData> { releasePlanData };
        }


        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="workOrderRoutingLayouts">新增工单工艺路线布局</param>
        /// <param name="labelPrintTemplates">新增工单打印模板</param>
        /// <param name="erpInsertWorkOrderList">新增工单信息</param>
        /// <param name="erpInsertWorkOrderBomList">新增工单bom信息</param>
        /// <param name="erpEditWorkOrderList">修改工单信息</param>
        /// <param name="erpEditWorkOrderBomList">修改工单bom信息</param>
        /// <param name="dateTime">日期时间</param>
        private void BatchSave(EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts, EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates, EntityList<WorkOrder> erpInsertWorkOrderList, EntityList<WorkOrderBom> erpInsertWorkOrderBomList, EntityList<WorkOrder> erpEditWorkOrderList, EntityList<WorkOrderBom> erpEditWorkOrderBomList, DateTime? dateTime)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //工单打印模板
                RF.BatchInsert(labelPrintTemplates);

                //工单工艺路线布局
                RF.BatchInsert(workOrderRoutingLayouts);

                erpInsertWorkOrderList.ForEach(wo =>
                {
                    if (wo.Template != null)
                        wo.TemplateId = wo.Template.Id;
                    if (wo.Layout != null)
                        wo.LayoutId = wo.Layout.Id;
                });
                // 新增工单
                RF.BatchInsert(erpInsertWorkOrderList);


                erpInsertWorkOrderBomList.ForEach(bom =>
                {
                    bom.WorkOrderId = bom.WorkOrder.Id;
                });
                // 新增工单bom
                RF.BatchInsert(erpInsertWorkOrderBomList);

                //工单工序清单的列表
                var routingProcesses = erpInsertWorkOrderList.SelectMany(x => x.RoutingProcessList).AsEntityList();
                routingProcesses.ForEach(x =>
                {
                    x.WorkOrderId = x.WorkOrder.Id;
                });
                RF.BatchInsert(routingProcesses);

                //工单工序清单的参数列表
                var processParameters = routingProcesses.SelectMany(x => x.ParameterList).AsEntityList();
                processParameters.ForEach(x =>
                {
                    x.ProcessId = x.Process.Id;
                    if (x.NextProcess != null)
                    {
                        x.NextProcessId = x.NextProcess.Id;
                    }
                });
                RF.BatchInsert(processParameters);

                //工单工序清单的工序BOM配置列表
                var routingBomConfigs = routingProcesses.SelectMany(x => x.BomConfigList).AsEntityList();
                routingBomConfigs.ForEach(x =>
                {
                    x.ProcessId = x.Process.Id;
                });
                RF.BatchInsert(routingBomConfigs);

                //工单工序BOM清单的列表
                var processBoms = erpInsertWorkOrderList.SelectMany(x => x.ProcessBomList).AsEntityList();
                //设置工序BOM的工单工序清单的ID
                processBoms.ForEach(x =>
                {
                    x.RoutingProcessId = x.RoutingProcess.Id;
                    x.WorkOrderId = x.WorkOrder.Id;
                });
                RF.BatchInsert(processBoms);

                //工单与包装规则关系的列表
                var packageRuleDetails = erpInsertWorkOrderList.SelectMany(x => x.PackageRuleDetailList).AsEntityList();
                packageRuleDetails.ForEach(x =>
                {
                    x.WorkOrderId = x.WorkOrder.Id;
                });
                RF.BatchInsert(packageRuleDetails);

                //包装单位与工序关系
                var processPackingUnits = packageRuleDetails.SelectMany(x => x.WorkOrderProcessPackingUnitList).AsEntityList();
                processPackingUnits.ForEach(x =>
                {
                    x.PackageRuleId = x.PackageRule.Id;
                });
                RF.BatchInsert(processPackingUnits);

                // 修改工单bom
                RF.Save(erpEditWorkOrderBomList);

                // 修改工单
                RF.Save(erpEditWorkOrderList);

                // 生成工单日志
                SaveWoLogs(erpInsertWorkOrderList, erpEditWorkOrderList, dateTime);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取工单相关联数量集
        /// </summary>
        /// <param name="erpInfoDatas">ERP工单数据集合</param>
        /// <param name="dicWorkOrder">工单数据字典</param>
        /// <param name="dicBom">工单BOM字典</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="dicFactory">工厂数据字典</param>
        /// <param name="dicWorkshop">车间数据字典</param>
        /// <param name="dicResource">资源数据字典</param>
        /// <param name="erpEmployee">写入员工</param>
        private void GetWorkOrderRelatedDatas(List<EbsWorkOrderData> erpInfoDatas, out Dictionary<string, WorkOrder> dicWorkOrder, out Dictionary<double, List<WorkOrderBom>> dicBom, out Dictionary<string, Item> dicItem, out Dictionary<string, Enterprise> dicFactory, out Dictionary<string, Enterprise> dicWorkshop, out Dictionary<string, WipResource> dicResource, out Employee erpEmployee)
        {
            List<string> workOrderNoList = erpInfoDatas.Where(p => !p.WorkOrderNo.IsNullOrEmpty()).Select(p => p.WorkOrderNo).Distinct().ToList();
            if (workOrderNoList.Count > 0)
            {
                //工单集合
                var woExp = workOrderNoList.CreateContainsExpression<WorkOrder>("x", nameof(WorkOrder.No));
                var workOrderList = _commonController.GetDatas(woExp, null, new EagerLoadOptions().LoadWith(WorkOrder.BomListProperty));
                dicWorkOrder = workOrderList.ToDictionary(p => p.No);
                //工单BOM集合
                List<double> workOrderIdList = workOrderList.Select(p => p.Id).ToList();
                var bomExp = workOrderIdList.CreateContainsExpression<WorkOrderBom>("x", nameof(WorkOrderBom.WorkOrderId));
                dicBom = _commonController.GetDatas(bomExp, null, new EagerLoadOptions().LoadWithViewProperty()).GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());
            }
            else
            {
                dicWorkOrder = new Dictionary<string, WorkOrder>();
                dicBom = new Dictionary<double, List<WorkOrderBom>>();
            }
            //产品集合
            List<string> productCodeList = erpInfoDatas.Select(p => p.ProductCode).ToList();
            List<string> itemCodeList = erpInfoDatas.SelectMany(p => p.BomList).Select(p => p.ItemCode).ToList();
            var sumItemCodeList = productCodeList.Union(itemCodeList).Where(p => !p.IsNullOrEmpty()).Distinct().ToList();
            if (sumItemCodeList.Count > 0)
            {
                var itemExp = sumItemCodeList.CreateContainsExpression<Item>("x", nameof(Item.Code));
                dicItem = _commonController.GetDatas(itemExp).ToDictionary(p => p.Code);
            }
            else
                dicItem = new Dictionary<string, Item>();
            //工厂集合
            List<string> factoryCodeList = erpInfoDatas.Where(p => !p.FactoryCode.IsNullOrEmpty()).Select(p => p.FactoryCode).Distinct().ToList();
            if (factoryCodeList.Count > 0)
            {
                var factoryExp = factoryCodeList.CreateContainsExpression<Enterprise>("x", nameof(Enterprise.Code));
                dicFactory = _commonController.GetDatas(factoryExp).ToDictionary(p => p.Code);
            }
            else
                dicFactory = new Dictionary<string, Enterprise>();
            //车间集合
            List<string> workshopCodeList = erpInfoDatas.Where(p => !p.WorkshopCode.IsNullOrEmpty()).Select(p => p.WorkshopCode).Distinct().ToList();
            if (workshopCodeList.Count > 0)
            {
                var workshopExp = workshopCodeList.CreateContainsExpression<Enterprise>("x", nameof(Enterprise.Code));
                dicWorkshop = _commonController.GetDatas(workshopExp).ToDictionary(p => p.Code);
            }
            else
                dicWorkshop = new Dictionary<string, Enterprise>();
            //资源集合
            List<string> resourceList = erpInfoDatas.Where(p => !p.ResourceCode.IsNullOrEmpty()).Select(p => p.ResourceCode).Distinct().ToList();
            if (resourceList.Count > 0)
            {
                var resourceExp = resourceList.CreateContainsExpression<WipResource>("x", nameof(WipResource.Code));
                dicResource = _commonController.GetDatas(resourceExp).ToDictionary(p => p.Code);
            }
            else
                dicResource = new Dictionary<string, WipResource>();
            // Erp员工
            List<string> employeeCodeList = new List<string> { "ERP" };
            var employeeExp = employeeCodeList.CreateContainsExpression<Employee>("x", nameof(Employee.Code));
            erpEmployee = _commonController.GetDatas(employeeExp).FirstOrDefault();
        }

        /// <summary>
        /// 验证工单数据
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="dicWorkOrder">工单数据字典</param>
        /// <param name="dicItem">物料数据字典</param
        /// <param name="dicFactory">工厂数据字典</param>
        /// <param name="dicWorkshop">车间数据字典</param>
        /// <param name="dicResource">产线数据字典</param>
        /// <param name="workOrder">工单</param>
        /// <param name="product">产品</param>
        /// <param name="factory">工厂</param>
        /// <param name="workshop">车间</param>
        /// <param name="resource">资源</param>
        private void ValidateWorkOrderData(EbsWorkOrderData data, Dictionary<string, WorkOrder> dicWorkOrder, Dictionary<string, Item> dicItem, Dictionary<string, Enterprise> dicFactory, Dictionary<string, Enterprise> dicWorkshop, Dictionary<string, WipResource> dicResource, out WorkOrder workOrder, out Item product, out Enterprise factory, out Enterprise workshop, out WipResource resource)
        {
            if (data.WorkOrderNo.IsNullOrEmpty())
            {
                throw new ValidationException("工单号不能为空".L10N());
            }
            dicWorkOrder.TryGetValue(data.WorkOrderNo, out workOrder);
            if (data.WorkOrderType < 0 || data.WorkOrderType > 3)
                throw new ValidationException("工单类型错误".L10N());
            if (data.ProductCode.IsNullOrEmpty())
                throw new ValidationException("产品不能为空".L10N());
            if (!dicItem.TryGetValue(data.ProductCode, out product))
                throw new ValidationException("未找到产品{0}".L10nFormat(data.ProductCode));
            if (product.Type != ItemType.Product && product.Type != ItemType.SemiFinished)
                throw new ValidationException("产品{0}类型不为成品或半成品".L10nFormat(data.ProductCode));
            if (data.FactoryCode.IsNullOrEmpty())
                throw new ValidationException("工厂不能为空".L10N());
            if (!dicFactory.TryGetValue(data.FactoryCode, out factory))
                throw new ValidationException("未找到工厂{0}".L10nFormat(data.FactoryCode));
            if (data.WorkshopCode.IsNotEmpty() && !dicWorkshop.TryGetValue(data.WorkshopCode, out workshop))
                throw new ValidationException("未找到车间{0}".L10nFormat(data.WorkshopCode));
            else
                workshop = null;
            if (data.ResourceCode.IsNotEmpty() && !dicResource.TryGetValue(data.ResourceCode, out resource))
                throw new ValidationException("未找到资源{0}".L10nFormat(data.ResourceCode));
            else
                resource = null;
            if (data.PlanQty <= 0)
                throw new ValidationException("计划数量不能小于0！".L10N());
            if (data.OrderQty <= 0)
                throw new ValidationException("订单数量不能小于0！".L10N());
            if (data.PlanQty > data.OrderQty)
                throw new ValidationException("计划数量不能大于订单数量！".L10N());
            if (data.PlanBeginDate > data.PlanEndDate)
                throw new ValidationException("计划开始时间不能大于计划结束时间！".L10N());

        }

        /// <summary>
        /// 创建工单
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="dicItem">物料</param>
        /// <param name="makeDate">制单时间</param>
        /// <param name="product">产品</param>
        /// <param name="factory">工厂</param>
        /// <param name="workShop">车间</param>
        /// <param name="resource">资源</param>
        /// <param name="employee">员工</param>
        /// <param name="woPackageGenerator">包装规则生成器</param>
        /// <param name="labelPrintTemplates">工单打印模板</param>
        /// <param name="woRoutingGenerator">工艺路线生成器</param>
        /// <param name="workOrderRoutingLayouts">工艺路线布局</param>
        /// <returns>工单</returns>
        private WorkOrder CreateWorkOrder(EbsWorkOrderData data, Dictionary<string, Item> dicItem, DateTime makeDate, Item product, Enterprise factory, Enterprise workShop, WipResource resource, Employee employee, WoPackageGenerator woPackageGenerator, EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates, WoRoutingGenerator woRoutingGenerator, EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts)
        {
            var workOrderController = RT.Service.Resolve<WorkOrderController>();
            WorkOrder workOrder = new WorkOrder()
            {
                No = data.WorkOrderNo,
                Product = product,
                Source = SIE.Common.SourceType.External,
                State = ErpWorkOrderStateMap(data.ErpState, Core.WorkOrders.WorkOrderState.Release),
                CustomerId = data.CustomerId,
                CustomerOrderNo = data.CustomerOrderNo,
                SaleOrderNo = data.SaleOrderNo,
                PlanQty = data.PlanQty,
                OrderQty = data.OrderQty,
                PlanBeginDate = data.PlanBeginDate,
                PlanEndDate = data.PlanEndDate,
                Type = (Core.WorkOrders.WorkOrderType)data.WorkOrderType,
                Factory = factory,
                WorkShop = workShop,
                Resource = resource,
            };
            // 设置工艺路线
            CreateWorkOrderLayout(workOrder, woRoutingGenerator, workOrderRoutingLayouts);

            // 生成包装规则
            CreateWorkOrderPkgRule(workOrder, woPackageGenerator);
            // 生成打印模板
            labelPrintTemplates.Add(woPackageGenerator.GenerateProductLabelTemplate(workOrder));

            // 生成工单bom
            CreateWorkOrderBom(data, dicItem, workOrder);
            return workOrder;
        }

        /// <summary>
        /// 设置工艺路线
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="woRoutingGenerator">工艺路线生成器</param>
        /// <param name="workOrderRoutingLayouts">生成工艺路线布局</param>
        /// <exception cref="NotImplementedException"></exception>
        private void CreateWorkOrderLayout(WorkOrder workOrder, WoRoutingGenerator woRoutingGenerator, EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts)
        {
            bool hasRouting;
            // 设置工艺路线，没有工艺路线需要暂停单
            woRoutingGenerator.SetWorkOrderRouting(workOrder, workOrderRoutingLayouts, out hasRouting);
            if (!hasRouting)
            {
                workOrder.IsPause = YesNo.Yes;
                return;
            }

            // 生成工单工序清单
            woRoutingGenerator.GenerateRoutingProcesss(workOrder);

            // 生成工单工序BOM
            woRoutingGenerator.GenerateProcessBoms(workOrder);
        }

        /// <summary>
        /// 生成工单包装规则
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="woPackageGenerator">包装规则生成器</param>
        private void CreateWorkOrderPkgRule(WorkOrder workOrder, WoPackageGenerator woPackageGenerator)
        {
            woPackageGenerator.GenerateWorkOrderPackageRule(workOrder);
        }

        /// <summary>
        /// 创建工单BOM
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="workOrder">工单</param>
        private void CreateWorkOrderBom(EbsWorkOrderData data, Dictionary<string, Item> dicItem, WorkOrder workOrder)
        {
            data.BomList.GroupBy(p => p.ItemCode).ForEach(gkey =>
            {
                var itemCode = gkey.Key;
                if (!dicItem.TryGetValue(itemCode, out Item item))
                    throw new ValidationException("工单Bom未找到物料{0}".L10nFormat(itemCode));
                var boms = gkey.ToList();
                decimal singleQty = boms.Sum(p => p.SingleQty);
                decimal requireQty = boms.Sum(p => p.RequireQty);
                var bom = boms.FirstOrDefault();
                workOrder.BomList.Add(new WorkOrderBom()
                {
                    ErpKey = bom.ErpKey,
                    Item = item,
                    WorkOrder = workOrder,
                    SingleQty = singleQty,
                    RequireQty = requireQty,
                    IsRecoilItem = bom.IsRecoilItem,
                    IsVritualItem = bom.IsVritualItem,
                    IsByBill = bom.IsByBill,
                    Remark = bom.Remark,
                });
            });
        }

        /// <summary>
        /// 校验工单是否可以修改
        /// </summary>
        /// <param name="workOrder">修改工单</param>
        /// <param name="data">修改工单数据</param>
        /// <param name="product">修改工单数据产品</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        private bool WorkOrderCanEdit(WorkOrder workOrder, EbsWorkOrderData data, Item product, out string errMsg)
        {
            if (workOrder.ProductId != product.Id)
            {
                errMsg = "传入工单【{0}】产品【{1}】与存在工单【{0}】产品冲突，修改失败！".L10nFormat(data.WorkOrderNo, product.Code, workOrder.No);
                return false;
            }
            if (workOrder.State != Core.WorkOrders.WorkOrderState.Release)
            {
                errMsg = "工单{0}非发放状态，不允许{1}".L10nFormat(workOrder.No, "修改");
                return false;
            }
            if (workOrder.State == Core.WorkOrders.WorkOrderState.Producing &&
                (workOrder.PlanQty != (decimal)data.PlanQty || workOrder.OrderQty != (decimal)data.OrderQty ||
                workOrder.PlanBeginDate != data.PlanBeginDate || workOrder.PlanBeginDate != data.PlanEndDate))
            {
                errMsg = "工单【{0}】状态为生产中，不可修改数量与日期！".L10nFormat(workOrder.No);
                return false;
            }
            errMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// 更新工单
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="existboms">原工单BOM集合</param>
        /// <param name="insertboms">修改新增工单BOM集合</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="workOrder">工单</param>
        private void UpdateWorkOrder(EbsWorkOrderData data, List<WorkOrderBom> existboms, List<WorkOrderBom> insertboms, Dictionary<string, Item> dicItem, WorkOrder workOrder)
        {
            workOrder.PlanQty = data.PlanQty;
            workOrder.OrderQty = data.OrderQty;
            workOrder.PlanBeginDate = data.PlanBeginDate;
            workOrder.PlanEndDate = data.PlanEndDate;
            workOrder.State = ErpWorkOrderStateMap(data.ErpState, workOrder.State);
            UpdateWorkOrderBom(data.BomList, existboms, insertboms, dicItem, workOrder);
        }

        /// <summary>
        /// 更新工单bom(对于传入的数据，没有的需要删除)
        /// </summary>
        /// <param name="bomList">erp传入工单bom</param>
        /// <param name="existboms">原工单bom</param>
        /// <param name="insertboms">修改新增工单BOM集合</param>
        /// <param name="dicItem">物料</param>
        /// <param name="workOrder">工单</param>
        /// <returns></returns>
        private void UpdateWorkOrderBom(List<EbsWorkOrderBomData> bomList, List<WorkOrderBom> existboms, List<WorkOrderBom> insertboms, Dictionary<string, Item> dicItem, WorkOrder workOrder)
        {
            // erp主键
            var erpKeys = bomList.Select(p => p.ErpKey).ToList();
            foreach (var bomInfo in bomList)
            {
                var existBom = existboms.FirstOrDefault(p => p.ErpKey == bomInfo.ErpKey);
                if (existBom == null)
                {
                    dicItem.TryGetValue(bomInfo.ItemCode, out Item item);
                    var insertBom = new WorkOrderBom()
                    {
                        Item = item,
                        WorkOrder = workOrder,
                        RequireQty = bomInfo.RequireQty,
                        SingleQty = bomInfo.SingleQty,
                        IsRecoilItem = bomInfo.IsRecoilItem,
                        IsVritualItem = bomInfo.IsVritualItem,
                        IsByBill = bomInfo.IsByBill,
                        Remark = bomInfo.Remark,
                    };
                    insertboms.Add(insertBom);
                }
                else
                {
                    existBom.RequireQty = bomInfo.RequireQty;
                    existBom.SingleQty = bomInfo.SingleQty;
                    existBom.IsRecoilItem = bomInfo.IsRecoilItem;
                    existBom.IsVritualItem = bomInfo.IsVritualItem;
                    existBom.IsByBill = bomInfo.IsByBill;
                    existBom.Remark = bomInfo.Remark;
                }
            }
            

            // 删除工单bom
            foreach (var del in existboms.Where(p => !erpKeys.Contains(p.ErpKey)).ToList())
            {
                del.PersistenceStatus = PersistenceStatus.Deleted;
            }
        }


        /// <summary>
        /// Ebs工单状态转化
        /// </summary>
        /// <param name="ErpState">Erp状态（1未释放 3已释放 4完成 7已取消 12已关闭）</param>
        /// <param name="woState"></param>
        /// <returns></returns>
        private SIE.Core.WorkOrders.WorkOrderState ErpWorkOrderStateMap(int ErpState, SIE.Core.WorkOrders.WorkOrderState woState)
        {
            bool flag = (woState == Core.WorkOrders.WorkOrderState.Release || woState == Core.WorkOrders.WorkOrderState.Producing);
            switch (ErpState)
            {
                case 1: // 未释放
                    return SIE.Core.WorkOrders.WorkOrderState.CancelRelease;
                case 3: // 已释放
                    if (flag && ErpState == 3)
                    {
                        return SIE.Core.WorkOrders.WorkOrderState.Release;
                    }
                    else
                    {
                        return woState;
                    }
                case 4: // 完成
                    return Core.WorkOrders.WorkOrderState.Finish;
                case 7: // 已取消
                    return Core.WorkOrders.WorkOrderState.CancelRelease;
                case 12: // 已关闭
                    return Core.WorkOrders.WorkOrderState.Close;
                default:
                    return woState;
            }
        }
    }
}
