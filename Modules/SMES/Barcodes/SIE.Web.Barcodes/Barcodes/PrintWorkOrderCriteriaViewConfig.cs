using SIE.Barcodes;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 查询实体界面配置类
    /// </summary>
    internal class PrintWorkOrderCriteriaViewConfig : WebViewConfig<PrintWorkOrderCriteria>
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
            });
            View.Property(p => p.CreateDate).ShowInDetail().UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Today;
            });
            View.Property(p => p.ItemName).Show(ShowInWhere.Hide).HasLabel("产品名称");
        }
    }
}