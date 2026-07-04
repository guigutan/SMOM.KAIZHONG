using SIE.Barcodes;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码查询视图配置
    /// </summary>
    public partial class BarcodeCriteriaViewConfig : WebViewConfig<BarcodeCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == BarcodeViewConfig.ReprintView)
                ConfigReprintView();
            if (ViewGroup == BarcodeViewConfig.PendingView)
                ConfigPendingView(); //条码挂起
        }

        /// <summary>
        /// 条码报废查询 视图配置方法
        /// </summary>
        protected override void ConfigQueryView()
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
                View.Property(p => p.Printer).UseDataSource((entity, pagingInfo, keyword) =>
                {
                    var printers = RT.Service.Resolve<SIE.Resources.Employees.EmployeeController>().GetEmployeeList(pagingInfo, keyword);
                    return printers;
                }).Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).Show(ShowInWhere.All).UseDateRangeEditor(e =>
                {
                    e.DateRangeType = ObjectModel.DateRangeType.Today;
                });
            }
        }

        /// <summary>
        /// 条码挂起 视图配置方法
        /// </summary>
        void ConfigPendingView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.Sn).Show(ShowInWhere.All);
                View.Property(p => p.IsPending).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.Printer).UseDataSource((entity, pagingInfo, keyword) =>
                {
                    var printers = RT.Service.Resolve<SIE.Resources.Employees.EmployeeController>().GetEmployeeList(pagingInfo, keyword);
                    return printers;
                }).Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).Show(ShowInWhere.All).UseDateRangeEditor(e =>
                {
                    e.DateRangeType = ObjectModel.DateRangeType.Today;
                });
            }
        }
    }
}
