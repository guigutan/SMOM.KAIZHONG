using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.Displays.Configs
{
    /// <summary>
    /// ESOP文档来源
    /// </summary>
    [DisplayName("ESOP文档来源")]
    [Description("ESOP文档来源")]
    public class DisplayPointDataConfig : ModuleConfig<DisplayPointDataConfigValue>
    {
        readonly DisplayPointDataConfigValue defaultValue = new DisplayPointDataConfigValue { DataFrom = Enums.DisplayDataSource.Document };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DisplayPointDataConfigValue DefaultValue 
        {
            get { return defaultValue; }
        }
    }
}
