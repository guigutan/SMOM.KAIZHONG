using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.Warehouses.Printables
{
    /// <summary>
    /// 库位标签
    /// </summary>
    [Serializable]
    [DisplayName("库位标签")]
    public class StorageLocationPrintable : LabelPrintable<StorageLocation>
    {
    }
}
