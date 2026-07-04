using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 设备型号润滑项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账润滑项目")]
    public partial class MeteringEquipAccountLubricationProject : EquipAccountLubricationProjectBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<MeteringEquipAccountLubricationProject>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<MeteringEquipmentAccount> EquipModelProperty = P<MeteringEquipAccountLubricationProject>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public MeteringEquipmentAccount EquipAccount
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<MeteringEquipAccountLubricationProject>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion 

        #region 备件清单列表 EquipAccountLubricaSparePartList
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipAccountLubricaSparePart>> LubricaSparePartListProperty = P<MeteringEquipAccountLubricationProject>.RegisterList(e => e.LubricaSparePartList);
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public EntityList<MeteringEquipAccountLubricaSparePart> LubricaSparePartList
        {
            get { return this.GetLazyList(LubricaSparePartListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 设备台账校验项目 实体配置
    /// </summary>
    internal class EquipAccountLubricationProjectConfig : EntityConfig<MeteringEquipAccountLubricationProject>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ACCOUNT_LUBRICAT_PRJ").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
