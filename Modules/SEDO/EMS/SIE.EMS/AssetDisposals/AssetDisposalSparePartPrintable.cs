using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.EMS.AssetDisposals
{
    /// <summary>
    /// 备件回收条码
    /// </summary>
    [Serializable]
    [DisplayName("备件回收条码")]
    public class AssetDisposalSparePartPrintable : LabelPrintable<AssetDisposalSparePart>
    {
    }
}
