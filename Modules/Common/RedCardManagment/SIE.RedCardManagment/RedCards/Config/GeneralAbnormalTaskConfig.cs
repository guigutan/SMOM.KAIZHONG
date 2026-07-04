using SIE.Common.Configs;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 自动生成异常任务
    /// </summary>
    [System.ComponentModel.DisplayName("自动生成异常任务")]
    [System.ComponentModel.Description("自动生成异常任务配置")]
    public class GeneralAbnormalTaskConfig : ModuleConfig<GeneralAbnormalTaskConfigValue>
    {
        /// <summary>
        /// 检验值样本数量
        /// </summary>
        readonly GeneralAbnormalTaskConfigValue defaultValue = new GeneralAbnormalTaskConfigValue { IsAuto = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override GeneralAbnormalTaskConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
