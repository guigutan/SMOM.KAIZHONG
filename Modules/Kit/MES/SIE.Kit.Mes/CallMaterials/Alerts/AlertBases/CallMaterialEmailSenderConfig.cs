using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Senders;
using System;
using System.Text.RegularExpressions;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 物料预警邮件推送配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料预警邮件推送配置")]
    public class CallMaterialEmailSenderConfig : EmailSenderConfig
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">配置JOSN字符串</param>
        public override void Initialize(string value)
        {
            base.Initialize(value);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string ret = base.ToString();
            return ret;
        }
    }

    /// <summary>
    ///  物料预警邮件推送配置实体配置类
    /// </summary>
    internal class CallMaterialEmailSenderConfigEntityConfig : EntityConfig<CallMaterialEmailSenderConfig>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">验证规则声明</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(CallMaterialEmailSenderConfig.SendFromProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9\._-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "邮箱格式不正确";
                }
            });
        }
    }
}
