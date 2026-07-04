using SIE.Barcodes;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码打印ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("单体打印")]
    public class BarcodePrintViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodePrintViewModel()
        { 
            PageCount = 1;
            SingleQty = 1;
            DumpingQty = 0;
        }
        #endregion

        #region WorkOrder 工单
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
           P<BarcodePrintViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PrintWorkOrder> WorkOrderProperty =
            P<BarcodePrintViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public PrintWorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region BarcodeRule 条码规则
        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("条码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<BarcodePrintViewModel>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<BarcodePrintViewModel>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
        //[Required]
        public static readonly Property<string> PrinterProperty = P<BarcodePrintViewModel>.Register(e => e.Printer);

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
        public static readonly IRefIdProperty TemplateIdProperty = P<BarcodePrintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<BarcodePrintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

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
        public static readonly Property<string> BeginSnProperty = P<BarcodePrintViewModel>.Register(e => e.BeginSn);

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
        public static readonly Property<string> EndSnProperty = P<BarcodePrintViewModel>.Register(e => e.EndSn);

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get { return this.GetProperty(EndSnProperty); }
            set { this.SetProperty(EndSnProperty, value); }
        }
        #endregion

        #region ResidualQty 剩余数量
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<int> ResidualQtyProperty = P<BarcodePrintViewModel>.Register(e => e.ResidualQty);

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
        public static readonly Property<int> PrintQtyProperty = P<BarcodePrintViewModel>.Register(e => e.PrintQty);

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
        public static readonly Property<int> PrintedQtyProperty = P<BarcodePrintViewModel>.Register(e => e.PrintedQty);

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
        public static readonly Property<int> PageCountProperty = P<BarcodePrintViewModel>.Register(e => e.PageCount);

        /// <summary>
        /// 打印份数
        /// </summary>
        public int PageCount
        {
            get { return this.GetProperty(PageCountProperty); }
            set { this.SetProperty(PageCountProperty, value); }
        }
        #endregion

        #region SingleQty 单张数量
        /// <summary>
        /// 单张数量
        /// </summary>
        [Label("单张数量")]
        [MinValue(1)]
        public static readonly Property<int> SingleQtyProperty = P<BarcodePrintViewModel>.Register(e => e.SingleQty);

        /// <summary>
        /// 单张数量
        /// </summary>
        public int SingleQty
        {
            get { return this.GetProperty(SingleQtyProperty); }
            set { this.SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region DumpingQty 报废数量
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<int> DumpingQtyProperty = P<BarcodePrintViewModel>.Register(e => e.DumpingQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public int DumpingQty
        {
            get { return this.GetProperty(DumpingQtyProperty); }
            set { this.SetProperty(DumpingQtyProperty, value); }
        }
        #endregion

        #region BarcodeRuleDtl 条码规则明细
        /// <summary>
        /// 条码规则明细
        /// </summary>
        [Label("规则明细")]
        public static readonly Property<string> BarcodeRuleDtlProperty = P<BarcodePrintViewModel>.Register(e => e.BarcodeRuleDtl);

        /// <summary>
        /// 条码规则明细
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
        public static readonly Property<bool> PrintControlProperty = P<BarcodePrintViewModel>.Register(e => e.PrintControl);

        /// <summary>
        /// 打印控制
        /// </summary>
        public bool PrintControl
        {
            get { return this.GetProperty(PrintControlProperty); }
            set { this.SetProperty(PrintControlProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BarcodePrintViewModel>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 工单计划数量 WorkOrderPlanQty
        /// <summary>
        /// 工单计划数量
        /// </summary>
        [Label("工单计划数量")]
        public static readonly Property<string> WorkOrderPlanQtyProperty = P<BarcodePrintViewModel>.RegisterView(e => e.WorkOrderPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单计划数量
        /// </summary>
        public string WorkOrderPlanQty
        {
            get { return this.GetProperty(WorkOrderPlanQtyProperty); }
        }
        #endregion

        #region 工单计划开始时间 WorkOrderPlanBeginDate
        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        [Label("工单计划开始时间")]
        public static readonly Property<DateTime> WorkOrderPlanBeginDateProperty = P<BarcodePrintViewModel>.RegisterView(e => e.WorkOrderPlanBeginDate, p => p.WorkOrder.PlanBeginDate);

        /// <summary>
        /// 工单计划开始时间
        /// </summary>
        public DateTime WorkOrderPlanBeginDate
        {
            get { return this.GetProperty(WorkOrderPlanBeginDateProperty); }
        }
        #endregion

        #region TemplateEntityType 模板实体类型
        /// <summary>
        /// 模板实体类型
        /// </summary>
        [Label("模板实体类型")]
        public static readonly Property<string> TemplateEntityTypeProperty = P<BarcodePrintViewModel>.Register(e => e.TemplateEntityType);

        /// <summary>
        /// 模板实体类型
        /// </summary>
        public string TemplateEntityType
        {
            get { return this.GetProperty(TemplateEntityTypeProperty); }
            set { this.SetProperty(TemplateEntityTypeProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 条码范围 实体配置
    /// </summary>
    internal class BarcodePrintViewModelConfig : EntityConfig<BarcodePrintViewModel>
    {
        /// <summary>
        /// 增加实体验证规则
        /// </summary>
        /// <param name="rules">实体验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(BarcodePrintViewModel.PrintQtyProperty, new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var model = o as BarcodePrintViewModel;
                    if (model != null && model.PrintQty > model.ResidualQty)
                        e.BrokenDescription = "打印数量：{0} 不能大于剩余数量：{1}".L10nFormat(model.PrintQty, model.ResidualQty);
                }
            });
            rules.Add(BarcodePrintViewModel.SingleQtyProperty, new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var model = o as BarcodePrintViewModel;
                    if (model != null && model.SingleQty > model.PrintQty)
                        e.BrokenDescription = "单张数量：{0} 不能大于打印数量：{1}".L10nFormat(model.SingleQty, model.PrintQty);
                }
            });
        }
    }

    /// <summary>
    /// 单体打印视图配置
    /// </summary>
    internal class BarcodePrintViewModelViewConfig : WebViewConfig<BarcodePrintViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDetail(dialogWidth: 900);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(PrintWorkOrder));
            View.UseCommands("SIE.Web.Barcodes.PrintCommand");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工单信息", 3))
                {
                    View.Property(p => p.WorkOrderNo).HasLabel("工单号").Readonly();
                    View.Property(p => p.WorkOrderPlanQty).HasLabel("计划数量").UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.WorkOrderPlanBeginDate).HasLabel("计划日期").UseDateTimeEditor().Readonly();
                    View.Property(p => p.NumberRule).UseNumberRuleBarcodeLookUpEditor().HasLabel("条码规则")
                        .UseListSetting(e => { e.HelpInfo = "显示规则类型为产品条码的编码规则"; });
                    View.Property(p => p.BarcodeRuleDtl).Readonly();
                }

                using (View.DeclareGroup("打印信息", 4))
                {
                    ////View.Property(p => p.Printer).HasLabel("打印机"); //.UsePrinterEditor();
                    View.Property(p => p.Template).UsePrintTemplateLookUpEditor().HasLabel("打印模板")
                        .UseListSetting(e => { e.HelpInfo = "显示当前条码规则的打印模板"; });
                    View.Property(p => p.PrintedQty).UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.ResidualQty).UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.PrintQty).UseSpinEditor(e => e.MinValue = 0);
                    View.Property(p => p.DumpingQty).UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.SingleQty).UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.PageCount).UseSpinEditor(e => e.MinValue = 0);
                    View.Property(p => p.BeginSn).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.EndSn).ShowInDetail(columnSpan: 2).Readonly();
                }

                using (View.DeclareGroup("打印控制"))
                {
                    View.Property(p => p.PrintControl);
                }
            }
        }
    }
}
