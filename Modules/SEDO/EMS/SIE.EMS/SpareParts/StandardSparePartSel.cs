using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件基础数据选择
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StandardSparePartSelCriteria))]
    [DisplayMember(nameof(SparePartName))]
    [Label("标准备件基础数据选择")]
    public partial class StandardSparePartSel : SparePart
    {

    }
}
