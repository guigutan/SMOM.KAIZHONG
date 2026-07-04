using SIE.Domain;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab
{
    /// <summary>
    /// 特种设备缸槽
    /// </summary>
    [ChildEntity, Serializable]
    [Label("缸槽管理")]
    [DisplayMember(nameof(Code))]
    public class MeterEquipAccountSlot : SlotBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<MeterEquipAccountSlot>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<MeteringEquipmentAccount> EquipAccountProperty = P<MeterEquipAccountSlot>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public MeteringEquipmentAccount EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备")]
        public static readonly Property<string> EquipAccountNameProperty = P<MeterEquipAccountSlot>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 缸槽 实体配置
    /// </summary>
    internal class MeterEquipAccountSlotConfig : EntityConfig<MeterEquipAccountSlot>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PCB_SLOT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
