using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.Common;
using System;

namespace SIE.Wpf.Barcodes.WipBatchs.ViewModels
{
    /// <summary>
    /// 生产批次补打 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("生产批次补打")]
    public class BatchReprintViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchReprintViewModel()
        {
            Printer = Settings.Default.PrinterName;
        }

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<BatchReprintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double TemplateId
        {
            get { return (double)this.GetRefId(TemplateIdProperty); }
            set { this.SetRefId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty =
            P<BatchReprintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 是否子批次 IsSubBatch
        /// <summary>
        /// 是否子批次
        /// </summary>
        [Label("是否子批次")]
        public static readonly Property<bool> IsSubBatchProperty = P<BatchReprintViewModel>.Register(e => e.IsSubBatch);

        /// <summary>
        /// 是否子批次
        /// </summary>
        public bool IsSubBatch
        {
            get { return this.GetProperty(IsSubBatchProperty); }
            set { this.SetProperty(IsSubBatchProperty, value); }
        }
        #endregion

        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        [Required]
        public static readonly Property<string> PrinterProperty = P<BatchReprintViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as BatchReprintViewModel).OnPrinterChanged()
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
    /// 生产批次补打 视图
    /// </summary>
    internal class BatchReprintViewModelConfig : WPFViewConfig<BatchReprintViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(WipBatch));
            View.Property(p => p.Printer).UsePrinterEditor();
            View.Property(p => p.Template).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as BatchReprintViewModel;
                Check.NotNull(model, nameof(BatchReprintViewModel));
                var qualifiedName = model.IsSubBatch ? typeof(WipBatchPrintable).GetQualifiedName() : typeof(WipBatchPrintable).GetQualifiedName();
                return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
            });
        }
    }
}
