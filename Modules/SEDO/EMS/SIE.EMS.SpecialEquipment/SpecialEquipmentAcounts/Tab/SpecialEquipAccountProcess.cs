using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts.Tab
{
    /// <summary>
    /// 特种设备台账工序
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备台账工序")]
    public class SpecialEquipAccountProcess : EquipAccountProcess
    {

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static new readonly IRefIdProperty EquipAccountIdProperty =
            P<SpecialEquipAccountProcess>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public new double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static new readonly RefEntityProperty<SpecialEquipmentAccount> EquipAccountProperty =
            P<SpecialEquipAccountProcess>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public new SpecialEquipmentAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion
    }
}
