using Irony.Parsing.Construction;
using Newtonsoft.Json;
using SIE.Api;
using SIE.Core;
using SIE.Core.Logs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Release;
using SIE.EventMessages.MES.Release.ApiModel;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessTechs;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 下达/取消下达控制器类
    /// </summary>
    public class TaskReleaseController : DomainController, IPlanTaskRelease
    {
        private const string I_PLAN_TASK_RELEASE = "IPlanTaskRelease";
        private const string TASK_RELEASE_CONTROLLER = "TaskReleaseController";
        private const string ITEM_CODE_NOEXSIT = "系统不存在编码【{0}】的物料";
        #region 下达接口
        /// <summary>
        /// 计划任务下达
        /// </summary>
        /// <param name="releasePlanDatas">下达计划数据集合</param>
        /// <returns>下达结果集合</returns>
        [ApiService("计划任务下达")]
        public virtual IReadOnlyList<ReleasePlanResult> TaskRelease([ApiParameter("下达计划数据集合")] IReadOnlyList<ReleasePlanData> releasePlanDatas)
        {
            using (PerformenceTracer.Start("下达接口【TaskRelease()】总用时"))
            {
                using (DataAuth.DataAuths.LoadAll())
                {
                    SaveTaskReleaseLog(releasePlanDatas);

                    List<ReleasePlanResult> releasePlanResults;

                    TaskReleaseExecutor taskReleaseExecutor = new TaskReleaseExecutor(releasePlanDatas);

                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        releasePlanResults = taskReleaseExecutor.TaskReleasePlanResult();

                        tran.Complete();
                    }

                    SaveTaskReleaseLog(releasePlanResults);

                    return releasePlanResults;
                }
            }
        }

        /// <summary>
        /// 外部工单写入接口
        /// </summary>
        /// <param name="paramaDatas"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("外部工单写入接口")]
        [return: ApiReturn("返回结果集合")]
        public virtual IReadOnlyList<ReleasePlanResult> WorkerOrderImport([ApiParameter("参数数据集合")] IReadOnlyList<WorkerOrderImportParamas> paramaDatas)
        {
            if (!paramaDatas.Any())
            {
                throw new ValidationException("调用失败，参数为空".L10N());
            }
            if (paramaDatas.Select(p => p.ExterInvOrg).Distinct().Count() != 1)
            {
                throw new ValidationException("调用失败，不同库存组织不能同时调用".L10N());
            }
            List<ReleasePlanData> releasePlanDatas = new List<ReleasePlanData>();
            Dictionary<string, Item> itemCache = new Dictionary<string, Item>();
            Dictionary<string, WipResource> wipResourceCache = new Dictionary<string, WipResource>();
            Dictionary<string, Enterprise> workShopCache = new Dictionary<string, Enterprise>();
            Dictionary<string, Customer> customerCache = new Dictionary<string, Customer>();
            Dictionary<string, Supplier> supplierCache = new Dictionary<string, Supplier>();
            Dictionary<string, ProcessTech> processTechCache = new Dictionary<string, ProcessTech>();

            List<string> itemCodeList = new List<string>();
            List<string> wipResourceCodeList = new List<string>();
            List<string> workShopCodeList = new List<string>();
            List<string> customerCodeList = new List<string>();
            List<string> supplierCodeList = new List<string>();
            List<string> processTechCodeList = new List<string>();
            string exInvOrg = paramaDatas.First()?.ExterInvOrg;
            // 0.接口调用切换库存组织
            ChangeRTInvOrg(exInvOrg);
            //1.取出所有的编码
            GetParamaCode(paramaDatas, itemCodeList, wipResourceCodeList, workShopCodeList, customerCodeList, supplierCodeList, processTechCodeList);
            //2将所有的编码一并获取出所有的对应的实体
            GetCacheData(itemCache, wipResourceCache, workShopCache, customerCache, supplierCache, processTechCache,
                itemCodeList, wipResourceCodeList, workShopCodeList, customerCodeList, supplierCodeList, processTechCodeList);
            //3.将Code换取Ids
            foreach (var paramaData in paramaDatas)
            {
                ReleasePlanData releasePlanData = new ReleasePlanData();
                GetReleasePlanData(wipResourceCache, workShopCache, paramaData, releasePlanData);
                foreach (var detailItem in paramaData.Details)
                {
                    ReleasePlanDetail releasePlanDetail = new ReleasePlanDetail();
                    GetReleasePlanDetail(itemCache, supplierCache, processTechCache, detailItem, releasePlanDetail);
                    releasePlanData.Details.Add(releasePlanDetail);
                    foreach (var paramasBomDetail in detailItem.BomDetails)
                    {
                        var bomDetail = new BomDetail();
                        GetBomDetails(itemCache, paramasBomDetail, bomDetail);

                        releasePlanDetail.BomDetails.Add(bomDetail);
                    }
                    foreach (var jointByProduct in detailItem.JointByProducts)
                    {
                        if (itemCache[jointByProduct.ItemCode] == null)
                        {
                            throw new ValidationException(ITEM_CODE_NOEXSIT.L10nFormat(jointByProduct.ItemCode));
                        }
                        jointByProduct.ItemId = itemCache[jointByProduct.ItemCode].Id;
                        releasePlanDetail.JointByProducts.Add(jointByProduct);
                    }
                }
                releasePlanDatas.Add(releasePlanData);
            }
            return TaskRelease(releasePlanDatas);
        }

        /// <summary>
        /// 切换当前运行时库存组织
        /// </summary>
        /// <param name="exInvOrg">外部传入库存组织Id</param>
        private void ChangeRTInvOrg(string exInvOrg)
        {
            // 不传入则无需处理
            if (exInvOrg.IsNotEmpty())
            {
                var orgList = RF.GetAll<InvOrg>();
                var org = orgList.FirstOrDefault(p => p.ExternalId == exInvOrg);
                if (org != null && org.Code > 0)
                {
                    RT.InvOrg = org.Code;
                }
                else
                {
                    throw new ValidationException("不存在库存组织Id【{0}】".L10nFormat(exInvOrg));
                }
            }
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="itemCache"></param>
        /// <param name="wipResourceCache"></param>
        /// <param name="workShopCache"></param>
        /// <param name="customerCache"></param>
        /// <param name="supplierCache"></param>
        /// <param name="processTechCache"></param>
        /// <param name="itemCodeList"></param>
        /// <param name="wipResourceCodeList"></param>
        /// <param name="workShopCodeList"></param>
        /// <param name="customerCodeList"></param>
        /// <param name="supplierCodeList"></param>
        /// <param name="processTechCodeList"></param>
        private void GetCacheData(Dictionary<string, Item> itemCache, Dictionary<string, WipResource> wipResourceCache,
            Dictionary<string, Enterprise> workShopCache, Dictionary<string, Customer> customerCache, Dictionary<string, Supplier> supplierCache,
            Dictionary<string, ProcessTech> processTechCache,
            List<string> itemCodeList, List<string> wipResourceCodeList,
            List<string> workShopCodeList, List<string> customerCodeList, List<string> supplierCodeList, List<string> processTechCodeList)
        {
            if (itemCodeList.Any())
            {
                var itemList = RT.Service.Resolve<ItemController>().GetItemListByCodeNoViewProperty(itemCodeList);
                foreach (var key in itemList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    itemCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (wipResourceCodeList.Any())
            {
                var wipResourceList = RT.Service.Resolve<WipResourceController>().GetWipResourceByCodeList(wipResourceCodeList);
                foreach (var key in wipResourceList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    wipResourceCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (workShopCodeList.Any())
            {
                var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetWorkShopByCodes(workShopCodeList);
                foreach (var key in enterpriseList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    workShopCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (customerCodeList.Any())
            {
                var customerList = RT.Service.Resolve<CustomerController>().GetCustomerByCodes(customerCodeList);
                foreach (var key in customerList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    customerCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (supplierCodeList.Any())
            {
                var supplierList = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodeList);
                foreach (var key in supplierList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    supplierCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (processTechCodeList.Any())
            {
                var processTechList = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechsFromCode(processTechCodeList);
                foreach (var key in processTechList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    processTechCache.Add(key.Key, values.FirstOrDefault());
                }
            }
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="itemCache"></param>
        /// <param name="factoryCache"></param>
        /// <param name="wipResourceCache"></param>
        /// <param name="workShopCache"></param>
        /// <param name="customerCache"></param>
        /// <param name="supplierCache"></param>
        /// <param name="processTechCache"></param>
        /// <param name="itemCodeList"></param>
        /// <param name="factoryCodeList"></param>
        /// <param name="wipResourceCodeList"></param>
        /// <param name="workShopCodeList"></param>
        /// <param name="customerCodeList"></param>
        /// <param name="supplierCodeList"></param>
        /// <param name="processTechCodeList"></param>
        private void EbsGetCacheData(Dictionary<string, Item> itemCache, Dictionary<string, Enterprise> factoryCache, Dictionary<string, WipResource> wipResourceCache,
            Dictionary<string, Enterprise> workShopCache, Dictionary<string, Customer> customerCache, Dictionary<string, Supplier> supplierCache,
            Dictionary<string, ProcessTech> processTechCache,
            List<string> itemCodeList, List<string> factoryCodeList, List<string> wipResourceCodeList,
            List<string> workShopCodeList, List<string> customerCodeList, List<string> supplierCodeList, List<string> processTechCodeList)
        {
            if (itemCodeList.Any())
            {
                var itemList = RT.Service.Resolve<ItemController>().GetItemListByCodeNoViewProperty(itemCodeList);
                foreach (var key in itemList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    itemCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (factoryCodeList.Any())
            {
                var factoryList = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodeList);
                foreach (var key in factoryList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    factoryCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (wipResourceCodeList.Any())
            {
                var wipResourceList = RT.Service.Resolve<WipResourceController>().GetWipResourceByCodeList(wipResourceCodeList);
                foreach (var key in wipResourceList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    wipResourceCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (workShopCodeList.Any())
            {
                var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetWorkShopByCodes(workShopCodeList);
                foreach (var key in enterpriseList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    workShopCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (customerCodeList.Any())
            {
                var customerList = RT.Service.Resolve<CustomerController>().GetCustomerByCodes(customerCodeList);
                foreach (var key in customerList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    customerCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (supplierCodeList.Any())
            {
                var supplierList = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodeList);
                foreach (var key in supplierList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    supplierCache.Add(key.Key, values.FirstOrDefault());
                }
            }
            if (processTechCodeList.Any())
            {
                var processTechList = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechsFromCode(processTechCodeList);
                foreach (var key in processTechList.GroupBy(p => p.Code))
                {
                    var values = key.ToList();
                    processTechCache.Add(key.Key, values.FirstOrDefault());
                }
            }
        }

        /// <summary>
        /// 获取参数中带编码的字段
        /// </summary>
        /// <param name="paramaDatas"></param>
        /// <param name="itemCodeList"></param>
        /// <param name="wipResourceCodeList"></param>
        /// <param name="workShopCodeList"></param>
        /// <param name="customerCodeList"></param>
        /// <param name="supplierCodeList"></param>
        /// <param name="processTechCodeList"></param>
        private void GetParamaCode(IReadOnlyList<WorkerOrderImportParamas> paramaDatas, List<string> itemCodeList, List<string> wipResourceCodeList, List<string> workShopCodeList, List<string> customerCodeList, List<string> supplierCodeList, List<string> processTechCodeList)
        {
            foreach (var parama in paramaDatas)
            {
                if (!parama.WipResourceCode.IsNullOrEmpty())
                {
                    wipResourceCodeList.Add(parama.WipResourceCode);
                }
                if (!parama.WorkShopCode.IsNullOrEmpty())
                {
                    workShopCodeList.Add(parama.WorkShopCode);
                }
                foreach (var detail in parama.Details)
                {
                    if (!detail.CustomerCode.IsNullOrEmpty())
                    {
                        customerCodeList.Add(detail.CustomerCode);
                    }
                    if (!detail.ItemCode.IsNullOrEmpty())
                    {
                        itemCodeList.Add(detail.ItemCode);
                    }
                    if (!detail.SupplierCode.IsNullOrEmpty())
                    {
                        supplierCodeList.Add(detail.SupplierCode);
                    }
                    if (!detail.ProcessTechCode.IsNullOrEmpty())
                    {
                        processTechCodeList.Add(detail.ProcessTechCode);
                    }
                    foreach (var bomDetail in detail.BomDetails)
                    {
                        if (!bomDetail.ItemCode.IsNullOrEmpty())
                        {
                            itemCodeList.Add(bomDetail.ItemCode);
                        }
                        if (!bomDetail.MainItemCode.IsNullOrEmpty())
                        {
                            itemCodeList.Add(bomDetail.MainItemCode);
                        }
                    }
                    itemCodeList.AddRange(detail.JointByProducts.Where(jointByProduct => !jointByProduct.ItemCode.IsNullOrEmpty())
                        .Select(jointByProduct => jointByProduct.ItemCode));
                }
            }
        }

        /// <summary>
        /// 获取参数中带编码的字段
        /// </summary>
        /// <param name="paramaDatas"></param>
        /// <param name="itemCodeList"></param>
        /// <param name="factoryCodeList"></param>
        /// <param name="wipResourceCodeList"></param>
        /// <param name="workShopCodeList"></param>
        /// <param name="customerCodeList"></param>
        /// <param name="supplierCodeList"></param>
        /// <param name="processTechCodeList"></param>
        private void EbsGetParamaCode(IReadOnlyList<WorkerOrderImportParamas> paramaDatas, List<string> itemCodeList, List<string> factoryCodeList, List<string> wipResourceCodeList, List<string> workShopCodeList, List<string> customerCodeList, List<string> supplierCodeList, List<string> processTechCodeList)
        {
            foreach (var parama in paramaDatas)
            {
                if (!parama.FactoryCode.IsNullOrEmpty())
                {
                    factoryCodeList.Add(parama.FactoryCode);
                }
                if (!parama.WipResourceCode.IsNullOrEmpty())
                {
                    wipResourceCodeList.Add(parama.WipResourceCode);
                }
                if (!parama.WorkShopCode.IsNullOrEmpty())
                {
                    workShopCodeList.Add(parama.WorkShopCode);
                }
                foreach (var detail in parama.Details)
                {
                    if (!detail.CustomerCode.IsNullOrEmpty())
                    {
                        customerCodeList.Add(detail.CustomerCode);
                    }
                    if (!detail.ItemCode.IsNullOrEmpty())
                    {
                        itemCodeList.Add(detail.ItemCode);
                    }
                    if (!detail.SupplierCode.IsNullOrEmpty())
                    {
                        supplierCodeList.Add(detail.SupplierCode);
                    }
                    if (!detail.ProcessTechCode.IsNullOrEmpty())
                    {
                        processTechCodeList.Add(detail.ProcessTechCode);
                    }
                    foreach (var bomDetail in detail.BomDetails)
                    {
                        if (!bomDetail.ItemCode.IsNullOrEmpty())
                        {
                            itemCodeList.Add(bomDetail.ItemCode);
                        }
                        if (!bomDetail.MainItemCode.IsNullOrEmpty())
                        {
                            itemCodeList.Add(bomDetail.MainItemCode);
                        }
                    }
                    itemCodeList.AddRange(detail.JointByProducts.Where(jointByProduct => !jointByProduct.ItemCode.IsNullOrEmpty())
                        .Select(jointByProduct => jointByProduct.ItemCode));
                }
            }
        }

        /// <summary>
        /// 保存APS下达日志
        /// </summary>
        /// <param name="releasePlanResults">下达计划任务数据结果列表</param>
        private void SaveTaskReleaseLog(IReadOnlyList<ReleasePlanResult> releasePlanResults)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(releasePlanResults);
                var inputValue = "下达计划任务数据列表结果:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = I_PLAN_TASK_RELEASE,
                    Method = "TaskRelease",
                    ControllerName = TASK_RELEASE_CONTROLLER,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存APS下达日志
        /// </summary>
        /// <param name="releasePlanDatas">下达计划任务数据列表</param>
        private void SaveTaskReleaseLog(IReadOnlyList<ReleasePlanData> releasePlanDatas)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(releasePlanDatas);
                var inputValue = "下达计划任务数据列表:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = I_PLAN_TASK_RELEASE,
                    Method = "TaskRelease",
                    ControllerName = TASK_RELEASE_CONTROLLER,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        #endregion 下达接口

        #region 取消下达接口
        /// <summary>
        /// 计划任务取消下达
        /// </summary>
        /// <param name="cancelReleasePlanDatas">取消下达数据集合</param>
        /// <returns>取消下达结果集合</returns>
        public  virtual IReadOnlyList<ReleasePlanResult> TaskCancelRelease(IReadOnlyList<CancelReleasePlanData> cancelReleasePlanDatas)
        {
            using (PerformenceTracer.Start("取消下达接口【TaskCancelRelease()】总用时"))
            {
                using (DataAuth.DataAuths.LoadAll())
                {
                    SaveTaskCancelReleaseLog(cancelReleasePlanDatas);

                    List<ReleasePlanResult> releasePlanResults;

                    TaskCancelReleaseFacade taskCancelReleaseFacade = new TaskCancelReleaseFacade(cancelReleasePlanDatas);

                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        releasePlanResults = taskCancelReleaseFacade.TaskCancelReleasePlanResult();
                        tran.Complete();
                    }

                    SaveTaskCancelReleasePlanResult(releasePlanResults);
                    return releasePlanResults;
                }
            }
        }


        /// <summary>
        /// 保存APS取消下达日志
        /// </summary>
        /// <param name="cancelReleasePlanResults">取消下达计划任务数据结果</param>
        private void SaveTaskCancelReleasePlanResult(IReadOnlyList<ReleasePlanResult> cancelReleasePlanResults)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(cancelReleasePlanResults);
                var inputValue = "取消下达计划任务数据:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = I_PLAN_TASK_RELEASE,
                    Method = "TaskCancelRelease",
                    ControllerName = TASK_RELEASE_CONTROLLER,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存APS取消下达日志
        /// </summary>
        /// <param name="cancelReleasePlanDatas">取消下达计划任务数据</param>
        private void SaveTaskCancelReleaseLog(IReadOnlyList<CancelReleasePlanData> cancelReleasePlanDatas)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(cancelReleasePlanDatas);
                var inputValue = "取消下达计划任务数据:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = I_PLAN_TASK_RELEASE,
                    Method = "TaskCancelRelease",
                    ControllerName = TASK_RELEASE_CONTROLLER,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
        private static void GetBomDetails(Dictionary<string, Item> itemCache, WokerOrderImportParamasBomDetail paramasBomDetail, BomDetail bomDetail)
        {
            bomDetail.AttritionRate = paramasBomDetail.AttritionRate;
            bomDetail.Remark = paramasBomDetail.Remark;
            if (paramasBomDetail.ItemCode.IsNotEmpty())
            {
                if (!itemCache.TryGetValue(paramasBomDetail.ItemCode, out Item item))
                {
                    throw new ValidationException("工单Bom不存在编码【{0}】的物料".L10nFormat(paramasBomDetail.ItemCode));
                }

                bomDetail.ItemId = item.Id;
            }
            else
            {
                throw new ValidationException("工单Bom物料编码不能为空！".L10N());
            }
            if (paramasBomDetail.MainItemCode.IsNotEmpty())
            {
                if (!itemCache.TryGetValue(paramasBomDetail.MainItemCode, out Item mainItem))
                {
                    throw new ValidationException("系统不存在主料编码【{0}】！".L10nFormat(paramasBomDetail.MainItemCode));
                }
                bomDetail.MainItemId = mainItem.Id;
            }

            bomDetail.IsRecoilItem = paramasBomDetail.IsRecoilItem;
            bomDetail.CombinationGroup = paramasBomDetail.CombinationGroup;
            bomDetail.IsByBill = paramasBomDetail.IsByBill;
            bomDetail.RequireQty = paramasBomDetail.RequireQty;
            bomDetail.IsVritualItem = paramasBomDetail.IsVritualItem;
            bomDetail.ItemExtProp = paramasBomDetail.ItemExtProp;
            bomDetail.Point = paramasBomDetail.Point;
            bomDetail.LackQty = paramasBomDetail.LackQty;
            bomDetail.SingleQty = paramasBomDetail.SingleQty;
            bomDetail.ItemExtPropName = paramasBomDetail.ItemExtPropName;
        }
        private static void GetReleasePlanData(Dictionary<string, WipResource> wipResourceCache, Dictionary<string, Enterprise> workShopCache, WorkerOrderImportParamas paramaData, ReleasePlanData releasePlanData)
        {
            releasePlanData.PlanNo = paramaData.PlanNo;
            releasePlanData.CombinedOrderCode = paramaData.CombinedOrderCode;
            releasePlanData.CombinedWorkCode = paramaData.CombinedWorkCode;
            releasePlanData.MouldBarId = paramaData.MouldBarId;
            releasePlanData.MouldId = paramaData.MouldId;
            releasePlanData.IsSameMode = paramaData.IsSameMode;
            releasePlanData.PlanTaskId = paramaData.PlanTaskId;
            releasePlanData.WithOutEnterprise = paramaData.WithOutEnterprise;
            if (!paramaData.WithOutEnterprise)
            {
                if (!workShopCache.TryGetValue(paramaData.WorkShopCode, out Enterprise workShop))
                {
                    throw new ValidationException("系统不存在编码【{0}】车间".L10nFormat(paramaData.WorkShopCode));
                }
                releasePlanData.WorkShopId = workShop.Id;
                if (workShop.InvOrgId != RT.InvOrg)
                {
                    throw new ValidationException("【{0}】车间不属于库存组织外部Id【{1}】".L10nFormat(paramaData.WorkShopCode, paramaData.ExterInvOrg));
                }
                if (!wipResourceCache.TryGetValue(paramaData.WorkShopCode, out WipResource wipResource))
                {
                    throw new ValidationException("系统不存在编码【{0}】的生产资源".L10nFormat(paramaData.WipResourceCode));
                }
                releasePlanData.WipResourceId = wipResource.Id;
            }
            
        }

        private static void EbsGetReleasePlanData(Dictionary<string, Enterprise> factoryCache, Dictionary<string, WipResource> wipResourceCache, Dictionary<string, Enterprise> workShopCache, WorkerOrderImportParamas paramaData, ReleasePlanData releasePlanData)
        {
            releasePlanData.PlanNo = paramaData.PlanNo;
            releasePlanData.CombinedOrderCode = paramaData.CombinedOrderCode;
            releasePlanData.CombinedWorkCode = paramaData.CombinedWorkCode;
            releasePlanData.MouldBarId = paramaData.MouldBarId;
            releasePlanData.MouldId = paramaData.MouldId;
            releasePlanData.IsSameMode = paramaData.IsSameMode;
            releasePlanData.PlanTaskId = paramaData.PlanTaskId;
            releasePlanData.WithOutEnterprise = paramaData.WithOutEnterprise;
            if (paramaData.FactoryCode.IsNotEmpty())
            {
                if (!factoryCache.TryGetValue(paramaData.FactoryCode, out Enterprise factory))
                {
                    throw new ValidationException("系统不存在名称【{0}】工厂".L10nFormat(paramaData.FactoryCode));
                }
                releasePlanData.FactoryId = factory.Id;
            }
            else
            {
                throw new ValidationException("工厂不能为空！".L10nFormat());
            }
            if (!paramaData.WithOutEnterprise)
            {
                if (!workShopCache.TryGetValue(paramaData.WorkShopCode, out Enterprise workShop))
                {
                    throw new ValidationException("系统不存在编码【{0}】车间".L10nFormat(paramaData.WorkShopCode));
                }
                releasePlanData.WorkShopId = workShop.Id;
                if (workShop.InvOrgId != RT.InvOrg)
                {
                    throw new ValidationException("【{0}】车间不属于库存组织外部Id【{1}】".L10nFormat(paramaData.WorkShopCode, paramaData.ExterInvOrg));
                }
                if (!wipResourceCache.TryGetValue(paramaData.WorkShopCode, out WipResource wipResource))
                {
                    throw new ValidationException("系统不存在编码【{0}】的生产资源".L10nFormat(paramaData.WipResourceCode));
                }
                releasePlanData.WipResourceId = wipResource.Id;
            }

        }

        private static void GetReleasePlanDetail(Dictionary<string, Item> itemCache, Dictionary<string, Supplier> supplierCache, Dictionary<string, ProcessTech> processTechCache, WokerOrderImportParamasDetail detailItem, ReleasePlanDetail releasePlanDetail)
        {
            releasePlanDetail.PanelQty = detailItem.PanelQty;
            releasePlanDetail.PanelWorkOrderNo = detailItem.PanelWorkOrderNo;
            releasePlanDetail.PlanAmount = detailItem.PlanAmount;
            releasePlanDetail.PlanEndTime = detailItem.PlanEndTime;
            releasePlanDetail.PlanStartTime = detailItem.PlanStartTime;
            releasePlanDetail.ProcessSurface = detailItem.ProcessSurface;
            if (!detailItem.ProcessTechCode.IsNullOrEmpty())
            {
                if (processTechCache.ContainsKey( detailItem.ProcessTechCode)&&   processTechCache[detailItem.ProcessTechCode] == null)
                {
                    throw new ValidationException("计划明细不存在编码【{0}】的制程工艺".L10nFormat(detailItem.ProcessTechCode));
                }
                releasePlanDetail.ProcessTechId = processTechCache[detailItem.ProcessTechCode].Id;
            }
            releasePlanDetail.ProcessTechOrderCode = detailItem.ProcessTechOrderCode;
            releasePlanDetail.ProductionOrderCode = detailItem.ProductionOrderCode;
            releasePlanDetail.IsOutsource = detailItem.IsOutsource;
            releasePlanDetail.WorkOrder = detailItem.WorkOrder;
            releasePlanDetail.WorkOrderType = detailItem.WorkOrderType;
            releasePlanDetail.WorkOrderState = detailItem.WorkOrderState;
            releasePlanDetail.DetailId = detailItem.DetailId;
            if (!detailItem.SupplierCode.IsNullOrEmpty())
            {
                if (supplierCache[detailItem.SupplierCode] == null)
                {
                    throw new ValidationException("计划明细不存在编码【{0}】的供应商".L10nFormat(detailItem.SupplierCode));
                }
                releasePlanDetail.SupplierId = supplierCache[detailItem.SupplierCode].Id;
            }
            releasePlanDetail.SaleOrderCode = detailItem.SaleOrderCode;
            if (!detailItem.CustomerCode.IsNullOrEmpty())
            {
                if (supplierCache[detailItem.CustomerCode] == null)
                {
                    throw new ValidationException("计划明细不存在编码【{0}】的客户".L10nFormat(detailItem.CustomerCode));
                }
                releasePlanDetail.CustomerId = supplierCache[detailItem.CustomerCode].Id;
            }
            
            releasePlanDetail.IsMainItem = detailItem.IsMainItem;
            releasePlanDetail.IsPanelWorkOrder = detailItem.IsPanelWorkOrder;
            releasePlanDetail.ItemExtProp = detailItem.ItemExtProp;
            releasePlanDetail.ItemExtPropName = detailItem.ItemExtPropName;
            if (detailItem.ItemCode.IsNotEmpty())
            {
                if (!itemCache.TryGetValue(detailItem.ItemCode, out Item item))
                {
                    throw new ValidationException("计划明细不存在编码【{0}】的物料".L10nFormat(detailItem.ItemCode));
                }
                releasePlanDetail.ItemId = item.Id;
            }
            else
            {
                throw new ValidationException("计划明细物料不能为空！".L10N());
            }
            releasePlanDetail.Proportion = detailItem.Proportion;
            releasePlanDetail.BeforeProcessTechOrderCodes = detailItem.BeforeProcessTechOrderCodes;
        }

        #endregion 取消下达接口

        #region 修改工单接口
        /// <summary>
        /// 修改工单校验
        /// </summary>
        /// <param name="modifyWoDatas"></param>
        /// <returns></returns>
        public virtual IReadOnlyList<ModifyWoResult> CheckModifyWoRelease(IReadOnlyList<ModifyWoData> modifyWoDatas)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                ModifyWorkOrderValidator modifyWorkOrderValidator = new ModifyWorkOrderValidator(modifyWoDatas);
                return modifyWorkOrderValidator.ValidateModifyWoData();
            }
        }

        /// <summary>
        /// 修改工单
        /// </summary>
        /// <param name="modifyWoDatas">修改工单的数据</param>
        /// <returns></returns>
        public virtual IReadOnlyList<ModifyWoResult> ModifyWoRelease(IReadOnlyList<ModifyWoData> modifyWoDatas)
        {
            if (modifyWoDatas == null)
            {
                throw new ArgumentNullException(nameof(modifyWoDatas));
            }

            using (DataAuth.DataAuths.LoadAll())
            {
                SaveModifyWoReleaseLog(modifyWoDatas);

                //修改验证
                ModifyWorkOrderValidator modifyWorkOrderValidator = new ModifyWorkOrderValidator(modifyWoDatas);
                var result = modifyWorkOrderValidator.ValidateModifyWoData();

                if (result.All(p => p.IsSuccess == "Y"))
                {
                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        foreach (var dtl in modifyWoDatas)
                        {
                            var wo = modifyWorkOrderValidator.GetWorkOrder(dtl.WorkOrder);

                            if (wo != null)
                            {
                                UpdateWorkOrder(dtl, wo);
                            }
                        }

                        tran.Complete();
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 更新工单
        /// </summary>
        /// <param name="modifyWoData">修改工单数据</param>        
        /// <param name="wo">工单</param>
        public virtual void UpdateWorkOrder(ModifyWoData modifyWoData, WorkOrder wo)
        {
            if (modifyWoData is null)
            {
                throw new ArgumentNullException(nameof(modifyWoData));
            }

            DB.Update<WorkOrder>()
                .Set(x => x.PlanBeginDate, modifyWoData.PlanStartTime)
                .Set(x => x.PlanEndDate, modifyWoData.PlanEndTime)
                .Set(x => x.ResourceId, modifyWoData.ResourceId)
                .Where(x => x.Id == wo.Id)
                .Execute();
        }

        /// <summary>
        /// 保存工单修改接口日志
        /// </summary>
        /// <param name="modifyWoDatas">工单修改的数据</param>
        private void SaveModifyWoReleaseLog(IReadOnlyList<ModifyWoData> modifyWoDatas)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(modifyWoDatas);
                var inputValue = "修改工单数据:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = I_PLAN_TASK_RELEASE,
                    Method = "ModifyWoRelease",
                    ControllerName = TASK_RELEASE_CONTROLLER,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
        #endregion

        #region Ebs新增、修改、取消工单

        /// <summary>
        /// Ebs下达
        /// </summary>
        /// <param name="releasePlanDatas"></param>
        /// <returns></returns>
        public virtual IEbsWorkOrderResult EbsTaskRelease([ApiParameter("下达计划数据集合")] IReadOnlyList<ReleasePlanData> releasePlanDatas)
        {
            using (PerformenceTracer.Start("下达接口【TaskRelease()】总用时"))
            {
                using (DataAuth.DataAuths.LoadAll())
                {
                    SaveTaskReleaseLog(releasePlanDatas);

                    List<ReleasePlanResult> releasePlanResults;

                    // Ebs信息
                    IEbsWorkOrderResult ebsWorkOrderResult = new IEbsWorkOrderResult();
                    ebsWorkOrderResult.BeginTime = DateTime.Now;

                    TaskReleaseExecutor taskReleaseExecutor = new TaskReleaseExecutor(releasePlanDatas);

                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        releasePlanResults = taskReleaseExecutor.EbsTaskReleasePlanResult();

                        tran.Complete();
                    }

                    SaveTaskReleaseLog(releasePlanResults);

                    // 转化
                    ChangeResult(ebsWorkOrderResult, releasePlanResults);
                    return ebsWorkOrderResult;
                }
            }
        }

        private void ChangeResult(IEbsWorkOrderResult ebsWorkOrderResult, List<ReleasePlanResult> releasePlanResults)
        {
            ebsWorkOrderResult.EndTime = DateTime.Now;
            foreach (var item in releasePlanResults)
            {
                DetailDataInfo detailDataInfo = new DetailDataInfo();
                detailDataInfo.IsSuccess = item.IsSuccess;
                detailDataInfo.Infkey = "MES_WORKORDER_RELEASE";
                detailDataInfo.IsChild = false;
                detailDataInfo.IsSuccess = item.IsSuccess;
                ebsWorkOrderResult.ErpErrorDatas.Add(detailDataInfo);
                if (item.IsSuccess)
                {
                    ebsWorkOrderResult.SuccessCount += 1;
                    detailDataInfo.ErrMsg = string.Empty;
                }
                else
                {
                    ebsWorkOrderResult.FailCount += 1;
                    detailDataInfo.ErrMsg = item.Details.Count > 0 ? item.Details[0].Message : item.Message;
                }
                
            }
            ebsWorkOrderResult.DataCount = ebsWorkOrderResult.SuccessCount + ebsWorkOrderResult.FailCount;
        }

        /// <summary>
        /// Ebs工单写入接口
        /// </summary>
        /// <param name="paramaDatas"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("Ebs工单写入接口")]
        [return: ApiReturn("返回结果集合")]
        public virtual IEbsWorkOrderResult EbsOperateWorkOrder([ApiParameter("参数数据集合")] IReadOnlyList<WorkerOrderImportParamas> paramaDatas)
        {
            if (!paramaDatas.Any())
            {
                throw new ValidationException("调用失败，参数为空".L10N());
            }
            if (paramaDatas.Select(p => p.ExterInvOrg).Distinct().Count() != 1)
            {
                throw new ValidationException("调用失败，不同库存组织不能同时调用".L10N());
            }
            List<ReleasePlanData> releasePlanDatas = new List<ReleasePlanData>();
            Dictionary<string, Item> itemCache = new Dictionary<string, Item>();
            Dictionary<string, WipResource> wipResourceCache = new Dictionary<string, WipResource>();
            Dictionary<string, Enterprise> workShopCache = new Dictionary<string, Enterprise>();
            Dictionary<string, Customer> customerCache = new Dictionary<string, Customer>();
            Dictionary<string, Supplier> supplierCache = new Dictionary<string, Supplier>();
            Dictionary<string, ProcessTech> processTechCache = new Dictionary<string, ProcessTech>();
            Dictionary<string, Enterprise> factoryCache = new Dictionary<string, Enterprise>();

            List<string> itemCodeList = new List<string>();
            List<string> factoryCodeList = new List<string>();
            List<string> wipResourceCodeList = new List<string>();
            List<string> workShopCodeList = new List<string>();
            List<string> customerCodeList = new List<string>();
            List<string> supplierCodeList = new List<string>();
            List<string> processTechCodeList = new List<string>();
            string exInvOrg = paramaDatas.First()?.ExterInvOrg;
            // 0.接口调用切换库存组织
            ChangeRTInvOrg(exInvOrg);
            //1.取出所有的编码
            EbsGetParamaCode(paramaDatas, itemCodeList, factoryCodeList, wipResourceCodeList, workShopCodeList, customerCodeList, supplierCodeList, processTechCodeList);
            //2将所有的编码一并获取出所有的对应的实体
            EbsGetCacheData(itemCache, factoryCache, wipResourceCache, workShopCache, customerCache, supplierCache, processTechCache,
                itemCodeList, factoryCodeList, wipResourceCodeList, workShopCodeList, customerCodeList, supplierCodeList, processTechCodeList);
            //3.将Code换取Ids
            foreach (var paramaData in paramaDatas)
            {
                ReleasePlanData releasePlanData = new ReleasePlanData();
                EbsGetReleasePlanData(factoryCache, wipResourceCache, workShopCache, paramaData, releasePlanData);
                foreach (var detailItem in paramaData.Details)
                {
                    ReleasePlanDetail releasePlanDetail = new ReleasePlanDetail();
                    GetReleasePlanDetail(itemCache, supplierCache, processTechCache, detailItem, releasePlanDetail);
                    releasePlanData.Details.Add(releasePlanDetail);
                    foreach (var paramasBomDetail in detailItem.BomDetails)
                    {
                        var bomDetail = new BomDetail();
                        GetBomDetails(itemCache, paramasBomDetail, bomDetail);

                        releasePlanDetail.BomDetails.Add(bomDetail);
                    }
                    foreach (var jointByProduct in detailItem.JointByProducts)
                    {
                        if (itemCache[jointByProduct.ItemCode] == null)
                        {
                            throw new ValidationException(ITEM_CODE_NOEXSIT.L10nFormat(jointByProduct.ItemCode));
                        }
                        jointByProduct.ItemId = itemCache[jointByProduct.ItemCode].Id;
                        releasePlanDetail.JointByProducts.Add(jointByProduct);
                    }
                }
                releasePlanDatas.Add(releasePlanData);
            }
            return EbsTaskRelease(releasePlanDatas);
        }
        #endregion
    }
}
