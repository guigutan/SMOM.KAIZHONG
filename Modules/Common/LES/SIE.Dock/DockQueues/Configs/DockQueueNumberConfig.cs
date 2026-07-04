using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockQueues.Configs
{
    /// <summary>
    /// 月台排队号生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("月台排队号生成规则")]
    [System.ComponentModel.Description("月台排队号生成规则")]
    public class DockQueueNumberConfig : ModuleConfig<DockQueueNumberConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly DockQueueNumberConfigValue defaultValue  = new DockQueueNumberConfigValue { NumberRule = null, PrintTemplate = null, MaxDelayNum = 2, CheckOutTimeOut = 4, CheckOutDelay = 10, AutoCheckIn = true };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DockQueueNumberConfigValue DefaultValue
        {
            get { return defaultValue; }    
        }
    }
}
