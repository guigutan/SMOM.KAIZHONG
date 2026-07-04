using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks;
using SIE.AbnormalInfo.AbnormalMonitors.Configs;
using SIE.AbnormalInfo.Common;
using SIE.Common.Configs;
using SIE.Core.AbnormalInfos;
using SIE.Domain;
using SIE.EventMessages.AbnormalInfo;
using SIE.EventMessages.AbnormalInfos;
using System;
using System.Threading.Tasks;

namespace SIE.AbnormalInfo.AbnormalEvent
{
    /// <summary>
    /// 来料检验单EventBus监听控制器
    /// </summary>
    public class AbnormalEventController : DomainController, IAbnormalMonitor
    {
        /// <summary>
        /// 订阅事件通知-生成异常任务
        /// </summary>
        /// <param name="taskEvent">红牌管理</param>
        public virtual void CreateAbnormalTask(PubTaskEvent taskEvent)
        {
            try
            {
                var task = Query<AbnormalMonitorTask>().Where(c => c.SourceDataKeys.Contains(taskEvent.PubKey) && (c.TaskState == TaskStateEnum.ToDo || c.TaskState == TaskStateEnum.Doing)).FirstOrDefault();
                AbnormalWarnDefine abnormalWarnDefine;
                if (task == null)
                {
                    task = new AbnormalMonitorTask();
                    task.GenerateId();
                    task.Code = RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateCode();
                    //Todo,红牌任务预警id，根据红牌名称匹配预警机智
                    var config = ConfigService.GetConfig(new WarnDefineForRedCardConfig(), typeof(AbnormalWarnDefine));//预警定义
                    if (config != null && config.AbnormalWarnDefineId == null)
                        throw new SIE.Domain.Validation.ValidationException("红牌管理-预警定义配置项未配置".L10N());

                    abnormalWarnDefine = config.AbnormalWarnDefine;

                    task.AbnormalWarnDefineId = abnormalWarnDefine?.Id;
                    task.AbnormalName = "红牌管理".L10N();
                    task.TaskState = TaskStateEnum.ToDo;
                    task.ProblemDescription = taskEvent.TaskDescription;
                    task.SourceDataKeys = taskEvent.PubKey;
                    task.SetEntityType(taskEvent.EntityType);
                }
                else
                {
                    task.ProblemDescription = taskEvent.TaskDescription;
                    abnormalWarnDefine = task.AbnormalWarnDefine;
                }
                RF.Save(task);

                //生成任务日志
                AbnormalMonitorTaskLog logGenerateTask = RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateLog(task.Id, Common.TaskStateEnum.ToDo, Common.TaskHandleAction.Action, "生成异常任务".L10N());
                RF.Save(logGenerateTask);

                //责任人发送预警（异步）
                var warnDefine = abnormalWarnDefine;
                if (warnDefine != null)
                {
                    Threading.AsyncHelper.InvokeSafe(() =>
                    {
                        RT.Service.Resolve<AbnormalMonitorTaskService>().SendForResponsibility(task, warnDefine);
                    });

                }

                //任务回调
                AbnormalTaskCall(task.SourceDataKeys, task.Code);
            }
            catch (Exception ex)
            {
                RT.Logger.Error($"红牌生成异常任务失败,异常信息：{ex.Message}".L10N());
                throw;
            }
        }

        /// <summary>
        /// 任务回调
        /// </summary>
        /// <param name="pubKey"></param>
        /// <param name="taskNo"></param>
        public virtual void AbnormalTaskCall(string pubKey, string taskNo)
        {
            //任务回调
            var call = new TaskCallEvent
            {
                PubKey = pubKey,
                TaskNo = taskNo
            };
            RT.EventBus.Publish(call);
        }

        /// <summary>
        /// 回调-任务处理
        /// </summary>
        /// <param name="task"></param>
        public virtual void AbnormalCallHandel(AbnormalMonitorTask task)
        {
            if (task.GetEntityType().IsNullOrEmpty() || task.SourceDataKeys.IsNullOrEmpty()) return;
            //任务回调
            var call = new TaskhandleEvent
            {
                PubKey = task.SourceDataKeys,
                EntityType = task.GetEntityType(),
                HandelContent = task.GetConfigValue()
            };
            RT.EventBus.Publish(call);
        }


        /// <summary>
        /// 生成异常任务（Spc）
        /// </summary>
        /// <param name="createTaskSpcEvent">参数</param>
        public virtual void CreateTaskBySpc(CreateTaskSpcEvent createTaskSpcEvent)
        {
            var task = Query<AbnormalMonitorTask>().Where(c => c.ProblemDescription == createTaskSpcEvent.ProblemDescription && (c.TaskState == TaskStateEnum.ToDo || c.TaskState == TaskStateEnum.Doing)).FirstOrDefault();
            if (task != null)
            {
                return;
            }
            task = new AbnormalMonitorTask();
            task.GenerateId();
            task.Code = RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateCode();
            task.AbnormalWarnDefineId = createTaskSpcEvent.AbnormalWarnDefineId;
            //task.AbnormalDefineId = config.AbnormalDefine?.Id;
            task.AbnormalName = "SPC判异规则异常任务";
            task.TaskState = TaskStateEnum.ToDo;
            task.ProblemDescription = createTaskSpcEvent.ProblemDescription;
            task.SourceDataKeys = "";
            task.CreateBy = createTaskSpcEvent.UserId;
            task.UpdateBy = createTaskSpcEvent.UserId;
            RF.Save(task);
            //生成任务日志
            AbnormalMonitorTaskLog logGenerateTask = RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateLog(task.Id, TaskStateEnum.ToDo, TaskHandleAction.Action, "生成异常任务".L10N());
            logGenerateTask.HandlerId = createTaskSpcEvent.UserId;
            RF.Save(logGenerateTask);
            //责任人发送预警（异步）
            //var warnDefine = createTaskSpcEvent.AbnormalWarnDefineId;
            var warnDefine = RF.GetById<AbnormalWarnDefine>(createTaskSpcEvent.AbnormalWarnDefineId); 
            if (warnDefine != null)
            {
                Threading.AsyncHelper.InvokeSafe(() =>
                {
                    RT.Service.Resolve<AbnormalMonitorTaskService>().SpcSendForResponsibility(task, warnDefine, createTaskSpcEvent.UserId);
                });

            }
        }
        /// <summary>
        /// 根据业务类型获取异常预警定义
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public virtual double? GetAbnormalWarnDefineId(BusinessType businessType)
        {
            double? result = null;
            if (businessType == BusinessType.RedCard)
            {
                var config = ConfigService.GetConfig(new WarnDefineForRedCardConfig(), typeof(AbnormalWarnDefine));//预警定义
                if (config != null && config.AbnormalWarnDefineId == null)
                    throw new SIE.Domain.Validation.ValidationException("红牌管理-预警定义配置项未配置".L10N());
                result = config?.AbnormalWarnDefineId;
            }
            else if (businessType == BusinessType.SPC) {
                var config = ConfigService.GetConfig(new WarnDefineForSpcConfig(), typeof(AbnormalWarnDefine));//预警定义
                if (config != null && config.AbnormalWarnDefineId == null)
                    throw new SIE.Domain.Validation.ValidationException("Spc-预警定义配置项未配置".L10N());
                result = config?.AbnormalWarnDefineId;
            }
            return result;
        }
    }
}
