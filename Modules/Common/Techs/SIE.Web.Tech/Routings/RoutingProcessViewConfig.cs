using SIE.Tech.Routings;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线工序视图配置
    /// </summary>
    internal class RoutingProcessViewConfig : WebViewConfig<RoutingProcess>
    {
        /// <summary>
        /// 下拉列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {            
            View.Property(p => p.Index);
            View.Property(p => p.ProcessCode);
            View.Property(p => p.ProcessName);            
            View.Property(p => p.ProcessType).HasLabel("工序类型");
            View.Property(p => p.ProcessSegmentName).HasLabel("工段名称");
            
        }
    }
}
