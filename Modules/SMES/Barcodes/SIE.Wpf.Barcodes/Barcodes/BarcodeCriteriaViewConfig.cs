using SIE.Barcodes;

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 条码查询视图配置
    /// </summary>
    public partial class BarcodeCriteriaViewConfig : WPFViewConfig<BarcodeCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == BarcodeViewConfig.ScrapView)
                ConfigScrapView();
            else if (ViewGroup == BarcodeViewConfig.ReprintView)
                ConfigReprintView();
            else
                ProductRouting();
        }

        /// <summary>
        /// 产品工艺路线 视图配置方法
        /// </summary>
        void ProductRouting()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.Sn).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 条码报废查询 视图配置方法
        /// </summary>
        void ConfigScrapView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInDetail();
                View.Property(p => p.Sn).ShowInDetail();
                View.Property(p => p.State).ShowInDetail();
                View.Property(p => p.IsScraped).UseCheckDropDownEditor().ShowInDetail();
                View.Property(p => p.IsPending).UseCheckDropDownEditor().ShowInDetail();
                View.Property(p => p.Printer).ShowInDetail();
                View.Property(p => p.PrintDate).ShowInDetail().UseDateRangeEditor(e =>
                {
                    e.DateRangeType = ObjectModel.DateRangeType.Today;
                    e.DateTimePart = ObjectModel.DateTimePart.Date;
                }).HasLabel("打印日期");
            }
        }

        /// <summary>
        /// 条码补打查询 视图配置方法
        /// </summary>
        void ConfigReprintView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.Sn).Show(ShowInWhere.All);
                View.Property(p => p.Printer).Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).Show(ShowInWhere.All).UseDateRangeEditor(e =>
                {
                    e.DateRangeType = ObjectModel.DateRangeType.Today;
                    e.DateTimePart = ObjectModel.DateTimePart.Date;
                });
            }
        }
    }
}
