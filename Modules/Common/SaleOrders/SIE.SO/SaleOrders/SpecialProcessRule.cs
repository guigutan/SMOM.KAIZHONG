using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Utils;
using System;
using System.ComponentModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 特殊工艺验证规则
    /// </summary>
    [DisplayName("特殊工艺验证")]
    [Description("特殊工艺层数只有1和偶数")]
    public class SaleOrderDetailLayerNumberRule : EntityRule<SpecialProcess>
    {
        /// <summary>
        /// 实体作用范围
        /// </summary>
        public SaleOrderDetailLayerNumberRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 验证特殊工艺层数只有1和偶数
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var detail = entity as SpecialProcess;
            if (detail != null)
            {
                if (detail.Process == Process.NumberLayer)
                {
                    if (detail.Value != 1 && detail.Value % 2 != 0)
                    {
                        e.BrokenDescription = "特殊工艺层数只有1或者偶数!".L10N();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 特殊工艺验证规则
    /// </summary>
    [DisplayName("重复验证")]
    [Description("特殊工艺不能重复")]
    public class SaleOrderDetailProcessRule : NotDuplicateRule<SpecialProcess>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public SaleOrderDetailProcessRule()
        {
            Properties.Add(SpecialProcess.SaleOrderDetailIdProperty);
            Properties.Add(SpecialProcess.ProcessProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as SpecialProcess;
                return "特殊工艺[{0}]不能重复".L10nFormat(EnumViewModel.EnumToLabel(r.Process));
            };
        }
    }
}
