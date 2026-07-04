using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.GlobalConfigs
{
    /// <summary>
    /// 设备点检、保养异常PDCA管控配置
    /// </summary>
    [System.ComponentModel.DisplayName("设备点检、保养异常PDCA管控配置")]
    [System.ComponentModel.Description("设备点检、保养异常PDCA管控配置")]
    public class CheckAndMaintainPdcaConfig : GlobalConfig<CheckAndMaintainPdcaConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckAndMaintainPdcaConfigValue defaultValue = new CheckAndMaintainPdcaConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckAndMaintainPdcaConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 设备点检和保养异常PDCA管控配置的配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备点检和保养异常PDCA管控")]
    public class CheckAndMaintainPdcaConfigValue : ConfigValue
    {
        #region 设备点检和保养异常PDCA管控 Pdca
        /// <summary>
        /// 设备点检、保养异常PDCA管控
        /// </summary>
        [Label("设备点检和保养异常PDCA管控")]
        public static readonly Property<YesNo> PdcaProperty = P<CheckAndMaintainPdcaConfigValue>.Register(e => e.Pdca);

        /// <summary>
        /// 设备点检、保养异常PDCA管控
        /// </summary>
        public YesNo Pdca
        {
            get { return this.GetProperty(PdcaProperty); }
            set { this.SetProperty(PdcaProperty, value); }
        }
        #endregion

    }
}
