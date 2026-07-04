using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Barcodes;
using SIE.MetaModel;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BarcodeProcessCriteria))]
    [Label("条码工序指派")]
    public class BarcodeProcess : Barcode
    {
       
    }
}
