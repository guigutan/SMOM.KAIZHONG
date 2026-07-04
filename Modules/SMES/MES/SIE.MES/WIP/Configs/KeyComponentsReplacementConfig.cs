using SIE.Common.Configs;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 换料后原关键件处理方式配置项
    /// </summary>
    [System.ComponentModel.DisplayName("置换后原关键件处理方式配置项")]
    [System.ComponentModel.Description("置换后原关键件处理方式配置项")]
    public class KeyComponentsReplacementConfig : ModuleConfig<KeyComponentsReplacementConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override KeyComponentsReplacementConfigValue DefaultValue => new KeyComponentsReplacementConfigValue()
        {
            HandleMethod = Reworks.ReplaceItemHandleMethod.Scrap
        };
    }
}
