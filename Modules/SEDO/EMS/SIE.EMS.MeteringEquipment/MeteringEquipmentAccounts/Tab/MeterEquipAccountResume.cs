using SIE.Domain;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab
{
    /// <summary>
    /// 计量设备履历
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备履历")]
    public class MeterEquipAccountResume : ResumeBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<MeterEquipAccountResume>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<MeteringEquipmentAccount> EquipAccountProperty =
            P<MeterEquipAccountResume>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public MeteringEquipmentAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 
    /// </summary>
    internal class MeterEquipAccountResumeConfig : EntityConfig<MeterEquipAccountResume>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_RESUME").MapAllProperties();
            Meta.Property(MeterEquipAccountResume.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
