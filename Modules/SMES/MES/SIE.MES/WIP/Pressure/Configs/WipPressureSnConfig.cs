using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using SIE.Packages.Packings.Enums;
using System;
using System.ComponentModel;

namespace SIE.MES.WIP.Pressure.Configs
{
    /// <summary>
    /// SN生成规则配置项
    /// </summary>
    [DisplayName("SN生成规则配置项")]
    [Description("用于SN生成的具体规则,具体规则详细请在条码规则进行配置")]
    public class WipPressureSnConfig : NoConfig
    {
    }

}