using SIE.Common.Properties;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Web.Warehouses.ViewModels
{
    /// <summary>
    /// 库位 打印标签命令
    /// </summary>
    [RootEntity, Serializable]
    [Label("打印标签")]
    [DisplayMember(nameof(StorageLocationPrintViewModel.Id))]
    public class StorageLocationPrintViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
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
        public static readonly Property<string> PrinterProperty = P<StorageLocationPrintViewModel>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 库位 打印标签视图配置
    /// </summary>
    public class StorageLocationPrintViewModelViewConfig : WebViewConfig<StorageLocationPrintViewModel>
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
            }
        }
    }
}
