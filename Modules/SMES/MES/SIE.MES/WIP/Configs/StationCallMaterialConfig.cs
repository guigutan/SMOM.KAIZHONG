using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 工位叫料配置
    /// </summary>
    [DisplayName("工位叫料配置")]
    [Description("配置工位叫料方式，自动/手动，默认手动")]
    public class StationCallMaterialConfig : ModuleConfig<StationCallMaterialConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override StationCallMaterialConfigValue DefaultValue { get; } = new StationCallMaterialConfigValue() { IsAuto = false };
    }

    /// <summary>
    /// 工位叫料配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位叫料配置值")]
    public class StationCallMaterialConfigValue : ConfigValue
    {
        #region 是否自动 IsAuto
        /// <summary>
        /// 是否自动
        /// </summary>
        [Label("是否自动")]
        public static readonly Property<bool> IsAutoProperty = P<StationCallMaterialConfigValue>.Register(e => e.IsAuto);

        /// <summary>
        /// 是否自动
        /// </summary>
        public bool IsAuto
        {
            get { return this.GetProperty(IsAutoProperty); }
            set { this.SetProperty(IsAutoProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>叫料方式</returns>
        public override string Display()
        {
            return IsAuto ? "自动叫料".L10N() : "手动叫料".L10N();
        }
    }
}