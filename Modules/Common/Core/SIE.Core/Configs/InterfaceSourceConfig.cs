using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 启用接口配置
    /// </summary>
    [System.ComponentModel.DisplayName("启用接口配置")]
    [System.ComponentModel.Description("启用接口配置")]
    public class InterfaceSourceConfig : GlobalConfig<InterfaceSourceConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly InterfaceSourceConfigValue defaultValue = new InterfaceSourceConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override InterfaceSourceConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 启用接口配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("启用接口配置")]
    public class InterfaceSourceConfigValue : ConfigValue
    {

        /// <summary>
        /// 启用接口配置
        /// </summary>
        [Label("启用接口")]
        public static readonly Property<InterfaceSourceType?> InterfaceSourceTypeProperty = P<InterfaceSourceConfigValue>.Register(e => e.InterfaceSourceType);

        /// <summary>
        /// 启用接口配置
        /// </summary>
        public InterfaceSourceType? InterfaceSourceType
        {
            get { return this.GetProperty(InterfaceSourceTypeProperty); }
            set { this.SetProperty(InterfaceSourceTypeProperty, value); }
        }

        /// <summary>
        /// 启用接口配置
        /// </summary>
        /// <returns>启用接口配置</returns>
        public override string Display()
        {
            return InterfaceSourceType.ToLabel().L10N();
        }
    }
}
