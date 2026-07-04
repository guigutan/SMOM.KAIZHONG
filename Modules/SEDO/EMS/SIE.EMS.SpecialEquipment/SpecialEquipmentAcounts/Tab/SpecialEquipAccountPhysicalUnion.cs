using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts.Tab
{
    /// <summary>
    /// 特种设备台账物联参数
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备台账物联参数")]
    public class SpecialEquipAccountPhysicalUnion : EquipAccountPhysicalUnion
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static new readonly IRefIdProperty EquipAccountIdProperty =
            P<SpecialEquipAccountPhysicalUnion>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
            P<SpecialEquipAccountPhysicalUnion>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
