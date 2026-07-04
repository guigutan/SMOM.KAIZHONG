using SIE.Common.Configs;

namespace SIE.DIST.Distribution.Configs
{
    /// <summary>
    /// 配送单单号生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("配送单单号生成规则")]
    [System.ComponentModel.Description("配送单单号生成规则")]
    public class BillNoConfig : ModuleConfig<BillNoConfigValue>
    {
        /// <summary>
        /// 配送单号配置默认值
        /// </summary>
        readonly BillNoConfigValue defaultValue = new BillNoConfigValue { NumberRule = null };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override BillNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
