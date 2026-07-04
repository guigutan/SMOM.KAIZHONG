using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 是否称重配置项
    /// </summary>
    [DisplayName("是否称重配置项")]
    [Description("用于配置资源工位是否启用称重")]
    public class WeightConfig : ModuleCategoryConfig<ResourceStation, WeightConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override WeightConfigValue DefaultValue { get; } = new WeightConfigValue();
    }

    /// <summary>
    /// 是否称重配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否称重配置值")]
    public class WeightConfigValue : ConfigValue
    {
        #region 是否称重 IsWeight
        /// <summary>
        /// 启用称重
        /// </summary>
        [Label("启用称重")]
        public static readonly Property<bool> IsWeightProperty = P<WeightConfigValue>.Register(e => e.IsWeight);

        /// <summary>
        /// 启用称重
        /// </summary>
        public bool IsWeight
        {
            get { return this.GetProperty(IsWeightProperty); }
            set { this.SetProperty(IsWeightProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>串口</returns>
        public override string Display()
        {
            return IsWeight ? "启用称重".L10N() : "未启用称重".L10N();
        }
    }
}