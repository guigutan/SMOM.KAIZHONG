using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 推式调度多触发方式同时满足的处理方式
    /// </summary>
    [System.ComponentModel.DisplayName("推式调度多触发方式同时满足的处理方式")]
    [System.ComponentModel.Description("推式调度多触发方式同时满足的处理方式")]
    public class PushedSchedulingMethodsConfig : ModuleConfig<PushedSchedulingMethodsConfigValue>
    {
        private readonly PushedSchedulingMethodsConfigValue defaultValue = new PushedSchedulingMethodsConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override PushedSchedulingMethodsConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
