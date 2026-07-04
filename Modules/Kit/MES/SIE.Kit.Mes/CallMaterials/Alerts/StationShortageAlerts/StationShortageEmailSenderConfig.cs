using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text.RegularExpressions;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工位缺料预警邮件推送插件配置类
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位缺料预警邮件推送插件配置类")]
    public class StationShortageEmailSenderConfig : CallMaterialEmailSenderConfig
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">邮件推送插件对象Json字符串</param>
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
    /// 工位缺料预警邮件推送插件配置实体配置类
    /// </summary>
    internal class StationShortageEmailSenderConfigEntityConfig : EntityConfig<StationShortageEmailSenderConfig>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">验证规则声明</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(StationShortageEmailSenderConfig.SendFromProperty, new RegexMatchRule()
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