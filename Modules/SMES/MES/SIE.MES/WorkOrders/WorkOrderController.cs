using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Scripting.Utils;
using NPOI.POIFS.FileSystem;
using SIE.Barcodes;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.WorkOrders;
using SIE.EventMessages.APS.PlanTasks;
using SIE.EventMessages.MES.Dispatchs;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.ProcessProperty;
using SIE.MES.Routings.RoutingBoms;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders._Routing_;
using SIE.MES.WorkOrders.Configs;
using SIE.MES.WorkOrders.Events;
using SIE.MES.WorkOrders.Models;
using SIE.MES.WorkOrders.WorkOrderPackageGenerators;
using SIE.MES.WorkOrders.WorkOrderProcessBomGenerators;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Item = SIE.Items.Item;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单控制器
    /// </summary>
    public partial class WorkOrderController : DomainController
    {
        #region 单位耗用量向上取整配置表

        /// <summary>
        /// 单位耗用量向上取整配置表查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SingleQtyRoundUp> CriteriaSingleQtyRoundUp(SingleQtyRoundUpCriteria criteria)
        {
            var q = Query<SingleQtyRoundUp>();
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.Item.Name.Contains(criteria.ItemName));
            if (criteria.ItemType != null)
                q.Where(p => p.Item.Type == criteria.ItemType);
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var itemIds = list.Select(p => p.ItemId).Distinct().ToList();
            //这边只用到库存类别
            var itemCategoryRelations = RT.Service.Resolve<SIE.Items.ItemController>().GetItemCategoryRelationByCategoryTypes(itemIds, Items.Items.CategoryType.Item);

            foreach (var l in list)
            {
                var itemCategoryRelation = itemCategoryRelations.FirstOrDefault(p => p.ItemId == l.ItemId);
                if (itemCategoryRelation != null)
                {
                    l.ItemCategoryCode = l.ItemCategoryCode;
                    l.ItemCategoryName = l.ItemCategoryName;
                }
            }
            return list;
        }

        /// <summary>
        /// 根据物料ID获取单位耗用量向上取整配置表
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<SingleQtyRoundUp> GetSingleQtyRoundUpsByItemIds(List<double> itemIds)
        {
            var list = itemIds.SplitContains(ids =>
            {
                return Query<SingleQtyRoundUp>().Where(p => ids.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        #endregion

        #region 工单工序BOM

        /// <summary>
        /// 更新工单工序BOM取样净重
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Weight"></param>
        public virtual void UpdateWorkProcessBomWeight(double id,decimal Weight)
        {
            //记录下原来的，然后更新的
            DB.Update<WorkOrderProcessBom>().Set(p => p.Weight, Weight).Where(p => p.Id == id).Execute();
        }

        #endregion

        #region 打开工单

        /// <summary>
        /// 打开工单
        /// </summary>
        /// <param name="workOrderId"></param>
        public virtual void OpenWorkOrder(double workOrderId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (wo != null && wo.ClosingState != null)
            {
                wo.IsPause = YesNo.No;
                wo.State = wo.ClosingState.Value;
                wo.ClosingState = null;
                wo.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(wo);
            }
        }

        #endregion

        /// <summary>
        /// 生成任务单
        /// </summary>
        /// <param name="workOrderIds"></param>
        public virtual void GeneraDispatchTask(List<double> workOrderIds)
        {
            RT.Service.Resolve<IDispatchs>().GenerateTaskByWorkOrderIds(workOrderIds);
        }

        /// <summary>
        /// 根据工单Id获取工艺路线信息
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<LayoutInfo> GetLayoutInfosByWorkOrderId(double workOrderId)
        {
            var list = Query<LayoutInfo>().Where(p => p.WorkOrderId == workOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据工单Id获取工艺路线信息
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<LayoutInfo> GetLayoutInfosByWorkOrderId(List<double> workOrderIds)
        {
            var list = workOrderIds.SplitContains(ids =>
            {
                return Query<LayoutInfo>().Where(p => ids.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取工单号
        /// </summary>
        /// <returns>工单号</returns>
        public virtual string GetWorkOrderNo()
        {
            var config = ConfigService.GetConfig(new WorkOrderNoConfig(), typeof(WorkOrder));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到工单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取工单号
        /// </summary>
        /// <returns>工单号</returns>
        public virtual string[] GetWorkOrderNos(int count)
        {
            var config = ConfigService.GetConfig(new WorkOrderNoConfig(), typeof(WorkOrder));
            if (config == null || config.BacodeRule == null)
            {
                throw new ValidationException("未找到工单号生成规则,请检查规则配置".L10N());
            }

            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule.Id, count).ToArray();
        }

        /// <summary>
        /// 修改工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="template">打印模板</param>
        /// <param name="IsWeb">是否BS调用</param>
        /// <param name="isErp">ERP过来的工单</param>
        public virtual void UpdateWorkOrder(WorkOrder workOrder, LabelPrintTemplate template, bool IsWeb = false, bool isErp = false)
        {
            var oldWorkOrder = GetById<WorkOrder>(workOrder.Id);
            if (oldWorkOrder.IsPause != YesNo.Yes && !isErp)
            {
                throw new ValidationException("工单:{0} 非暂停状态,不允许修改".L10nFormat(oldWorkOrder.No));
            }

            if (workOrder.Version == null || workOrder.Version.Layout == null)
            {
                throw new ValidationException("工单工艺路线版本不能为空".L10N());
            }

            if (workOrder.RoutingProcessList.Count == 0)
            {
                throw new ValidationException("工单工艺路线工序为空，请检查工艺路线".L10N());
            }

            if (!workOrder.RoutingProcessList.Any(p => p.ParameterList.Count > 0))
            {
                throw new ValidationException("工单工艺路线工序参数为空，请检查工艺路线".L10N());
            }

            //是否重新生成任务单
            bool reGenerateTask = false;

            //是否更新任务单
            bool updateTask = false;

            //工序BOM已变更
            bool processBomDirty = false;

            //工艺路线版本修改或工单计划数量修改，且询问用户是否重新生成任务单，用户选择【是】（workOrder.IsReGenerateTask）
            if ((oldWorkOrder.VersionId != workOrder.VersionId || oldWorkOrder.PlanQty != workOrder.PlanQty) && workOrder.IsReGenerateTask)
            {
                reGenerateTask = true;
            }
            else
            {
                if (oldWorkOrder.ResourceId != workOrder.ResourceId || oldWorkOrder.WorkShopId != workOrder.WorkShopId
                || oldWorkOrder.PlanBeginDate != workOrder.PlanBeginDate || oldWorkOrder.PlanEndDate != workOrder.PlanEndDate
                || workOrder.ProcessBomList.IsDirty)
                {
                    updateTask = true;
                    if (workOrder.ProcessBomList.IsDirty)
                    {
                        processBomDirty = true;
                    }
                }
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (workOrder.Layout == null && workOrder.VersionId.HasValue && workOrder.Version != null)
                {
                    var layout = new WorkOrderRoutingLayout();
                    layout.Layout = workOrder.Version.Layout.Layout;
                    layout.PersistenceStatus = PersistenceStatus.New;
                    RF.Save(layout);
                    workOrder.Layout = layout;
                }

                if (template != null)
                {
                    template.PersistenceStatus = template.PersistenceStatus != PersistenceStatus.New ? PersistenceStatus.Modified : PersistenceStatus.New;

                    RF.Save(template);
                    workOrder.TemplateId = template.Id;
                }
                workOrder.PersistenceStatus = PersistenceStatus.Modified;
                if (IsWeb && workOrder.PackageRuleDetailList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
                {
                    oldWorkOrder.PackageRuleDetailList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                    RF.Save(oldWorkOrder);
                }
                if (IsWeb)
                {
                    double[] routingProcessIds = workOrder.RoutingProcessList.Select(p => p.Id).ToArray();
                    DB.Delete<WorkOrderRoutingProcess>().Where(p => p.WorkOrderId == workOrder.Id && !routingProcessIds.Contains(p.Id)).Execute();
                }
                // 如果制程工艺发生改变，需要清空原工序bom
                DB.Delete<WorkOrderProcessBom>().Where(p => p.WorkOrderId == workOrder.Id).Execute();
                foreach (var item in workOrder.BomList)
                {
                    if (oldWorkOrder.BomList.FindIndex(m => m.Id == item.Id) >= 0)
                    {
                        item.PersistenceStatus = PersistenceStatus.Modified;
                    }
                }
                RF.Save(workOrder);

                var attacWoWipBatch = workOrder.GetProperty(WoWipBatchExt.AttacWoWipBatchProperty);
                if (attacWoWipBatch != null)
                {
                    RF.Save(attacWoWipBatch);
                }
                tran.Complete();
            }

            if (reGenerateTask)
            {
                //发布工单创建事件，订阅端目前只有派工管理工程，会重新创建任务单
                RT.EventBus.Publish(new WorkOrderEditEvent() { WorkOrderId = workOrder.Id });
            }
            else
            {
                if (updateTask)
                {
                    //发布工单修改更新工单任务单事件，订阅端目前只有派工管理工程，会更新任务单
                    RT.EventBus.Publish(new WorkOrderUpdateDispathTaskEvent()
                    {
                        WorkOrderId = workOrder.Id,
                        ProcessBomDirty = processBomDirty
                    });
                }
            }
        }

        /// <summary>
        /// 验证物料扩展属性是否为空
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="isWoBomOrProcessBom">-1代表是工单主体验证 0代表工单BOM验证 1代表工序BOM</param>
        /// <exception cref="ValidationException"></exception>
        private void ValidateItemExtensionProperty(double itemId, string itemExtProp, int isWoBomOrProcessBom = -1)
        {
            var item = RF.GetById<SIE.Items.Item>(itemId);
            if (item.EnableExtendProperty && itemExtProp.IsNullOrEmpty())
            {
                var appendTip = string.Empty;
                if (isWoBomOrProcessBom == 0)
                {
                    appendTip = "工单BOM".L10N();
                }
                if (isWoBomOrProcessBom == 1)
                {
                    appendTip = "工序BOM".L10N();
                }

                var tips = appendTip + "物料[{0}]启用扩展属性，必须输入扩展属性！".L10nFormat(item.Name);
                throw new ValidationException(tips);
            }
        }
        /// <summary>
        /// 保存工单
        /// </summary>
        /// <param name="workOrder">工单(参数)</param>
        /// <param name="template">打印模板</param>
        /// <param name="wologType">工单日志操作类型</param>
        /// <param name="reason">原因</param>
        /// <returns>工单(返回)</returns>
        public virtual WorkOrder SaveWorkOrder(WorkOrder workOrder, LabelPrintTemplate template, WorkOrderLogType wologType, string reason = "")
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            //验证工单号是否重复
            WorkOrderValidator.ValidateNoIfDuplicate(new List<WorkOrder> { workOrder });

            if (workOrder.PlanEndDate < workOrder.PlanBeginDate)
            {
                throw new ValidationException("计划完成时间必须大于计划开始时间".L10N());
            }

            if (workOrder.OrderQty <= 0)
            {
                throw new ValidationException("工单订单数量必须大于0".L10N());
            }

            if (workOrder.PlanQty <= 0)
            {
                throw new ValidationException("工单计划数量必须大于0".L10N());
            }

            if (workOrder.Version == null || workOrder.Version.Layout == null)
            {
                throw new ValidationException("工单工艺路线版本不能为空".L10N());
            }

            if (!workOrder.RoutingProcessList.Any())
            {
                throw new ValidationException("工单工艺路线工序为空，请检查工艺路线".L10N());
            }

            if (workOrder.RoutingProcessList.GroupBy(x => x.Index).Any(g => g.Count() > 1))
            {
                throw new ValidationException("工单工艺路线工序存在重复，请检查工艺路线".L10N());
            }

            if (!workOrder.RoutingProcessList.Any(p => p.ParameterList.Count > 0))
            {
                throw new ValidationException("工单工艺路线工序参数为空，请检查工艺路线".L10N());
            }

            //设置包装规则后进行验证
            WorkOrderPackageRuleValidator.Validate(workOrder);

            //验证工序BOM
            WorkOrderProcessBomValidator.Validate(workOrder);

            //验证物料扩展属性
            ValidateItemExtensionProperty(workOrder.ProductId, workOrder.ItemExtProp);

            foreach (var processBom in workOrder.ProcessBomList)
            {
                ValidateItemExtensionProperty(processBom.ItemId, processBom.ItemExtProp, 1);
            }

            foreach (var bomItem in workOrder.BomList)
            {
                ValidateItemExtensionProperty(bomItem.ItemId, bomItem.ItemExtProp, 0);
                if (bomItem.RequireQty <= 0)
                {
                    throw new ValidationException("工单BOM[{0}]的需求数须大于0,请修改".L10nFormat(bomItem.Item.Name));
                }
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (workOrder.PersistenceStatus == PersistenceStatus.New && workOrder.VersionId.HasValue)
                {
                    RT.Service.Resolve<RoutingController>().UpdateVersionRefTimes(workOrder.VersionId.Value, 1);
                }


                if (workOrder.Layout == null && workOrder.VersionId.HasValue && workOrder.Version != null)
                {
                    var layout = new WorkOrderRoutingLayout();
                    layout.Layout = workOrder.Version.Layout.Layout;
                    layout.PersistenceStatus = PersistenceStatus.New;
                    RF.Save(layout);
                    workOrder.Layout = layout;
                }

                if (template == null)
                {
                    template = new Core.Items.LabelPrintTemplate();
                }

                RF.Save(template);
                workOrder.TemplateId = template.Id;
                workOrder.PersistenceStatus = PersistenceStatus.New;
                RF.Save(workOrder);

                //批次属性保存
                var attacWoWipBatch = workOrder.GetProperty(WoWipBatchExt.AttacWoWipBatchProperty);
                if (attacWoWipBatch != null)
                {
                    attacWoWipBatch.WorkOrderId = workOrder.Id;
                    RF.Save(attacWoWipBatch);
                }

                var nowTime = RF.Find<WorkOrder>().GetDbTime();
                SaveWorkOrderLog(workOrder.Id, wologType, reason, nowTime);
                tran.Complete();
            }

            RT.EventBus.Publish(new WorkOrderCreateEvent() { WorkOrderId = workOrder.Id });

            return workOrder;
        }

        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="criteria">工单查询实体</param> 
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(WorkOrderCriteria criteria)
        {
            var q = Query<WorkOrder>();
            if (criteria.WorkOrderNo.IsNotEmpty())
                q.Where(p => p.No.Contains(criteria.WorkOrderNo));
            if (criteria.CustomerOrderNo.IsNotEmpty())
                q.Where(p => p.CustomerOrderNo.Contains(criteria.CustomerOrderNo));
            if (criteria.SaleOrderNo.IsNotEmpty())
                q.Where(p => p.SaleOrderNo.Contains(criteria.SaleOrderNo));
            if (criteria.ProcessTechOrderCode.IsNotEmpty())
                q.Where(p => p.ProcessTechOrderCode.Contains(criteria.ProcessTechOrderCode));
            if (criteria.ItemCode.IsNotEmpty())
                q.Where(p => p.Product.Code.Contains(criteria.ItemCode));
            if (criteria.ItemName.IsNotEmpty())
                q.Where(p => p.Product.Name.Contains(criteria.ItemName));
            if (criteria.PlanNo.IsNotEmpty())
                q.Where(p => p.PlanNo.Contains(criteria.PlanNo));

            ////产品型号查询...
            if (!criteria.WorkShopCode.IsNullOrEmpty())
                q.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criteria.WorkShopCode.IsNotEmpty(), p => p.Code.Contains(criteria.WorkShopCode)));
            if (criteria.Workshop != null)
                q.Where(p => p.WorkShopId == criteria.WorkshopId);
            if (criteria.Resource != null)
                q.Where(p => p.ResourceId == criteria.ResourceId);

            // 由于工单表中工单状态、是否暂停是分开存储的，而查询实体里面是集中到一个字段查询，所以根据不同状态进行过滤
            if (criteria.State.IsNotEmpty())
            {
                var stateList = new List<int>();
                var pauseList = new List<int>();
                criteria.State.Split(',').ForEach(s =>
                {
                    stateList.Add(int.Parse(s));
                });

                if ((stateList.Contains(0) || stateList.Contains(1)) && criteria.IsPause != null)
                {
                    if (stateList.Contains(0))
                    {
                        stateList.RemoveAll(x => x == 0);
                        pauseList.Add(0);
                    }
                    if (stateList.Contains(1))
                    {
                        stateList.RemoveAll(x => x == 1);
                        pauseList.Add(1);
                    }
                    if (criteria.IsPause == YesNo.Yes)
                    {
                        q.Where(p => (pauseList.Contains((int)p.State) && p.IsPause == YesNo.Yes) || stateList.Contains((int)p.State));

                    }
                    else
                    {
                        q.Where(p => (pauseList.Contains((int)p.State) && p.IsPause == YesNo.No) || stateList.Contains((int)p.State));
                    }
                }
                else
                {
                    q.Where(p => stateList.Contains((int)p.State));
                }
            }

            if (criteria.State.IsNullOrEmpty() && criteria.IsPause.HasValue)
            {
                q.Where(p => p.IsPause == criteria.IsPause);
            }
            if (criteria.KitType.HasValue)
                q.Where(p => p.KitType == criteria.KitType);
            if (criteria.PlanBeginDate.BeginValue.HasValue)
                q.Where(p => p.PlanBeginDate >= criteria.PlanBeginDate.BeginValue);
            if (criteria.PlanBeginDate.EndValue.HasValue)
                q.Where(p => p.PlanBeginDate <= criteria.PlanBeginDate.EndValue);
            if (criteria.PlanEndDate.BeginValue.HasValue)
                q.Where(p => p.PlanEndDate >= criteria.PlanEndDate.BeginValue);
            if (criteria.PlanEndDate.EndValue.HasValue)
                q.Where(p => p.PlanEndDate <= criteria.PlanEndDate.EndValue);
            if (criteria.Source != null)
                q.Where(p => p.Source == criteria.Source);
            if (criteria.PanelWorkOrderNo.IsNotEmpty())
                q.Where(p => p.PanelWorkOrder.No.Contains(criteria.PanelWorkOrderNo));
            if (criteria.ProjectMaintainId != 0 && criteria.ProjectMaintainId != null)
                q.Where(p => p.ProjectMaintainId == criteria.ProjectMaintainId);
            if (criteria.ProductShortDescription.IsNotEmpty())
            {
                q.Where(p => p.Product.ShortDescription.Contains(criteria.ProductShortDescription));
            }
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            if (criteria.UpdateDate.BeginValue != null)
                q.Where(p => p.UpdateDate >= criteria.UpdateDate.BeginValue.Value);
            if (criteria.UpdateDate.EndValue != null)
                q.Where(p => p.UpdateDate <= criteria.UpdateDate.EndValue.Value);
            if (criteria.IsClose == false || criteria.IsClose == null)
                q.Where(p => p.State != WorkOrderState.Close);

            ExtensionWoCrieriaCondition(q, criteria);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">下拉查询条件</param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WorkOrder>();
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.No.Contains(keyword));
            query.Where(p => p.State != WorkOrderState.Close && p.State != WorkOrderState.Finish);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取所有工单列表
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">下拉查询条件</param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrderList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WorkOrder>();
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.No.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过资源获取工单
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">下拉查询条件</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns>根据产品获取未完成和非关闭的工单</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(PagingInfo pagingInfo, string keyword, double resourceId)
        {
            var query = Query<WorkOrder>();
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.No.Contains(keyword));
            query.Where(p => p.ResourceId == resourceId && p.State != WorkOrderState.Close && p.State != WorkOrderState.Finish);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="resourceId">产线Id</param>
        /// <param name="stateList">状态</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(double resourceId, List<int> stateList, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrder>().Where(w => w.ResourceId == resourceId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.No.Contains(keyword));
            if (stateList != null && stateList.Any())
                query.Where(p => stateList.Contains((int)p.State));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据车间和产线获取工单列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isFilterPause">是否过滤暂停</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(double? workShopId, double? resourceId, PagingInfo pagingInfo, string keyword, bool isFilterPause)
        {
            var query = Query<WorkOrder>().Where(p => p.State == WorkOrderState.Producing || p.State == WorkOrderState.Release);
            if (workShopId.HasValue)
                query.Where(p => p.WorkShopId == workShopId);
            if (resourceId.HasValue)
                query.Where(p => p.ResourceId == resourceId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.No.Contains(keyword));
            if (isFilterPause)
                query.Where(p => p.IsPause == YesNo.No);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单列表
        /// Expression不支持序列号，前端不允许调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(Expression<Func<WorkOrder, bool>> exp, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrder>();
            if (exp != null)
                query.Where(exp);
            if (pagingInfo == null)
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单集合
        /// </summary>
        /// <param name="no">工单号</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(string no, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrder>();
            if (!no.IsNullOrEmpty())
                query.Where(p => p.No.Contains(no));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取工单集合
        /// </summary>
        /// <param name="nos">工单号列表</param>        
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(List<string> nos)
        {
            return nos.SplitContains(tempNos =>
            {
                var query = Query<WorkOrder>()
                    .Where(p => tempNos.Contains(p.No));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取组合板工单
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetPanelWorkOrders(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WorkOrder>().Where(p => p.IsPanelWorkOrder);
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.No.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 工单查询扩展条件
        /// </summary>
        /// <param name="query">实体查询器</param>
        /// <param name="criteria">工单查询实体</param>
        protected virtual void ExtensionWoCrieriaCondition(IEntityQueryer<WorkOrder> query, WorkOrderCriteria criteria)
        {
        }

        /// <summary>
        /// 根据工单Id列表获取工单列表
        /// </summary>
        /// <param name="ids">工单Id列表</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrderList(List<double> ids)
        {
            return ids.SplitContains((tempIds) =>
            {
                return Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        #region 改变工单状态
        /// <summary>
        /// 验证工单改变状态的参数
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="reason">原因</param>
        /// <returns>工单</returns>
        WorkOrder ValidateChangedStateArgument(double workOrderId, string reason)
        {
            if (reason.IsNullOrWhiteSpace())
                throw new ValidationException("改变工单状态的原因不能为空".L10N());
            var workOrder = GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            return workOrder;
        }

        /// <summary>
        /// 恢复工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="reason">原因</param>
        public virtual void Resume(double workOrderId, string reason)
        {
            var workOrder = ValidateChangedStateArgument(workOrderId, reason);
            var helper = RT.Service.Resolve<WorkOrderHelper>();
            if (!helper.CanResume(workOrder))
                throw new ValidationException("不允许恢复:工单是否已暂停：{0} 状态为：{1}".L10nFormat(workOrder.IsPause.ToLabel(), workOrder.State.ToLabel()));
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                workOrder.IsPause = YesNo.No;
                RF.Save(workOrder);
                var nowTime = RF.Find<WorkOrder>().GetDbTime();
                SaveWorkOrderLog(workOrder.Id, WorkOrderLogType.Resume, reason, nowTime);
                tran.Complete();
            }

            NotifyResume(workOrder);
        }

        /// <summary>
        /// 暂停工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="reason">原因</param>
        public virtual void Pause(double workOrderId, string reason)
        {
            var workOrder = ValidateChangedStateArgument(workOrderId, reason);
            if (workOrder.IsPause == YesNo.Yes)
                throw new ValidationException("工单{0}已暂停".L10nFormat(workOrder.No));
            if (workOrder.State != WorkOrderState.Release && workOrder.State != WorkOrderState.Producing)
                throw new ValidationException("不允许暂停，工单{0}状态为{1}".L10nFormat(workOrder.No, workOrder.State.ToLabel()));
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                workOrder.IsPause = YesNo.Yes;
                //先把关闭前的状态存起来，防止后面再次打开
                workOrder.ClosingState = workOrder.State;
                RF.Save(workOrder);
                var nowTime = RF.Find<WorkOrder>().GetDbTime();
                SaveWorkOrderLog(workOrder.Id, WorkOrderLogType.Pause, reason, nowTime);
                tran.Complete();
            }

            NotifyPause(workOrder);
        }

        /// <summary>
        /// 关闭工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="reason">原因</param>
        public virtual void Close(double workOrderId, string reason)
        {
            var workOrder = ValidateChangedStateArgument(workOrderId, reason);
            var helper = RT.Service.Resolve<WorkOrderHelper>();
            if (!helper.CanClose(workOrder))
                throw new ValidationException("不允许关闭:工单是否已暂停：{0} 状态为：{1}".L10nFormat(workOrder.IsPause.ToLabel(), workOrder.State.ToLabel()));

            //存在派工任务不能关闭
            var taskInfos = RT.Service.Resolve<EventMessages.Release.IWorkOrderTask>().GetWorkOrderDispatchTasks(workOrderId);
            if (taskInfos.Exists(m => m.TaskStatus < 50))
            {
                throw new ValidationException("不允许关闭:工单存在未完成或未关闭的任务单".L10N());
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                workOrder.State = WorkOrderState.Close;
                RF.Save(workOrder);
                var nowTime = RF.Find<WorkOrder>().GetDbTime();
                SaveWorkOrderLog(workOrder.Id, WorkOrderLogType.Close, reason, nowTime);
                tran.Complete();
            }

            NotifyClose(workOrder);
        }
        #endregion

        /// <summary>
        /// 提前完工
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="reason"></param>
        public virtual void Advance(double workOrderId, string reason)
        {
            var workOrder = ValidateChangedStateArgument(workOrderId, reason);
            if (workOrder.State != WorkOrderState.Release && workOrder.State != WorkOrderState.Producing)
            {
                throw new ValidationException("工单{0}状态不为发放或生产中，不能提前完工！".L10nFormat(workOrder.No));
            }
            if (CheckProductBarcode(workOrder.Id))
            {
                throw new ValidationException("工单{0}存在已上线未完工的条码/批次，不能手工提前完工".L10nFormat(workOrder.No));
            }
            // 调用APS接口反馈工单完工状态
            var result = FeedBackSituation(workOrder.Id, workOrder.No);
            // if true 事务
            if (result != null && result.IsSuccess)
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    // 更新为已完工
                    workOrder.State = WorkOrderState.Finish;
                    RF.Save(workOrder);
                    // 更新日志
                    var nowTime = RF.Find<WorkOrder>().GetDbTime();
                    SaveWorkOrderLog(workOrder.Id, WorkOrderLogType.Finish, reason, nowTime);
                    tran.Complete();
                }
            }
            // else 报错
            else
            {
                throw new ValidationException("{0}".L10nFormat(result.Message));
            }
            NotifyClose(workOrder);
        }

        /// <summary>
        /// 校验是否存在已上线在制条码
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns>存在返回true</returns>
        private bool CheckProductBarcode(double workOrderId)
        {
            bool exists = false;
            // 生产通用报表未完工的数据为已上线在制条码
            var versionCount = Query<WipProductVersion>().Where(p => p.WorkOrderId == workOrderId && !p.IsFinish).Count();
            var batchVersionCount = Query<BatchWipProductVersion>().Where(p => p.WorkOrderId == workOrderId && !p.IsFinish).Count();
            if (versionCount + batchVersionCount > 0)
            {
                exists = true;
            }
            return exists;
        }

        /// <summary>
        /// 反馈工单完成情况
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workOrderNo"></param>
        /// <returns></returns>
        private FinishTaskDetailResult FeedBackSituation(double workOrderId, string workOrderNo)
        {
            var taskUnionDetail = Query<TaskUnionDetail>().Where(p => p.WorkOrderId == workOrderId).FirstOrDefault();

            if (taskUnionDetail != null)
            {
                var taskUnion = Query<TaskUnion>().Where(p => p.Id == taskUnionDetail.TaskUnionId).FirstOrDefault();
                List<FinishTaskData> finishTaskDatas = new List<FinishTaskData>();
                FinishTaskDetailData finishTaskDetailData = new FinishTaskDetailData
                {
                    PlanTaskDetailId = taskUnionDetail.DetailId,
                    WorkOrder = workOrderNo,
                };
                FinishTaskData finishTaskData = new FinishTaskData
                {
                    PlanTaskId = taskUnion.PlanTaskId,
                };
                finishTaskData.FinishTaskDetailDatas.Add(finishTaskDetailData);
                finishTaskDatas.Add(finishTaskData);
                var finishTaskResult = RT.Service.Resolve<IFinishPlanTask>().FinishPlanTasks(finishTaskDatas).FirstOrDefault();
                var resule = finishTaskResult?.FinishTaskDetailResults.Find(p => p.WorkOrder == workOrderNo);
                return resule;
            }
            else
            {
                throw new ValidationException("工单未找到计划任务关联明细！".L10N());
            }
        }

        /// <summary>
        /// 更新工单上线数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="qty">更新上线数量</param>
        public virtual void AddOnlineQty(double workOrderId, decimal qty)
        {
            DB.Update<WorkOrder>()
                .Set(p => p.OnlineQty, p => p.OnlineQty + qty)
                .Where(p => p.Id == workOrderId)
                .Execute();
        }

        /// <summary>
        /// 更新工单完工数量
        /// </summary>
        /// <param name="workOrderMove">过站工单</param>
        /// <param name="qty">更新完工数量</param>
        public virtual void AddFinishQty(WorkOrderMove workOrderMove, decimal qty)
        {
            if (workOrderMove.PlanQty <= workOrderMove.FinishQty+ qty)// workOrderMove.ScrapQty  csp 2024/11/15 要求完工数不包含报废数
            {
                var nowTime = RF.Find<WorkOrder>().GetDbTime();
                DB.Update<WorkOrder>()
                    .Set(p => p.FinishQty, p => p.FinishQty + qty)
                    .Set(p => p.State, WorkOrderState.Finish)
                    .Set(p => p.ActuFinishDate, nowTime)
                    .Where(p => p.Id == workOrderMove.Id)
                    .Execute();

                SaveWorkOrderLog(workOrderMove.Id, WorkOrderLogType.Finish, string.Empty, nowTime);

                RT.EventBus.Publish(new WorkOrderFinishedEvent() { WorkOrderId = workOrderMove.Id });
            }
            else
            {
                DB.Update<WorkOrder>()
                   .Set(p => p.FinishQty, p => p.FinishQty + qty)
                   .Where(p => p.Id == workOrderMove.Id)
                   .Execute();
            }
        }

        /// <summary>
        /// 更新工单报废数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="qty">更新报废数量</param>
        public virtual void AddScrapQty(double workOrderId, decimal qty)
        {
            DB.Update<WorkOrder>()
                .Set(p => p.ScrapQty, p => p.ScrapQty + qty)
                .Where(p => p.Id == workOrderId)
                .Execute();
        }

        /// <summary>
        /// 工单开始生产
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        public virtual void StartProducing(double workOrderId)
        {
            var nowTime = RF.Find<WorkOrder>().GetDbTime();
            DB.Update<WorkOrder>()
                .Set(p => p.State, WorkOrderState.Producing)
                .Set(p => p.ActuStartDate, nowTime)
                .Where(p => p.Id == workOrderId && p.State == WorkOrderState.Release)
                .Execute();

            //组合板工单上线时，更新子工单为生产中
            DB.Update<WorkOrder>()
                .Set(p => p.State, WorkOrderState.Producing)
                .Set(p => p.ActuStartDate, nowTime)
                .Where(p => p.PanelWorkOrderId == workOrderId && p.State == WorkOrderState.Release)
                .Execute();

            SaveWorkOrderLog(workOrderId, WorkOrderLogType.Producing, string.Empty, nowTime);
        }

        /// <summary>
        /// 更新工艺路线
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="layout">工艺路线布局</param>
        public virtual void UpdateLayout(double workOrderId, string layout)
        {
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                workOrder.RoutingProcessList.Clear();
                UpdateRoutingProcessList(workOrder, layout);
                ////创建已上线的未修改过产品工艺路线，的产品工艺路线信息
                CreateProductRouting(workOrder.Id, workOrder.Layout.Layout);
                workOrder.Layout.Layout = layout;
                RF.Save(workOrder.Layout);
                ////更新工序BOM
                foreach (var processBom in workOrder.ProcessBomList)
                {
                    var process = workOrder.RoutingProcessList.FirstOrDefault(p => p.ProcessId == processBom.ProcessId);
                    if (process == null)
                    {
                        processBom.PersistenceStatus = PersistenceStatus.Deleted;
                    }
                    else
                    {
                        processBom.RoutingProcessId = process.Id;
                    }

                    RF.Save(processBom);
                }

                RF.Save(workOrder);
                var nowTime = RF.Find<WorkOrder>().GetDbTime();
                SaveWorkOrderLog(workOrder.Id, WorkOrderLogType.UpdateRouting, "修改工艺路线".L10N(), nowTime);
                tran.Complete();
            }
            //推送工单变更消息到边端
            RT.EventBus.Publish(new WorkOrderInfo() { WorkOrderId = workOrder.Id, WorkOrderNo = workOrder.No, MsgType = "2" });
        }

        /// <summary>
        /// 创建产品工艺路线
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="layout">工艺路线</param>
        public virtual void CreateProductRouting(double workOrderId, string layout)
        {
            var ctl = RT.Service.Resolve<WipProductRoutingController>();
            var versions = Query<WipProductVersion>().Where(p => p.WorkOrderId == workOrderId && !p.IsFinish && p.Product.CurrentVersionId == p.Id).ToList();
            foreach (var version in versions)
            {
                var wipProductRouting = ctl.GetWipProductRouting(version.Id);
                if (wipProductRouting == null)
                {
                    var model = new ContainerModel();
                    model.Deserialize(layout);
                    UpdateProcessBoms(model, version.Product.Puid);
                    ctl.SaveProductRoutingInfo(version, layout, model.Serialize(), "工单工艺路线变更");
                }
            }
        }

        /// <summary>
        /// 更新工单工艺路线工序清单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="layout">工艺路线</param>
        void UpdateRoutingProcessList(WorkOrder workOrder, string layout)
        {
            var container = new ContainerModel();
            container.Deserialize(layout);
            container.ValidateSave();
            bool isPassRate = false;
            StringBuilder sb = new StringBuilder();
            //记录开始工序的Activity
            IActivity startActivity = null;
            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                var process = RF.GetById<Process>(activity.ProcessId);
                var mAcModel = (ActivityModel)activity;
                if (process == null && mAcModel != null && !mAcModel.IsGroup)
                {
                    throw new ValidationException("工序：{0} 不存在".L10nFormat(activity.Text));
                }
                if (isPassRate && activity.IsPassRate)
                {
                    sb.Append(activity.Text);
                    throw new ValidationException("直通率工序【{0}】最多只能勾选一个".L10nFormat(sb.ToString()));
                }
                if (activity.IsPassRate)
                {
                    isPassRate = true;
                    sb.Append(activity.Text + ",");
                }
                WorkOrderRoutingProcess routingProcess = CreateRoutingProcess(workOrder, activity, process);
                var beginRule = activity.EndRules.FirstOrDefault(p => p.BeginActivity.Type == ActivityType.Initial);
                if (beginRule != null)
                {
                    routingProcess.Sign = RoutingProcessSign.Start;
                    startActivity = activity;
                }
                var endRule = activity.BeginRules.FirstOrDefault(p => p.EndActivity.Type == ActivityType.Completion);
                if (endRule != null)
                    routingProcess.Sign |= RoutingProcessSign.End;
                if (beginRule == null && endRule == null)
                    routingProcess.Sign = RoutingProcessSign.Normal;
                foreach (var item in activity.Bom)
                {
                    var routingProcessBomConfig = new WorkOrderRoutingProcessBom()
                    {
                        ItemId = item.ItemId,
                        ProcessId = routingProcess.Id,
                        ItemExtProp = item.ItemExtProp,
                        ItemExtPropName = item.ItemExtPropName
                    };
                    routingProcess.BomConfigList.Add(routingProcessBomConfig);
                }

                workOrder.RoutingProcessList.Add(routingProcess);
            }

            //获取工序属性维护
            var processIds = workOrder.RoutingProcessList.Select(p => p.ProcessId.Value).Distinct().ToList();
            var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds);
            //获取工艺路线信息
            var layoutInfoIds = workOrder.RoutingProcessList.Select(p => p.LayoutInfoId).Distinct().ToList();
            var layoutInfos = workOrder.LayoutInfoList;//RT.Service.Resolve<WorkOrderController>().GetLayoutInfosByWorkOrderId(workOrder.Id);
            double? startProcess = null;
            double? endProcess = null;

            if (workOrder.Type == Core.WorkOrders.WorkOrderType.Rework)
            {
                //返工工单只有最后一个工序需要生成任务单不管是不是工序属性有没有配置
                //var layoutInfo = layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
                //var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode);
                //startProcess = processPty.ProcessId;
                //endProcess = processPty.ProcessId;
                //返工任务单只有最后一个工序,设置为首末工序
                //判定首工序的时候，排除PP04不参与首工序判定
                startProcess = workOrder.RoutingProcessList.Where(p => p.LayoutInfo == null || (p.LayoutInfo != null && p.LayoutInfo.Steus != "PP04")).OrderByDescending(p => p.Index).FirstOrDefault().ProcessId.Value;
                endProcess = workOrder.RoutingProcessList.OrderByDescending(p => p.Index).FirstOrDefault().ProcessId.Value;
            }
            else
            {
                //判断首工序
                //判定首工序的时候，排除PP04不参与首工序判定
                foreach (var layoutInfo in layoutInfos.Where(p=>p.Steus != "PP04").OrderBy(p => Convert.ToDecimal(p.Vornr)))
                {
                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
                    if (processPty != null)
                    {
                        startProcess = processPty.ProcessId;
                        break;
                    }
                }
                //判断尾工序
                foreach (var layoutInfo in layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)))
                {
                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
                    if (processPty != null)
                    {
                        endProcess = processPty.ProcessId;
                        break;
                    }
                }
            }
            foreach (var item in workOrder.RoutingProcessList)
            {
                item.StartProcess = startProcess;
                item.EndProcess = endProcess;
            }

            RF.Save(workOrder.RoutingProcessList);
            //更新任务单首末工序
            RT.Service.Resolve<IDispatchs>().UpdateTaskStartEndProcess(startProcess, endProcess, workOrder.Id);

            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                var routingProcess = workOrder.RoutingProcessList.FirstOrDefault(p => p.ActivityId == activity.Id);
                foreach (var rule in activity.BeginRules)
                {
                    var routingProcessParameter = new WorkOrderRoutingProcessParameter()
                    {
                        Description = rule.Text,
                        Process = routingProcess,
                        ResultType = rule.ParamResultType,
                        Expression = rule.Expression,
                        RuleId = rule.Id,
                    };
                    if (rule.EndActivity != null)
                    {
                        routingProcessParameter.NextProcess = workOrder.RoutingProcessList.FirstOrDefault(p => p.ActivityId == rule.EndActivity.Id);
                    }

                    routingProcess.ParameterList.Add(routingProcessParameter);
                }
                if (routingProcess.ParameterList.Count == 0 && (activity as ActivityModel).GroupId.IsNullOrEmpty())
                    throw new ValidationException("工单工艺路线工序参数为空，请检查工艺路线".L10N());
                RF.Save(routingProcess.ParameterList);
            }
            if (startActivity != null)
            {
                SaveNextProcess(workOrder, container, startActivity);
            }
        }

        /// <summary>
        /// 保存下一个工序信息
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="container"></param>
        /// <param name="startActivity"></param>
        /// <returns></returns>
        public virtual void SaveNextProcess(WorkOrder workOrder, ContainerModel container, IActivity startActivity)
        {
            bool isWhile = true;
            while (isWhile)
            {
                var routingProcess = workOrder.RoutingProcessList.FirstOrDefault(p => p.ActivityId == startActivity.Id);

                var rule = startActivity.BeginRules.FirstOrDefault(p => p.Text == "任意" || p.Text == "通过");
                if (rule == null || string.IsNullOrWhiteSpace(rule.EndActivity.Id) || !routingProcess.IsOptional)
                {
                    break;
                }
                var nextProcess = workOrder.RoutingProcessList.FirstOrDefault(p => p.ActivityId == rule.EndActivity.Id);
                if (nextProcess.Sign == RoutingProcessSign.Normal)
                {
                    nextProcess.Sign = RoutingProcessSign.Start;
                }
                else if (nextProcess.Sign == RoutingProcessSign.End)
                {
                    nextProcess.Sign |= RoutingProcessSign.Start;
                }
                else
                {
                    //
                }
                RF.Save(nextProcess);
                if (!nextProcess.IsOptional)
                {
                    break;
                }
                //控制取值从下一节点开始
                startActivity = container.Activitys.FirstOrDefault(p => p.Id == nextProcess.ActivityId);
                if (startActivity == null)
                {
                    isWhile = false;
                }
            }
        }

        /// <summary>
        /// 生成工单工序清单
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="activity"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public virtual WorkOrderRoutingProcess CreateRoutingProcess(WorkOrder workOrder, IActivity activity, Process process)
        {
            var result = new WorkOrderRoutingProcess()
            {
                ActivityId = activity.Id,
                LayoutInfoId = activity.LayoutInfoId,
                WorkOrder = workOrder,
                IsOptional = activity.IsOptional,
                IsRepeat = activity.IsRepeat,
                CreateSku = activity.CreateSku,
                IsCalculate = activity.IsCalculate,
                IsGenerateTask = true,//activity.IsGenerateTask,
                IsRequirementTask= activity.IsRequirementTask,
                IsBuckleMaterial = activity.IsBuckleMaterial,
                StartProcess = activity.StartProcess,
                NormalVictoryId = activity.NormalVictory,
                RepairVictoryId = activity.RepairVictory,
                IsStricter = activity.IsStricter,
                Overtime = activity.Overtime,
                IsPassRate = activity.IsPassRate,
                IsBinding = activity.IsBinding,
                IsUnBinding = activity.IsUnBinding,
                Index = activity.Index,
                Process = process,
                MaxPassNum = activity.MaxPassNum,
                IsNextMoveIn = activity.IsNextMoveIn,
                EnableMoveInControl = activity.EnableMoveInControl,
                TransferType = activity.TransferType,
                ParentNodeId = activity.ParentNodeId,
                Outsourcing=activity.IsOutsourcing,
            };
            if (process != null)
            {
                result.SegmentId = process.SegmentId;
                result.Name = process.Name;
                result.ProcessType = (ProcessType)process.Type;
            }
            var model = activity as ActivityModel;
            if (model != null)
            {
                result.IsGroup = model.IsGroup;
                result.GroupId = model.GroupId;
            }


            return result;
        }

        /// <summary>
        /// 更新产品工艺路线布局工序BOM
        /// </summary>
        /// <param name="model">容器对象</param>
        /// <param name="puid">产品Puid</param>
        void UpdateProcessBoms(ContainerModel model, string puid)
        {
            var product = RT.Service.Resolve<RuntimeController>().FindProduct(puid);
            if (product == null)
            {
                return;
            }
            foreach (var activity in model.Activitys)
            {
                var process = product.Routing.Processes.FirstOrDefault(p => p.ProcessId == activity.ProcessId);
                if (process == null)
                {
                    continue;
                }
                foreach (var bom in process.Boms)
                {
                    activity.ProcessBoms.Add(new ProcessBom()
                    {
                        ItemId = bom.ItemId,
                        Qty = bom.Qty,
                        IsBuckleMaterial = bom.IsBuckleMaterial,
                        ItemExtProp = bom.ItemExtProp,
                        ItemExtPropName = bom.ItemExtPropName,
                    });
                }
            }
        }

        /// <summary>
        /// 判断工序是否有包装权限
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="rule">规则</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool PackingUnitHasProcess(double processId, WorkOrderPackageRuleDetail rule)
        {
            if (processId == 0)
            {
                throw new ArgumentException("工序ID为空".L10N(), nameof(processId));
            }
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(processId));
            }
            return !rule.WorkOrderProcessPackingUnitList.Any()
                || rule.WorkOrderProcessPackingUnitList.Any(f => f.ProcessId == processId);
        }

        /// <summary>
        /// 获取工单中的产品
        /// </summary>
        /// <param name="workorderNo">工单号</param>
        /// <returns>产品Id列表</returns>
        public virtual List<double> GetItemIdsFromWO(string workorderNo)
        {
            if (string.IsNullOrWhiteSpace(workorderNo))
            {
                return new List<double>();
            }
            var nos = workorderNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] nolist = new string[nos.Length];
            for (int i = 0; i < nos.Length; i++)
            {
                nolist[i] = string.Format("{0}{1}{0}", "'", nos[i]);
            }

            var wo = DB.Query<WorkOrder>("w");
            var entityMeta = RF.Find<WorkOrder>().EntityMeta;
            var woorkorderList = wo.Where(p => p.SQL<bool>(new Data.FormattedSql(@"w.{0} in ({1})".FormatArgs(entityMeta.Property(WorkOrder.NoProperty).ColumnMeta.ColumnName, string.Join(",", nolist))))).ToList();
            var productIds = woorkorderList.Select(p => p.ProductId);
            List<double> itemIds = new List<double>();
            itemIds.AddRange(productIds);
            return itemIds;
        }

        /// <summary>
        /// 根据工单号获取工单
        /// </summary>
        /// <param name="no">工单号</param>
        /// <returns>工单</returns>
        public virtual WorkOrder GetWorkOrder(string no)
        {
            return Query<WorkOrder>().Where(p => p.No == no).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单Id获取工单
        /// </summary>
        /// <param name="id">工单Id</param>
        /// <returns>工单</returns>
        public virtual WorkOrder GetWorkOrder(double id)
        {
            return Query<WorkOrder>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码获取工单
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>工单</returns>
        public virtual WorkOrder GetWorkOrderBySn(string barcode)
        {
            return Query<WorkOrder>().Exists<Barcode>((a, b) => b.Where(f => f.WorkOrderId == a.Id && f.Sn == barcode)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据ERP工单号获取ERP工单
        /// </summary>
        /// <param name="no">ERP工单号</param>
        /// <returns>ERP工单</returns>
        public virtual ErpWorkOrder GetErpWorkOrder(string no)
        {
            return Query<ErpWorkOrder>().Where(p => p.No == no).FirstOrDefault();
        }

        /// <summary>
        /// 保存工单日志
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="type">操作类型</param>
        /// <param name="reason">原因</param>
        /// <param name="nowTime">时间</param>
        public virtual void SaveWorkOrderLog(double workOrderId, WorkOrderLogType type, string reason, DateTime nowTime)
        {
            WorkOrderLog log = CreateWorkOrderLog(workOrderId, type, reason, nowTime);
            RF.Save(log);
        }
        /// <summary>
        /// 创建工单日志
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="type">类型</param>
        /// <param name="reason">原因</param>
        /// <param name="nowTime">时间</param>
        /// <returns></returns>
        public virtual WorkOrderLog CreateWorkOrderLog(double workOrderId, WorkOrderLogType type, string reason, DateTime nowTime)
        {
            return new WorkOrderLog()
            {
                WorkOrderId = workOrderId,
                Reason = reason,
                OperatorId = RT.IdentityId == 0 ? RT.Service.Resolve<EmployeeController>().GetEmployeeByCode("SysAdmin").Id : RT.IdentityId,
                OperatDate = nowTime,
                Type = type,
            };
        }

        /// <summary>
        /// 根据车间获取当日产线
        /// </summary>
        /// <param name="workShopName">车间名称</param>
        /// <returns>当日产线列表</returns>
        public virtual EntityList<WorkOrder> GetCurrentDayLineList(string workShopName)
        {
            DateTime currentDay = RF.Find<WorkOrder>().GetDbTime().Date;
            var query = Query<WorkOrder>();
            query.Where(p => p.WorkShop.Name == workShopName && p.PlanBeginDate >= currentDay && p.PlanBeginDate < currentDay.AddDays(1) && p.PlanEndDate >= currentDay && p.PlanEndDate < currentDay.AddDays(1) && p.State != WorkOrderState.Close);
            return query.ToList();
        }

        /// <summary>
        /// 根据工单引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">资源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>bool</returns>
        public virtual bool WorkOrderHasUsedResourse(double id, SyncSourceType sourceType)
        {
            //根据企业模型或设备中获取资源ID
            //判断该资源ID是否有被工单使用
            var res = RT.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null)
            {
                return true;
            }
            return Query<WorkOrder>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 判断工单是否引用指定的生产资源
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns>bool: false--工单未引用生产资源；true--工单已引用生产资源</returns>
        public virtual bool WorkOrderHasUsedWipResource(double wipResourceId)
        {
            var workOrder = Query<WorkOrder>().Where(x => x.ResourceId == wipResourceId).FirstOrDefault();
            if (workOrder == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="lineId">线别ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param> 
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrder> GetWoList(double lineId, DateTime beginDate, DateTime endDate)
        {
            var result1 = Query<WorkOrder>()
                        .Where(p => (p.PlanBeginDate >= beginDate && p.PlanBeginDate <= endDate)
                            || (p.ActuStartDate >= beginDate && p.ActuStartDate <= endDate)
                            || (p.ActuStartDate <= beginDate && (p.ActuFinishDate == null || p.ActuFinishDate >= beginDate))
                            || (p.PlanBeginDate < beginDate && p.PlanEndDate > beginDate))
                        .Where(p => p.State != WorkOrderState.Close && p.ResourceId == lineId && p.ActuStartDate != null)
                        .OrderBy(p => p.ActuStartDate)
                        .ToList();
            var result2 = Query<WorkOrder>()
                        .Where(p => (p.PlanBeginDate >= beginDate && p.PlanBeginDate <= endDate)
                            || (p.ActuStartDate >= beginDate && p.ActuStartDate <= endDate)
                            || (p.PlanBeginDate < beginDate && p.PlanEndDate > beginDate))
                        .Where(p => p.ResourceId == lineId && p.State != WorkOrderState.Close && p.ActuStartDate == null)
                        .ToList();
            result1.AddRange(result2);
            return result1;
        }

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="itemId">产品Id</param>
        /// <param name="listState">工单状态列表</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWoList(double itemId, List<WorkOrderState> listState = null)
        {
            var query = Query<WorkOrder>().Where(p => p.ProductId == itemId && p.IsPause == YesNo.No);
            if (listState != null && listState.Count > 0)
            {
                List<int> stateValueList = listState.Select(p => (int)p).ToList();
                query = query.Where(p => stateValueList.Contains((int)p.State));
            }

            return query.ToList();
        }

        /// <summary>
        /// 工单准时达成率报表
        /// </summary>
        /// <param name="shopId">车间</param>
        /// <param name="resourceId">产线</param>
        /// <param name="shift">班次</param>
        /// <param name="modelId">机型</param>
        /// <param name="dateRange">日期范围</param>
        /// <param name="info">分页</param>
        /// <param name="detailRange">点击达成率的日期范围</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrderReachReportData(double? shopId, double? resourceId, Shift shift, double? modelId, DateRange dateRange = null, PagingInfo info = null, DateRange detailRange = null)
        {
            var query = Query<WorkOrder>();
            if (resourceId != null && resourceId != 0)
            {
                query.Where(p => p.ResourceId == resourceId);
            }
            else if (shopId != null)
            {
                query.Where(p => p.WorkShopId == shopId);
            }
            else
            {
                //
            }

            if (shift != null)
            {
                query.Where(p => (p.PlanBeginDate >= shift.BeginTime && p.PlanBeginDate <= shift.EndTime) || (p.PlanEndDate >= shift.BeginTime && p.PlanEndDate <= shift.EndTime) || (shift.BeginTime >= p.PlanBeginDate && shift.BeginTime <= p.PlanEndDate));
            }

            if (modelId != null)
            {
                query.Where(p => p.Product.ModelId == modelId);
            }

            if (dateRange != null && dateRange.BeginValue != null && dateRange.EndValue != null)
            {
                query.Where(p => p.PlanEndDate >= dateRange.BeginValue && p.PlanEndDate <= dateRange.EndValue);
            }

            if (detailRange != null && detailRange.BeginValue != null && detailRange.EndValue != null)
            {
                query.Where(p => p.PlanEndDate >= detailRange.BeginValue && p.PlanEndDate < detailRange.EndValue);
            }

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单工序清单
        /// </summary>
        /// <param name="routingProcessIds">工序id</param>
        /// <returns>工序清单</returns>
        public virtual EntityList<WorkOrderRoutingProcessParameter> GetWorkOrderRoutingProcessParameter(List<double> routingProcessIds)
        {
            return Query<WorkOrderRoutingProcessParameter>().Where(w => routingProcessIds.Contains(w.ProcessId)).ToList();
        }

        ///// <summary>
        ///// 获取工单工序BOM信息
        ///// </summary>
        ///// <param name="woId">工单Id</param>
        ///// <param name="pagingInfo">分页</param>
        ///// <returns>工序BOM信息</returns>
        //public virtual EntityList<WorkOrderProcessBom> GetWoProcessBom(double woId, PagingInfo pagingInfo)
        //{
        //    return Query<WorkOrderProcessBom>().Where(p => p.WorkOrderId == woId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        //}

        /// <summary>
        /// 获取工单工序BOM信息
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns>工序BOM信息</returns>
        public virtual EntityList<WorkOrderProcessBom> GetWoProcessBom(double woId)
        {
            return Query<WorkOrderProcessBom>().Where(p => p.WorkOrderId == woId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取多工单的工单BOM集合
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBomList(List<double> woIds)
        {
            return Query<WorkOrderBom>().Where(p => woIds.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单工序BOM信息
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="isFilterPull">是否过滤拉式物料</param>
        /// <returns>工序BOM信息</returns>
        public virtual EntityList<WorkOrderProcessBom> GetWoProcessBom(double woId, bool isFilterPull)
        {
            var query = Query<WorkOrderProcessBom>().Where(p => p.WorkOrderId == woId);
            if (isFilterPull)
                query.Where(p => p.Item.ConsumeMode != Items.ConsumeMode.Pull);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单Id列表获取工单工序Bom列表
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderProcessBom> GetWoProcessBomList(List<double> woIds)
        {
            return Query<WorkOrderProcessBom>().Where(p => woIds.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单需要装配物料数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>物料数量</returns>
        public virtual decimal GetWorkOrderNeetItemQty(double workOrderId, double itemId, double processId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId);
            var singleQty = GetWorkOrderBomSingleQty(workOrderId, itemId, processId);
            decimal assemblyQty = singleQty * GetAssemblyProductQty(workOrderId, itemId, processId);
            return wo.PlanQty * singleQty - assemblyQty;
        }

        /// <summary>
        /// 获取工单需要装配物料数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>BOM单机定额</returns>
        decimal GetWorkOrderBomSingleQty(double workOrderId, double itemId, double processId)
        {
            var bom = Query<WorkOrderProcessBom>().Where(p => p.WorkOrderId == workOrderId && p.ProcessId == processId && p.ItemId == itemId).FirstOrDefault();
            if (bom == null)
                return 0;
            return bom.SingleQty;
        }

        /// <summary>
        /// 获取工序已装配某物料的工单产品数量 
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>已转配产品数量</returns>
        private int GetAssemblyProductQty(double workOrderId, double itemId, double processId)
        {
            var query = Query<WipProductProcess>()
                .Exists<WipProductProcessKeyItem>((x, y) => y.Where(p => !p.IsUnbound && p.ProcessId == x.Id && p.ItemId == itemId))
                .Where(p => p.Version.WorkOrderId == workOrderId && p.ProcessId == processId);
            return query.Count();
        }

        /// <summary>
        /// 获取计划开始时间是months个月的订单
        /// </summary>
        /// <param name="workshopId">车间Id</param>
        /// <param name="dbTime">数据库时间</param>
        /// <param name="months">查询几个月的数据</param>
        /// <returns>订单数据集合</returns>
        public virtual EntityList<WorkOrder> GetMonthWorkOrders(double workshopId, DateTime dbTime, int months = 1)
        {
            var lastmonths = months - 1;
            var curMonth = DateTime.Parse(dbTime.AddMonths(-lastmonths).Year + "-" + dbTime.AddMonths(-lastmonths).Month + "-01");
            var nexMonth = curMonth.AddMonths(months);
            DateRange dr = new DateRange() { BeginValue = curMonth, EndValue = nexMonth };
            return GetWorkOrdersByDateRange(workshopId, dr);
        }

        /// <summary>
        /// 获取车间工单信息
        /// </summary>
        /// <param name="workshopId">车间Id</param>
        /// <param name="drTime">计划开始时间范围</param>
        /// <returns>工单信息集合</returns>
        public virtual EntityList<WorkOrder> GetWorkOrdersByDateRange(double workshopId, DateRange drTime)
        {
            return Query<WorkOrder>().Where(p => p.PlanBeginDate >= drTime.BeginValue && p.PlanBeginDate < drTime.EndValue
                                        && p.Resource.WorkShopId == workshopId).ToList();
        }

        /// <summary>
        /// 获取非关闭状态的工单
        /// </summary>
        /// <param name="workshopId">车间Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="drTime">时间范围</param>
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrder> GetFinishWorkOrders(double? workshopId, double? resourceId, DateRange drTime)
        {
            var query = Query<WorkOrder>().Where(p => p.State != WorkOrderState.Close && p.PlanEndDate >= drTime.BeginValue && p.PlanEndDate < drTime.EndValue);
            if (resourceId.HasValue)
            {
                query.Where(p => p.ResourceId == resourceId);
            }
            else if (workshopId.HasValue)
            {
                query.Where(p => p.WorkShopId == workshopId);
            }
            else
            {
                //
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取工单By资源
        /// </summary>
        /// <param name="resourceId">资源id</param>
        /// <param name="dr">计划时间区间</param>
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrder> GetWorkOrdersByResource(double resourceId, DateRange dr = null)
        {
            var query = Query<WorkOrder>().Where(p => p.ResourceId != null && resourceId == p.ResourceId);
            if (dr != null)
            {
                query.Where(p => (p.PlanBeginDate >= dr.BeginValue.Value && p.PlanBeginDate < dr.EndValue.Value.AddDays(1)) || (dr.BeginValue.Value >= p.PlanBeginDate && dr.BeginValue.Value <= p.PlanEndDate));
            }

            return query.ToList();
        }

        /// <summary>
        /// 根据工单Id集合获取对应的工序Id集合
        /// </summary>
        /// <param name="woIds">工单Id集合</param>
        /// <returns>工序Id集合</returns>
        public virtual List<double> GetProcessIdList(List<double> woIds)
        {
            List<double> processIds = new List<double>();
            foreach (var item in woIds)
            {
                var wo = RF.GetById<WorkOrder>(item);
                if (wo != null)
                {
                    var list = wo.RoutingProcessList
                        .Where(p => p.WorkOrderId == wo.Id && p.ProcessId != null)
                        .Select(p => p.ProcessId.Value).ToList();
                    processIds.AddRange(list);
                }
            }

            return processIds.Distinct().ToList();
        }

        /// <summary>
        /// 获取工单工序BOM物料ID集合
        /// </summary>
        /// <param name="woIds">工单ID数组</param>
        /// <returns>物料ID集合</returns>
        public virtual IList<double> GetProcessBomItemIds(double[] woIds)
        {
            return Query<WorkOrderProcessBom>()
                  .Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id && woIds.Contains(y.Id))
                  .Select(p => p.ItemId)
                  .ToList<double>();
        }

        /// <summary>
        /// 获取物料追溯方式
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>物料追溯方式</returns>
        public virtual RetrospectType? GetRetrospectType(double itemId)
        {
            return Query<ItemBatchRule>().Exists<Item>((x, y) => y.Where(p => p.Id == x.ItemId && p.Id == itemId)).FirstOrDefault()?.RetrospectType;
        }

        /// <summary>
        /// 获取共模的辅料工单(排除取消下达的工单，这些工单来源于APS取消下达遗留的工单）
        /// </summary>
        /// <param name="planNo">计划单号</param>
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrder> GetCommonModeWorkOrders(string planNo)
        {
            return Query<WorkOrder>().Where(p => p.PlanNo == planNo && p.IsCommonMode && !p.IsMainMaterial && p.State != WorkOrderState.CancelRelease).ToList();
        }

        /// <summary>
        /// 根据计划单号获取主料的工单
        /// </summary>
        /// <param name="planNo">计划单号</param>
        /// <returns>主料的工单</returns>
        public virtual WorkOrder GetMainWorkOrder(string planNo)
        {
            return Query<WorkOrder>().Where(p => p.PlanNo == planNo && p.IsCommonMode && p.IsMainMaterial && p.State != WorkOrderState.CancelRelease).FirstOrDefault();
        }

        /// <summary>
        /// 判断工单是否存在包装规则
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistPackingUnit(double workOrderId)
        {
            return RF.GetById<WorkOrder>(workOrderId)?.PackageRuleDetailList.Count > 0;
        }

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="woIds">工单IDs</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrdersByWoIds(List<double> woIds)
        {
            return woIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取工单工序清单
        /// </summary>
        /// <param name="rProcessIds">工序清单Ids</param>
        /// <returns>工单工序清单列表</returns>
        public virtual List<double> GetRoutingProcess(List<double> rProcessIds)
        {
            return Query<WorkOrderRoutingProcess>().Where(p => rProcessIds.Contains(p.Id)).Select(p => p.Id).ToList<double>().ToList();
        }

        /// <summary>
        /// 获取工单工序清单
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns>工单工序清单列表</returns>
        public virtual EntityList<WorkOrderRoutingProcess> GetRoutingProcess(double woId)
        {
            return Query<WorkOrderRoutingProcess>().Where(p => p.WorkOrderId == woId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单ID集合获取工艺工序集合
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderRoutingProcess> GetRoutingProcessByWoIds(List<double> woIds)
        {
            return Query<WorkOrderRoutingProcess>().Where(p => woIds.Contains(p.WorkOrderId))
                  .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 判断是否存在工单
        /// Expression不支持序列号，前端不允许调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public virtual bool IsExistWorkOrder(Expression<Func<WorkOrder, bool>> exp)
        {
            var query = Query<WorkOrder>();
            if (exp != null)
                query.Where(exp);
            return query.Count() > 0;
        }

        /// <summary>
        /// 创建工单
        /// </summary>
        /// <returns>工单</returns>
        public virtual WorkOrder CreateWorkOrder()
        {
            var workOrder = new WorkOrder();
            workOrder.Source = SIE.Common.SourceType.Internal;
            workOrder.State = WorkOrderState.Release;
            workOrder.KitType = null;
            workOrder.Type = WorkOrderType.Mass;
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
            workOrder.PlanBeginDate = workOrder.MakeDate.Date;
            workOrder.PlanEndDate = workOrder.MakeDate.Date;
            workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
            SetAddWorkOrderExtProperty(workOrder);
            workOrder.GenerateId();
            return workOrder;
        }

        /// <summary>
        /// 设置新增工单扩展属性信息
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void SetAddWorkOrderExtProperty(WorkOrder workOrder)
        {
        }

        /// <summary>
        /// 复制工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="oldWorkOrderId">旧工单ID</param>
        /// <returns>工单</returns>
        public virtual WorkOrder CopyWorkOrder(WorkOrder workOrder, double oldWorkOrderId)
        {
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
            workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
            workOrder.PlanBeginDate = DateTime.Parse(workOrder.MakeDate.ToShortDateString());
            workOrder.PlanEndDate = workOrder.PlanBeginDate;
            workOrder.ActuFinishDate = null;
            workOrder.ActuStartDate = null;
            //var labelTemplate = new LabelPrintTemplate();
            //labelTemplate.GenerateId();
            //workOrder.TemplateId = labelTemplate.Id;
            workOrder.GenerateId();
            SetCopyWorkOrderExtProperty(workOrder, RF.GetById<WorkOrder>(oldWorkOrderId));
            return workOrder;
        }

        /// <summary>
        /// 设置复制工单扩展属性信息
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="oldWorkOrder">旧工单</param>
        protected virtual void SetCopyWorkOrderExtProperty(WorkOrder workOrder, WorkOrder oldWorkOrder)
        {
        }

        /// <summary>
        /// 获取工单工序bom
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>工单工序bom列表</returns>
        public virtual EntityList<WorkOrderProcessBom> GetWorkOrderProcessBoms(double workOrderId, double processId)
        {
            return Query<WorkOrderProcessBom>().Where(p => p.WorkOrderId == workOrderId && p.ProcessId == processId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单工序bom
        /// </summary>
        /// <param name="workOrder"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderProcessBom> RoutingVersionChangedProcessBom(WorkOrder workOrder)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            RT.Service.Resolve<ErpWorkOrderPropertyChanged>()
                .GenerateProcessBoms(workOrder);

            return workOrder.ProcessBomList;
        }

        /// <summary>
        /// 按组合板工单ID查找工单
        /// </summary>
        /// <param name="panelWorkOrderId">组合板工单ID</param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrdersByPanelWorkId(double panelWorkOrderId)
        {
            return Query<WorkOrder>().Where(x => x.PanelWorkOrderId == panelWorkOrderId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 按组合板工单ID查找工单
        /// </summary>
        /// <param name="panelWorkOrderId">组合板工单ID</param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderMove> GetWorkOrderMovesByPanelWorkId(double panelWorkOrderId)
        {
            return Query<WorkOrderMove>().Where(x => x.PanelWorkOrderId == panelWorkOrderId)
                .ToList();
        }


        /// <summary>
        /// 获取生成批次
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns>生产批次实体</returns>
        public virtual LabelPrintTemplate GetLabelPrintTemplateOfWorkOrder(double id)
        {
            return Query<SIE.Core.Items.LabelPrintTemplate>()
                .Join<WorkOrder>((x, y) => x.Id == y.TemplateId)
                .Where<WorkOrder>((x, y) => y.Id == id)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产线ids获取工单
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetSameResourceWorkOrders(List<double> resourceIds)
        {
            List<WorkOrderState> stateList = new List<WorkOrderState> { WorkOrderState.Release, WorkOrderState.Producing };
            return resourceIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>()
                .Where(p => tempIds.Contains((double)p.ResourceId) && stateList.Contains(p.State)).ToList();
            });
        }

        /// <summary>
        /// 获取同产线的工单信息
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public virtual List<WorkOrderBaseData> GetSameResourceBaseWorkOrders(List<double> resourceIds)
        {
            List<WorkOrderState> stateList = new List<WorkOrderState>() { WorkOrderState.Release, WorkOrderState.Producing };
            List<WorkOrderBaseData> workOrderBaseDatas = new List<WorkOrderBaseData>();
            resourceIds.SplitDataExecute(tempIds =>
            {
                var list = Query<WorkOrder>()
                .Where(p => tempIds.Contains((double)p.ResourceId) && stateList.Contains(p.State))
                .Select(p => new
                {
                    p.Id,
                    p.ProductId,
                    p.PlanQty,
                })
                .ToList<WorkOrderBaseData>();
                workOrderBaseDatas.AddRange(list);
            });
            return workOrderBaseDatas;
        }

        /// <summary>
        /// 根据工单ids获取工单bom
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBomsByWoIds(List<double> woIds)
        {
            return woIds.SplitContains(tempIds =>
            {
                return Query<WorkOrderBom>().Where(p => tempIds.Contains(p.WorkOrderId)).ToList();
            });
        }

        public virtual WorkOrder GetWorkOrderNo(string No)
        {
            return Query<WorkOrder>().Where(p => p.No == No).FirstOrDefault();
        }
    }
}