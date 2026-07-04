using SIE.Domain;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab
{
    /// <summary>
    /// 计量设备台账位置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备台账位置")]
    public class MeterEquipAccountLocation : LocationBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<MeterEquipAccountLocation>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<MeteringEquipmentAccount> EquipAccountProperty = P<MeterEquipAccountLocation>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public MeteringEquipmentAccount EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class MeterEquipAccountLocationConfig : EntityConfig<MeterEquipAccountLocation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT_LOC").MapAllProperties();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}
