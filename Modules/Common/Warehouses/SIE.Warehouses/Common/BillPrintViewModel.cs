using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// WMS单据打印
    /// </summary>
    [Serializable, RootEntity]
    [Label("WMS单据打印")]
    public class BillPrintViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BillPrintViewModel()
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
            P<BillPrintViewModel>.RegisterRefId(e => e.BillTemplateId, ReferenceType.Normal);

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
            P<BillPrintViewModel>.RegisterRef(e => e.BillTemplate, BillTemplateIdProperty);

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
        public static readonly Property<int> PrintCountProperty = P<BillPrintViewModel>.Register(e => e.PrintCount);

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
        public static readonly Property<string> PrinterProperty = P<BillPrintViewModel>.Register(e => e.Printer);

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
    /// 打印数据
    /// </summary>
    public class PrintDatas
    {
        /// <summary>
        /// 发运单Id集合
        /// </summary>
        public double[] BillIdList { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public double BillTemplateId { get; set; }
    }
}