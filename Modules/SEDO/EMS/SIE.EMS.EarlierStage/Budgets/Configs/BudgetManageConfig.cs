using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EarlierStage.Budgets.Configs
{
    /// <summary>
    /// 预算配置项
    /// </summary>
    [System.ComponentModel.DisplayName("预算管理配置")]
    [System.ComponentModel.Description("用于配置财年结束日期、审批流程")]
    [ConfigForEntity(typeof(Budget))]
    public class BudgetManageConfig : ModuleConfig<BudgetManageConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly BudgetManageConfigValue defaultValue = new BudgetManageConfigValue
        {
            FiscalYearEndDate = new DateTime(2000, 1, 1)
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override BudgetManageConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 预算管理配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("预算管理配置值")]
    public class BudgetManageConfigValue : ConfigValue
    {
        #region 财年结束日期 FiscalYearEndDate
        /// <summary>
        /// 财年结束日期
        /// </summary>
        [Label("财年结束日期")]
        public static readonly Property<DateTime> FiscalYearEndDateProperty = P<BudgetManageConfigValue>.Register(e => e.FiscalYearEndDate);

        /// <summary>
        /// 财年结束日期
        /// </summary>
        public DateTime FiscalYearEndDate
        {
            get { return this.GetProperty(FiscalYearEndDateProperty); }
            set { this.SetProperty(FiscalYearEndDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns>显示值</returns>
        public override string Display()
        {
            var date = FiscalYearEndDate.AddHours(8);
            return "财年结束日期:{0}-{1} {2}:{3}:{4}".L10nFormat(date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
    }
}
