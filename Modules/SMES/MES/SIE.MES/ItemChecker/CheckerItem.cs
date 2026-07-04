using Com.Ctrip.Framework.Apollo.Core.Utils;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Checker;
using SIE.MES.ItemFixture;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemChecker
{
    /// <summary>
    /// 检具与产品的关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(CheckerItemCriterial))]
    [Label("检具与产品的关系")]
    public partial class CheckerItem :DataEntity
    {
        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<CheckerItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<CheckerItem>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly IRefIdProperty ProcessIdProperty = P<CheckerItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<CheckerItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 检具编码 CheckerUphold
        /// <summary>
        /// 检具编码Id
        /// </summary>
        [Label("检具编码")]
        public static readonly IRefIdProperty CheckerUpholdIdProperty =
            P<CheckerItem>.RegisterRefId(e => e.CheckerUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 检具编码Id
        /// </summary>
        public double CheckerUpholdId
        {
            get { return (double)this.GetRefNullableId(CheckerUpholdIdProperty); }
            set { this.SetRefNullableId(CheckerUpholdIdProperty, value); }
        }

        /// <summary>
        /// 检具编码
        /// </summary>
        public static readonly RefEntityProperty<CheckerUphold> CheckerUpholdProperty =
            P<CheckerItem>.RegisterRef(e => e.CheckerUphold, CheckerUpholdIdProperty);

        /// <summary>
        /// 检具编码
        /// </summary>
        public CheckerUphold CheckerUphold
        {
            get { return this.GetRefEntity(CheckerUpholdProperty); }
            set { this.SetRefEntity(CheckerUpholdProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<CheckerItem>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return this.GetProperty(DrawingNoProperty); }
            set { this.SetProperty(DrawingNoProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<CheckerItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion

        #region 检具有效期 CheckerUpholdEDate
        /// <summary>
        /// 检具有效期
        /// </summary>
        [Label("检具有效期")]
        public static readonly Property<DateTime> CheckerUpholdEDateProperty = P<CheckerItem>.RegisterView(e => e.CheckerUpholdEDate, p => p.CheckerUphold.EffectiveDate);

        /// <summary>
        /// 检具有效期
        /// </summary>
        public DateTime CheckerUpholdEDate
        {
            get { return this.GetProperty(CheckerUpholdEDateProperty); }
        }
        #endregion

        #region 检具编码 CheckerCode
        /// <summary>
        /// 检具编码
        /// </summary>
        [Label("检具编码")]
        public static readonly Property<string> CheckerCodeProperty = P<CheckerItem>.RegisterView(e => e.CheckerCode, p => p.CheckerUphold.CheckerCode);

        /// <summary>
        /// 检具编码
        /// </summary>
        public string CheckerCode
        {
            get { return this.GetProperty(CheckerCodeProperty); }
        }

        #endregion

        #region 检具名称 CheckerName
        /// <summary>
        /// 检具名称
        /// </summary>
        [Label("检具名称")]
        public static readonly Property<string> CheckerNameProperty = P<CheckerItem>.RegisterView(e => e.CheckerName, p => p.CheckerUphold.CheckerName);

        /// <summary>
        /// 检具名称
        /// </summary>
        public string CheckerName
        {
            get { return this.GetProperty(CheckerNameProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<CheckerItem>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 图号 DrawingNo
        ///// <summary>
        ///// 图号
        ///// </summary>
        //[Label("图号")]
        //public static readonly Property<string> DrawingNoProperty = P<CheckerItem>.RegisterView(e => e.DrawingNo, p => p.CheckerUphold.DrawingNo);

        ///// <summary>
        ///// 图号
        ///// </summary>
        //public string DrawingNo
        //{
        //    get { return this.GetProperty(DrawingNoProperty); }
        //}
        #endregion

        #endregion
    }
    /// <summary>
    /// 检具与产品的关系 实体配置
    /// </summary>
    internal class CheckerItemConfig : EntityConfig<CheckerItem>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    CheckerItem.ItemIdProperty,
                    CheckerItem.ProcessIdProperty,
                    CheckerItem.CheckerUpholdIdProperty,
                    CheckerItem.DrawingNoProperty
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
            Meta.MapTable("CHECKER_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 物料被检具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料被检具与产品关系引用不允许删除")]
    [System.ComponentModel.Description("物料被检具与产品关系引用不允许删除")]
    public partial class UndeleteCheckerItem : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteCheckerItem()
        {
            Properties.Add(CheckerItem.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用,不能删除".L10nFormat(item.Code, "检具与产品的关系".L10N());
            };
        }
    }
    /// <summary>
    /// 工序被检具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("工序被检具与产品关系引用不允许删除")]
    [System.ComponentModel.Description("工序被检具与产品关系引用不允许删除")]
    public partial class UndeleteCheckerProcess : NoReferencedRule<Process>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteCheckerProcess()
        {
            Properties.Add(CheckerItem.ProcessIdProperty);
            MessageBuilder = (o, e) =>
            {
                var process = o as Process;
                return "工序[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "检具与产品的关系".L10N());
            };
        }
    }

    /// <summary>
    /// 检具被检具与产品关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("检具被检具与产品关系引用不允许删除")]
    [System.ComponentModel.Description("检具被检具与产品关系引用不允许删除")]
    public partial class UndeleteCheckerCheckerUphold : NoReferencedRule<CheckerUphold>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteCheckerCheckerUphold()
        {
            Properties.Add(CheckerItem.CheckerUpholdIdProperty);
            MessageBuilder = (o, e) =>
            {
                var checkerUphold = o as CheckerUphold;
                return "检具[{0}]已经被[{1}]引用,不能删除".L10nFormat(checkerUphold.CheckerCode, "检具与产品的关系".L10N());
            };
        }
    }
}
