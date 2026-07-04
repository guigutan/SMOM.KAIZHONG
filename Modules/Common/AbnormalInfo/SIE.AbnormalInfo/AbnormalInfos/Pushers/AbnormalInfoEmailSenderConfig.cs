using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Senders;
using System;
using System.Text.RegularExpressions;

namespace SIE.AbnormalInfo.AbnormalInfos.Pushers
{
    /// <summary>
    /// 异常处理延迟推送升级配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("异常处理延迟推送升级配置")]
    public class AbnormalInfoEmailSenderConfig : EmailSenderConfig
    {
      
    }

    /// <summary>
    /// 异常信息邮件推送配置
    /// </summary>
    internal class AbnormalInfoEmailSenderConfigEntityConfig : EntityConfig<AbnormalInfoEmailSenderConfig>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">验证规则声明</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(AbnormalInfoEmailSenderConfig.SendFromProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9\._-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "邮箱格式不正确".L10N();
                }
            });
        }
    }
}
