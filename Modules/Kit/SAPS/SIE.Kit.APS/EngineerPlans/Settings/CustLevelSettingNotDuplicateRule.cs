using SIE.APS.Common.Tools;
using SIE.MetaModel;
using System;

namespace SIE.Kit.APS.EngineerPlan.Settings
{
    /// <summary>
    /// 【工厂】【客户】【优先级】不允许重复
    /// </summary>
    [System.ComponentModel.DisplayName("【工厂】【客户】【优先级】不允许重复")]
    [System.ComponentModel.Description("【工厂】【客户】【优先级】不允许重复")]
    public class CustLevelSettingNotDuplicateRule : NotDuplicateRuleEx<CustLevelSetting>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustLevelSettingNotDuplicateRule()
        {
            Properties.Add(CustLevelSetting.FactoryIdProperty);
            Properties.Add(CustLevelSetting.CustomerIdProperty);
            Properties.Add(CustLevelSetting.CustLevelIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;

            MessageBuilder = e =>
            {
                var entity = e as CustLevelSetting;
                return "工厂【{0}】客户【{1}】优先级【{2}】不允许重复!".L10nFormat(entity.Factory == null ? "" : entity.Factory.Name, entity.Customer == null ? "" : entity.Customer.Name, entity.CustLevel == null ? "" : entity.CustLevel.LevelName);
            };
        }
    }
}
