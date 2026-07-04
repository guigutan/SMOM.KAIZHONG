using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Items
{
    /// <summary>
    /// 打印模板设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("打印模板")]
    public class LabelPrintTemplate : DataEntity
    {
        #region 条码规则 NumberRule
        /// <summary>
        /// 条码规则Id
        /// </summary>
        [Label("条码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<LabelPrintTemplate>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<LabelPrintTemplate>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 条码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 标签模板 LabelTemplate
        /// <summary>
        /// 标签模板Id
        /// </summary>
        [Label("标签模板")]
        public static readonly IRefIdProperty LabelTemplateIdProperty =
            P<LabelPrintTemplate>.RegisterRefId(e => e.LabelTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 标签模板Id
        /// </summary>
        public double? LabelTemplateId
        {
            get { return (double?)this.GetRefNullableId(LabelTemplateIdProperty); }
            set { this.SetRefNullableId(LabelTemplateIdProperty, value); }
        }

        /// <summary>
        /// 标签模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> LabelTemplateProperty =
            P<LabelPrintTemplate>.RegisterRef(e => e.LabelTemplate, LabelTemplateIdProperty);

        /// <summary>
        /// 标签模板
        /// </summary>
        public PrintTemplate LabelTemplate
        {
            get { return this.GetRefEntity(LabelTemplateProperty); }
            set { this.SetRefEntity(LabelTemplateProperty, value); }
        }
        #endregion

        #region 包装模板 PackingTemplate
        /// <summary>
        /// 包装模板Id
        /// </summary>
        [Label("包装模板")]
        public static readonly IRefIdProperty PackingTemplateIdProperty =
            P<LabelPrintTemplate>.RegisterRefId(e => e.PackingTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 包装模板Id
        /// </summary>
        public double? PackingTemplateId
        {
            get { return (double?)this.GetRefNullableId(PackingTemplateIdProperty); }
            set { this.SetRefNullableId(PackingTemplateIdProperty, value); }
        }

        /// <summary>
        /// 包装模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PackingTemplateProperty =
            P<LabelPrintTemplate>.RegisterRef(e => e.PackingTemplate, PackingTemplateIdProperty);

        /// <summary>
        /// 包装模板
        /// </summary>
        public PrintTemplate PackingTemplate
        {
            get { return this.GetRefEntity(PackingTemplateProperty); }
            set { this.SetRefEntity(PackingTemplateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 规则编码 RuleCode
        /// <summary>
        /// 规则编码
        /// </summary>
        [Label("规则编码")]
        public static readonly Property<string> RuleCodeProperty = P<LabelPrintTemplate>.RegisterView(e => e.RuleCode, p => p.NumberRule.Code);

        /// <summary>
        /// 规则编码
        /// </summary>
        public string RuleCode
        {
            get { return this.GetProperty(RuleCodeProperty); }
        }
        #endregion

        #region 标签模板文件名 LabelFileName
        /// <summary>
        /// 标签模板文件名
        /// </summary>
        [Label("标签模板文件名")]
        public static readonly Property<string> LabelFileNameProperty = P<LabelPrintTemplate>.RegisterView(e => e.LabelFileName, p => p.LabelTemplate.FileName);

        /// <summary>
        /// 标签模板文件名
        /// </summary>
        public string LabelFileName
        {
            get { return this.GetProperty(LabelFileNameProperty); }
        }
        #endregion

        #region 包装模板文件名 PackingFileName
        /// <summary>
        /// 包装模板文件名
        /// </summary>
        [Label("包装模板文件名")]
        public static readonly Property<string> PackingFileNameProperty = P<LabelPrintTemplate>.RegisterView(e => e.PackingFileName, p => p.PackingTemplate.FileName);

        /// <summary>
        /// 包装模板文件名
        /// </summary>
        public string PackingFileName
        {
            get { return this.GetProperty(PackingFileNameProperty); }
        }
        #endregion

        #region 标签模板实体类型 LabelTemplateEntityType
        /// <summary>
        /// 标签模板实体类型
        /// </summary>
        [Label("标签模板实体类型")]
        public static readonly Property<string> LabelTemplateEntityTypeProperty
            = P<LabelPrintTemplate>.RegisterView(e => e.LabelTemplateEntityType, p => p.LabelTemplate.EntityType);

        /// <summary>
        /// 标签模板实体类型
        /// </summary>
        public string LabelTemplateEntityType
        {
            get { return this.GetProperty(LabelTemplateEntityTypeProperty); }
        }
        #endregion

        #region 包装模板实体类型 PackingTemplateEntityType
        /// <summary>
        /// 包装模板实体类型
        /// </summary>
        [Label("包装模板实体类型")]
        public static readonly Property<string> PackingTemplateEntityTypeProperty
            = P<LabelPrintTemplate>.RegisterView(e => e.PackingTemplateEntityType, p => p.PackingTemplate.EntityType);

        /// <summary>
        /// 包装模板实体类型
        /// </summary>
        public string PackingTemplateEntityType
        {
            get { return this.GetProperty(PackingTemplateEntityTypeProperty); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == LabelPrintTemplate.NumberRuleIdProperty)
            {
                LabelTemplate = null;
                PackingTemplate = null;
            }
        }
    }

    /// <summary>
    /// 打印模板配置
    /// </summary>
    internal class PrintTemplateConfig : EntityConfig<LabelPrintTemplate>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_LABEL_TEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
