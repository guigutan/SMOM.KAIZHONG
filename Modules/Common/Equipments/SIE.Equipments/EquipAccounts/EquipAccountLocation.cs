using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipAccountLocations
{
    /// <summary>
    /// 设备台账位置
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账位置")]
    public partial class EquipAccountLocation : LocationBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipAccountLocation>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty = P<EquipAccountLocation>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备台账位置 实体配置
    /// </summary>
    internal class EquipAccountLocationConfig : EntityConfig<EquipAccountLocation>
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