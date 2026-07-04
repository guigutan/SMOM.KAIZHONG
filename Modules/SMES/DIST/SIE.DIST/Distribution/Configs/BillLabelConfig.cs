using SIE.Common.Configs;

namespace SIE.DIST.Distribution.Configs
{
    /// <summary>
    /// 配送单退料单标签生成和打印模板规则
    /// </summary>
    [System.ComponentModel.DisplayName("配送单退料单标签生成和打印模板规则")]
    [System.ComponentModel.Description("配送单退料单标签生成和打印模板规则")]
    public class BillLabelConfig : ModuleConfig<BillLabelConfigValue>
    {
        /// <summary>
        /// 单据条码配置项默认值
        /// </summary>
        readonly BillLabelConfigValue defaultValue = new BillLabelConfigValue { NumberRule = null, PrintTemplate = null };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override BillLabelConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
