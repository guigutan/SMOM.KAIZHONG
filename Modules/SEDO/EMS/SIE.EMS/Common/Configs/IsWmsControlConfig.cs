using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Common.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("备件启用WMS管控")]
    [System.ComponentModel.Description("用于配置备件是否启用WMS进行管控")]
    public class IsWmsControlConfig : GlobalConfig<IsWmsControlConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsWmsControlConfigValue defaultValue = new IsWmsControlConfigValue { IsWmsControl = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsWmsControlConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("备件是否启用WMS管控")]
    public class IsWmsControlConfigValue : ConfigValue
    {
        #region 备件是否启用WMS管控 IsWmsControl
        /// <summary>
        /// 备件是否启用WMS管控
        /// </summary>
        [Label("备件是否启用WMS管控")]
        public static readonly Property<YesNo> IsWmsControlProperty = P<IsWmsControlConfigValue>.Register(e => e.IsWmsControl);

        /// <summary>
        /// 备件是否启用WMS管控
        /// </summary>
        public YesNo IsWmsControl
        {
            get { return this.GetProperty(IsWmsControlProperty); }
            set { this.SetProperty(IsWmsControlProperty, value); }
        }
        #endregion


        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsWmsControl == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
