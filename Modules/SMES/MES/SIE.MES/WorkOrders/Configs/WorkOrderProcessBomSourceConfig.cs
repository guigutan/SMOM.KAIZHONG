using SIE.Common.Configs;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;

namespace SIE.MES.WorkOrders.Configs
{    /// <summary>
    /// 工单工序BOM配置项
    /// </summary>
    [System.ComponentModel.DisplayName("工单工序BOM类型配置项")]
    [System.ComponentModel.Description("工单工序BOM类型配置项")]
    [ConfigForEntity(typeof(WorkOrder))]
    public class WorkOrderProcessBomSourceConfig : ModuleConfig<WorkOrderProcessBomSourceConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly WorkOrderProcessBomSourceConfigValue defaultValue = new WorkOrderProcessBomSourceConfigValue { ProcessBomType = WorkOrderBomSourceType.RoutingProcessBom };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override WorkOrderProcessBomSourceConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
