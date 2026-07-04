using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 任务单打印
    /// </summary>
    [Serializable, RootEntity]
    [Label("任务单打印")]
    public class DispatchTaskPrintViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DispatchTaskPrintViewModel()
        {
            PrintCount = 1;
            Printer = Settings.Default.PrinterName;
        }

        #region 单据模板 BillTemplate
        /// <summary>
        /// 单据模板Id
        /// </summary>
        [Label("单据模板")]
        public static readonly IRefIdProperty BillTemplateIdProperty =
            P<DispatchTaskPrintViewModel>.RegisterRefId(e => e.BillTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 单据模板Id
        /// </summary>
        public double? BillTemplateId
        {
            get { return (double?)this.GetRefNullableId(BillTemplateIdProperty); }
            set { this.SetRefNullableId(BillTemplateIdProperty, value); }
        }

        /// <summary>
        /// 单据模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> BillTemplateProperty =
            P<DispatchTaskPrintViewModel>.RegisterRef(e => e.BillTemplate, BillTemplateIdProperty);

        /// <summary>
        /// 单据模板
        /// </summary>
        public PrintTemplate BillTemplate
        {
            get { return this.GetRefEntity(BillTemplateProperty); }
            set { this.SetRefEntity(BillTemplateProperty, value); }
        }
        #endregion

        #region 打印份数 PrintCount
        /// <summary>
        /// 打印份数
        /// </summary>
        [Label("打印份数")]
        public static readonly Property<int> PrintCountProperty = P<DispatchTaskPrintViewModel>.Register(e => e.PrintCount);

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
        public static readonly Property<string> PrinterProperty = P<DispatchTaskPrintViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as DispatchTaskPrintViewModel).OnPrinterChanged(o, e)
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
        /// <param name="o">托管属性对象</param>
        /// <param name="e">属性变更事件</param>
        void OnPrinterChanged(ManagedPropertyObject o, ManagedPropertyChangedEventArgs e)
        {
            Settings.Default.PrinterName = Printer;
            //Settings.Default.Save();
        }
        #endregion
    }
}
