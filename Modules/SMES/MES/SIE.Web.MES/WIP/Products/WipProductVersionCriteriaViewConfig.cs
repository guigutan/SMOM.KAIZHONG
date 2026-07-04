using SIE.MES.WIP.Products;
using SIE.Tech.Processs;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品版本查询实体视图配置
    /// </summary>
    internal class WipProductVersionCriteriaViewConfig : WebViewConfig<WipProductVersionCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            //View.Property(p => p.PanelWorkOrderNo).ShowInDetail();
            View.Property(p => p.No).ShowInDetail();
            View.Property(p => p.Sn).ShowInDetail();
            View.Property(p => p.ItemSN).ShowInDetail().HasLabel("关键件来源条码");
            //View.Property(p => p.PanelCode).ShowInDetail();
            View.Property(p => p.KeyLabel).ShowInDetail();
            View.Property(p => p.Process).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProcessController>().GetProcessList(pagingInfo, keyword);
            }).ShowInDetail();
            View.Property(p => p.NextProcess).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProcessController>().GetProcessList(pagingInfo, keyword);
            }).ShowInDetail();
            View.Property(p => p.ProductCode).ShowInDetail();
            View.Property(p => p.ProductName).ShowInDetail();
            View.Property(p => p.StartDate).ShowInDetail().UseDateRangeEditor(p =>
            {
                p.Format = "Y/M/d";
                p.DateRangeType = ObjectModel.DateRangeType.Today;
            });
            View.Property(p => p.IsFinish).ShowInDetail();
        }
    }
}