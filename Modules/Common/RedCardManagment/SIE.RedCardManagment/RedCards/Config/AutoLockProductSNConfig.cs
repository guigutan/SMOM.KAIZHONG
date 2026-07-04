using SIE.Common.Configs;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 自动生成异常任务
    /// </summary>
    [System.ComponentModel.DisplayName("自动锁定产品SN")]
    [System.ComponentModel.Description("自动锁定产品SN配置")]
    public class AutoLockProductSNConfig : ModuleConfig<AutoLockProductSNConfigValue>
    {
        /// <summary>
        /// 检验值样本数量
        /// </summary>
        readonly AutoLockProductSNConfigValue defaultValue = new AutoLockProductSNConfigValue { IsAuto = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override AutoLockProductSNConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
