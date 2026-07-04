using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账视图 （只为做选择视图）
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Code))]
    [ConditionQueryType(typeof(EquipAccountSelectCriteria))]    
    [Label("设备台账视图")]
    public partial class EquipAccountSelect : EquipAccount
    {
    }

    /// <summary>
    /// 设备台账 实体配置
    /// </summary>
    internal class EquipAccountSelectConfig : EntityConfig<EquipAccountSelect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();                        
            Meta.Property(EquipAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(EquipAccount.ReasonProperty).DontMapColumn();
            Meta.Property(EquipAccount.IsCalibrationProperty).DontMapColumn();
            Meta.EnablePhantoms();
            Meta.IsTreeEntity = false;
        }
    }
}