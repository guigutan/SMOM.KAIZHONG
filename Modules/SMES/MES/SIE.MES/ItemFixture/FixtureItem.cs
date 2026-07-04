using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Checker;
using SIE.MES.Fixture;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemFixture
{
    /// <summary>
    /// 工装与产品的关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureItemCriterial))]
    [Label("工装与产品的关系")]
    public partial class FixtureItem : DataEntity
    {
        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<FixtureItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<FixtureItem>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly IRefIdProperty ProcessIdProperty = P<FixtureItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<FixtureItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工装唯一码 FixtureUphold
        /// <summary>
        /// 工装唯一码
        /// </summary>
        [Label("工装唯一码")]
        public static readonly IRefIdProperty FixtureUpholdIdProperty =
            P<FixtureItem>.RegisterRefId(e => e.FixtureUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 工装唯一码Id
        /// </summary>
        public double FixtureUpholdId
        {
            get { return (double)this.GetRefNullableId(FixtureUpholdIdProperty); }
            set { this.SetRefNullableId(FixtureUpholdIdProperty, value); }
        }

        /// <summary>
        /// 工装唯一码
        /// </summary>
        public static readonly RefEntityProperty<FixtureUphold> FixtureUpholdProperty =
            P<FixtureItem>.RegisterRef(e => e.FixtureUphold, FixtureUpholdIdProperty);

        /// <summary>
        /// 工装唯一码
        /// </summary>
        public FixtureUphold FixtureUphold
        {
            get { return this.GetRefEntity(FixtureUpholdProperty); }
            set { this.SetRefEntity(FixtureUpholdProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<FixtureItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion

        #region 工装状态 FixtureUpholdState
        /// <summary>
        /// 工装状态
        /// </summary>
        [Label("工装状态")]
        public static readonly Property<string> FixtureUpholdStateProperty = P<FixtureItem>.RegisterView(e => e.FixtureUpholdState, p => p.FixtureUphold.FixtureState);

        /// <summary>
        /// 工装状态
        /// </summary>
        public string FixtureUpholdState
        {
            get { return this.GetProperty(FixtureUpholdStateProperty); }
        }
        #endregion

        #region 工装唯一码 FixtureCode
        /// <summary>
        /// 工装唯一码
        /// </summary>
        [Label("工装唯一码")]
        public static readonly Property<string> FixtureCodeProperty = P<FixtureItem>.RegisterView(e => e.FixtureCode, p => p.FixtureUphold.FixtureCode);

        /// <summary>
        /// 工装唯一码
        /// </summary>
        public string FixtureCode
        {
            get { return this.GetProperty(FixtureCodeProperty); }
        }

        #endregion

        #region 工装物料描述 FixtureName
        /// <summary>
        /// 工装物料描述
        /// </summary>
        [Label("工装物料描述")]
        public static readonly Property<string> FixtureNameProperty = P<FixtureItem>.RegisterView(e => e.FixtureName, p => p.FixtureUphold.FixtureName);

        /// <summary>
        /// 工装物料描述
        /// </summary>
        public string FixtureName
        {
            get { return this.GetProperty(FixtureNameProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<FixtureItem>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 图号 Drawn
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawnProperty = P<FixtureItem>.RegisterView(e => e.Drawn, p => p.FixtureUphold.Drawn);

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 工装与产品的关系 实体配置
    /// </summary>
    internal class FixtureItemConfig : EntityConfig<FixtureItem>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    FixtureItem.ItemIdProperty,
                    FixtureItem.ProcessIdProperty,
                    FixtureItem.FixtureUpholdIdProperty
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
            Meta.MapTable("FIXTURE_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
    /// <summary>
    /// 物料被工装与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料被工装与产品关系引用不允许删除")]
    [System.ComponentModel.Description("物料被工装与产品关系引用不允许删除")]
    public partial class UndeleteFixtureItem : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureItem()
        {
            Properties.Add(FixtureItem.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用,不能删除".L10nFormat(item.Code, "工装与产品的关系".L10N());
            };
        }
    }
    /// <summary>
    /// 工序被工装与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("工序被工装与产品关系引用不允许删除")]
    [System.ComponentModel.Description("工序被工装与产品关系引用不允许删除")]
    public partial class UndeleteFixtureProcess : NoReferencedRule<Process>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureProcess()
        {
            Properties.Add(FixtureItem.ProcessIdProperty);
            MessageBuilder = (o, e) =>
            {
                var process = o as Process;
                return "工序[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "工装与产品的关系".L10N());
            };
        }
    }

    /// <summary>
    /// 工装被工装与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("工装被工装与产品关系引用不允许删除")]
    [System.ComponentModel.Description("工装被工装与产品关系引用不允许删除")]
    public partial class UndeleteFixtureUphold : NoReferencedRule<FixtureUphold>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureUphold()
        {
            Properties.Add(FixtureItem.FixtureUpholdIdProperty);
            MessageBuilder = (o, e) =>
            {
                var checkerUphold = o as FixtureUphold;
                return "工装[{0}]已经被[{1}]引用,不能删除".L10nFormat(checkerUphold.FixtureCode, "工装与产品的关系".L10N());
            };
        }
    }
}
