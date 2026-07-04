using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.Domain.Validation;
using SIE.Senders;
using System;
using System.Text;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工位缺料预警邮件推送插件类
    /// </summary>
    [Serializable]
    [Sender("工位缺料预警邮件推送插件", typeof(StationShortageEmailSenderConfig), SenderType.Email)]
    public class StationShortageEmailSender : CallMaterialEmailSender
    {
        /// <summary>
        /// 创建邮件消息模板对象
        /// </summary>
        /// <returns>信息模板</returns>
        public override MessageTemplateBase CreateMessageTemplate()
        {
            var messageTemplate = base.CreateMessageTemplate();
            return messageTemplate;
        }

        /// <summary>
        /// 创建发送参数对象
        /// </summary>
        /// <param name="result">预警结果</param>
        /// <param name="param">接收信息参数</param>
        /// <returns>发送参数对象</returns>
        public override ISendParam CreateSendParam(AlertResultBase result, ReceiveParam param)
        {
            EmailSendParam emailSendParam = (EmailSendParam)base.CreateSendParam(result, param);
            ProcessEmailSendParam(emailSendParam, result, param);

            return emailSendParam;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="param">发送参数</param>
        public override void Send(ISendParam param)
        {
            try
            {
                base.Send(param);
            }
            catch (Exception ex)
            {
                //修改异常信息
                Logging.LogManager.Logger.Error("发送邮件失败。" + ex.GetExceptionDetail());
                throw new ValidationException("邮件推送配置错误，请检查。".L10N());
            }
        }

        /// <summary>
        /// 处理邮件对象数据
        /// </summary>
        /// <param name="emailSendParam">邮件对象数据</param>
        /// <param name="result">预警参数</param>
        /// <param name="param">接收参数</param>
        /// <returns>邮件推送参数</returns>
        public override ISendParam ProcessEmailSendParam(EmailSendParam emailSendParam, AlertResultBase result, ReceiveParam param)
        {
            StationShortageAlertResult stationResult = result as StationShortageAlertResult;
            var emailBodys = emailSendParam.Body.Split('*');
            int length = emailBodys.Length;
            int index = 0;
            StringBuilder strBuilder = new StringBuilder();
            if (stationResult != null)
            {
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", param.Level.ToLabel()));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", stationResult.StationCode));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", stationResult.Line));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", stationResult.WorkOrderNO));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", stationResult.StationCode));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", stationResult.AlertMaterialCode));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", stationResult.AlertMaterialName));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", param.Level.ToLabel()));
                if (index < length)
                    strBuilder.Append(emailBodys[index].Replace("XXX", stationResult.AlertTime.ToString()));
            }
            emailSendParam.Body = strBuilder.ToString();
            return emailSendParam;
        }
    }
}
