using SIE.Dock.DockMaintains;

namespace SIE.Web.Dock.DockMaintains
{
    /// <summary>
    /// 月台维护查询视图配置
    /// </summary>
    internal class DockMaintainCriteriaViewConfig : WebViewConfig<DockMaintainCriteria>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.IsReceive).UseCheckDropDownEditor().Show();
                View.Property(p => p.IsShip).UseCheckDropDownEditor().Show();
                View.Property(p => p.State).Show();
            }
        }
    }
}