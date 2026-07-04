using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 包装号补打ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("包装号补打")]
    public class ReprintInfoViewModel : ViewModel
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ReprintInfoViewModel() 
        {
            Times = 1;
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="barcodeList">条码列表</param>
        public ReprintInfoViewModel(IEnumerable<PackingBarcode> barcodeList)
        {
            Times = 1;
            Printer = Settings.Default.PrinterName;
            BarcodeList.AddRange(barcodeList);
        }

        #region 条码列表  
        /// <summary>
        /// 条码列表
        /// </summary>
        public EntityList<PackingBarcode> BarcodeList { get; set; } = new EntityList<PackingBarcode>();
        #endregion

        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        [Required]
        public static readonly Property<string> ReasonProperty = P<ReprintInfoViewModel>.Register(e => e.Reason);

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
        public static readonly Property<int> TimesProperty = P<ReprintInfoViewModel>.Register(e => e.Times);

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
        public static readonly IRefIdProperty TemplateIdProperty =P<ReprintInfoViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<ReprintInfoViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

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
        public static readonly Property<string> PrinterProperty = P<ReprintInfoViewModel>.Register(e => e.Printer, new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as ReprintInfoViewModel).OnPrinterChanged()
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
}
