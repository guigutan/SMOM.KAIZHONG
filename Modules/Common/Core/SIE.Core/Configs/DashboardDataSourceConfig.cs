using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 看板数据来源配置
    /// </summary>
    [System.ComponentModel.DisplayName("看板数据来源配置")]
    [System.ComponentModel.Description("看板数据来源配置")]
    public class DashboardDataSourceConfig : GlobalConfig<DashboardDataSourceConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly DashboardDataSourceConfigValue defaultValue = new DashboardDataSourceConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DashboardDataSourceConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// RFID类型配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("看板数据来源")]
    public class DashboardDataSourceConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DashboardDataSourceConfigValue()
        {
            DashboardDataSourceType = DashboardDataSourceType.FromProdution;
        }

        /// <summary>
        /// 看板数据来源类型
        /// </summary>
        [Label("看板数据来源类型")]
        public static readonly Property<DashboardDataSourceType> DashboardDataSourceTypeProperty = P<DashboardDataSourceConfigValue>.Register(e => e.DashboardDataSourceType);

        /// <summary>
        /// 看板数据来源类型
        /// </summary>
        public DashboardDataSourceType DashboardDataSourceType
        {
            get { return this.GetProperty(DashboardDataSourceTypeProperty); }
            set { this.SetProperty(DashboardDataSourceTypeProperty, value); }
        }

        /// <summary>
        /// 显示看板数据来源类型
        /// </summary>
        /// <returns>返回看板数据来源类型</returns>
        public override string Display()
        {
            return DashboardDataSourceType.ToLabel().L10N();
        }
    }
}
