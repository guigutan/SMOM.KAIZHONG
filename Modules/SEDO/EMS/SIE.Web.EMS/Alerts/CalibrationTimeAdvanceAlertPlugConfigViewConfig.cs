using SIE.EMS.AlertPlugs;

namespace SIE.Web.EMS.Alerts
{
    /// <summary>
    /// 校验任务提前生成提醒时间视图
    /// </summary>
    internal class CalibrationTimeAdvanceAlertPlugConfigViewConfig : WebViewConfig<CalibrationTimeAdvanceAlertPlugConfig>
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
            View.Property(p => p.TimeAdvance);
        }
    }
}
