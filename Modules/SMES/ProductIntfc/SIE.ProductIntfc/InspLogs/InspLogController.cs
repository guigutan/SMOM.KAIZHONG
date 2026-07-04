using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Common.SnModels;
using SIE.EventMessages.Inspection;
using SIE.EventMessages.MES.Inspection;
using SIE.MES.WorkOrders;
using SIE.ProductIntfc.Configs;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.InspSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SIE.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检日志控制器
    /// </summary>
    public class InspLogController : DomainController, IFirstInsp
    {
        /// <summary>
        /// InspLogController控制器
        /// </summary>
        private readonly string _inspLogControllerString = "InspLogController";

        /// <summary>
        /// 获取报检日志
        /// </summary>
        /// <param name="woId">工单ID</param>
        /// <param name="shopId">车间ID</param>
        /// <param name="resourceId">产线ID</param>
        /// <param name="inspType">检验类型</param>
        /// <returns>报检日志</returns>
        public virtual InspLog GetInspLog(double woId, double shopId, double resourceId, InspType inspType)
        {
            return Query<InspLog>().Where(p => p.WorkOrderId == woId && p.ShopId == shopId && p.ResourceId == resourceId && p.InspType == InspType.Product).FirstOrDefault();
        }

        /// <summary>
        /// 获取报检单列表
        /// </summary>
        /// <param name="criteria">报检单查询实体</param>
        /// <returns>报检单列表</returns>
        public virtual EntityList<InspLog> GetInspLogs(InspLogCriteria criteria)
        {
            var query = Query<InspLog>().Where(p => p.InspType == InspType.Product);
            //报检单查询只为首件报检服务
            if (!criteria.InspNo.IsNullOrEmpty())
                query.Where(p => p.InspNo.Contains(criteria.InspNo));
            if (!criteria.WoNo.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.No.Contains(criteria.WoNo));
            if (criteria.InspType.HasValue)
                query.Where(p => p.InspType == criteria.InspType);
            if (!criteria.ProductCode.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.Product.Name.Contains(criteria.ProductName));
            if (criteria.ShopId.HasValue)
                query.Where(p => p.ShopId == criteria.ShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (!criteria.InspUser.IsNullOrEmpty())
                query.Where(p => p.OperateBy.Name.Contains(criteria.InspUser));
            if (criteria.InspDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.InspDate.BeginValue);
            if (criteria.InspDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.InspDate.EndValue);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取首检记录列表
        /// </summary>
        /// <param name="criteria">首检记录查询实体</param>
        /// <returns>首检记录列表</returns>
        public virtual EntityList<FirstInsp> GetFirstInsps(FirstInspCriteria criteria)
        {
            var query = DB.Query<FirstInsp>().Where(p => p.InspType == InspType.FirstProduct);
            if (criteria.ShopId.HasValue)
                query.Where(p => p.ShopId == criteria.ShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (!criteria.WorkOrder.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrder));
            if (!criteria.Barcode.IsNullOrEmpty())
            {
                query.Exists<InspBarcodeLog>((x, y) => y.Where(p => p.InspLogId == x.Id && p.Barcode.Contains(criteria.Barcode)));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 首件报检
        /// </summary>
        /// <param name="barcodeLogIds">报检条码Id列表</param>
        public virtual string GenerateFirstInsp(List<double> barcodeLogIds)
        {
            string errMsg = string.Empty;
            try
            {
                FirstInsp(barcodeLogIds);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
                throw new ValidationException(exc.Message);
            }

            return errMsg;
        }

        /// <summary>
        /// 首件报检，生成报检单
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        public void GenerateFirstInsp(FirstInspEvent inspEvent)
        {
            SaveGenerateFirstInspLog(inspEvent);
            var paramter = RT.Service.Resolve<InspParameterController>().GetInspParameter(inspEvent.ItemId, InspType.FirstProduct);
            if (paramter == null)//获取不到对应产品的报检参数，获取通用的报检参数
            {
                paramter = RT.Service.Resolve<InspParameterController>().GetGeneralInspParameter(InspType.FirstProduct);
            }
            if (CheckInspParameter(paramter, inspEvent))
            {
                SaveFirstInsp(paramter, inspEvent);
            }
        }

        /// <summary>
        /// 任务单首件报检，生成报检单
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        public void GenerateTaskFirstInsp(FirstInspEvent inspEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                SaveGenerateFirstInspLog(inspEvent);
                CheckTaskInspParameter(inspEvent);
                if (inspEvent.FirstInspQty > 0)
                {
                    SaveTaskFirstInsp(inspEvent);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存首件报检，生成报检单日志
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        private void SaveGenerateFirstInspLog(FirstInspEvent inspEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strInputValue = JsonConvert.SerializeObject(inspEvent);
                var inputValue = "首件报检事件:{0}".L10nFormat(strInputValue);
                var log = new InterfaceLog()
                {
                    Name = "IFirstInsp",
                    Method = "GenerateFirstInsp",
                    ControllerName = _inspLogControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 首件报检
        /// </summary>
        /// <param name="barcodeLogIds">报检条码Id列表</param> 
        public virtual void FirstInsp(List<double> barcodeLogIds)
        {
            var barcodeLogs = GetBarcodeLogs(barcodeLogIds);
            int qty = barcodeLogs.Count();
            if (qty == 0)
                return;
            using (var tran = DB.TransactionScope(CommonEntityDataProvider.ConnectionStringName))
            {
                var barcodeLog = barcodeLogs.OrderByDescending(p => p.CollectionDate).FirstOrDefault();
                var inspLog = barcodeLog.InspLog;
                qty = inspLog.DispatchTaskNo.IsNotEmpty() ? (int)inspLog.InspectionQty : qty;
                var rep = barcodeLog.GetRepository() as EntityRepository;
                var inspDate = rep.GetDbTime();
                inspLog.InspState = InspState.Inspection;
                inspLog.InspectionDate = inspDate;
                var billEvent = CreateInspBillEvent(barcodeLog, qty, barcodeLogs.Select(p => p.Barcode).ToList());
                //记录接口日志
                SaveGenerateFirstInspBillLog(billEvent);
                RT.Service.Resolve<ICreateFirstInspBill>().GenerateFirstInspBill(billEvent);
                barcodeLogs.ForEach(p =>
                {
                    p.InspState = InspState.Inspection;
                    p.InspectionDate = inspDate;
                });
                RF.Save(barcodeLogs);
                inspLog.InspectionQty = inspLog.DispatchTaskNo.IsNotEmpty() ? inspLog.InspectionQty : Query<InspBarcodeLog>().Where(b => b.InspState == InspState.Inspection && b.InspLogId == inspLog.Id).ToList().Count;////更新已报检数量
                RF.Save(inspLog);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存生成首检检验单日志
        /// </summary>
        /// <param name="billEvent">报检参数</param>
        public virtual void SaveGenerateFirstInspBillLog(FirstInspBillEvent billEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(billEvent);
                var inputValue = "报检参数:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "ICreateFirstInspBill",
                    Method = "GenerateFirstInspBill",
                    ControllerName = _inspLogControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证是否已报检（任务单）
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        /// <returns>满足返回true，不满足返回false</returns>
        public virtual void CheckTaskFinishInsp(FirstInspEvent inspEvent)
        {
            var rules = GetFirstInspRuleList().Where(p => p.IsSelect).ToList();
            var latestInsp = GetFirstInspList(inspEvent, rules).OrderByDescending(p => p.CreateDate).FirstOrDefault();
            if (latestInsp == null)
            {
                //获取首件报检参数
                var paramter = GetFirstInspParameter(inspEvent);

                if (paramter != null)
                    throw new ValidationException("任务单未进行首件报检，不允许报工".L10N());
            }
            else
            {
                if (latestInsp.InspectionResult == null)
                    throw new ValidationException("首件检验单【{0}】未完成，不允许报工".L10nFormat(latestInsp.InspNo));
                if (latestInsp.InspectionResult == InspectionResult.Fail)
                    throw new ValidationException("首检检验单【{0}】检验不合格，请重新报检".L10nFormat(latestInsp.InspNo));
            }
        }

        /// <summary>
        /// 验证是否满足报检条件（任务单）
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        /// <returns>满足返回true，不满足返回false</returns>
        public virtual void CheckTaskInspParameter(FirstInspEvent inspEvent)
        {
            var paramter = GetFirstInspParameter(inspEvent);
            if (paramter == null)
            {
                if (inspEvent.ProcessId == 0)
                    throw new ValidationException("产品【{0}】没有维护首检参数，不需要进行首件报检".L10nFormat(inspEvent.ItemCode));
                if (inspEvent.ProcessId > 0)
                    throw new ValidationException("产品【{0}】+工序【{1}】没有维护首检参数，不需要进行首件报检".L10nFormat(inspEvent.ItemCode, inspEvent.ProcessName));
            }

            var rules = GetFirstInspRuleList().Where(p => p.IsSelect).ToList();
            var firstInspList = GetFirstInspList(inspEvent, rules);
            if (firstInspList.Any(p => p.InspectionResult == null))
                throw new ValidationException("已进行首检报检，不用重复报检".L10N());
            if (firstInspList.Any(p => p.InspectionResult == InspectionResult.Pass))
                throw new ValidationException("已完成首检报检，不用重复报检".L10N());
        }

        /// <summary>
        /// 获取报检参数
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="inspType">报检类型</param>
        /// <param name="inspProcess">工序类型</param>
        /// <returns>报检参数</returns>
        public virtual InspParameter GetInspParameter(double productId, double processId, InspType inspType, InspProcess? inspProcess)
        {
            InspParameter paramter = null;
            if (processId == 0)
            {
                paramter = RT.Service.Resolve<InspParameterController>().GetInspParameter(productId, inspType);
                if (paramter == null)//获取不到对应产品的报检参数，获取通用的报检参数
                    paramter = RT.Service.Resolve<InspParameterController>().GetGeneralInspParameter(inspType);

            }
            if (processId > 0)
            {
                paramter = RT.Service.Resolve<InspParameterController>().GetInspParameter(productId, processId, inspType);
                if (paramter == null && inspProcess != null)//获取不到对应工序的报检参数，获取通用的报检参数
                    paramter = RT.Service.Resolve<InspParameterController>().GetInspParameter(productId, inspProcess.Value, inspType);
            }
            return paramter;
        }

        /// <summary>
        /// 转换工序类型
        /// </summary>
        /// <param name="IsStartProcess"></param>
        /// <param name="IsEndProcess"></param>
        /// <returns></returns>
        public virtual InspProcess? ConvertInspProcess(bool IsStartProcess, bool IsEndProcess)
        {
            if (IsStartProcess)
            {
                return InspProcess.First;
            }
            if (IsEndProcess)
            {
                return InspProcess.Last;
            }
            return null;

        }

        /// <summary>
        /// 获取首件报检参数
        /// </summary>
        /// <param name="inspEvent"></param>
        /// <returns></returns>
        InspParameter GetFirstInspParameter(FirstInspEvent inspEvent)
        {
            var inspProcess = ConvertInspProcess(inspEvent.IsStartProcess, inspEvent.IsEndProcess);

            return GetInspParameter(inspEvent.ItemId, inspEvent.ProcessId, InspType.FirstProduct, inspProcess);
        }

        /// <summary>
        /// 创建报检参数
        /// </summary>
        /// <param name="barcode">报检条码</param>
        /// <param name="qty">报检数量</param>      
        /// <param name="barcodes">条码列表</param>
        /// <returns>报检参数</returns>
        FirstInspBillEvent CreateInspBillEvent(InspBarcodeLog barcode, decimal qty, List<string> barcodes)
        {
            var inspLog = barcode.InspLog;
            var wo = RF.GetById<WorkOrder>(inspLog.WorkOrderId);
            if (wo == null)
                throw new ValidationException("创建报检参数失败，找不到id为{0}的工单".L10nFormat(inspLog.WorkOrderId));
            var firstInspBillEvent = new FirstInspBillEvent()
            {
                InspNo = inspLog.InspNo,
                WorkOrderId = wo.Id,
                ItemId = wo.ProductId,
                Qty = qty,
                ShopId = inspLog.ShopId,
                ResourceId = inspLog.ResourceId,
                ProcessIdList = inspLog.Process != null ? new List<double> { inspLog.ProcessId.Value } : null,
                CollectionDate = (DateTime)barcode.CollectionDate,
                Barcodes = barcodes,
                InspLogId = inspLog.Id,
                FactoryId = wo.FactoryId
            };
            RT.Service.Resolve<InspRecordBaseController>().SetFirstInspBillEvent(inspLog.Id, firstInspBillEvent);
            return firstInspBillEvent;
        }

        /// <summary>
        /// 验证是否满足报检条件
        /// </summary>
        /// <param name="paramter">报检参数</param>
        /// <param name="inspEvent">首件报检事件</param>
        /// <returns>满足返回true，不满足返回false</returns>
        public virtual bool CheckInspParameter(InspParameter paramter, FirstInspEvent inspEvent)
        {
            if (paramter == null || paramter.InspType != InspType.FirstProduct)
                return false;
            ////配置为最后工序，而当前采集非最后工序
            if (paramter.ProcessType == InspProcess.Last && !inspEvent.IsEndProcess)
                return false;
            else if (paramter.ProcessType == InspProcess.First && !inspEvent.IsStartProcess)
                return false;
            ////配置为自定义工序，而当前工序不一致
            else if (paramter.ProcessType == InspProcess.Custom && inspEvent.ProcessId != paramter.InspProcessId)
                return false;
            return true;
        }

        /// <summary>
        /// 保存报检记录
        /// </summary>
        /// <param name="paramter">报检参数</param>
        /// <param name="inspEvent">首件报检事件</param>
        private void SaveFirstInsp(InspParameter paramter, FirstInspEvent inspEvent)
        {
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                //加锁，解决并发问题
                DB.Update<InspParameter>().Set(p => p.InspType, p => p.InspType).Where(p => p.Id == paramter.Id).Execute();
                var rules = GetFirstInspRuleList().Where(p => p.IsSelect).ToList();
                InspLog firstInsp = GetFirstInsp(inspEvent, rules);
                if (firstInsp == null)
                {
                    firstInsp = new InspLog();
                    firstInsp.InspNo = GetInspLogNo();
                    firstInsp.WorkOrderId = inspEvent.WorkOrderId;
                    firstInsp.ResourceId = inspEvent.ResourceId;
                    firstInsp.InspectionQty = 0;
                    firstInsp.CustomerId = inspEvent.CustomerId;
                    firstInsp.ShopId = inspEvent.ShopId;
                    firstInsp.InspType = paramter.InspType;
                    firstInsp.OperateById = inspEvent.EmployeeId;
                    firstInsp.ProcessId = inspEvent.ProcessId;
                    firstInsp.InspState = InspState.UnInspection;
                    if (firstInsp.WorkOrder != null)
                    {
                        //填写默认报检数
                        var param = Query<InspParameter>().Where(p => p.ProductId == firstInsp.WorkOrder.ProductId //产品ID
                        && p.InspType == InspType.FirstProduct && p.InspDimension == InspDimension.BatchQty).FirstOrDefault();
                        if (param != null)
                            firstInsp.InspectionQty = param.InspParm;
                        else  //取不到对应产品的参数，可能是通用的
                            firstInsp.InspectionQty = paramter.InspParm;
                    }

                }
                else
                {
                    //判断数量,已经报检过的单不再接收新的条码
                    if (InspDimension.BatchQty == paramter.InspDimension && firstInsp.InspBarcodeLogList.Count >= firstInsp.InspectionQty
                        || firstInsp.InspBarcodeLogList.FirstOrDefault(e => e.InspState == InspState.Inspection) != null)
                        return;
                }

                InspBarcodeLog barcodeLog = new InspBarcodeLog();
                barcodeLog.Barcode = inspEvent.Barcode;
                barcodeLog.ProcessId = inspEvent.ProcessId;
                barcodeLog.StationId = inspEvent.StationId;
                barcodeLog.InspState = InspState.UnInspection;
                barcodeLog.CollectionDate = inspEvent.CollectionDate;

                //firstInsp.InspBarcodeLogList.Add(barcode); 2023/3/27  csp 优化为 barcodeLog.InspLogId = firstInsp.Id; 单独保存barcodeLog
                RF.Save(firstInsp);
                barcodeLog.InspLogId = firstInsp.Id;
                RF.Save(barcodeLog);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存任务单首件报检记录
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        private void SaveTaskFirstInsp(FirstInspEvent inspEvent)
        {
            var firstInsp = new InspLog();
            firstInsp.InspNo = GetInspLogNo();
            firstInsp.DispatchTaskNo = inspEvent.DispatchTaskNo;
            firstInsp.WorkOrderId = inspEvent.WorkOrderId;
            firstInsp.ResourceId = inspEvent.ResourceId;
            firstInsp.InspectionQty = inspEvent.FirstInspQty;
            firstInsp.CustomerId = inspEvent.CustomerId;
            firstInsp.ShopId = inspEvent.ShopId;
            firstInsp.InspType = InspType.FirstProduct;
            firstInsp.OperateById = inspEvent.EmployeeId;
            firstInsp.ProcessId = inspEvent.ProcessId;
            firstInsp.InspState = InspState.UnInspection;

            InspBarcodeLog barcode = new InspBarcodeLog();
            barcode.Barcode = inspEvent.Barcode;
            barcode.ProcessId = inspEvent.ProcessId;
            barcode.StationId = inspEvent.StationId;
            barcode.InspState = InspState.UnInspection;
            barcode.CollectionDate = inspEvent.CollectionDate;
            firstInsp.InspBarcodeLogList.Add(barcode);
            RF.Save(firstInsp);

            //调用QMS首检接口进行报检，这里报检失败时不影响前面的执行结果
            var barcodeLogIds = firstInsp.InspBarcodeLogList.Select(p => p.Id).ToList();
            RT.Service.Resolve<InspLogController>().GenerateFirstInsp(barcodeLogIds);
        }

        /// <summary>
        /// 获取首检单号
        /// </summary>
        /// <returns>报检单号</returns>
        public virtual string GetInspLogNo()
        {
            var config = ConfigService.GetConfig(new FirstInspNoConfig(), typeof(FirstInsp));
            if (config == null || !config.NumberRuleId.HasValue)
                throw new ValidationException("未找到首检单号配置规则，请配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取首检单据
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        /// <param name="ruleParams">报检规则</param>
        /// <returns>首检单据</returns>
        public virtual InspLog GetFirstInsp(FirstInspEvent inspEvent, List<FirstInspRule> ruleParams)
        {
            var query = Query<InspLog>().Where(p => p.WorkOrderId == inspEvent.WorkOrderId && p.InspType == InspType.FirstProduct);
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.WipResource))
            {
                query.Where(p => p.ResourceId == inspEvent.ResourceId);
            }
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.ProductionDate))
            {
                var ruledate = inspEvent.CollectionDate.Date;
                query.Exists<InspBarcodeLog>((x, y) => y.Where(e => e.InspLogId == x.Id && e.CollectionDate >= ruledate && e.CollectionDate < ruledate.AddDays(1)));
            }
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.InspNGRework))
            {
                query.Where(p => p.InspectionResult == null || p.InspectionResult == InspectionResult.Pass);
            }
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取首检单据列表
        /// </summary>
        /// <param name="inspEvent">首件报检事件</param>
        /// <param name="ruleParams">报检规则</param>
        /// <returns>首检单据列表</returns>
        public virtual EntityList<InspLog> GetFirstInspList(FirstInspEvent inspEvent, List<FirstInspRule> ruleParams)
        {
            var query = Query<InspLog>().Where(p => p.WorkOrderId == inspEvent.WorkOrderId && p.InspType == InspType.FirstProduct);
            query.WhereIf(inspEvent.ProcessId != 0, p => p.ProcessId == inspEvent.ProcessId);
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.WipResource))
            {
                query.Where(p => p.ResourceId == inspEvent.ResourceId);
            }
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.ProductionDate))
            {
                var ruledate = inspEvent.CollectionDate.Date;
                query.Exists<InspBarcodeLog>((x, y) => y.Where(e => e.InspLogId == x.Id && e.CollectionDate >= ruledate && e.CollectionDate < ruledate.AddDays(1)));
            }
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.InspNGRework))
            {
                query.Where(p => p.InspectionResult == null || p.InspectionResult == InspectionResult.Pass);
            }
            if (ruleParams.Exists(p => p.Parameter == FirstInspParam.DispatchTaskBill))
            {
                query.Where(p => p.DispatchTaskNo == inspEvent.DispatchTaskNo);
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取首件规则，没有则默认生成数据
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<FirstInspRule> GetFirstInspRuleList()
        {
            EntityList<FirstInspRule> list = GetAll<FirstInspRule>();
            if (list.Count == 0)
            {
                foreach (Enum item in Enum.GetValues(typeof(FirstInspParam)))
                {
                    FirstInspRule entity = new FirstInspRule();
                    entity.PersistenceStatus = PersistenceStatus.New;
                    entity.Parameter = (FirstInspParam)item;
                    if (entity.Parameter == FirstInspParam.WorkOrder)
                        entity.IsSelect = true;
                    list.Add(entity);
                }
            }
            else if (list.Count < Enum.GetValues(typeof(FirstInspParam)).Length)
            {
                foreach (Enum item in Enum.GetValues(typeof(FirstInspParam)))
                {
                    var entityitem = (FirstInspParam)item;
                    if (list.FirstOrDefault(p => p.Parameter == entityitem) == null)
                    {
                        FirstInspRule entity = new FirstInspRule();
                        entity.PersistenceStatus = PersistenceStatus.New;
                        entity.Parameter = entityitem;
                        if (entity.Parameter == FirstInspParam.WorkOrder)
                            entity.IsSelect = true;
                        list.Add(entity);
                    }
                }
            }
            var ruleList = new EntityList<FirstInspRule>();
            ruleList.AddRange(list.OrderBy(p => p.Parameter));
            return ruleList;
        }

        /// <summary>
        /// 保存首件规则
        /// </summary>
        /// <param name="firstInspRules">修改首件规则列表</param>
        public virtual void SaveFirstInspRules(EntityList<FirstInspRule> firstInspRules)
        {
            RF.Save(firstInspRules);
        }

        /// <summary>
        /// 获取报检规则
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<FirstInspRule> GetRuleParam()
        {
            return Query<FirstInspRule>().Where(p => p.IsSelect).ToList();
        }

        /// <summary>
        /// 更新首检单信息
        /// </summary>
        /// <param name="billEvent">首检检验提交后事件集合</param>
        public virtual void UpdateFirstInspResult(FirstInspBillSubmittedEvent billEvent)
        {
            SaveUpdateFirstInspResultLog(billEvent);
            var inspLog = RF.GetById<InspLog>(billEvent.InspLogId);
            if (inspLog == null)
                throw new EntityNotFoundException(typeof(InspLog), billEvent.InspLogId);
            inspLog.InspectionResult = billEvent.Result;
            inspLog.InspectDate = billEvent.InspectDate;
            inspLog.CheckNo = billEvent.CheckNo;

            if (billEvent.DefectIdList != null && billEvent.DefectIdList.Count > 0)
            {
                inspLog.DefectIds = string.Join(",", billEvent.DefectIdList);
            }

            RF.Save(inspLog);
        }

        /// <summary>
        /// 保存更新首检单信息日志
        /// </summary>
        /// <param name="billEvent">首检检验提交后事件集合</param>
        private void SaveUpdateFirstInspResultLog(FirstInspBillSubmittedEvent billEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(billEvent);
                var inputValue = "首检检验提交后事件集合:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IFirstInsp",
                    Method = "UpdateFirstInspResult",
                    ControllerName = _inspLogControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新首检单信息
        /// </summary>
        /// <param name="billEvent">首检不合格审核提交后事件集合</param>
        public virtual void UpdateFirstAuditResult(FirstInspBillSubmittedEvent billEvent)
        {
            SaveUpdateFirstAuditResultLog(billEvent);
            var inspLog = RF.GetById<InspLog>(billEvent.InspLogId);
            if (inspLog == null)
                throw new EntityNotFoundException(typeof(InspLog), billEvent.InspLogId);
            inspLog.ProcessMode = Utils.EnumViewModel.EnumToLabel(billEvent.ProcessMode).L10N();
            inspLog.InspectDate = billEvent.InspectDate;
            inspLog.InspectionResult = billEvent.Result;
            if (billEvent.ProcessMode == EventMessages.ProcessMode.StopToCorrect)
                RT.Service.Resolve<WorkOrderController>().Pause((double)billEvent.WorkOrderId, "首检不合格");
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                RF.Save(inspLog);
                RT.Service.Resolve<InspRecordBaseController>().UpdateFirstAuditResultExt(inspLog);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存更新首检单信息日志
        /// </summary>
        /// <param name="billEvent">首检不合格审核提交后事件集合</param>
        private void SaveUpdateFirstAuditResultLog(FirstInspBillSubmittedEvent billEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(billEvent);
                var inputValue = "首检不合格审核提交后事件集合:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IFirstInsp",
                    Method = "UpdateFirstAuditResult",
                    ControllerName = _inspLogControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 自动报检
        /// </summary>
        /// <returns>自动报检结果信息</returns>
        public virtual string AutoFirstInsp()
        {
            StringBuilder message = new StringBuilder();
            var inspLogs = GetNeedFirstInsp();
            if (inspLogs.Count == 0)
                return "检查不到需要报检数据GetNeedFirstInsp()";
            foreach (InspLog insgLpg in inspLogs)
            {
                try
                {
                    var count = insgLpg.InspBarcodeLogList.Count;
                    if (count == 0) continue;
                    Logging.LogManager.GetLogger("job").Info("开始工单[{0}]首件自动报检".L10nFormat(insgLpg.WorkOrder?.No));
                    var para = GetParamQty(insgLpg.WorkOrder.ProductId);
                    if (para == null)
                        para = GetParamQty(null);//取不到产品的取通用的
                    if (para == null)
                        continue;
                    decimal inspCount = para.InspParm;
                    if (count < inspCount)
                        continue;
                    var barcodes = insgLpg.InspBarcodeLogList.OrderBy(p => p.Id).Take((int)inspCount);
                    var barcodeLogIds = barcodes.Select(p => p.Id).Distinct().ToList();
                    FirstInsp(barcodeLogIds);
                    Logging.LogManager.GetLogger("job").Info("结束工单[{0}]首件自动报检".L10nFormat(insgLpg.WorkOrder?.No));
                }
                catch (Exception exc)
                {
                    message.AppendLine("工单[{0}]首件自动报检失败，失败信息：{1}".L10nFormat(insgLpg.WorkOrder?.No, exc.Message));
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }

            return message.ToString();
        }


        /// <summary>
        /// 获取待自动报检报检记录,已有报检过的单不再报检
        /// </summary>
        /// <returns>报检记录</returns>
        public virtual EntityList<InspLog> GetNeedFirstInsp()
        {
            return Query<InspLog>().NotExists<InspBarcodeLog>((x, y) => y.Where(e => e.InspLogId == x.Id && e.InspState == InspState.Inspection))
                //存在通用的参数，不可用产品来筛选
                //.Join<InspParameter>((c, d) => c.WorkOrder.ProductId == d.ProductId && d.InspDimension == InspDimension.BatchQty && d.InspType == InspType.FirstProduct)
                .Where(p => p.InspType == InspType.FirstProduct).ToList();
        }

        /// <summary>
        /// 获取需要报检的数量
        /// </summary>
        /// <param name="itemid">产品ID</param>
        /// <returns>报检参数</returns>
        public virtual InspParameter GetParamQty(double? itemid)
        {
            return Query<InspParameter>().Where(p => p.InspDimension == InspDimension.BatchQty && p.ProductId == itemid && p.InspType == InspType.FirstProduct).FirstOrDefault();
        }

        /// <summary>
        /// 获取条码日志明细By LogId
        /// </summary>
        /// <param name="pid">InspLog Id</param>
        /// <returns></returns>
        public virtual EntityList<InspBarcodeLog> GetBarcodeLog(double pid)
        {
            return Query<InspBarcodeLog>().Where(p => p.TreePId == pid).ToList();
        }

        /// <summary>
        /// 根据报检条码日志明细Id列表获取报检条码日志明细列表
        /// </summary>
        /// <param name="barcodeLogIds">报检条码日志明细Id列表</param>
        /// <returns>报检条码日志明细列表</returns>
        public virtual EntityList<InspBarcodeLog> GetBarcodeLogs(List<double> barcodeLogIds)
        {
            return Query<InspBarcodeLog>().Where(p => barcodeLogIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(InspBarcodeLog.InspLogProperty));
        }

        /// <summary>
        /// 根据报检日志Id获取报检条码日志
        /// </summary>
        /// <param name="inspLogId">报检日志Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="sortInfo">排序参数</param>
        /// <returns>报检条码日志</returns>
        public virtual EntityList<InspBarcodeLog> GetBarcodeLogs(double inspLogId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<InspBarcodeLog>().Where(p => p.InspLogId == inspLogId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码获取报检条码日志
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="panelCode">拼板码</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>报检条码日志</returns>
        public virtual EntityList<InspLog> GetBarcodeLogs(string sn, string panelCode, double workOrderId)
        {
            return Query<InspLog>()
                .Join<InspBarcodeLog>((x, y) => x.Id == y.InspLogId)
                .Where<InspBarcodeLog>((x, y) => (y.Barcode == sn || y.Barcode == panelCode) && x.WorkOrderId == workOrderId)
                .OrderByDescending(x => x.InspectDate)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码和类型获取报检条码日志
        /// </summary>
        /// <param name="sns">条码</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检条码日志</returns>
        public virtual EntityList<InspBarcodeLog> GetBarcodeLogsBySnType(List<string> sns, InspType inspType)
        {
            return Query<InspBarcodeLog>().Join<InspLog>((a, b) => a.InspLogId == b.Id && b.InspType == inspType)
                .Where(p => sns.Contains(p.Barcode)).ToList();
        }

        /// <summary>
        /// 手动审核提交
        /// </summary>
        /// <param name="inspLog"></param>
        public virtual void ExamineSubmit(InspLog inspLog)
        {
            if (inspLog.InspectionResult == null)
            {
                throw new ValidationException("检验结果必填！".L10N());
            }
            inspLog.InspectionStatus = Enums.InspectionStatus.Inspectioned;
            inspLog.InspectDate = DateTime.Now;
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                RF.Save(inspLog);
                // todo 回写报工记录
                if (inspLog.ReportRecordId > 0)
                {
                    var barcodeEvent = new ToStorageBarcodeEvent()
                    {
                        WorkOrderId = inspLog.WorkOrderId,
                        ReportRecordId = inspLog.ReportRecordId,
                        Qty = inspLog.InspectionQty,
                        InspectionResult = Convert.ToInt32(inspLog.InspectionResult),
                        InspectionStatus = Convert.ToInt32(inspLog.InspectionStatus),
                        ProcessMode = inspLog.ProcessMode,
                    };
                    RT.Service.Resolve<ITaskReport>().ToUpdateTaskReportState(barcodeEvent);// 更新报工记录状态
                }
                tran.Complete();
            }

        }

        /// <summary>
        /// 验证Sn是否属于单据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual ValidateResultInfo ValidateSnInBill(ValidateRequestInfo info)
        {
            ValidateResultInfo result = new ValidateResultInfo();

            var bill = Query<FirstInsp>().Where(c => c.Id == info.BillId).FirstOrDefault();
            if (bill == null)
            {
                result.Code = 0;
                result.ErrorMsg = "报检单号不存在".L10N();
                return result;
            }

            var inventoryInspectionSn = Query<InspBarcodeLog>().Where(c => c.InspLogId == bill.Id && c.Barcode == info.Sn).FirstOrDefault();
            if (inventoryInspectionSn == null)
            {
                result.Code = 0;
                result.ErrorMsg = "SN不存在于此报检单中".L10N();
                return result;
            }
            result.Code = 1;
            return result;
        }

        /// <summary>
        /// 获取报工记录对应成品检验单
        /// </summary>
        /// <param name="recordIds">报工记录</param>
        /// <returns></returns>
        public virtual Dictionary<double, string> GetRecordInspLogByIds(List<double> recordIds)
        {
            List<InspLog> inspLogs = new List<InspLog>();
            recordIds.SplitDataExecute(tempIds =>
            {
                var list = Query<InspLog>().Where(p => p.ReportRecordId != null && tempIds.Contains((double)p.ReportRecordId)).Select(p => new { Report_Record_Id = p.ReportRecordId, Check_No = p.CheckNo }).ToList();
                inspLogs.AddRange(list);
            });
            return inspLogs.ToDictionary(p => (double)p.ReportRecordId, p => p.CheckNo);
        }
    }
}