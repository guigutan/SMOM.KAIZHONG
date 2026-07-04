using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts.Tab
{
    /// <summary>
    /// 特种设备缸槽
    /// </summary>
    [ChildEntity, Serializable]
    [Label("缸槽管理")]
    [DisplayMember(nameof(Code))]
    public class SpecialEquipAccountSlot : EquipAccountSlot
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备")]
        public static new readonly IRefIdProperty EquipAccountIdProperty = P<SpecialEquipAccountSlot>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public new double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }
        /// <summary>
        /// 设备台账
        /// </summary>
        public static new readonly RefEntityProperty<SpecialEquipmentAccount> EquipAccountProperty = P<SpecialEquipAccountSlot>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public new SpecialEquipmentAccount EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion
    }
}
