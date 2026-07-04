using MimeKit;
using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.Senders;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 物料预警邮件推送插件类
    /// </summary>
    [Serializable]
    [Sender("物料预警邮件推送插件", typeof(CallMaterialEmailSenderConfig), SenderType.Email)]
    public abstract class CallMaterialEmailSender : EmailSender
    {
        /// <summary>
        /// 创建邮件消息模板对象
        /// </summary>
        /// <returns>信息模板</returns>
        public override MessageTemplateBase CreateMessageTemplate()
        {
            return base.CreateMessageTemplate();
        }

        /// <summary>
        /// 处理邮件对象数据
        /// </summary>
        /// <param name="emailSendParam">邮件对象数据</param>
        /// <param name="result">预警参数</param>
        /// <param name="param">接收参数</param>
        /// <returns>邮件推送参数</returns>
        public abstract ISendParam ProcessEmailSendParam(EmailSendParam emailSendParam, AlertResultBase result, ReceiveParam param);

        /// <summary>
        /// 创建发送参数对象
        /// </summary>
        /// <param name="result">预警结果</param>
        /// <param name="param">接收信息参数</param>
        /// <returns>发送参数对象</returns>
        public override ISendParam CreateSendParam(AlertResultBase result, ReceiveParam param)
        {
            EmailSendParam emailSendParam = (EmailSendParam)CreateEmailSendParam(result, param);
            return emailSendParam;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="param">发送参数</param>
        public override void Send(ISendParam param)
        {
            base.Send(param);
        }

        /// <summary>
        /// 创建物料预警邮件发送参数对象
        /// </summary>
        /// <param name="result">预警结果</param>
        /// <param name="param">接收信息参数</param>
        /// <returns>发送参数对象</returns>
        public ISendParam CreateEmailSendParam(AlertResultBase result, ReceiveParam param)
        {
            var emailSendParam = new EmailSendParam();

            for (int i = 0; i < param.Employees.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(param.Employees[i].Email))
                    emailSendParam.SendTos.Add(new MailboxAddress(param.Employees[i].Email));
            }

            emailSendParam.Attachments = param.EmailAttachmentCollection;

            if (!param.MessageTemplateJson.IsNullOrWhiteSpace())
            {
                var emailMessageTemplate = JsonConvert.DeserializeObject<EmailMessageTemplate>(param.MessageTemplateJson);
                if (emailMessageTemplate != null)
                {
                    emailSendParam.Subject = emailMessageTemplate.Subject;
                    emailSendParam.Body = emailMessageTemplate.Message;
                }
            }

            return emailSendParam;
        }
    }
}
