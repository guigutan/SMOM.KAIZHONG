using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;


namespace SIE.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 包装号印ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装号打印")]
    public class PackingBarcodePrintViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingBarcodePrintViewModel()
        {
            PageCount = 1;
        }
        #endregion

        #region WorkOrder 工单
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<PackingBarcodePrintViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<PackingWorkOrder> WorkOrderProperty = P<PackingBarcodePrintViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public PackingWorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单包装规则 WorkOrderPackageRuleDetail
        /// <summary>
        ///工单包装规则Id
        /// </summary>
        public static readonly IRefIdProperty PackageRuleDetailIdProperty = P<PackingBarcodePrintViewModel>.RegisterRefId(e => e.PackageRuleDetailId, ReferenceType.Normal);

        /// <summary>
        /// 工单包装规则Id
        /// </summary>
        public string PackageRuleDetailId
        {
            get { return (string)GetRefNullableId(PackageRuleDetailIdProperty); }
            set { SetRefNullableId(PackageRuleDetailIdProperty, value); }
        }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly RefEntityProperty<PackageRuleDetailViewModel> PackageRuleDetailProperty = P<PackingBarcodePrintViewModel>.RegisterRef(e => e.PackageRuleDetail, PackageRuleDetailIdProperty);

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public PackageRuleDetailViewModel PackageRuleDetail
        {
            get { return GetRefEntity(PackageRuleDetailProperty); }
            set { SetRefEntity(PackageRuleDetailProperty, value); }
        }
        #endregion

        #region BarcodeRule 条码规则
        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("条码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<PackingBarcodePrintViewModel>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =P<PackingBarcodePrintViewModel>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 条码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<PackingBarcodePrintViewModel>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion

        #region Template 模板
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<PackingBarcodePrintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<PackingBarcodePrintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region BeginSn 开始条码
        /// <summary>
        /// 开始条码
        /// </summary>
        [Label("起始条码")]
        public static readonly Property<string> BeginSnProperty = P<PackingBarcodePrintViewModel>.Register(e => e.BeginSn);

        /// <summary>
        /// 开始条码
        /// </summary>
        public string BeginSn
        {
            get { return this.GetProperty(BeginSnProperty); }
            set { this.SetProperty(BeginSnProperty, value); }
        }
        #endregion

        #region EndSn 结束条码
        /// <summary>
        /// 结束条码
        /// </summary>
        [Label("结束条码")]
        public static readonly Property<string> EndSnProperty = P<PackingBarcodePrintViewModel>.Register(e => e.EndSn);

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get { return this.GetProperty(EndSnProperty); }
            set { this.SetProperty(EndSnProperty, value); }
        }
        #endregion

        #region ProductQty 产品数
        /// <summary>
        /// 产品数
        /// </summary>
        [Label("产品数")]
        public static readonly Property<int> ProductQtyProperty = P<PackingBarcodePrintViewModel>.Register(e => e.ProductQty);

        /// <summary>
        /// 产品数
        /// </summary>
        public int ProductQty
        {
            get { return this.GetProperty(ProductQtyProperty); }
            set { this.SetProperty(ProductQtyProperty, value); }
        }
        #endregion

        #region ResidualQty 剩余数量
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<int> ResidualQtyProperty = P<PackingBarcodePrintViewModel>.Register(e => e.ResidualQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int ResidualQty
        {
            get { return this.GetProperty(ResidualQtyProperty); }
            set { this.SetProperty(ResidualQtyProperty, value); }
        }
        #endregion

        #region PrintQty 打印数量
        /// <summary>
        /// 打印数量
        /// </summary>
        [Label("打印数量")]
        [MinValue(1)]
        public static readonly Property<int> PrintQtyProperty = P<PackingBarcodePrintViewModel>.Register(e => e.PrintQty);

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty
        {
            get { return this.GetProperty(PrintQtyProperty); }
            set { this.SetProperty(PrintQtyProperty, value); }
        }
        #endregion

        #region PrintedQty 已打印数量
        /// <summary>
        /// 已打印数量
        /// </summary>
        [Label("已打印数量")]
        public static readonly Property<int> PrintedQtyProperty = P<PackingBarcodePrintViewModel>.Register(e => e.PrintedQty);

        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty
        {
            get { return this.GetProperty(PrintedQtyProperty); }
            set { this.SetProperty(PrintedQtyProperty, value); }
        }
        #endregion

        #region PageCount 打印份数
        /// <summary>
        /// 打印份数
        /// </summary>
        [Label("打印份数")]
        [MinValue(1)]
        public static readonly Property<int> PageCountProperty = P<PackingBarcodePrintViewModel>.Register(e => e.PageCount);

        /// <summary>
        /// 打印份数
        /// </summary>
        public int PageCount
        {
            get { return this.GetProperty(PageCountProperty); }
            set { this.SetProperty(PageCountProperty, value); }
        }
        #endregion

        #region BarcodeRuleDtl 规则明细
        /// <summary>
        /// 规则明细
        /// </summary>
        [Label("规则明细")]
        public static readonly Property<string> BarcodeRuleDtlProperty = P<PackingBarcodePrintViewModel>.Register(e => e.BarcodeRuleDtl);

        /// <summary>
        /// 规则明细
        /// </summary>
        public string BarcodeRuleDtl
        {
            get { return this.GetProperty(BarcodeRuleDtlProperty); }
            set { this.SetProperty(BarcodeRuleDtlProperty, value); }
        }
        #endregion

        #region PrintControl 打印控制（反打）
        /// <summary>
        /// 打印控制
        /// </summary>
        [Label("反打")]
        public static readonly Property<bool> PrintControlProperty = P<PackingBarcodePrintViewModel>.Register(e => e.PrintControl);

        /// <summary>
        /// 打印控制
        /// </summary>
        public bool PrintControl
        {
            get { return this.GetProperty(PrintControlProperty); }
            set { this.SetProperty(PrintControlProperty, value); }
        }
        #endregion

        #region TemplateEntityType 模板实体类型
        /// <summary>
        /// 模板实体类型
        /// </summary>
        [Label("模板实体类型")]
        public static readonly Property<string> TemplateEntityTypeProperty = P<PackingBarcodePrintViewModel>.Register(e => e.TemplateEntityType);

        /// <summary>
        /// 模板实体类型
        /// </summary>
        public string TemplateEntityType
        {
            get { return this.GetProperty(TemplateEntityTypeProperty); }
            set { this.SetProperty(TemplateEntityTypeProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingBarcodePrintViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 工单计划数量 WorkOrderPlanQty
        /// <summary>
        /// 工单计划数量
        /// </summary>
        [Label("工单计划数量")]
        public static readonly Property<string> WorkOrderPlanQtyProperty = P<PackingBarcodePrintViewModel>.Register(e => e.WorkOrderPlanQty);

        /// <summary>
        /// 工单计划数量
        /// </summary>
        public string WorkOrderPlanQty
        {
            get { return this.GetProperty(WorkOrderPlanQtyProperty); }
            set { this.SetProperty(WorkOrderPlanQtyProperty, value); }
        }
        #endregion

        #region 工单计划开始时间 WorkOrderPlanBeginDate
        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        [Label("工单计划开始时间")]
        public static readonly Property<DateTime> WorkOrderPlanBeginDateProperty = P<PackingBarcodePrintViewModel>.Register(e => e.WorkOrderPlanBeginDate);

        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        public DateTime WorkOrderPlanBeginDate
        {
            get { return this.GetProperty(WorkOrderPlanBeginDateProperty); }
            set { this.SetProperty(WorkOrderPlanBeginDateProperty, value); }
        }
        #endregion
    }
}
