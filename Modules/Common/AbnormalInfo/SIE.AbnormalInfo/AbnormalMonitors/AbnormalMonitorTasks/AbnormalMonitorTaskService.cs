using MimeKit;
using SIE.AbnormalInfo.AbnormalEvent;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks;
using SIE.AbnormalInfo.AbnormalMonitors.Configs;
using SIE.AbnormalInfo.AbnormalMonitors.Pushers;
using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.AbnormalInfo.Common;
using SIE.Common;
using SIE.Common.Alert;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Common.Sender;
using SIE.Core.Anomalymonitors;
using SIE.Core.Common.Controllers;
using SIE.Core.Common.Models;
using SIE.Core.Common.Service;
using SIE.Defects;
using SIE.Defects.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.PDCA;
using SIE.EventMessages.TOPS;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 
    /// </summary>
    public class AbnormalMonitorTaskService : DomainService
    {
        private readonly AbnormalMonitorTaskDao _abnormalMonitorTaskDao;
        private readonly AbnormalDecisionRuleService _abnormalDecisionRuleService;
        private readonly AbnormalWarnDefineDao _abnormalWarnDefineDao;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalMonitorTaskDao"></param>
        /// <param name="abnormalDecisionRuleService"></param>
        /// <param name="abnormalDefineDao"></param>
        /// <param name="abnormalWarnDefineDao"></param>
        public AbnormalMonitorTaskService(AbnormalMonitorTaskDao abnormalMonitorTaskDao, AbnormalDecisionRuleService abnormalDecisionRuleService, AbnormalWarnDefineDao abnormalWarnDefineDao)
        {
            _abnormalMonitorTaskDao = abnormalMonitorTaskDao;
            _abnormalDecisionRuleService = abnormalDecisionRuleService;
            _abnormalWarnDefineDao = abnormalWarnDefineDao;
        }

        /// <summary>
        /// 获取异常定义列表
        /// </summary>
        /// <param name="criteria">异常定义查询实体</param>
        /// <returns>异常定义实体列表</returns>
        public virtual EntityList<AbnormalMonitorTask> GetAbnormalTasks(AbnormalMonitorTaskCriteria criteria)
        {
            return _abnormalMonitorTaskDao.GetAbnormalTasks(criteria);
        }

        /// <summary>
        /// 生成编码
        /// </summary>
        /// <returns>编码</returns>
        public virtual string GenerateCode()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AbnormalMonitorTask));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到异常任务编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 生成异常清单编码
        /// </summary>
        /// <returns>编码</returns>
        public virtual string GenerateInventoryCode()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AbnormalMonitorInventory));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到异常任务编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 异常清单是否生成异常任务
        /// </summary>
        /// <returns>编码</returns>
        public virtual bool InventoryAutoTask()
        {
            var config = ConfigService.GetConfig<AbmMonitorInventoryConfigValue>(new AbmMonitorInventoryConfig(), typeof(AbnormalMonitorInventory));
            return config.AutoTask;
        }

        /// <summary>
        ///  生成日志（不保存）
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="handleNode"></param>
        /// <param name="taskHandleAction"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual AbnormalMonitorTaskLog GenerateLog(double taskId, TaskStateEnum handleNode, TaskHandleAction taskHandleAction, string content)
        {
            AbnormalMonitorTaskLog log = new AbnormalMonitorTaskLog();

            var logIdList = RT.Service.Resolve<CommonController>().GetBatchIdList<AbnormalMonitorSendLog>(1);
            var logIdGetter = new EntityIdGetter(logIdList);
            log.Id = logIdGetter.GetNextId();
            log.AbnormalMonitorTaskId = taskId;
            log.HandleNode = handleNode;
            log.HandlerId = RT.IdentityId;
            log.TaskHandleAction = taskHandleAction;
            log.Content = content;
            return log;
        }

        /// <summary>
        /// 人工添加任务
        /// </summary>
        /// <param name="task"></param>
        public virtual void AddTaskForManual(AbnormalMonitorTask task)
        {
            if (!task.AbnormalDefineId.HasValue)
                throw new ValidationException("异常定义必填".L10N());
            if (task.ProblemDescription.IsNullOrEmpty())
                throw new ValidationException("异常描述必填".L10N());

            using (var trans = DB.TransactionScope(AbnormalInfoDataProvider.ConnectionStringName))
            {
                //1.异常任务
                task.TaskState = TaskStateEnum.ToDo;
                var persistenceStatus = task.PersistenceStatus;
                RF.Save(task);
                //2.新建任务，需要做任务日志和责任人推送
                if (persistenceStatus == PersistenceStatus.New)
                {
                    //2.1.异常任务日志
                    AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Action, "手工创建异常任务".L10N());
                    RF.Save(log);
                    //2.2.责任人发送预警（异步）
                    if (task.AbnormalWarnDefineId.HasValue)
                    {
                        var warnDefine = _abnormalWarnDefineDao.GetById(task.AbnormalWarnDefineId.Value);
                        if (warnDefine != null)
                        {
                            Threading.AsyncHelper.InvokeSafe(() =>
                            {
                                SendForResponsibility(task, warnDefine);
                            });

                        }
                    }
                }
                trans.Complete();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalMonitorTaskId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual bool Cancel(double abnormalMonitorTaskId, string reason)
        {
            using (var trans = DB.TransactionScope(AbnormalInfoDataProvider.ConnectionStringName))
            {
                var task = _abnormalMonitorTaskDao.GetById(abnormalMonitorTaskId);
                if (task == null)
                    throw new SIE.Domain.Validation.ValidationException("异常任务不存在".L10N());
                task.CancelReason = reason;
                task.TaskState = Common.TaskStateEnum.Cancel;
                task.WarnTimes = 0;//状态改变，预警次数清零
                RF.Save(task);
                AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Action, "取消任务".L10N());
                RF.Save(log);
                trans.Complete();
                return true;
            }
        }
        /// <summary>
        /// 异常处理保存
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual AbnormalMonitorTask ProcessSave(AbnormalMonitorTask task)
        {
            task.TaskState = Common.TaskStateEnum.Doing;
            _abnormalMonitorTaskDao.Save(task);
            return task;
        }

        /// <summary>
        /// 异常处理提交
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual AbnormalMonitorTask ProcessSubmit(AbnormalMonitorTask task)
        {
            using (var trans = DB.TransactionScope(AbnormalInfoDataProvider.ConnectionStringName))
            {
                ProcessSubmitValidation(task);
                AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, task.TaskState, Common.TaskHandleAction.Action, "异常处理提交".L10N());
                task.TaskState = Common.TaskStateEnum.Done;
                task.WarnTimes = 0;//状态改变，预警次数清零

                //任务回调-任务处理
                RT.Service.Resolve<AbnormalEventController>().AbnormalCallHandel(task);
                if (task.PushMethord.HasValue)
                {
                    if (task.PushMethord == PushMethordEnum.PDCA)
                    {
                        //触发pdca
                        task.TriggerNo = GeneratePdca(task);
                    }
                    else if (task.PushMethord == PushMethordEnum.EightD)
                    {
                        //触发8D
                        task.TriggerNo = GenerateRecovery(task);
                    }

                }

                RF.Save(task);
                RF.Save(log);
                trans.Complete();
                return task;
            }
        }

        #region 发起PDCA

        /// <summary>
        /// 发起PDCA
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private string GeneratePdca(AbnormalMonitorTask task)
        {
            CreatePdcaReportEvent reportEvent;
            var defectCodes = task.JoinDefectCodes.Split(',').ToList();
            var defectList = RT.Service.Resolve<WorkShopController>().GetDefects(defectCodes);

            var highLevelDefect = GetHighLevelDefect(defectList);
            // 将检验单据的所有检验项目的缺陷、缺陷等级记录推送到PDCA改善报告的问题列表中
            List<DefectInfo> defects = TransferToDefectInfoList(defectList);

            // 创建PDCA改善报告
            reportEvent = new CreatePdcaReportEvent
            {
                BillId = null,
                BillNo = string.Empty,
                BatchNo = string.Empty,
                BatchQty = null,
                ItemId = null,
                QualityCategoryId = null,
                DefectId = highLevelDefect.Id,
                DefectLevel = highLevelDefect.DefectLevel,
                DefectDescription = string.Empty,
                InspectionType = null,
                DefectInfoList = defects.DistinctBy(item => item.DefectId).ToList(), //去掉重复的问题
            };
            return RT.Service.Resolve<ICreatePdcaReportEvent>().GeneratePdcaReport(reportEvent);
        }


        /// <summary>
        /// 计算最高等级缺陷
        /// </summary>
        /// <param name="defectList"></param>
        /// <returns></returns>
        protected virtual Defect GetHighLevelDefect(EntityList<Defect> defectList)
        {
            Defect highLevelDefect = null;

            if (defectList.IsNullOrEmpty())
            {
                throw new ValidationException("缺陷代码为空，不能计算最高等级缺陷".L10N());
            }
            else
            {
                highLevelDefect = defectList.First();
            }

            // 查找缺陷等级最高的排序
            DefectSeverity highLevel = DefectSeverityHelper.LowestDefectSeverity;

            foreach (var item in defectList)
            {
                DefectSeverity curLevel = item.DefectSeverity;
                if (DefectSeverityHelper.IsHigherThan(curLevel, highLevel))
                {
                    highLevel = curLevel;
                    highLevelDefect = item;
                }
            }
            if (highLevelDefect == null)
                throw new ValidationException("缺陷未维护缺陷等级".L10N());
            return highLevelDefect;
        }

        #endregion

        #region 发起8D

        /// <summary>
        /// 发起8D
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private string GenerateRecovery(AbnormalMonitorTask task)
        {
            CreateRecoveryEvent reportEvent;
            var defectCodes = task.JoinDefectCodes.Split(',').ToList();
            var defectList = RT.Service.Resolve<WorkShopController>().GetDefects(defectCodes);

            List<DefectInfo> defects = TransferToDefectInfoList(defectList);
            StringBuilder problemDes = new StringBuilder();
            problemDes.AppendLine("异常任务编码：{0}".L10nFormat(task.Code));
            defectList.ForEach(defect =>
            {
                problemDes.AppendLine("缺陷代码：{0}，缺陷描述：{1}".L10nFormat(defect.Code, defect.Description));
            });

            reportEvent = new CreateRecoveryEvent
            {
                HandlerId = task.TaskHandlerId.Value,
                ProblemDescription = problemDes.ToString(),
                ItemId = null,
                InspectionType = null,
                DefectInfoList = defects, //去掉重复的问题
            };

            return RT.Service.Resolve<ICreateRecovery>().GenerateRecovery(reportEvent);
        }

        /// <summary>
        /// 缺陷信息转换
        /// </summary>
        /// <param name="defectList"></param>
        /// <returns></returns>
        protected virtual List<DefectInfo> TransferToDefectInfoList(EntityList<Defect> defectList)
        {
            var defects = new List<DefectInfo>();
            foreach (var item in defectList)
            {
                if (defects.All(p => p.DefectId != item.Id))
                    defects.Add(new DefectInfo() { DefectCode = item.Code, DefectId = item.Id, DefectLevel = item.DefectLevel, QualityType = EnumViewModel.EnumToLabel(item.QualityType).L10N(), DefectDescription = item.Description });
            }
            return defects;
        }

        #endregion


        private void ProcessSubmitValidation(AbnormalMonitorTask task)
        {
            if (task.AbnormalReason.IsNullOrEmpty())
                throw new ValidationException("异常原因不能为空".L10N());
            if (task.TempMeasures.IsNullOrEmpty())
                throw new ValidationException("临时对策不能为空".L10N());
            if (task.LongMeasures.IsNullOrEmpty())
                throw new ValidationException("长期对策不能为空".L10N());
        }

        /// <summary>
        /// 异常清单
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="define"></param>
        /// <returns></returns>
        private AbnormalMonitorInventory GenerateAbnormalInventory(AbnormalDecisionRule rule, AbnormalDefine define)
        {
            var inventory = new AbnormalMonitorInventory();
            inventory.GenerateId();
            inventory.Code = this.GenerateInventoryCode();
            inventory.AbnormalName = rule.RuleName;
            inventory.AbnormalDefineId = define.Id;
            inventory.AbnormalWarnDefineId = define.AbnormalWarnDefineId;
            inventory.ProblemCondition = string.Join(" ", rule.LayerConditionsList.Where(c => c.IsWhere).Select(item =>
            {
                return $"[{item.LayerName}:{item.Value1}]";
            }));
            inventory.ProblemDescription += $"{rule.IndicatorName} {rule.Operator?.ToLabel()} [{rule.Value1}]";
            RF.Save(inventory);
            return inventory;
        }

        private AbnormalMonitorTask GenerateTask(AbnormalDecisionRule rule, AbnormalDefine define)
        {
            AbnormalMonitorTask task = new AbnormalMonitorTask();
            task.GenerateId();
            task.Code = this.GenerateCode();
            task.AbnormalName = rule.RuleName;
            task.TaskState = Common.TaskStateEnum.ToDo;
            task.AbnormalDefineId = define.Id;
            task.AbnormalWarnDefineId = define.AbnormalWarnDefineId;
            task.ProblemCondition = string.Join(" ", rule.LayerConditionsList.Where(c => c.IsWhere).Select(item =>
            {
                return $"[{item.LayerName}:{item.Value1}]";
            }));
            task.ProblemDescription += $"{rule.IndicatorName} {rule.Operator?.ToLabel()} [{rule.Value1}]";
            return task;
        }

        /// <summary>
        /// 生成异常任务
        /// </summary>
        /// <param name="inventorys"></param>
        /// <returns></returns>
        public virtual List<AbnormalMonitorInventory> InventoryGenerateTask(List<AbnormalMonitorInventory> inventorys)
        {
            //检验是否能生成新的任务
            var notTasks = _abnormalMonitorTaskDao.CannotMonitorTask(inventorys);
            inventorys = inventorys.Except(notTasks).ToList();
            var taskLogs = new EntityList<AbnormalMonitorTaskLog>();
            var tasks = new EntityList<AbnormalMonitorTask>();
            inventorys.ForEach(item =>
            {
                //1.生成异常任务
                AbnormalMonitorTask task = new AbnormalMonitorTask();
                task.GenerateId();
                task.Code = this.GenerateInventoryCode();
                task.AbnormalWarnDefineId = item.AbnormalWarnDefineId;
                task.AbnormalName = item.AbnormalName;
                task.TaskState = Common.TaskStateEnum.ToDo;
                task.AbnormalDefineId = item.AbnormalDefineId;
                task.ProblemDescription = item.ProblemDescription;
                task.ProblemCondition = item.ProblemCondition;
                task.WarnTimes = 0;//状态改变，预警次数清零
                tasks.Add(task);

                //2.生成任务日志
                taskLogs.Add(this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Action, "生成异常任务".L10N()));
            });
            RF.Save(tasks);
            RF.Save(taskLogs);

            //3.责任人发送预警（异步）
            var warnDefineIds = tasks.Where(c => c.AbnormalWarnDefineId.HasValue).Select(c => c.AbnormalWarnDefineId.Value).Distinct().ToList();
            if (warnDefineIds.IsNullOrEmpty()) return notTasks;
            var warnDefines = warnDefineIds.SplitContains<AbnormalWarnDefine, double>(tempIds =>
            {
                return _abnormalWarnDefineDao.FindMany(c => tempIds.Contains(c.Id));
            });
            if (warnDefines.IsNullOrEmpty()) return notTasks;
            foreach (var task in tasks)
            {
                if (task.AbnormalWarnDefineId.HasValue)
                {
                    var warnDefine = warnDefines.FirstOrDefault(c => c.Id == task.AbnormalWarnDefineId);
                    if (warnDefine == null) continue;
                    Threading.AsyncHelper.InvokeSafe(() =>
                    {
                        SendForResponsibility(task, warnDefine);
                    });
                }
            }
            return notTasks;
        }

        /// <summary>
        /// 生成异常任务事件处理(前方用EventBus解耦)
        /// </summary>
        /// <param name="define"></param>
        public virtual string GenerateTaskEventHandle(AbnormalDefine define)
        {
            var rule = define.AbnormalRule;
            //1.0.判断规则是否能生成任务
            var (canGenerate, msg) = _abnormalDecisionRuleService.AbnomalRuleMultTest(rule.Id);
            if (canGenerate)
            {
                using (var trans = DB.TransactionScope(AbnormalInfoDataProvider.ConnectionStringName))
                {
                    //2.0.生成异常清单
                    var inventory = GenerateAbnormalInventory(rule, define);
                    //是否生成任务
                    var canGeneralTask = DB.Query<AbnormalMonitorTask>()
 .Where(c => c.AbnormalDefineId == inventory.AbnormalDefineId && c.ProblemDescription == inventory.ProblemDescription
       && c.AbnormalName == inventory.AbnormalName && c.TaskType == TaskType.Auto && (c.TaskState == TaskStateEnum.Doing || c.TaskState == TaskStateEnum.ToDo)).Count() > 0;
                    if (InventoryAutoTask() && !canGeneralTask)
                    {
                        //3.0.生成任务
                        AbnormalMonitorTask task = GenerateTask(rule, define);
                        RF.Save(task);

                        //4.0.生成任务日志
                        AbnormalMonitorTaskLog logGenerateTask = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Action, "生成异常任务".L10N());
                        RF.Save(logGenerateTask);

                        //5.0.责任人发送预警（异步）
                        var warnDefine = define.AbnormalWarnDefine;
                        if (warnDefine != null)
                        {
                            Threading.AsyncHelper.InvokeSafe(() =>
                            {
                                SendForResponsibility(task, warnDefine);
                            });

                        }
                    }
                    trans.Complete();
                }
            }
            return msg;
        }



        /// <summary>
        /// 责任人推送通知
        /// </summary>
        /// <param name="task"></param>
        /// <param name="warnDefine"></param>
        /// <returns></returns>
        public virtual void SendForResponsibility(AbnormalMonitorTask task, AbnormalWarnDefine warnDefine)
        {
            if (warnDefine.JoinEmployeeIds.IsNotEmpty() && warnDefine.PushModuleId.HasValue)
            {
                var pusher = warnDefine.PushModule;
                List<double> employeeIds = warnDefine.JoinEmployeeIds.Split(',').Select(c => Convert.ToDouble(c)).ToList();
                if (employeeIds.IsNotEmpty())
                {
                    var employeeList = RT.Service.Resolve<SIE.Common.Employees.EmployeeController>().GetEmplotees(employeeIds, new List<double?>());
                    warnDefine.JoinEmployeeNames = String.Join(",", employeeList.Select(c => c.Name));
                    string errorMsg = string.Empty;
                    AbnormalMonitorAlertResult result = CreateAlertResult(task);

                    if (this.Send(pusher, employeeList, result, out errorMsg))
                    {
                        string content = "推送通知给责任人，推送人员：{0}".L10nFormat(warnDefine.JoinEmployeeNames);
                        AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Message, content);
                        RF.Save(log);
                    }
                    else
                    {
                        string content = "推送通知给责任人，失败原因：{0}".L10nFormat(errorMsg);
                        AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Message, content);
                        RF.Save(log);
                    }

                }
            }
        }

        /// <summary>
        /// SPC责任人推送通知
        /// </summary>
        /// <param name="task"></param>
        /// <param name="warnDefine"></param>
        /// <returns></returns>
        public virtual void SpcSendForResponsibility(AbnormalMonitorTask task, AbnormalWarnDefine warnDefine,double userId)
        {
            if (warnDefine.JoinEmployeeIds.IsNotEmpty() && warnDefine.PushModuleId.HasValue)
            {
                var pusher = warnDefine.PushModule;
                List<double> employeeIds = warnDefine.JoinEmployeeIds.Split(',').Select(c => Convert.ToDouble(c)).ToList();
                if (employeeIds.IsNotEmpty())
                {
                    var employeeList = RT.Service.Resolve<SIE.Common.Employees.EmployeeController>().GetEmplotees(employeeIds, new List<double?>());
                    warnDefine.JoinEmployeeNames = String.Join(",", employeeList.Select(c => c.Name));
                    string errorMsg = string.Empty;
                    AbnormalMonitorAlertResult result = CreateAlertResult(task);

                    if (this.Send(pusher, employeeList, result, out errorMsg))
                    {
                        string content = "推送通知给责任人，推送人员：{0}".L10nFormat(warnDefine.JoinEmployeeNames);
                        AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Message, content);
                        log.HandlerId = userId;
                        RF.Save(log);
                    }
                    else
                    {
                        string content = "推送通知给责任人，失败原因：{0}".L10nFormat(errorMsg);
                        AbnormalMonitorTaskLog log = this.GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Message, content);
                        log.HandlerId = userId;
                        RF.Save(log);
                    }

                }
            }
        }

        /// <summary>
        /// 生成异常信息推送信息
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private AbnormalMonitorAlertResult CreateAlertResult(AbnormalMonitorTask task)
        {
            var result = new AbnormalMonitorAlertResult() { AlertInfoList = new List<AbnormalMonitorPusher>() };
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "推送说明".L10N(),
                Value = "您有新的异常任务产生"
            });
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "异常编码".L10N(),
                Value = task.Code
            });
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "发生时间".L10N(),
                Value = task.CreateDate.ToString("yyyy年MM月dd日 HH:mm")
            });
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "异常描述".L10N(),
                Value = task.ProblemDescription
            });
            return result;
        }

        /// <summary>
        /// 对象
        /// </summary>
        object g_locker = new object();

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="pusher"></param>
        /// <param name="result"></param>
        /// <param name="employees"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public virtual bool Send(Pusher pusher, EntityList<SIE.Common.Employees.Employee> employees, AbnormalMonitorAlertResult result, out string errorMsg)
        {
            try
            {
                errorMsg = string.Empty;

                if (employees.IsNullOrEmpty()) return true;

                ReceiveParam rparam = new ReceiveParam();
                rparam.MessageTemplateJson = pusher.MessageTemplate;
                rparam.Employees.AddRange(employees);
                rparam.EmailAttachmentCollection = (new BodyBuilder()).Attachments;

                //获取推送插件
                ISender sender = RT.Service.Resolve<PushPlugController>().GetSender(pusher.PushPlug);

                //初始化推送插件参数
                sender.Config.Initialize(pusher.PushPlug.Config);

                //创建推送参数
                ISendParam sparam = null;
                lock (g_locker)
                {
                    sparam = sender.CreateSendParam(result, rparam);
                }

                //推送信息
                sender.Send(sparam);
                return true;
            }
            catch
            {
                errorMsg = "推送异常，请检查推送模块配置".L10N();
                return false;
            }

        }


        #region 异常处理视图-扩展
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual AbnormalMonitorTask GetExtentionViewData(double taskId)
        {
            var task = _abnormalMonitorTaskDao.GetById(taskId, new EagerLoadOptions().LoadWithViewProperty());
            var typeStr = task.GetEntityType();
            if (typeStr.IsNullOrEmpty())
                typeStr = task.MonitorType;
            if (typeStr.IsNullOrEmpty()) return null;
            var type = Type.GetType(typeStr);
            if (type == null) return null;
            task.SetEntityType(typeStr);
            var configAttribute = type.GetCustomAttributes(false).FirstOrDefault(c => { return c is EntityExtensionConfigAttribute; });;
            if (configAttribute != null)
            {
                if (configAttribute is EntityExtensionConfigAttribute config)
                {
                    task.SetExtName(config.Name);
                    task.SetTypeName(config.ConfigType.FullName);
                    task.PersistenceStatus = PersistenceStatus.Modified;
                }
            }
            RF.Save(task);
            return task;
        }
        #endregion
    }
}
