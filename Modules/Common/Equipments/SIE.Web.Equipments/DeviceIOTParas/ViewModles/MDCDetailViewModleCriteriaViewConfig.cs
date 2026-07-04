using SIE.Equipments.DeviceIOTParas.ViewModles;

namespace SIE.Web.Equipments.DeviceIOTParas.ViewModles
{
    /// <summary>
    /// MDC接口视图
    /// </summary>
    public class MDCDetailViewModleCriteriaViewConfig : WebViewConfig<MDCDetailViewModleCriteria>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.MesModel);
        }
    }
}
