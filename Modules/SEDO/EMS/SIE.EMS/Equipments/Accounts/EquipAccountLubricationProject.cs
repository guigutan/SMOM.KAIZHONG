using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备型号润滑项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账润滑项目")]
    public partial class EquipAccountLubricationProject : EquipAccountLubricationProjectBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipAccountLubricationProject>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<EquipAccount> EquipModelProperty = P<EquipAccountLubricationProject>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount
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
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipAccountLubricationProject>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

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
        public static readonly ListProperty<EntityList<EquipAccountLubricaSparePart>> LubricaSparePartListProperty = P<EquipAccountLubricationProject>.RegisterList(e => e.LubricaSparePartList);
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public EntityList<EquipAccountLubricaSparePart> LubricaSparePartList
        {
            get { return this.GetLazyList(LubricaSparePartListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 设备台账校验项目 实体配置
    /// </summary>
    internal class EquipAccountLubricationProjectConfig : EntityConfig<EquipAccountLubricationProject>
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
