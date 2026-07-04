using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.Barcodes.Printables
{
    /// <summary>
    /// 单体条码
    /// </summary>
    [Serializable]
    [DisplayName("条码")]
    public class BarcodePrintable : LabelPrintable<Barcode>
    {
    }
}
