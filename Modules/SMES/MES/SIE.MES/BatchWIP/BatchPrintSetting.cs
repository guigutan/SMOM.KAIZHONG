using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP
{
    /// <summary>
	/// 批次打印设置
	/// </summary>
	[RootEntity, Serializable]
    [Label("批次打印设置")]
    public partial class BatchPrintSetting : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchPrintSetting()
        {
            PageCount = 1;
        }

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("子批次预设数量")]
        public static readonly Property<decimal?> QtyProperty = P<BatchPrintSetting>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<BatchPrintSetting>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return GetProperty(PrinterProperty); }
            set { SetProperty(PrinterProperty, value); }
        }
        #endregion

        #region 打印份数 PageCount
        /// <summary>
        /// 打印份数
        /// </summary>
        [Label("打印份数")]
        [MinValue(1)]
        public static readonly Property<int> PageCountProperty = P<BatchPrintSetting>.Register(e => e.PageCount);

        /// <summary>
        /// 打印份数
        /// </summary>
        public int PageCount
        {
            get { return GetProperty(PageCountProperty); }
            set { SetProperty(PageCountProperty, value); }
        }
        #endregion

        #region 生成并打印 PrintControl
        /// <summary>
        /// 生成并打印
        /// </summary>
        [Label("生成并打印")]
        public static readonly Property<bool> PrintControlProperty = P<BatchPrintSetting>.Register(e => e.PrintControl);

        /// <summary>
        /// 生成并打印
        /// </summary>
        public bool PrintControl
        {
            get { return GetProperty(PrintControlProperty); }
            set { SetProperty(PrintControlProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则Id
        /// </summary>
        [Label("条码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<BatchPrintSetting>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)GetRefNullableId(NumberRuleIdProperty); }
            set { SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty = P<BatchPrintSetting>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return GetRefEntity(NumberRuleProperty); }
            set { SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty = P<BatchPrintSetting>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)GetRefNullableId(PrintTemplateIdProperty); }
            set { SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty = P<BatchPrintSetting>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return GetRefEntity(PrintTemplateProperty); }
            set { SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BatchPrintSetting>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BatchPrintSetting>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(NumberRule))
                PrintTemplate = null;
        }
    }

    /// <summary>
    /// 批次打印设置 实体配置
    /// </summary>
    internal class BatchPrintSettingConfig : EntityConfig<BatchPrintSetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BATCH_PRINT_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}