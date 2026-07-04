using SIE.Common.Prints;
using SIE.Warehouses;
using System;
using System.ComponentModel;

namespace SIE.Wpf.Warehouses.Printables
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
