using SIE.MES.DashBoard.Reports.FpySettings;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 直通率设置视图配置
    /// </summary>
    internal class FpySettingViewConfig : WPFViewConfig<FpySetting>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior(typeof(FpySettingBehavior));
        }
    }
}