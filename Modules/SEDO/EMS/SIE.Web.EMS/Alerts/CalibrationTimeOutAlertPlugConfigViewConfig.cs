using SIE.EMS.AlertPlugs;

namespace SIE.Web.EMS.Alerts
{
    /// <summary>
    /// 设备校验任务超时提醒视图
    /// </summary>
    internal class CalibrationTimeOutAlertPlugConfigViewConfig : WebViewConfig<CalibrationTimeOutAlertPlugConfig>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.TimeOut);
        }
    }
}
