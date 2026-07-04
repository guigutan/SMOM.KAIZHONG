using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
	/// 报工规则配置
	/// </summary>
	[RootEntity, Serializable]
    [Label("报工规则配置")]
    public partial class ReportRuleConfig : DataEntity
    {
        #region 报工数量 ReportQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<ReportRuleConfig>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 是否剩余数报工 IsModReport
        /// <summary>
        /// 是否剩余数报工
        /// </summary>
        [Label("是否剩余数报工")]
        public static readonly Property<bool> IsModReportProperty = P<ReportRuleConfig>.Register(e => e.IsModReport);

        /// <summary>
        /// 是否剩余数报工
        /// </summary>
        public bool IsModReport
        {
            get { return GetProperty(IsModReportProperty); }
            set { SetProperty(IsModReportProperty, value); }
        }
        #endregion

        #region 是否共模报工 IsSyntype
        /// <summary>
        /// 是否共模报工
        /// </summary>
        [Label("是否共模报工")]
        public static readonly Property<bool> IsSyntypeProperty = P<ReportRuleConfig>.Register(e => e.IsSyntype);

        /// <summary>
        /// 是否共模报工
        /// </summary>
        public bool IsSyntype
        {
            get { return GetProperty(IsSyntypeProperty); }
            set { SetProperty(IsSyntypeProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty = P<ReportRuleConfig>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty = P<ReportRuleConfig>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return GetRefEntity(ProductFamilyProperty); }
            set { SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 报工方式 ReportMode
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<ReportMode> ReportModeProperty = P<ReportRuleConfig>.Register(e => e.ReportMode);

        /// <summary>
        /// 报工方式
        /// </summary>
        public ReportMode ReportMode
        {
            get { return GetProperty(ReportModeProperty); }
            set { SetProperty(ReportModeProperty, value); }
        }
        #endregion


        #region 不合格报工数量是否耗用物料 IsExpendItem
        /// <summary>
        /// 不合格报工数量是否耗用物料
        /// </summary>
        [Label("不合格报工数量是否耗用物料")]
        public static readonly Property<bool> IsExpendItemProperty = P<ReportRuleConfig>.Register(e => e.IsExpendItem);

        /// <summary>
        /// 不合格报工数量是否耗用物料
        /// </summary>
        public bool IsExpendItem
        {
            get { return this.GetProperty(IsExpendItemProperty); }
            set { this.SetProperty(IsExpendItemProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 报工规则配置 实体配置
    /// </summary>
    internal class ReportRuleConfigConfig : EntityConfig<ReportRuleConfig>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_RULE_CFG").MapAllProperties();
            Meta.Property(ReportRuleConfig.ProductFamilyIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 报工规则配置信息
    /// </summary>
    [Serializable]
    public class ReportRuleConfigInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportRuleConfigInfo()
        {
            Family = -1;
            ReportQty = 1;
        }

        /// <summary>
        /// 报工方式：0自动  1手动
        /// </summary>
        public int ReportMode { get; set; }

        /// <summary>
        /// 0按默认值报工  1剩余可报工数报工
        /// </summary>
        public int ModReport { get; set; }

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 共模报工
        /// </summary>
        public bool IsSyntype { get; set; }

        /// <summary>
        /// 控制界面控件禁用
        /// </summary>
        public int Family { get; set; }

        /// <summary>
        /// 不合格报工数量 是否耗用物料
        /// </summary>
        public bool IsExpendItem { get; set; }
    }

    /// <summary>
    /// 产品族报工规则设置
    /// </summary>
    [Serializable]
    public class FamilyReportRuleConfig
    {
        /// <summary>
        /// 产品族ID
        /// </summary>
        public double FamilyId { get; set; }

        /// <summary>
        /// 报工规则配置信息
        /// </summary>
        public ReportRuleConfigInfo Config { get; set; } = new ReportRuleConfigInfo();
    }
}