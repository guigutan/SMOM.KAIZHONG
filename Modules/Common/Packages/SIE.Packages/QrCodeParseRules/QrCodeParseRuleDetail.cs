using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("二维码解析规则明细")]
    public partial class QrCodeParseRuleDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<QrCodeParseRuleDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 截取开始位 InterceptStart
        /// <summary>
        /// 截取开始位
        /// </summary>
        [Label("截取开始位")]
        public static readonly Property<int?> InterceptStartProperty = P<QrCodeParseRuleDetail>.Register(e => e.InterceptStart);

        /// <summary>
        /// 截取开始位
        /// </summary>
        public int? InterceptStart
        {
            get { return GetProperty(InterceptStartProperty); }
            set { SetProperty(InterceptStartProperty, value); }
        }
        #endregion

        #region 截取结束位 InterceptEnd
        /// <summary>
        /// 截取结束位
        /// </summary>
        [Label("截取结束位")]
        public static readonly Property<int?> InterceptEndProperty = P<QrCodeParseRuleDetail>.Register(e => e.InterceptEnd);

        /// <summary>
        /// 截取结束位
        /// </summary>
        public int? InterceptEnd
        {
            get { return GetProperty(InterceptEndProperty); }
            set { SetProperty(InterceptEndProperty, value); }
        }
        #endregion

        #region 二维码解析规则 QrCodeParseRule
        /// <summary>
        /// 二维码解析规则Id
        /// </summary>
        public static readonly IRefIdProperty QrCodeParseRuleIdProperty = P<QrCodeParseRuleDetail>.RegisterRefId(e => e.QrCodeParseRuleId, ReferenceType.Parent);

        /// <summary>
        /// 二维码解析规则Id
        /// </summary>
        public double QrCodeParseRuleId
        {
            get { return (double)GetRefId(QrCodeParseRuleIdProperty); }
            set { SetRefId(QrCodeParseRuleIdProperty, value); }
        }

        /// <summary>
        /// 二维码解析规则
        /// </summary>
        public static readonly RefEntityProperty<QrCodeParseRule> QrCodeParseRuleProperty = P<QrCodeParseRuleDetail>.RegisterRef(e => e.QrCodeParseRule, QrCodeParseRuleIdProperty);

        /// <summary>
        /// 二维码解析规则
        /// </summary>
        public QrCodeParseRule QrCodeParseRule
        {
            get { return GetRefEntity(QrCodeParseRuleProperty); }
            set { SetRefEntity(QrCodeParseRuleProperty, value); }
        }
        #endregion

        #region 截取方式 InterceptWay
        /// <summary>
        /// 截取方式
        /// </summary>
        [Label("截取方式")]
        public static readonly Property<InterceptWay> InterceptWayProperty = P<QrCodeParseRuleDetail>.RegisterView(e => e.InterceptWay, p => p.QrCodeParseRule.InterceptWay);

        /// <summary>
        /// 截取方式
        /// </summary>
        public InterceptWay InterceptWay
        {
            get { return this.GetProperty(InterceptWayProperty); }
        }
        #endregion

        #region 解析栏位 ParseField
        /// <summary>
        /// 解析栏位
        /// </summary>
        [Label("字段名称")]
        public static readonly Property<ParseField> ParseFieldProperty = P<QrCodeParseRuleDetail>.Register(e => e.ParseField);

        /// <summary>
        /// 解析栏位
        /// </summary>
        public ParseField ParseField
        {
            get { return GetProperty(ParseFieldProperty); }
            set { SetProperty(ParseFieldProperty, value); }
        }
        #endregion

        #region 测试结果 TestResult
        /// <summary>
        /// 测试结果
        /// </summary>
        [Label("测试结果")]
        public static readonly Property<string> TestResultProperty = P<QrCodeParseRuleDetail>.Register(e => e.TestResult);

        /// <summary>
        /// 测试结果
        /// </summary>
        public string TestResult
        {
            get { return this.GetProperty(TestResultProperty); }
            set { this.SetProperty(TestResultProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 二维码解析规则明细 实体配置
    /// </summary>
    internal class QrCodeParseRuleDetailConfig : EntityConfig<QrCodeParseRuleDetail>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    QrCodeParseRuleDetail.QrCodeParseRuleIdProperty,
                    QrCodeParseRuleDetail.ParseFieldProperty,
                },
                MessageBuilder = o =>
                {
                    return "已经存在重复的字段名称".L10N();
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QR_CODE_PARSE_RULE_DTL").MapAllProperties();
            Meta.Property(QrCodeParseRuleDetail.TestResultProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
