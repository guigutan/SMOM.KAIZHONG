using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 仪器参数
    /// </summary>
    [RootEntity, Serializable]    
    [Label("仪器参数")]
    [DisplayMember(nameof(Name))]
    public partial class MeteringEquipParam : EquipParamBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<MeteringEquipParam>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
            P<MeteringEquipParam>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
    /// 仪器参数 实体配置
    /// </summary>
    internal class EquipParamConfig : EntityConfig<MeteringEquipParam>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_PARAM").MapAllProperties();
            Meta.Property(MeteringEquipParam.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}