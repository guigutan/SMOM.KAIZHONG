using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 产品版本查询实体视图配置
    /// </summary>
    internal class WipProductVersionCriteriaViewConfig : WPFViewConfig<WipProductVersionCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.PanelWorkOrderNo).ShowInDetail();
            View.Property(p => p.No).ShowInDetail();
            View.Property(p => p.Sn).ShowInDetail();
            View.Property(p => p.ItemSN).ShowInDetail();
            //View.Property(p => p.PanelCode).ShowInDetail();
            View.Property(p => p.KeyLabel).ShowInDetail();
            View.Property(p => p.Process).ShowInDetail();
            View.Property(p => p.NextProcess).ShowInDetail();
            View.Property(p => p.StartDate).ShowInDetail()
                .UseDateRangeEditor(p =>
                {
                    p.DateTimePart = ObjectModel.DateTimePart.Date;
                    p.DateRangeType = ObjectModel.DateRangeType.Today;
                });
        }
    }
}