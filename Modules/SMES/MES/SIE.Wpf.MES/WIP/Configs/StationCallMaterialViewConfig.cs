using SIE.MES.WIP.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 工位叫料配置值视图配置
    /// </summary>
    internal class StationCallMaterialViewConfig : WPFViewConfig<StationCallMaterialConfigValue>
    {
        /// <summary>
        /// 配置明细视图 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsAuto);
        }
    }
}