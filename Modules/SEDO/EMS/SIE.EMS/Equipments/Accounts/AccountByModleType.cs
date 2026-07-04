using SIE.EMS.Equipments.Accounts.Criterias;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AccountByModleTypeCriteria))]
    [Label("设备台账")]
    public class AccountByModleType : EquipAccount
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
