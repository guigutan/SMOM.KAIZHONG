using SIE.Barcodes;
using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.MES.WIP.Moves
{
    /// <summary>
    /// 外标签条码打印
    /// </summary>
    [Serializable]
    [DisplayName("外标签条码打印")]
    public class BarcodePrintable : LabelPrintable<Barcode>
    {
    }
}
