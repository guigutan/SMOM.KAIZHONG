using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件基础数据选择
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SparePartSelCriteria))]
    [DisplayMember(nameof(SparePartName))]
    [Label("备件基础数据选择")]
    public partial class SparePartSel : SparePart
    {

    }
}