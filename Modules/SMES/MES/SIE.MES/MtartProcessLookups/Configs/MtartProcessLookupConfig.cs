using SIE.Common.Configs;
using SIE.MES.WorkOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.MtartProcessLookups.Configs
{
    /// <summary>
    /// 物料分类与工序关系对照表配置项
    /// </summary>
    [System.ComponentModel.DisplayName("物料分类与工序关系对照表配置项")]
    [System.ComponentModel.Description("物料分类与工序关系对照表配置项")]
    public class MtartProcessLookupConfig : ModuleConfig<MtartProcessLookupConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MtartProcessLookupConfigValue defaultValue = new MtartProcessLookupConfigValue { GenrateStartProcessBom = null };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override MtartProcessLookupConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
