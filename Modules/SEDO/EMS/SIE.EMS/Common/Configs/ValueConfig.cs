using SIE.Common.Configs;

namespace SIE.EMS.Common.Configs
{
    /// <summary>
    /// 数据列数量配置项
    /// </summary>
    [System.ComponentModel.DisplayName("配置默认数据列数量")]
    [System.ComponentModel.Description("初始单据默认数据列数量")]
    public class ValueConfig : ModuleConfig<ValueConfigValue>
    {
        /// <summary>
        /// 数据列数量
        /// </summary>
        readonly ValueConfigValue defaultValue = new ValueConfigValue { Qty = 3 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ValueConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
