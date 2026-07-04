using MimeKit;
using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.ISript;
using SIE.Senders;
using System;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalInfos.Pushers
{
    /// <summary>
    /// 异常处理延迟推送升级插件类
    /// </summary>
    [Serializable]
    [Sender("异常处理延迟推送升级插件", typeof(AbnormalInfoEmailSenderConfig), SenderType.Email)]
    public class AbnormalInfoEmailSender : EmailSender
    {
        /// <summary>
        /// 创建发送参数对象
        /// </summary>
        /// <param name="result">预警结果</param>
        /// <param name="param">接收信息参数</param>
        /// <returns>发送参数对象</returns>
        public override ISendParam CreateSendParam(AlertResultBase result, ReceiveParam param)
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
                    //var razorScript = RT.Service.Resolve<IRazorScript>();
                    //emailSendParam.Subject = razorScript.Parse(emailMessageTemplate.Subject, result);
                    emailSendParam.Subject = emailMessageTemplate.Subject;
                    if (!emailMessageTemplate.Message.IsNullOrEmpty() && !emailMessageTemplate.Message.IsNullOrWhiteSpace())   //当推送方式的信息模板中的内容为空时，不做转换，避免内容为空转换过程中引起Razor编译模板失败的错误；在SQL SERVER环境下，信息模板未填写任何内容时，解析出来的JSON字符串中Message的取值是null，转换过程中会引起值不能为null的错误，故当信息模板内容为空白时不做转换

                        emailSendParam.Body = emailMessageTemplate.Message;
                }
            }
            ProcessEmailSendParam(emailSendParam, result, param);
            return emailSendParam;
        }

        /// <summary>
        /// 处理邮件对象数据
        /// </summary>
        /// <param name="emailSendParam">邮件对象数据</param>
        /// <param name="result">预警参数</param>
        /// <param name="param">接收参数</param>
        /// <returns>邮件推送参数</returns>
        public ISendParam ProcessEmailSendParam(EmailSendParam emailSendParam, AlertResultBase result, ReceiveParam param)
        {
            AbnormalInfoAlertResult alertResult = result as AbnormalInfoAlertResult;
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("您好! ".L10N() + "<br/>");
            strBuilder.Append(Environment.NewLine);
            if (alertResult != null)
            {
                foreach (var item in alertResult.AlertInfoList)
                {
                    strBuilder.Append(string.Format("{0}: {1}" + "<br/>", item.Name, item.Value));
                    strBuilder.Append(Environment.NewLine);
                }
            }
            emailSendParam.Body = strBuilder.ToString();
            return emailSendParam;
        }
    }
}
