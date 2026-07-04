using SIE.MES.WorkOrders.Configs;

namespace SIE.Web.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工序 BOM 参考工单 BOM 配置值 视图
    /// </summary>
    internal class WorkOrderProcessBomSourceConfigValueViewConfig : WebViewConfig<WorkOrderProcessBomSourceConfigValue>
    { 
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessBomType).Show(ShowInWhere.All);
            }
        }
    }
}
