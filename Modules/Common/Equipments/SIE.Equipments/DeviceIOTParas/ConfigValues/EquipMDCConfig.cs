using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.DeviceIOTParas.ConfigValues
{
    /// <summary>
    /// 设备WebApi地址配置
    /// </summary>
    [System.ComponentModel.DisplayName("MDCWebApi地址配置值")]
    [System.ComponentModel.Description("用于设置MDCWebApi地址")]
    public class EquipMDCConfig : ModuleConfig<EquipMDCConfigValue>
    { /// <summary>
      /// 默认值
      /// </summary>
        readonly EquipMDCConfigValue defaultValue = new EquipMDCConfigValue { Url = "" };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override EquipMDCConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
