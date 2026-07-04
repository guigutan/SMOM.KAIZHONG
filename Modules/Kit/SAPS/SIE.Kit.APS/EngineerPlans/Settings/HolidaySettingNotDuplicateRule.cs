using SIE.APS.Common.Tools;
using SIE.MetaModel;
using System;

namespace SIE.Kit.APS.EngineerPlan.Settings
{
    /// <summary>
    /// 区间不能重复项验证
    /// </summary>
    [System.ComponentModel.DisplayName("区间不能重复项验证")]
    [System.ComponentModel.Description("区间不能重复项验证")]
    public class HolidaySettingNotDuplicateRule : NotDuplicateRuleEx<HolidaySetting>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HolidaySettingNotDuplicateRule()
        {
            Properties.Add(HolidaySetting.FactoryIdProperty);
            Properties.Add(HolidaySetting.StartDateProperty);
            Properties.Add(HolidaySetting.EndDateProperty);
            Scope = EntityStatusScopes.Update | EntityStatusScopes.Add;

            MessageBuilder = (e) =>
            {
                var entity = e as HolidaySetting;
                return "工厂【{0}】同一区间不允许重复".L10nFormat(entity.Factory == null ? "" : entity.Factory.Name);
            };
        }
    }
}
