using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收序列号
    /// </summary>
    [Serializable]
    [DisplayName("设备接收序列号")]
    public class EquipReceiveSnPrintable : LabelPrintable<ReceiveScanSnViewModel>
    {
    }
}
