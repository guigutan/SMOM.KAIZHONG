using SIE.MES.WIP.Pressure.Configs;

namespace SIE.Web.MES.WIP.Pressure.Configs
{
    /// <summary>
    /// 耐压采集SN超打配置值 视图配置
    /// </summary>
    class WipPressureVerifyCodeConfigValueViewConfig : WebViewConfig<WipPressureVerifyCodeConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.VerifyCode).Show(ShowInWhere.All);
                View.Property(p => p.OverPrintPercent).Show(ShowInWhere.All);
                View.Property(p => p.IsNotValidatePressureReport).Show().UseListSetting(p => p.HelpInfo = "工序报工时,不校验耐压工序前置校验"); ;
            }
        }
    }
}