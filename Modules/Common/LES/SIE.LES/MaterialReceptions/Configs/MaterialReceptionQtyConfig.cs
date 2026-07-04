using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.LES.MaterialReceptions.Configs
{
    /// <summary>
    /// 物料接收配置项
    /// </summary>
    [DisplayName("按单接收默认接收数量")]
    [Description("按单接收默认接收数量")]
    public class MaterialReceptionQtyConfig : ModuleConfig<MaterialReceptionQtyConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaterialReceptionQtyConfigValue defaultValue = new MaterialReceptionQtyConfigValue();

        /// <summary>
        /// 初始化
        /// </summary>
        public override MaterialReceptionQtyConfigValue DefaultValue
        {
            get { return defaultValue; }
        }

    }
}
