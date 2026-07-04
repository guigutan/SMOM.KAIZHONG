using SIE.MES.WIP.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 换料后原关键件处理方式配置项的值 视图配置
    /// </summary>
    public class KeyComponetsReplacementConfigValueViewConfig : WPFViewConfig<KeyComponentsReplacementConfigValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.HandleMethod);
        }
    }
}
