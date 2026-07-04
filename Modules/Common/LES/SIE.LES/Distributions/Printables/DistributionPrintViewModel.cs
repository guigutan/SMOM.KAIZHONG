using SIE.Common.Properties;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.LES.Distributions.Printables
{
    /// <summary>
    /// 配送单打印
    /// </summary>
    [Serializable, RootEntity]
    [Label("配送单打印")]
    public class DistributionPrintViewModel : BillPrintViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DistributionPrintViewModel()
        {
            PrintCount = 1;
            Printer = Settings.Default.PrinterName;
        }         
    }
}