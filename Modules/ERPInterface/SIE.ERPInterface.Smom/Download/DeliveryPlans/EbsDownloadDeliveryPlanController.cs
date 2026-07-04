using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Common;
using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.ShipPlan;


namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 下载发货计划控制器
    /// </summary>
    public class EbsDownloadDeliveryPlanController : DomainController
    {
        #region 销售发货
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="planDatas"></param>
        /// /// <param name="extentInvOrg">ERP库存组织Id</param>
        /// <returns></returns>
        public virtual ApiResult DownloadDeliveryPlanToBusiness(List<EbsDeliveryPlanData> planDatas, string extentInvOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.EbsApiSaveBusinessData<EbsDeliveryPlanData>(
                planDatas,
                p => this.SaveDeliveryPlans(p),
                JobType.SaleOut,
                extentInvOrg);
        }

        /// <summary>
        /// 保存库存调拨数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SaveDeliveryPlans(List<EbsDeliveryPlanData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var customerCodes = datas.Select(a => a.CustomerCode).Distinct().ToList();
            var cusDics = RT.Service.Resolve<CustomerController>().GetCustomers(customerCodes).ToDictionary(p => p.Code, p => p.Id);

            var itemCodes = datas.Select(a => a.ItemCode).Distinct().ToList();
            var itemDics = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty()).ToDictionary(p => p.Code, p => p);
            var itemIds = itemDics.Select(a => a.Value.Id).ToList();
            var secondUnits = RT.Service.Resolve<ItemUnitController>().GetAllItemUnits(itemIds);
            var planNos = datas.Select(a => a.OrderNumber).Distinct().ToList();
            var plans = RT.Service.Resolve<DeliveryPlanController>().GetDeliveryPlans(planNos);
            var saleOutPlans = plans.Where(f => f.OrderType == OrderType.SaleOut).AsEntityList();
            var erpDetails = plans.Select(a => a.ErpDetailId).ToList();
            //EntityList<ShippingOrderDetail> shippingOrderDetails = new EntityList<ShippingOrderDetail>();
            //erpDetails.SplitDataExecute(a =>
            //{
            //    shippingOrderDetails.AddRange(DB.Query<ShippingOrderDetail>().Where(f => f.ShippingOrder.OrderType == OrderType.SaleOut && f.OrderState != ShippingOrderState.Cancel && a.Contains(f.SourceKey)).ToList());
            //});

            datas.Where(f => f.LineState == 0).GroupBy(f => f.OrderNumber).ForEach(q =>
            {
                var lineNo = saleOutPlans.Any(a => a.OrderNo == q.Key) ? saleOutPlans.Where(a => a.OrderNo == q.Key).Max(a => a.LineNo) + 1 : 1;
                q.ForEach(p =>
                    {//只要可发货的明细
                        try
                        {
                            if (p.CustomerCode.IsNullOrEmpty())
                                throw new ValidationException("客户编码不能为空".L10nFormat(p.CustomerCode));
                            if (!p.CustomerCode.IsNullOrEmpty() && !cusDics.ContainsKey(p.CustomerCode))
                                throw new ValidationException("客户编码{0}不存在".L10nFormat(p.CustomerCode));

                            ValidateData(p, itemDics);
                            var item = itemDics.GetValue<Item>(p.ItemCode);
                            SetSecondQty(item, p, secondUnits);

                            DeliveryPlan plan = new DeliveryPlan();

                            bool isCreateOrUpdateData = true;//是否创建或更新数据
                            var explanDt = saleOutPlans.FirstOrDefault(a => a.OrderNo == p.OrderNumber && a.ErpDetailId == p.ErpDetailId);

                            if (explanDt == null)//当前下载的数据不存在
                            {//ERP传过来可发货的明细p, 在SMOM已经存在,则更新数据
                                if (p.ErpSplitFromDetailId.IsNotEmpty() && p.ErpSplitFromState == 1)
                                {//当前发货明细是从原有的明细拆分出来的，用当前明细的Id覆盖已经下载的Id
                                    var sourcePlan = saleOutPlans.FirstOrDefault(a => a.OrderNo == p.OrderNumber && a.ErpDetailId == p.ErpSplitFromDetailId);
                                    if (sourcePlan != null)
                                    {//找到来源的那条明细,覆盖
                                        sourcePlan.ErpDetailId = p.ErpDetailId;//来源的数据除了这个Id不能再变更
                                        sourcePlan.OrderLineNo = p.ErpLineNo;
                                        RF.Save(sourcePlan);
                                        errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                                        isCreateOrUpdateData = false;
                                    }
                                }
                            }
                            else
                            {//当前下载的数据已经存在，判断是否要做更新
                                if (explanDt.State != DeliveryState.Aduited && explanDt.State != DeliveryState.Created)
                                {//创建和审核直接覆盖数据，执行中的状态，只更新数量                                     
                                    UpdateQty(explanDt, p, errors);
                                    isCreateOrUpdateData = false;
                                }
                                else
                                {
                                    plan = explanDt;
                                }
                            }


                            if (isCreateOrUpdateData)
                            {
                                plan.RequireQty = p.RequireQty;
                                plan.OrderNo = p.OrderNumber;
                                plan.StorerCode = p.StorerCode;
                                plan.ProjectNo = p.ProjectNo;
                                plan.TaskNo = p.TaskNo;
                                plan.LotCode = p.LotCode;
                                plan.ProductBatch = p.ProductBatch;
                                plan.OrderType = OrderType.SaleOut;
                                plan.CustomerId = p.CustomerCode == null ? null : cusDics.GetValue<double?>(p.CustomerCode);
                                plan.ItemId = item.Id;
                                plan.State = DeliveryState.Aduited;
                                plan.SourceType = DeliverySourceType.Erp;
                                plan.CreateQty = p.RequireQty;
                                plan.OrderLineNo = p.ErpLineNo;
                                plan.ErpOrderId = p.OrderId;
                                plan.ErpOrganizationName = p.OrganizationName;
                                plan.ErpOrgName = p.OrgName;
                                plan.ErpDetailId = p.ErpDetailId;
                                plan.ErpWoNo = p.WorkOrderNo;
                                plan.ScheduleShipDate = p.ScheduleShipDate;
                                plan.NoCreateQty = p.RequireQty;
                                if (plan.PersistenceStatus == PersistenceStatus.New)
                                {
                                    plan.LineNo = lineNo;
                                    lineNo++;
                                    //确定要创建数据，看下单号是否被占用
                                    if (plans.Any(a => a.No == p.OrderNumber))
                                        plan.No = p.OrderNumber + "_ERP";
                                    else
                                        plan.No = p.OrderNumber;
                                    plan.CreateDate = p.CreateDate;
                                    plan.GenerateId();
                                    if (DateTime.TryParse(p.ScheduleShipDate, out DateTime dt))
                                        plan.DeliveryDate = dt;
                                    else
                                        plan.DeliveryDate = DateTime.Now;
                                    var rule = RT.Service.Resolve<AssignWarehouseRuleController>().GetAssignWarehouseRuleData(plan.OrderType, item, plan.CustomerId, plan.SupplierId, plan.EnterpriseId, plan.ResourceId);
                                    if (rule != null)
                                    {
                                        plan.WarehouseId = rule.WarehouseId;
                                    }
                                    else
                                        throw new ValidationException("单号{0}明细行{1}仓库分配失败，请检查仓库分配规则".L10nFormat(p.OrderNumber, p.ErpLineNo));
                                }

                                RF.Save(plan);
                                errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                            }

                        }
                        catch (Exception ex)
                        {
                            errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.ErpDetailId });
                        }
                    });
            });
            HandleCancelOrderDtls(datas, saleOutPlans, errors);
            return errors;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 更新数量
        /// </summary>      
        private void UpdateQty(DeliveryPlan explanDt, EbsDeliveryPlanData p, List<ErpErrorData> errors)
        {
            //创建和审核直接覆盖数据，执行中的状态，只更新数量
            if (explanDt.State != DeliveryState.Executing)
                errors.Add(new ErpErrorData() { ErrMsg = "单据{0}行号{1}ERP主键{2},状态已经变更，不再更新数据".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
            var mQty = p.RequireQty - explanDt.RequireQty;//差值
            if (mQty > 0)
            {//,例如原来是80，现在ERP改成100，差值就是20
                explanDt.CreateQty += mQty;
                explanDt.RequireQty += mQty;
                explanDt.NoCreateQty += mQty;
                errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
            }
            else
            { //,例如原来是80，现在ERP改成60，差值就是-20，要看未建单数够不够扣
                if (explanDt.NoCreateQty + mQty < 0)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = "单据{0}行号{1}ERP主键{2},已创单数量{3}大于当前需求数{4}".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId, explanDt.RequireQty - explanDt.NoCreateQty, p.RequireQty), Infkey = p.ErpDetailId });
                }
                else
                {
                    explanDt.CreateQty += mQty;
                    explanDt.RequireQty += mQty;
                    explanDt.NoCreateQty += mQty;
                    errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                }
            }
            if (explanDt.RequireQty <= explanDt.DeliveryQty)
                explanDt.State = DeliveryState.Finished;
            RF.Save(explanDt);
        }

        /// <summary>
        /// 验证数据
        /// </summary>       
        private void ValidateData(EbsDeliveryPlanData p, Dictionary<string, Item> itemDics)
        {
            if (p.OrderNumber.IsNullOrEmpty())
                throw new ValidationException("单号不能为空".L10N());
            if (p.RequireQty <= 0)
                throw new ValidationException("需求数量必须大于0".L10N());
            if (p.ItemCode.IsNullOrEmpty())
                throw new ValidationException("物料不能为空".L10N());
            if (!p.ItemCode.IsNullOrEmpty() && !itemDics.ContainsKey(p.ItemCode))
                throw new ValidationException("物料{0}不存在".L10nFormat(p.ItemCode));
            if (p.UnitCode.IsNullOrEmpty())
                throw new ValidationException("单位不能为空".L10N());
        }

        /// <summary>
        /// 设置辅助单位数量
        /// </summary>        
        private void SetSecondQty(Item item, EbsDeliveryPlanData p, EntityList<SIE.Items.ItemUnit> secondUnits)
        {
            if (item.UnitCode.ToUpper() != p.UnitCode.ToUpper())
            {//不是物料的主单位，找是否有辅助单位对应
                var secondUnit = secondUnits.FirstOrDefault(a => a.UnitCode.ToUpper() == p.UnitCode.ToUpper() && a.MainUnitId == item.UnitId && (a.ItemId == item.Id || a.IsBaseUnit));
                if (secondUnit == null)
                    throw new ValidationException("ERP物料{0}单位{1}跟MOM的单位{2}不一致，而且在单位转换中找不到记录".L10nFormat(item.Code, p.UnitCode, item.UnitName));
                var den = ((decimal)secondUnit.Numerator / (decimal)secondUnit.Denominator);
                var changeQty = RT.Service.Resolve<ItemUnitController>().TrancateTradeQty(p.RequireQty / den, item.UnitPrecision, item.UnitTradeType);
                p.RequireQty = changeQty;
            }
        }

        /// <summary>
        /// 处理ERP取消的单据明细
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="plans"></param>
        /// <param name="errors"></param>
        private void HandleCancelOrderDtls(List<EbsDeliveryPlanData> datas, EntityList<DeliveryPlan> plans, List<ErpErrorData> errors)
        {
            datas.Where(f => f.LineState == 1).ForEach(p =>
            {//只取消审核状态的单据
                var existsplan = plans.FirstOrDefault(a => a.ErpDetailId == p.ErpDetailId);
                if (existsplan != null)
                {
                    if (existsplan.State == DeliveryState.Aduited)
                    {
                        existsplan.State = DeliveryState.Cancel;
                        existsplan.Remark = "ERP取消||" + existsplan.Remark;
                        RF.Save(existsplan);
                        errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                    }
                    else
                    {
                        errors.Add(new ErpErrorData() { ErrMsg = "取消失败:SMOM明细行状态已发生变更单号{0}ERP主键{2}".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
                    }
                }

            });
        }

        #endregion

        #region 工单发料
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="planDatas"></param>
        /// /// <param name="extentInvOrg">ERP库存组织Id</param>
        /// <returns></returns>
        public virtual ApiResult DownloadSaveWrokFeedToBusiness(List<EbsDeliveryPlanData> planDatas, string extentInvOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.EbsApiSaveBusinessData<EbsDeliveryPlanData>(
                planDatas,
                p => this.SaveWrokFeed(p),
                JobType.WorkFeed,
                extentInvOrg);
        }

        /// <summary>
        /// 保存库存调拨数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SaveWrokFeed(List<EbsDeliveryPlanData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var enterCodes = datas.Select(a => a.EnterpriseCode).Distinct().ToList();
            var enterDics = RT.Service.Resolve<EnterpriseController>().GetEnterprises(enterCodes).ToDictionary(p => p.Code, p => p.Id);
            var itemCodes = datas.Select(a => a.ItemCode).Distinct().ToList();
            var itemDics = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty()).ToDictionary(p => p.Code, p => p);
            var itemIds = itemDics.Select(a => a.Value.Id).ToList();
            var secondUnits = RT.Service.Resolve<ItemUnitController>().GetAllItemUnits(itemIds);
            var planNos = datas.Select(a => a.OrderNumber).Distinct().ToList();
            var plans = RT.Service.Resolve<DeliveryPlanController>().GetDeliveryPlans(planNos);
            var workFeedPlans = plans.Where(f => f.OrderType == OrderType.WorkFeed).AsEntityList();
            datas.Where(f => f.LineState == 0).GroupBy(f => f.OrderNumber).ForEach(f =>
            {
                var lineNo = workFeedPlans.Any(a => a.OrderNo == f.Key) ? workFeedPlans.Where(a => a.OrderNo == f.Key).Max(a => a.LineNo) + 1 : 1;
                f.ForEach(p =>
                {//只要可发货的明细
                    try
                    {
                        if (p.EnterpriseCode.IsNotEmpty() && !enterDics.ContainsKey(p.EnterpriseCode))
                            throw new ValidationException("生产部门{0}不存在".L10nFormat(p.EnterpriseCode));

                        if (p.WorkOrderNo.IsNullOrEmpty())
                            throw new ValidationException("工单号不能为空".L10N());

                        ValidateData(p, itemDics);
                        var item = itemDics.GetValue<Item>(p.ItemCode);
                        SetSecondQty(item, p, secondUnits);

                        DeliveryPlan plan = new DeliveryPlan();

                        bool isCreateOrUpdateData = true;//是否创建或更新数据
                        var explanDt = workFeedPlans.FirstOrDefault(a => a.OrderNo == p.OrderNumber && a.ErpDetailId == p.ErpDetailId);

                        if (explanDt != null)//当前下载的数据已存在                   
                        {//当前下载的数据已经存在，判断是否要做更新
                            if (explanDt.State != DeliveryState.Aduited && explanDt.State != DeliveryState.Created)
                            //创建和审核直接覆盖数据，执行中的状态，只更新数量
                            {
                                UpdateQty(explanDt, p, errors);
                                isCreateOrUpdateData = false;
                            }
                            else
                                plan = explanDt;
                        }

                        if (isCreateOrUpdateData)
                        {
                            plan.RequireQty = p.RequireQty;
                            plan.OrderNo = p.OrderNumber;
                            plan.StorerCode = p.StorerCode;
                            plan.ProjectNo = p.ProjectNo;
                            plan.TaskNo = p.TaskNo;
                            plan.LotCode = p.LotCode;
                            plan.ProductBatch = p.ProductBatch;
                            plan.OrderType = OrderType.WorkFeed;
                            plan.EnterpriseId = p.EnterpriseCode == null ? null : enterDics.GetValue<double?>(p.EnterpriseCode);
                            plan.ItemId = item.Id;
                            plan.State = DeliveryState.Aduited;
                            plan.SourceType = DeliverySourceType.Erp;
                            plan.CreateQty = p.RequireQty;
                            plan.OrderLineNo = p.ErpLineNo;
                            plan.ErpOrderId = p.OrderId;
                            plan.ErpOrganizationName = p.OrganizationName;
                            plan.ErpOrgName = p.OrgName;
                            plan.ErpDetailId = p.ErpDetailId;
                            plan.ErpWoNo = p.WorkOrderNo;
                            plan.ScheduleShipDate = p.ScheduleShipDate;
                            plan.NoCreateQty = p.RequireQty;
                            if (plan.PersistenceStatus == PersistenceStatus.New)
                            {
                                plan.LineNo = lineNo;
                                lineNo++;
                                //确定要创建数据，看下单号是否被占用
                                if (plans.Any(a => a.No == p.OrderNumber))
                                    plan.No = p.OrderNumber + "_ERP";
                                else
                                    plan.No = p.OrderNumber;
                                plan.CreateDate = p.CreateDate;
                                plan.GenerateId();
                                if (DateTime.TryParse(p.ScheduleShipDate, out DateTime dt))
                                    plan.DeliveryDate = dt;
                                else
                                    plan.DeliveryDate = DateTime.Now;

                                var rule = RT.Service.Resolve<AssignWarehouseRuleController>().GetAssignWarehouseRuleData(plan.OrderType, item, plan.CustomerId, plan.SupplierId, plan.EnterpriseId, plan.ResourceId);
                                if (rule != null)
                                {
                                    plan.WarehouseId = rule.WarehouseId;
                                }
                                else
                                    throw new ValidationException("单号{0}明细行{1}仓库分配失败，请检查仓库分配规则".L10nFormat(p.OrderNumber, p.ErpLineNo));

                            }

                            RF.Save(plan);
                            errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                        }

                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.ErpDetailId });
                    }
                });
            });
            HandleCancelOrderDtls(datas, workFeedPlans, errors);
            return errors;
        }
        #endregion
    }
}
