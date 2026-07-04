using SIE.Tech.Routings;
using SIE.MES.Routings.RoutingBoms;
using SIE.Resources.ProcessSegments;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工序bom查询界面
    /// </summary>
    internal class RoutingBomCriteriaViewConfig : WebViewConfig<RoutingBomCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProductCode).HasLabel("产品编码").UseTextEditor(p => p.MaxLength = 250);
            View.Property(p => p.ProductName).HasLabel("产品名称").UseTextEditor(p => p.MaxLength = 250);

            View.Property(p => p.ItemCode).HasLabel("物料编码");
            View.Property(p => p.ItemName).HasLabel("物料名称").UseTextEditor(p => p.MaxLength = 250);
            View.Property(p => p.ProcessSegment).HasLabel("工段").UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProcessSegmentController>().GetProcessSegments(pagingInfo, keyword);
            });
        }
    }
}
