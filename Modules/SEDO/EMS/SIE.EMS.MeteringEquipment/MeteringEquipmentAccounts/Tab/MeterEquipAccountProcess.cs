using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab
{
    /// <summary>
    /// 计量设备台账工序
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备台账工序")]
    public class MeterEquipAccountProcess : ProcessBase
    {

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<MeterEquipAccountProcess>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
            P<MeterEquipAccountProcess>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
    public class MeterEquipAccountProcessConfig : EntityConfig<MeterEquipAccountProcess>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_PROCESS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
