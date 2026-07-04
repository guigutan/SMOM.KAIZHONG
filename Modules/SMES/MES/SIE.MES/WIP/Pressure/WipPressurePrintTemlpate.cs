using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压工序产品与打印模板关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("耐压工序产品与打印模板关系")]
    [CriteriaQuery]
    [DisplayMember(nameof(Id))]
    public partial class WipPressurePrintTemlpate : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipPressurePrintTemlpate()
        {
        }

        #region 客户 Customer
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerProperty = P<WipPressurePrintTemlpate>.Register(e => e.Customer);

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer
        {
            get { return this.GetProperty(CustomerProperty); }
            set { this.SetProperty(CustomerProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<WipPressurePrintTemlpate>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)this.GetRefId(ProductIdProperty); }
            set { this.SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<WipPressurePrintTemlpate>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region SN编码规则 NumberRule 
        /// <summary>
        /// SN编码规则ID
        /// </summary>
        [Label("SN编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<WipPressurePrintTemlpate>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// SN编码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// SN编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty = P<WipPressurePrintTemlpate>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// SN编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region SN编码规则(暗码) NumberRule 
        /// <summary>
        /// SN编码规则ID(暗码)
        /// </summary>
        [Label("SN编码规则(暗码)")]
        public static readonly IRefIdProperty NumberRuleId2Property = P<WipPressurePrintTemlpate>.RegisterRefId(e => e.NumberRuleId2, ReferenceType.Normal);

        /// <summary>
        /// SN编码规则ID(暗码)
        /// </summary>
        public double? NumberRuleId2
        {
            get { return (double?)this.GetRefNullableId(NumberRuleId2Property); }
            set { this.SetRefNullableId(NumberRuleId2Property, value); }
        }

        /// <summary>
        /// SN编码规则(暗码)
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRule2Property = P<WipPressurePrintTemlpate>.RegisterRef(e => e.NumberRule2, NumberRuleId2Property);

        /// <summary>
        /// SN编码规则(暗码)
        /// </summary>
        public NumberRule NumberRule2
        {
            get { return this.GetRefEntity(NumberRule2Property); }
            set { this.SetRefEntity(NumberRule2Property, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<WipPressurePrintTemlpate>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<WipPressurePrintTemlpate>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        //#region 打印设置 PrinterSettingTpl
        ///// <summary>
        ///// 打印设置Id
        ///// </summary>
        //[Label("打印设置")]
        //public static readonly IRefIdProperty PrinterSettingTplIdProperty =
        //    P<WipPressurePrintTemlpate>.RegisterRefId(e => e.PrinterSettingTplId, ReferenceType.Normal);

        ///// <summary>
        ///// 打印设置Id
        ///// </summary>
        //public double? PrinterSettingTplId
        //{
        //    get { return (double?)this.GetRefNullableId(PrinterSettingTplIdProperty); }
        //    set { this.SetRefNullableId(PrinterSettingTplIdProperty, value); }
        //}

        ///// <summary>
        ///// 打印设置
        ///// </summary>
        //public static readonly RefEntityProperty<PrinterSettingTpl> PrinterSettingTplProperty =
        //    P<WipPressurePrintTemlpate>.RegisterRef(e => e.PrinterSettingTpl, PrinterSettingTplIdProperty);

        ///// <summary>
        ///// 打印设置
        ///// </summary>
        //public PrinterSettingTpl PrinterSettingTpl
        //{
        //    get { return this.GetRefEntity(PrinterSettingTplProperty); }
        //    set { this.SetRefEntity(PrinterSettingTplProperty, value); }
        //}
        //#endregion


        #region 视图属性

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipPressurePrintTemlpate>.RegisterView(e => e.ProductCode, e => e.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WipPressurePrintTemlpate>.RegisterView(e => e.ProductName, e => e.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 打印模板 PrintTemplateFileName
        /// <summary>
        /// 打印模板
        /// </summary>
        [Label("打印模板")]
        public static readonly Property<string> PrintTemplateFileNameProperty = P<WipPressurePrintTemlpate>.RegisterView(e => e.PrintTemplateFileName, p => p.PrintTemplate.FileName);

        /// <summary>
        /// 打印模板
        /// </summary>
        public string PrintTemplateFileName
        {
            get { return this.GetProperty(PrintTemplateFileNameProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class WipPressurePrintTemlpateEntityConfig : EntityConfig<WipPressurePrintTemlpate>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    WipPressurePrintTemlpate.ProductIdProperty,
                    WipPressurePrintTemlpate.CustomerProperty,
                },
                MessageBuilder = (e) =>
                {
                    return "产品与客户不能重复!".L10N();
                }
            });
            base.AddValidations(rules);
        }

        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PRESSURE_TPL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}