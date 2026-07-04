using SIE.Domain;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
	/// 设备台账工序
	/// </summary>
	[RootEntity, Serializable]
    [Label("设备台账工序")]
    public partial class EquipAccountProcess : ProcessBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipAccountProcess>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<EquipAccountProcess>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备台账工序 实体配置
    /// </summary>
    internal class EquipAccountProcessConfig : EntityConfig<EquipAccountProcess>
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