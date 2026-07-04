using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.InventoryBalances.Configs
{
    /// <summary>
    /// 报废类型配置
    /// </summary>
    [DisplayName("报废类型配置")]
    [Description("用于配置报废类型")]
    public class BalanceScrapTypeConfig : ModuleConfig<BalanceScrapTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly BalanceScrapTypeConfigValue defaultValue = new BalanceScrapTypeConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override BalanceScrapTypeConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 报废类型配置
    /// </summary>
    [RootEntity, Serializable]
    public class BalanceScrapTypeConfigValue : ConfigValue
    {
        #region 报废类型 ScrapType
        /// <summary>
        /// 报废类型
        /// </summary>
        [Label("报废类型")]
        public static readonly Property<string> ScrapTypeProperty = P<BalanceScrapTypeConfigValue>.Register(e => e.ScrapType);

        /// <summary>
        /// 报废类型
        /// </summary>
        public string ScrapType
        {
            get { return this.GetProperty(ScrapTypeProperty); }
            set { this.SetProperty(ScrapTypeProperty, value); }
        }
        #endregion
    }
}
