using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts.TabBases;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账物联参数
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账物联参数")]
    public class EquipAccountPhysicalUnion : PhysicalUnionBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipAccountPhysicalUnion>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
            P<EquipAccountPhysicalUnion>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
    /// 设备台账物联参数 实体配置
    /// </summary>
    internal class EquipAccountPhysicalUnionConfig : EntityConfig<EquipAccountPhysicalUnion>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_PHYSICAL_UNION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
