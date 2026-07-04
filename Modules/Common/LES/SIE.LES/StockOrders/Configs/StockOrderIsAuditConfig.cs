using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 手工单据是否需审核配置
    /// </summary>
    [System.ComponentModel.DisplayName("手工单据是否需审核配置")]
    [System.ComponentModel.Description("手工单据是否需审核配置")]
    public class StockOrderIsAuditConfig : ModuleConfig<StockOrderIsAuditConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly StockOrderIsAuditConfigValue defaultValue = new StockOrderIsAuditConfigValue { IsAudit = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override StockOrderIsAuditConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
