using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 单位转换
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("单位转换")]
    [DisplayMember(nameof(Id))]
    public partial class UnitConvert : ItemUnit
    {
    }
}
