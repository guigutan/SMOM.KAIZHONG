using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipStatus.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus
{
    /// <summary>
    /// 设备状态
    /// </summary>
    [RootEntity,Serializable]
    [ConditionQueryType(typeof(EquipStatusCriteria))]
    [Label("设备状态")]
    public class EquipStatus : DataEntity
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipStatus>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
            P<EquipStatus>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<EquipStatusDetailStatus> StatusProperty = P<EquipStatus>.Register(e => e.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public EquipStatusDetailStatus Status
        {
            get { return this.GetProperty(StatusProperty); }
            set { this.SetProperty(StatusProperty, value); }
        }
        #endregion

        #region 当前工厂 Factory
        /// <summary>
        /// 当前工厂
        /// </summary>
        [Label("当前工厂")]
        public static readonly Property<string> FactoryProperty = P<EquipStatus>.Register(e => e.Factory);

        /// <summary>
        /// 当前工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 设备状态明细 EquipStatusDetailList
        /// <summary>
        /// 设备状态明细
        /// </summary>
        [Label("设备状态明细")]
        public static readonly ListProperty<EntityList<EquipStatusDetail>> EquipStatusDetailListProperty = P<EquipStatus>.RegisterList(e => e.EquipStatusDetailList);

        /// <summary>
        /// 设备状态明细
        /// </summary>
        public EntityList<EquipStatusDetail> EquipStatusDetailList
        {
            get { return this.GetLazyList(EquipStatusDetailListProperty); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipStatus>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipStatus>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

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

    internal class EquipStatusConfig : EntityConfig<EquipStatus>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("EQUIP_STATUS").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
