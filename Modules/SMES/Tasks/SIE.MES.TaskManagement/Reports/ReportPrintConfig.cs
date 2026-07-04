using SIE.Common.Prints;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工打印设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("报工打印设置")]
    public partial class ReportPrintConfig : DataEntity
    {
        #region Template 打印模板
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<ReportPrintConfig>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double?)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<ReportPrintConfig>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty = P<ReportPrintConfig>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double ProductFamilyId
        {
            get { return (double)GetRefId(ProductFamilyIdProperty); }
            set { SetRefId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty = P<ReportPrintConfig>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return GetRefEntity(ProductFamilyProperty); }
            set { SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 报工打印设置 实体配置
    /// </summary>
    internal class ReportPrintConfigConfig : EntityConfig<ReportPrintConfig>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_PRT_CFG").MapAllProperties();
            Meta.Property(ReportPrintConfig.ProductFamilyIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 附件关联属性打印模板
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public static class FamilyExtReportPrint
    {
        #region 打印模板 GetFamilyPrintRule
        /// <summary>
        /// 打印模板 扩展属性。
        /// </summary>
        public static readonly Property<ReportPrintConfig> FamilyPrintRuleProperty =
            P<ProductFamily>.RegisterExtension<ReportPrintConfig>("FamilyPrintRule", typeof(FamilyExtReportPrint));

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="me">产品族</param>
        public static ReportPrintConfig GetFamilyPrintRule(ProductFamily me)
        {
            return me.GetProperty(FamilyPrintRuleProperty);
        }

        /// <summary>
        /// 设置打印模板
        /// </summary>
        /// <param name="me">产品族</param>
        /// <param name="value">打印模板</param>
        public static void SetPropertyName(Item me, ReportPrintConfig value)
        {
            me.SetProperty(FamilyPrintRuleProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 作用是映射的时候能找到对应的实体
    /// </summary>
    internal class FamilyExtReportPrintConfig : EntityConfig<ProductFamily>
    {
        protected override void ConfigMeta()
        {
            Meta.Property(FamilyExtReportPrint.FamilyPrintRuleProperty).DontMapColumn();
        }
    }
}
