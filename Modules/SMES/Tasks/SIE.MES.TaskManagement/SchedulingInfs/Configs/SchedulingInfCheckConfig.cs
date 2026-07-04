using SIE.Common.Configs;
using SIE.Common.Configs.Module;
using SIE.MES.WorkOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs.Configs
{
    /// <summary>
    /// 排程校验规则
    /// </summary>
    [System.ComponentModel.DisplayName("排程校验规则")]
    [System.ComponentModel.Description("用于选择排程校验")]
    public class SchedulingInfCheckConfig : ModuleConfig<SchedulingInfCheckConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly SchedulingInfCheckConfigValue defaultValue = new SchedulingInfCheckConfigValue();

        /// <summary>
        /// 默认值 
        /// </summary>
        public override SchedulingInfCheckConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
