using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码
    /// </summary>
    [Serializable]
    [DisplayName("拼板码")]
    public class PanelPrintable : LabelPrintable<Panel>
    {
    }
}