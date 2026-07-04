using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PackRule
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料二维码规则关系")]
    public partial class ItemQRCodeRule : DataEntity
    {
        #region 客户零件号 CustomPn
        /// <summary>
        /// 客户零件号
        /// </summary>
        [Required]
        [Label("客户零件号")]
        public static readonly Property<string> CustomPnProperty = P<ItemQRCodeRule>.Register(e => e.CustomPn);

        /// <summary>
        /// 客户零件号
        /// </summary>
        public string CustomPn
        {
            get { return GetProperty(CustomPnProperty); }
            set { SetProperty(CustomPnProperty, value); }
        }
        #endregion

        #region 客户版本号 VersionNumber
        /// <summary>
        /// 客户版本号
        /// </summary>
        [Label("客户版本号")]
        public static readonly Property<string> VersionNumberProperty = P<ItemQRCodeRule>.Register(e => e.VersionNumber);

        /// <summary>
        /// 客户版本号
        /// </summary>
        public string VersionNumber
        {
            get { return GetProperty(VersionNumberProperty); }
            set { SetProperty(VersionNumberProperty, value); }
        }
        #endregion

        #region 二维码规则与物料二维码关系 QRCodeRule
        /// <summary>
        /// 二维码规则与物料二维码关系Id
        /// </summary>
        public static readonly IRefIdProperty QRCodeRuleIdProperty = P<ItemQRCodeRule>.RegisterRefId(e => e.QRCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 二维码规则与物料二维码关系Id
        /// </summary>
        public double QRCodeRuleId
        {
            get { return (double)GetRefId(QRCodeRuleIdProperty); }
            set { SetRefId(QRCodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 规则编号
        /// </summary>
        [Label("规则编号")]
        [Required]
        public static readonly RefEntityProperty<QRCodeRule> QRCodeRuleProperty = P<ItemQRCodeRule>.RegisterRef(e => e.QRCodeRule, QRCodeRuleIdProperty);
        /// <summary>
        /// 规则编号
        /// </summary>
        public QRCodeRule QRCodeRule
        {
            get { return GetRefEntity(QRCodeRuleProperty); }
            set { SetRefEntity(QRCodeRuleProperty, value); }
        }
        #endregion

        #region 物料与物料二维码关系 Item
        /// <summary>
        /// 物料与物料二维码关系Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<ItemQRCodeRule>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料与物料二维码关系Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料与物料二维码关系
        /// </summary>
        [Required]
        [Label("物料编码")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemQRCodeRule>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 规则编号 RuleNumber
        /// <summary>
        /// 规则编号
        /// </summary>
        [Label("规则编号")]
        public static readonly Property<string> RuleNumberProperty = P<ItemQRCodeRule>.RegisterView(e => e.RuleNumber, p => p.QRCodeRule.RuleNumber);

        /// <summary>
        /// 规则编号
        /// </summary>
        public string RuleNumber
        {
            get { return GetProperty(RuleNumberProperty); }
            set { SetProperty(RuleNumberProperty, value); }
        }
        #endregion

        #region 规则描述 RuleNumberDesc
        /// <summary>
        /// 规则描述
        /// </summary>
        [Label("规则描述")]
        public static readonly Property<string> RuleNumberDescProperty = P<ItemQRCodeRule>.RegisterView(e => e.RuleNumberDesc, p => p.QRCodeRule.RuleNumberDesc);

        /// <summary>
        /// 规则描述
        /// </summary>
        public string RuleNumberDesc
        {
            get { return GetProperty(RuleNumberDescProperty); }
            set { SetProperty(RuleNumberDescProperty, value); }
        }
        #endregion

        #region 物料编码 Code
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<ItemQRCodeRule>.RegisterView(e => e.Code, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料名称 Name
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> NameProperty = P<ItemQRCodeRule>.RegisterView(e => e.Name, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ItemQRCodeRule>.RegisterView(e => e.ShortDescription, p => p.Item.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return GetProperty(ShortDescriptionProperty); }
            set { SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 验证质量状态 IsQualityStatus
        /// <summary>
        /// 验证质量状态
        /// </summary>
        [Label("验证质量状态")]
        public static readonly Property<bool?> IsQualityStatusProperty = P<ItemQRCodeRule>.Register(e => e.IsQualityStatus);

        /// <summary>
        /// 验证质量状态
        /// </summary>
        public bool? IsQualityStatus
        {
            get { return this.GetProperty(IsQualityStatusProperty); }
            set { this.SetProperty(IsQualityStatusProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class ItemQRCodeRuleConfig : EntityConfig<ItemQRCodeRule>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    ItemQRCodeRule.ItemIdProperty,
                    ItemQRCodeRule.QRCodeRuleIdProperty
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
            Meta.MapTable("ItemQRCodeRule").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 二维码规则编码被物料二维码规则关系引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("二维码规则编码被物料二维码规则关系引用不允许删除")]
    [System.ComponentModel.Description("二维码规则编码被物料二维码规则关系引用不允许删除")]
    public partial class UndeleteFixtureProcess : NoReferencedRule<QRCodeRule>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteFixtureProcess()
        {
            Properties.Add(ItemQRCodeRule.QRCodeRuleIdProperty);
            MessageBuilder = (o, e) =>
            {
                var qRCodeRule = o as QRCodeRule;
                return "二维码规则编码[{0}]已经被[{1}]引用,不能删除".L10nFormat(qRCodeRule.RuleNumber, "物料二维码规则关系".L10N());
            };
        }
    }
}