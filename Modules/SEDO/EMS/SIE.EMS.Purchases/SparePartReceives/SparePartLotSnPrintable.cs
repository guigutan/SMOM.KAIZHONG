using SIE.Common.Prints;
using SIE.EMS.Purchases.SparePartReceives.ViewModels;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收条码
    /// </summary>
    [Serializable]
    [DisplayName("备件接收条码")]
    public class SparePartLotSnPrintable : LabelPrintable<SparePartLotSnViewModel>
    {
    }
}
