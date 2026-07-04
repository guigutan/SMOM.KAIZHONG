using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.MES.LoadItems.Configs
{
    /// <summary>
    /// 工单耗用量耗用单号编码规则配置项
    /// </summary>
    [DisplayName("工单耗用量耗用单号编码默认值")]
    [Description("工单耗用量耗用单号编码默认值")]
    public class WoCostItemNoConfig : ModuleConfig<WoCostItemNoConfigValue>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        readonly WoCostItemNoConfigValue defaultValue = new WoCostItemNoConfigValue { CostNoRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override WoCostItemNoConfigValue DefaultValue
        { 
            get { return defaultValue; }
        }
    }
}
