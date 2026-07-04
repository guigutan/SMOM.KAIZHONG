using SIE.MES.Configs;
using SIE.MES.WIP.Configs;

namespace SIE.Web.MES.WIP.Configs
{

    /// <summary>
    /// 工序交接校验配置值视图配置
    /// </summary>
    internal class ProcessTransferCheckConfigValueViewConfig : WebViewConfig<ProcessTransferCheckConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.IsCheck).Show(ShowInWhere.All);
        }
    }
}