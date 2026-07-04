using SIE;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM明细选择
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备BOM明细选择")]
    [ConditionQueryType(typeof(EquipBomDetailSelCriteria))]
    public partial class EquipBomDetailSel : EquipBomDetail
    {
    }
}