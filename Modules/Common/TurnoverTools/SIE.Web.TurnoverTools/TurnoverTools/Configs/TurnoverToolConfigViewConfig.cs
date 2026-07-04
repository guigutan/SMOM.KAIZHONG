using SIE.TurnoverTools.TurnoverTools.Configs;

namespace SIE.Web.Elec.MES.TurnoverTools.Configs
{
    /// <summary>
    /// 周转工具配置值视图配置
    /// </summary>
    internal class TurnoverToolConfigsValueViewConfig : WebViewConfig<TurnoverToolConfigsValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置默认视图
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(c => c.MixedWorkOrderMode).Show(ShowInWhere.All);
        }
    }
}