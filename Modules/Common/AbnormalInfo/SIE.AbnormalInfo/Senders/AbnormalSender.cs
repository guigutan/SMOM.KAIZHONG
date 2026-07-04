using SIE.Alert;
using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.Common.Sender.SystemSender;
using SIE.Domain;
using SIE.AbnormalInfo.AbnormalInfos;
using System;
using System.Linq;

namespace SIE.AbnormalInfo.Senders
{

    /// <summary>
    /// 异常任务推送插件
    /// </summary>
    [RootEntity, Serializable]
    [Sender("异常任务推送插件", typeof(AbnormalSenderConfig), SenderType.SystemTask)]
    //[AlertSystemSender(typeof(AlertSystemSenderConfig))]
    public class AbnormalSender : SystemSenderBase
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
            var alertedId = sysPlug.Severity.AlerterId;
            return new AbnormalSendParam()
            {
                AlertValue = result.Value,
                AlertLevel = severityLevel,
                AlerterId = alertedId
            };
        }

        /// <summary>
        /// 重写方法
        /// </summary>
        /// <param name="param">param</param>
        public override void Send(ISendParam param)
        {
            if (param is AbnormalSendParam)
            {
                var abnormalParam = param as AbnormalSendParam;
                //查询是否有相关异常定义
                AbnormalInfoController abController = RT.Service.Resolve<AbnormalInfoController>();
                var def = abController.GetAlertAbnormalDefinitions(AbnormalSource.Alert, abnormalParam.AlerterId, null).FirstOrDefault();
                if (def != null)
                {
                    //创建异常管理
                    var newAbInfo = CreateNewAbnormalInfo(def);
                    abController.SaveNewAbnormalInfo(newAbInfo);
                }
            }
        }

        /// <summary>
        /// 根据整改任务生成异常信息
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private AbnormalInfor CreateNewAbnormalInfo(AbnormalInfoDefinition def)
        {
            var abnormal = new AbnormalInfor()
            {
                No = RT.Service.Resolve<AbnormalInfoController>().GetNewAbnormalInfoNo(),
                AbnormalStatus = AbnormalStatus.ToProcess,
                IsSendPdca = false,
                IsRectificationTask = false,
                AbnormalInfoDefinitionId = def.Id,
            };
            return abnormal;
        }
    }
}
