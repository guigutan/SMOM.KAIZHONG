using SIE.Common.Configs;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 信息追溯天数配置项
    /// </summary>
    [System.ComponentModel.DisplayName("信息追溯天数配置项")]
    [System.ComponentModel.Description("信息追溯天数配置项。（当红牌只维护物料和供应商，信息追溯时会根据该配置进行追溯）")]
    public class SyncDaysConfig : ModuleConfig<SyncDaysConfigValue>
    {
        /// <summary>
        /// 检验值样本数量
        /// </summary>
        readonly SyncDaysConfigValue defaultValue = new SyncDaysConfigValue { Days = 30 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override SyncDaysConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
