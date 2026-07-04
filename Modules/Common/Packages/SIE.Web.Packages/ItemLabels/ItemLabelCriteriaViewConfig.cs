using SIE.Packages.ItemLabels;

namespace SIE.Web.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签查询实体视图配置
    /// </summary>
    internal class ItemLabelCriteriaViewConfig : WebViewConfig<ItemLabelCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Label).HasOrderNo(10);
            View.Property(p => p.Exidv).Show();
            View.Property(p => p.Exidv2).Show();
            View.Property(p => p.WorkOrderNo).HasOrderNo(20);
            View.Property(p => p.Lot).Show();
            View.Property(p => p.Licha).Show();
            View.Property(p => p.ItemId).HasOrderNo(30).HasLabel("物料编码");
            View.Property(p => p.ShortDescription).Show().HasOrderNo(35);
            View.Property(p => p.MrpController).Show().HasOrderNo(36);
            View.Property(p => p.ItemType).HasOrderNo(40);            
            View.Property(p => p.SourceType).HasOrderNo(60);
            View.Property(p => p.ShowZero).HasOrderNo(70);
            View.Property(p => p.ItemLabelState).HasOrderNo(80);
            View.Property(p => p.CreateDate).Show().UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Today;
            }).HasOrderNo(80);
        }
    }
}