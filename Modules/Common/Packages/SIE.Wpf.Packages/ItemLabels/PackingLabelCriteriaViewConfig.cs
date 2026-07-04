using SIE.Packages.ItemLabels;

namespace SIE.Wpf.Packages.ItemLabels
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class PackingLabelCriteriaViewConfig : WPFViewConfig<PackingLabelCriteria>
    {
        /// <summary>
        ///  配置查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("标签条码查询");
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo).Show();
                View.Property(p => p.No).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.LotCode).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateTimePart = ObjectModel.DateTimePart.DateTime; p.DateRangeType = ObjectModel.DateRangeType.LastMonth; }).Show();
            }
        }
    }
}
