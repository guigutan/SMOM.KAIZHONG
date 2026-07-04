using SIE.Common.Sender;
using SIE.Senders;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件管理邮件推送
    /// </summary>
    [Serializable]
    [Sender("文件管理邮件推送", typeof(EmailSenderConfig), SenderType.Email)]
    public class FileManageSender : EmailSender
    {
    }
}
