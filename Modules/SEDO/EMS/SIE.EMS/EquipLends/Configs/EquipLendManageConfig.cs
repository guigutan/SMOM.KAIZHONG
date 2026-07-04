using SIE.Common.Configs;
using SIE.Common.Configs.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.EMS.EquipLends.Configs
{
    /// <summary>
    /// 设备借还管理配置项
    /// </summary>
    [DisplayName("设备借还管理配置项")]
    [Description("设备借还管理配置项")]
    public class EquipLendManageConfig : ModuleConfig<EquipLendManageConfigValue>
    {
        readonly EquipLendManageConfigValue defaultValue = new EquipLendManageConfigValue { NoRuleId = null, LendExamine = false, ReturnExamine = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipLendManageConfigValue DefaultValue 
        {  
            get
            {
                return defaultValue; 
            } 
        }
    }
}
