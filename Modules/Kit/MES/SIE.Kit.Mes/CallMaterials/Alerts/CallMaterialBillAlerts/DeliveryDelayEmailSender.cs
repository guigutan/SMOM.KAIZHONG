using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.Domain.Validation;
using SIE.Senders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 叫料单配送超时邮件推送插件类
    /// </summary>
    [Serializable]
    [Sender("叫料单配送超时邮件推送插件", typeof(DeliveryDelayEmailSenderConfig), SenderType.Email)]
    public class DeliveryDelayEmailSender : CallMaterialEmailSender
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
            DeliveryDelayAlertResult delayAlertResult = result as DeliveryDelayAlertResult;
            var emailBodys = emailSendParam.Body.Split('*');
            int length = emailBodys.Length;
            int index = 0;
            StringBuilder strBuilder = new StringBuilder();
            if (delayAlertResult != null)
            {
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", param.Level.ToLabel()));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", delayAlertResult.StationCode));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", delayAlertResult.CallMaterialBillNo));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", delayAlertResult.Line));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", delayAlertResult.WorkOrderNO));
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", delayAlertResult.StationCode));
                ProcessEmailSendAlertMaterials(strBuilder, emailBodys[index++], emailBodys[index++], delayAlertResult.MaterialAlerts);
                if (index < length)
                    strBuilder.Append(emailBodys[index++].Replace("XXX", param.Level.ToLabel()));
                if (index < length)
                    strBuilder.Append(emailBodys[index].Replace("XXX", delayAlertResult.AlertTime.ToString()));
            }
            emailSendParam.Body = strBuilder.ToString();
            return emailSendParam;
        }

        /// <summary>
        /// 处理邮件主体多个物料添加
        /// </summary>
        /// <param name="strBuilder">字符串集合对象</param>
        /// <param name="emailMaterialCode">物料编码信息</param>
        /// <param name="emailMaterialName">物料名称信息</param>
        /// <param name="alertMaterials">预警物料信息</param>
        public void ProcessEmailSendAlertMaterials(StringBuilder strBuilder, string emailMaterialCode, string emailMaterialName, List<MaterialAlert> alertMaterials)
        {
            foreach (var alertItem in alertMaterials)
            {
                strBuilder.Append(emailMaterialCode.Replace("XXX", alertItem.MaterialCode));
                strBuilder.Append(emailMaterialName.Replace("XXX", alertItem.MaterialName));
            }
        }
    }
}
