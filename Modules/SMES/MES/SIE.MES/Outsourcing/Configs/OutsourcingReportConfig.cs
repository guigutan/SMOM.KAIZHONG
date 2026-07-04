using SIE.Common.Configs;
using SIE.MES.WorkOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Configs
{
    /// <summary>
    /// 委外报工配置项
    /// </summary>
    [System.ComponentModel.DisplayName("委外报工配置项")]
    [System.ComponentModel.Description("委外报工配置项")]
    public class OutsourcingReportConfig: ModuleConfig<OutsourcingReportConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly OutsourcingReportConfigValue defaultValue = new OutsourcingReportConfigValue { IsInAutoReport = null, IsValidOutsourcingReportLog = false, IsOutsourcingInsVaildReportLog = false };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override OutsourcingReportConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
