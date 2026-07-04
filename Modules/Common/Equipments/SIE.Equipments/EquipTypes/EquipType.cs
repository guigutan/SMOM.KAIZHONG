using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipTypes
{
    /// <summary>
    /// 设备类型
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(TypeCode))]
    [Label("设备类型")]
    public partial class EquipType : SIE.Core.Equipments.EquipType
    {
    }

    /// <summary>
    /// 设备类型 实体配置
    /// </summary>
    internal class EquipTypeConfig : EntityConfig<EquipType>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_TYPE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}