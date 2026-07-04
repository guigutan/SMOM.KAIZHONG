using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 
    /// </summary>
    public class FixtureAcceptanceItemRules
    {

    }
    #region 验收项目名称非重复验证规则
    /// <summary>
    /// 验收项目名称非重复验证规则
    /// </summary>
    [DisplayName("验收项目名称非重复验证规则")]
    [Description("验收项目名称非重复验证规则")]
    public class FixtureAcceptanceItemNotDuplicateRule : NotDuplicateRule<FixtureAcceptanceItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixtureAcceptanceItemNotDuplicateRule()

        {
            Properties.Add(FixtureAcceptanceItem.FixtureAcceptanceIdProperty);
            Properties.Add(FixtureAcceptanceItem.ItemNameProperty);
            MessageBuilder = (e) => { return "同一个验收单下的校验项目名称需唯一".L10N(); };
        }
    }
    #endregion
}
