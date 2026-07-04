using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 选择设备BOM
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipBomSelectCriteria))]
    [Label("选择设备BOM")]
    public class EquipBomSelect : EquipBom
    {
    }
}
