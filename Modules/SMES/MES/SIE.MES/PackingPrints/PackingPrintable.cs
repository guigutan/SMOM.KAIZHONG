using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.MES.PackingPrints
{
    /// <summary>
    /// 包装号
    /// </summary>
    [Serializable]
    [DisplayName("包装号")]
    public class PackingPrintable : LabelPrintable<PackingBarcode>
    {
    }
}
