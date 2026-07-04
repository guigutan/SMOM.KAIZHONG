using Newtonsoft.Json;
using SIE.Alert;
using SIE.Alert.AlertManages;
using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.Common.Sender.SystemSender;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.Abnormal.SysSenders
{
    /// <summary>
    /// 停线管理推送插件
    /// </summary>
    [RootEntity, Serializable]
    [Sender("停线管理推送插件", typeof(AbnormalCauseSenderConfig), SenderType.SystemTask)]
    [AlertSystemSender(typeof(AbnormalAlertSysSenderConfig))]
    public class AbnormalCauseSender : SystemSenderBase
    {
        /// <summary>
        /// 创建发送参数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="severityLevel"></param>
        /// <param name="paramEntity"></param>
        /// <returns></returns>
        public override ISendParam CreateSendParam(AlertResultBase result, AlertLevel severityLevel, Entity paramEntity)
        {
            var sysPlug = paramEntity as SeveritySysPlug;
            if (sysPlug == null)
                return null;
            List<double> lineIdList = null, equipAccountIdList = null;

            if (result is AbnormalCauseResult)
            {
                //获取预警插件处理后的结果
                var abCauseResult = result as AbnormalCauseResult;
                lineIdList = abCauseResult.LineIdList;
                equipAccountIdList = abCauseResult.EquipAccountIdList;
            }
            else
            {
                //默认获取触发任务配置项
                if (sysPlug.Config != null)
                {
                    AbnormalAlertSysSenderConfig abConfig = new AbnormalAlertSysSenderConfig();
                    abConfig.Initialize(sysPlug.Config);
                    if (abConfig.LineId.HasValue)
                        lineIdList = new List<double>() { abConfig.LineId.Value };
                    if (abConfig.EquipAccountId.HasValue)
                        equipAccountIdList = new List<double>() { abConfig.EquipAccountId.Value };
                }
            }
            var alerterId = sysPlug.Severity.AlerterId;
            var alertManageId = AppRuntime.Service.Resolve<AlertManageController>().GetActiveAlertManage(alerterId)?.Id;
            return new AbnormalCauseSendParam()
            {
                LineIdList = lineIdList,
                EquipAccountIdList = equipAccountIdList,
                AlerterId = alerterId,
                AlertManageId = alertManageId
            };
        }

        /// <summary>
        /// 重写方法
        /// </summary>
        /// <param name="param">param</param>
        public override void Send(ISendParam param)
        {
            if (param is AbnormalCauseSendParam)
            {
                EntityList<AbnormalCause> abnormalCauses = new EntityList<AbnormalCause>();
                var abParam = param as AbnormalCauseSendParam;
                DateTime curTime = DateTime.Now;
                if (abParam.AlerterId == null)
                    throw new ValidationException("预警配置Id不能为空。".L10N());
                var alertId = abParam.AlerterId.Value;
                var alerter = RF.GetById<Alerter>(alertId);
                if (alerter == null)
                    throw new ValidationException("预警配置不能为空。".L10N());
                var alertManageId = abParam.AlertManageId;
                if (abParam.LineIdList.IsNotEmpty())
                {
                    foreach (var lineId in abParam.LineIdList)
                    {
                        abnormalCauses.Add(CreateAbnormalCauseByLine(lineId, alerter, curTime, alertManageId));
                    }
                }
                if (abParam.EquipAccountIdList.IsNotEmpty())
                {
                    foreach (var equipId in abParam.EquipAccountIdList)
                    {
                        abnormalCauses.Add(CreateAbnormalCauseByEquip(equipId, alerter, curTime, alertManageId));
                    }
                }
                if (abnormalCauses.IsNotEmpty())
                {
                    AppRuntime.Service.Resolve<AbnormalCauseController>().SaveAlertAbnormalCauses(abnormalCauses);
                }
                else
                {
                    RT.Logger.Warn("预警管理触发异常停线任务时，没有产线或设备信息。".L10N());
                }
            }
        }

        /// <summary>
        /// 预警取消处理
        /// </summary>
        /// <param name="paramJson">推送时参数</param>
        public override void onAutoCancel(string paramJson)
        {
            if (paramJson.IsNullOrEmpty())
                return;
            try
            {
                var abParam = JsonConvert.DeserializeObject<AbnormalCauseSendParam>(paramJson);
                if (abParam != null)
                {
                    if (abParam.AlerterId == null)
                        throw new ValidationException("预警配置Id不能为空。".L10N());
                    if (abParam.LineIdList.IsNotEmpty() || abParam.EquipAccountIdList.IsNotEmpty())
                    {
                        AppRuntime.Service.Resolve<AbnormalCauseController>().RestoreAbnormalCauseAuto(abParam.LineIdList, abParam.EquipAccountIdList);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogManager.Logger.Error("解除预警时恢复停线失败。".L10N(), ex);
            }
        }

        /// <summary>
        /// 根据产线创建异常停线
        /// </summary>
        /// <param name="lineId">产线</param>
        /// <param name="alert">预警配置</param>
        /// <param name="curTime">当前时间</param>
        /// <param name="alertManageId">预警管理ID</param>
        /// <returns></returns>
        private AbnormalCause CreateAbnormalCauseByLine(double? lineId, Alerter alert, DateTime curTime, double? alertManageId)
        {
            AbnormalCause abnormal = new AbnormalCause()
            {
                ResourceId = lineId.Value,
                SourceType = ExceptionStopSourceType.Alerter,
                ExceptionStopType = ExceptionStopType.StopLine,
                BeginDate = curTime,
                AlerterId = alert.Id,
                AbnormalReason = alert?.Name,
                AlerterManageId = alertManageId
            };
            return abnormal;
        }

        /// <summary>
        /// 根据设备创建异常停线
        /// </summary>
        /// <param name="equipId">产线</param>
        /// <param name="alert">预警配置</param>
        /// <param name="curTime">当前时间</param>
        /// <param name="alertManageId">预警管理ID</param>
        /// <returns></returns>
        private AbnormalCause CreateAbnormalCauseByEquip(double? equipId, Alerter alert, DateTime curTime, double? alertManageId)
        {
            AbnormalCause abnormal = new AbnormalCause()
            {
                EquipAccountId = equipId.Value,
                SourceType = ExceptionStopSourceType.Alerter,
                ExceptionStopType = ExceptionStopType.StopLine,
                BeginDate = curTime,
                AlerterId = alert.Id,
                AbnormalReason = alert?.Name,
                AlerterManageId = alertManageId
            };
            return abnormal;
        }
    }
}
