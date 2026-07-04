using SIE.Barcodes.WipBatchs;

namespace SIE.Wpf.Barcodes.WipBatchs
{
    /// <summary>
    /// 实体页面配置
    /// </summary>
    internal class BatchWorkOrderCriteriaViewConfig : WPFViewConfig<BatchWorkOrderCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).HasLabel("物料编码").Show(ShowInWhere.Hide);
            View.Property(p => p.No).ShowInDetail();
            View.Property(p => p.CreateBy).ShowInDetail();
            View.Property(p => p.PlanBeginDate).ShowInDetail().UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Today;
                e.DateTimePart = ObjectModel.DateTimePart.Date;
            });
            View.Property(p => p.CreateDate).ShowInDetail().UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Today;
                e.DateTimePart = ObjectModel.DateTimePart.Date;
            });
        }
    }
}