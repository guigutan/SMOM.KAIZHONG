using SIE.Domain;
using SIE.Equipments.EquipAccountLocations;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts.Tab
{
    /// <summary>
    /// 特种设备台账位置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备台账位置")]
    public class SpecialEquipAccountLocation : EquipAccountLocation
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static new readonly IRefIdProperty EquipAccountIdProperty = P<SpecialEquipAccountLocation>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static new readonly RefEntityProperty<SpecialEquipmentAccount> EquipAccountProperty = P<SpecialEquipAccountLocation>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
