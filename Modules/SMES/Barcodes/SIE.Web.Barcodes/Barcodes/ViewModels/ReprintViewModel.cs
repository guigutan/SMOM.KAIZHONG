using SIE.Barcodes;
using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Barcodes.ViewModels
{
    /// <summary>
    /// 条码报废 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("条码补打")]
    public class ReprintViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ReprintViewModel()
        {
            Times = 1;
            Printer = Settings.Default.PrinterName;
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="barcodeList">条码列表</param>
        public ReprintViewModel(IEnumerable<Barcode> barcodeList)
        {
            Times = 1;
            Printer = Settings.Default.PrinterName;
            BarcodeList.AddRange(barcodeList);
            Template = RT.Service.Resolve<BarcodeController>().GetPrintTemplateByWo(BarcodeList.FirstOrDefault().WorkOrder.Id);
        }
        #endregion

        #region 条码列表
        /// <summary>
        /// 条码列表
        /// </summary>
        EntityList<Barcode> BarcodeList = new EntityList<Barcode>();
        #endregion

        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        [Required]
        public static readonly Property<string> ReasonProperty = P<ReprintViewModel>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 补打份数
        /// <summary>
        /// 补打份数
        /// </summary>
        [Label("补打份数")]
        [MinValue(1)]
        public static readonly Property<int> TimesProperty = P<ReprintViewModel>.Register(e => e.Times);

        /// <summary>
        /// 补打份数
        /// </summary>
        public int Times
        {
            get { return this.GetProperty(TimesProperty); }
            set { this.SetProperty(TimesProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<ReprintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<ReprintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        [Required]
        public static readonly Property<string> PrinterProperty = P<ReprintViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as ReprintViewModel).OnPrinterChanged()
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
        /// 打印机
        /// </summary>
        void OnPrinterChanged()
        {
            Settings.Default.PrinterName = Printer;
            Settings.Default.Save();
        }
        #endregion 
    }

    /// <summary>
    /// 条码补打 视图
    /// </summary>
    internal class ReprintViewModelViewConfig : WebViewConfig<ReprintViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(BarcodeReprint));
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.Times).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.Template).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(typeof(SIE.Barcodes.Printables.BarcodePrintable).GetQualifiedName(), p, r);
            });
            View.Property(p => p.Reason).UseMemoEditor().ShowInDetail(rowSpan: 3, columnSpan: 2);
        }
    }
}