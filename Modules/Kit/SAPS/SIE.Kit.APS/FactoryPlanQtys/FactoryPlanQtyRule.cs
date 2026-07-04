using SIE.APS.Common.Tools;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Kit.APS.FactoryPlanQtys
{
    /// <summary>
    /// 工厂,尺寸不允许重复
    /// </summary>
    [DisplayName("工厂不允许重复)")]
    [Description("工厂不允许重复")]
    public class FactoryPlanQtyNotDuplicateRule : NotDuplicateRuleEx<FactoryPlanQty>
    {
        /// <summary>
        /// 实体作用范围
        /// </summary>
        public FactoryPlanQtyNotDuplicateRule()
        {
            Properties.Add(FactoryPlanQty.FactoryIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var entity = e as FactoryPlanQty;
                return "工厂【{0}】不允许重复!".L10nFormat(entity.Factory == null ? "" : entity.Factory.Name);
            };
        }
    }
}
