using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Configs
{
    /// <summary>
    /// QT调度获取条码时间范围配置项
    /// </summary>
    [DisplayName("QT调度获取条码时间范围配置项")]
    [Description("QT调度获取条码时间范围配置项")]
    public class QTimeJobTimeRangeConfig : ModuleConfig<QTimeJobTimeRangeConfigValue>
    {
        readonly QTimeJobTimeRangeConfigValue defaultValue = new QTimeJobTimeRangeConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override QTimeJobTimeRangeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
