using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MES.ItemChecker;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.ItemEquipAccount
{
    /// <summary>
    /// 模具与产品的关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipAccountItemCriterial))]
    [EntityWithConfig(typeof(Configs.EquipAccountItemConfig))]
    [Label("模具与产品的关系")]
    public class EquipAccountItem : DataEntity
    {
        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<EquipAccountItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<EquipAccountItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工序名称 Process
        /// <summary>
        /// 工序名称Id
        /// </summary>
        [Label("工序名称")]
        public static readonly IRefIdProperty ProcessIdProperty = P<EquipAccountItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序名称Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<EquipAccountItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 模具编码 EquipAccount
        /// <summary>
        /// 模具编码Id
        /// </summary>
        [Label("模具编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipAccountItem>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 模具编码Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 模具编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<EquipAccountItem>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 模具编码
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 唯一码 UniqueCode
        /// <summary>
        /// 唯一码
        /// </summary>
        [Label("唯一码")]
        public static readonly Property<string> UniqueCodeProperty = P<EquipAccountItem>.Register(e => e.UniqueCode);

        /// <summary>
        /// 唯一码
        /// </summary>
        public string UniqueCode
        {
            get { return this.GetProperty(UniqueCodeProperty); }
            set { this.SetProperty(UniqueCodeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<EquipAccountItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<EquipAccountItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 旧料号 OldItem
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> OldItemProperty = P<EquipAccountItem>.RegisterView(e => e.OldItem, p => p.Item.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldItem
        {
            get { return this.GetProperty(OldItemProperty); }
        }

        #endregion

        #region 物料类型 ItemMtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> ItemMtartProperty = P<EquipAccountItem>.RegisterView(e => e.ItemMtart, p => p.Item.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ItemMtart
        {
            get { return this.GetProperty(ItemMtartProperty); }
        }
        #endregion


        #region 模具状态 EquipAccountState
        /// <summary>
        /// 模具状态
        /// </summary>
        [Label("模具状态")]
        public static readonly Property<AccountState?> EquipAccountStateProperty = P<EquipAccountItem>.RegisterView(e => e.EquipAccountState, p => p.EquipAccount.State);

        /// <summary>
        /// 模具状态
        /// </summary>
        public AccountState? EquipAccountState
        {
            get { return this.GetProperty(EquipAccountStateProperty); }
        }
        #endregion

        #region 模具编码 EquipAccountCode
        /// <summary>
        /// 模具编码
        /// </summary>
        [Label("模具编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipAccountItem>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 模具编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 模具名称 EquipAccountName
        /// <summary>
        /// 模具名称
        /// </summary>
        [Label("模具名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipAccountItem>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 模具名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<EquipAccountItem>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 模具图号 Drawn
        /// <summary>
        /// 模具图号
        /// </summary>
        [Label("模具图号")]
        public static readonly Property<string> DrawnProperty = P<EquipAccountItem>.RegisterView(e => e.Drawn, p => p.EquipAccount.Drawn);

        /// <summary>
        /// 模具图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
        }
        #endregion

        #region 模具穴位 EquipAcupoint
        /// <summary>
        /// 模具穴位
        /// </summary>
        [Label("模具穴位")]
        public static readonly Property<string> EquipAcupointProperty = P<EquipAccountItem>.RegisterView(e => e.EquipAcupoint, p => p.EquipAccount.Acupoint);

        /// <summary>
        /// 模具穴位
        /// </summary>
        public string EquipAcupoint
        {
            get { return this.GetProperty(EquipAcupointProperty); }
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 模具与产品的关系 实体配置
    /// </summary>
    internal class EquipAccountItemConfig : EntityConfig<EquipAccountItem>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    EquipAccountItem.ItemIdProperty,
                    EquipAccountItem.ProcessIdProperty,
                    EquipAccountItem.EquipAccountIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EQUIPACCOUNT_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 物料被模具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料被模具与产品关系引用不允许删除")]
    [System.ComponentModel.Description("物料被模具与产品关系引用不允许删除")]
    public partial class UndeleteEquipAccountItem : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteEquipAccountItem()
        {
            Properties.Add(EquipAccountItem.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用,不能删除".L10nFormat(item.Code, "模具与产品的关系".L10N());
            };
        }
    }
    /// <summary>
    /// 工序被模具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("工序被模具与产品关系引用不允许删除")]
    [System.ComponentModel.Description("工序被模具与产品关系引用不允许删除")]
    public partial class UndeleteFixtureProcess : NoReferencedRule<Process>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureProcess()
        {
            Properties.Add(EquipAccountItem.ProcessIdProperty);
            MessageBuilder = (o, e) =>
            {
                var process = o as Process;
                return "工序[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "模具与产品的关系".L10N());
            };
        }
    }

    /// <summary>
    /// 设备被模具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("设备被模具与产品关系引用不允许删除")]
    [System.ComponentModel.Description("设备被模具与产品关系引用不允许删除")]
    public partial class UndeleteFixtureUphold : NoReferencedRule<EquipAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureUphold()
        {
            Properties.Add(EquipAccountItem.EquipAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                var checkerUphold = o as EquipAccount;
                return "设备[{0}]已经被[{1}]引用,不能删除".L10nFormat(checkerUphold.Code, "模具与产品的关系".L10N());
            };
        }
    }
}
