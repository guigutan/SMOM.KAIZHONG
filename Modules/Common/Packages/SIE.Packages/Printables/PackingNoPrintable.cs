using SIE.Common.Prints;
using System;
using System.ComponentModel;

namespace SIE.Packages.Printables
{
    /// <summary>
    /// 包装条码
    /// </summary>
    [Serializable]
    [DisplayName("包装条码")]
    public class PackingRelationPrintable : LabelPrintable<PackingRelation>
    {
    }

    /// <summary>
    /// 包装条码
    /// </summary>
    [Serializable]
    [DisplayName("批次包装条码")]
    public class BatchPackingRelationPrintable : LabelPrintable<BatchPackingRelation>
    {
    }
}
