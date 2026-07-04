using SIE.Common.Properties;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using SIE.Wpf.Common;

namespace SIE.Wpf.Warehouses.ViewModels
{
    /// <summary>
    /// 库位打印标签命令
    /// </summary>
    [RootEntity]
    [Label("打印标签")]
    [DisplayMember(nameof(StorageLocationPrintViewModel.Id))]
    public class StorageLocationPrintViewModel : ViewModel
    {
        /// <summary>
        /// 库位打印标签命令
        /// </summary>
        public StorageLocationPrintViewModel()
        {
            PrintCount = 1;
             Printer = Settings.Default.PrinterName;
        }

        #region 打印份数 PrintCount
        /// <summary>
        /// 打印份数
        /// </summary>
        [Label("打印份数")]
        [MinValue(1)]
        public static readonly Property<int> PrintCountProperty = P<StorageLocationPrintViewModel>.Register(e => e.PrintCount);

        /// <summary>
        /// 打印份数
        /// </summary>
        public int PrintCount
        {
            get { return this.GetProperty(PrintCountProperty); }
            set { this.SetProperty(PrintCountProperty, value); }
        }
        #endregion

        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<StorageLocationPrintViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as StorageLocationPrintViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }

        /// <summary>
        /// 打印机变更
        /// </summary>
        void OnPrinterChanged()
        {
            Settings.Default.PrinterName = Printer;
            Settings.Default.Save();
        }
        #endregion
    }

    /// <summary>
    /// 库位 打印标签视图配置
    /// </summary>
    public class StorageLocationPrintViewModelViewConfig : WPFViewConfig<StorageLocationPrintViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(StorageLocation));
            using (View.OrderProperties())
            {
                View.Property(p => p.PrintCount).Show(ShowInWhere.All);
                View.Property(p => p.Printer).UsePrinterEditor().Show(ShowInWhere.All);
            }
        }
    }
}
