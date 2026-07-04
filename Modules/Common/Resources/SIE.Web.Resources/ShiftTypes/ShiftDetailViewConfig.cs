using SIE.MetaModel.View;
using SIE.Resources.ShiftTypes;

namespace SIE.Web.Resources.ShiftTypes
{
    /// <summary>
    /// 班制时间明细视图配置
    /// </summary>
    public class ShiftDetailViewConfig : WebViewConfig<ShiftDetail>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);

            using (View.OrderProperties())
            {
                View.Property(p => p.BeginTime).Show(ShowInWhere.All);
                View.Property(p => p.EndTime).Show(ShowInWhere.All);
            }
        }
    }
}
