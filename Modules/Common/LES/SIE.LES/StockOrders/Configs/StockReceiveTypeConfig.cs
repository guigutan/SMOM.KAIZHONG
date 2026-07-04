using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 接收方式的默认值
    /// </summary>
    [System.ComponentModel.DisplayName("接收方式的默认值")]
    [System.ComponentModel.Description("接收方式的默认值")]
    public class StockReceiveTypeConfig : ModuleConfig<StockReceiveTypeConfigValue>
    {
        private readonly StockReceiveTypeConfigValue defaultValue = new StockReceiveTypeConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override StockReceiveTypeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
