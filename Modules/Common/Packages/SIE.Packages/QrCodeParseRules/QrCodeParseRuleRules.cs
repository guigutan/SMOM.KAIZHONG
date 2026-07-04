using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("二维码解析规则验证规则")]
    [System.ComponentModel.Description("二维码解析规则验证规则")]
    public class QrCodeParseRuleRules : EntityRule<QrCodeParseRule>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public QrCodeParseRuleRules()
        {
            ConnectToDataSource = true;
            Property = QrCodeParseRule.IdProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var qrCodeRule = entity as QrCodeParseRule;
            RT.Service.Resolve<QrCodeParseRuleController>().ValidQrCodeRule(qrCodeRule);
        }
    }
}
