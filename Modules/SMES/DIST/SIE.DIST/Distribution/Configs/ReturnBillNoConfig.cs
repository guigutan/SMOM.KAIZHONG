using SIE.Common.Configs;

namespace SIE.DIST.Distribution.Configs
{
    /// <summary>
    /// 配送单退料单号生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("配送单退料单号生成规则")]
    [System.ComponentModel.Description("配送单退料单号生成规则")]
    public class ReturnBillNoConfig : ModuleConfig<ReturnBillNoConfigValue>
    {
        /// <summary>
        /// 配送单退料单号配置默认值
        /// </summary>
        readonly ReturnBillNoConfigValue defaultValue = new ReturnBillNoConfigValue { NumberRule = null };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override ReturnBillNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
