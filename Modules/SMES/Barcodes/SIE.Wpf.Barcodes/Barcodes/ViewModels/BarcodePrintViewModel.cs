using SIE.Barcodes;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Wpf.Barcodes.Commonds;
using SIE.Wpf.Common;
using System;
using System.Text;

namespace SIE.Wpf.Barcodes.ViewModels
{
    /// <summary>
    /// 条码打印视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("条码打印视图")]
    public class BarcodePrintViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodePrintViewModel()
        {
            Printer = Settings.Default.PrinterName;
            PageCount = 1;
            SingleQty = 1;
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
        public static readonly IRefIdProperty NumberRuleIdProperty = P<BarcodePrintViewModel>.RegisterRefId(e => e.NumberRuleId,
        new RegisterRefIdArgs<double>()
        {
            ReferenceType = ReferenceType.Normal,
            PropertyChangedCallBack = (o, e) => (o as BarcodePrintViewModel).OnBarcodeRuleChanged()  //属性变更回调
        });

        #region 属性变更事件
        /// <summary>
        /// 条码规则改变事件
        /// </summary>
        private void OnBarcodeRuleChanged()
        {
            this.BarcodeRuleDtl = string.Empty;  //条码明细
            Template = null;
            if (NumberRule == null)
                return;
            StringBuilder sb = new StringBuilder();
            foreach (var ruleDtl in NumberRule.DetailList)
            {
                sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
            }
            BarcodeRuleDtl = sb.ToString();
            Preview();
        }
        #endregion

        /// <summary>
        /// 条码规则ID
        /// </summary>
        public double NumberRuleId
        {
            get { return (double)this.GetRefId(NumberRuleIdProperty); }
            set { this.SetRefId(NumberRuleIdProperty, value); }
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
        [Required]
        public static readonly Property<string> PrinterProperty = P<BarcodePrintViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as BarcodePrintViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }

        /// <summary>
        /// 打印机变更
        /// </summary>
        void OnPrinterChanged()
        {
            Settings.Default.PrinterName = Printer;
            Settings.Default.Save();
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
        public double TemplateId
        {
            get { return (double)this.GetRefId(TemplateIdProperty); }
            set { this.SetRefId(TemplateIdProperty, value); }
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
        public static readonly Property<int> PrintQtyProperty = P<BarcodePrintViewModel>.Register(e => e.PrintQty, new RegisterRefIdArgs<int>()
        {
            ReferenceType = ReferenceType.Normal,
            PropertyChangedCallBack = (o, e) => (o as BarcodePrintViewModel).OnPrintQtyChanged()
        });

        /// <summary>
        /// 打印数量属性变更事件
        /// </summary>
        private void OnPrintQtyChanged()
        {
            Preview();
        }

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
        public static readonly Property<int> SingleQtyProperty = P<BarcodePrintViewModel>.Register(e => e.SingleQty, new PropertyMetadata<int>()
        {
            PropertyChangedCallBack = (o, e) => (o as BarcodePrintViewModel).OnSingleQtyChanged()
        });

        /// <summary>
        /// 单张数量变更时同步更新条码范围
        /// </summary>
        private void OnSingleQtyChanged()
        {
            Preview();
        }

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
        public static readonly Property<bool> PrintControlProperty = P<BarcodePrintViewModel>.Register(e => e.PrintControl, new RegisterRefIdArgs<bool>()
        {
            ReferenceType = ReferenceType.Normal,
            PropertyChangedCallBack = (o, e) => (o as BarcodePrintViewModel).OnPrintControlChanged()
        });

        /// <summary>
        /// 勾选（取消）反打事件
        /// </summary>
        private void OnPrintControlChanged()
        {
            Control();
        }

        /// <summary>
        /// 打印控制
        /// </summary>
        public bool PrintControl
        {
            get { return this.GetProperty(PrintControlProperty); }
            set { this.SetProperty(PrintControlProperty, value); }
        }
        #endregion

        #region 预览、打印控制
        /// <summary>
        /// 编码规则控制器对象
        /// </summary>
        NumberRuleController controller = RT.Service.Resolve<NumberRuleController>();

        /// <summary>
        /// 预览
        /// </summary>
        private void Preview()
        {
            if (this.NumberRule == null || this.PrintQty <= 0 || SingleQty <= 0) //条码规则不空，打印数量打印0
                return;
            int fullBoxCount = PrintQty / SingleQty;
            var printQty = PrintQty % SingleQty == 0 ? fullBoxCount : fullBoxCount + 1;
            this.BeginSn = controller.GetStartSegment(NumberRuleId, WorkOrder);
            this.EndSn = controller.GetEndSegment(NumberRuleId, printQty, WorkOrder);
        }

        /// <summary>
        /// 打印控制
        /// </summary>
        private void Control()
        {
            if (this.NumberRule == null || this.PrintQty <= 0) //条码规则不空，打印数量大于0
                return;
            if (!PrintControl) //默认为正打
            {
                this.BeginSn = controller.GetStartSegment(NumberRuleId, WorkOrder);
                this.EndSn = controller.GetEndSegment(NumberRuleId, PrintQty, WorkOrder);
            }
            else
            {
                this.EndSn = controller.GetStartSegment(NumberRuleId, WorkOrder);
                this.BeginSn = controller.GetEndSegment(NumberRuleId, PrintQty, WorkOrder);
            }
        }
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
    internal class BarcodePrintViewModelViewConfig : WPFViewConfig<BarcodePrintViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(PrintWorkOrder));
            View.UseDetail(dialogWidth: 800);
            View.UseCommands(typeof(PrintCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工单信息", 3))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号");
                    View.Property(p => p.WorkOrder.PlanQty).HasLabel("计划数量");
                    View.Property(p => p.WorkOrder.PlanBeginDate).HasLabel("计划日期").UseDateTimeEditor();
                    View.Property(p => p.NumberRule).UseNumberRuleBarcodeLookUpEditor();
                    View.Property(p => p.BarcodeRuleDtl).Readonly();
                }

                using (View.DeclareGroup("打印信息", 4))
                {
                    View.Property(p => p.Printer).UsePrinterEditor();
                    View.Property(p => p.Template).UsePrintTemplateLookUpEditor(p => p.ReloadDataOnPopping = true);
                    View.Property(p => p.PrintedQty).Readonly();
                    View.Property(p => p.ResidualQty).Readonly();
                    View.Property(p => p.PrintQty);
                    View.Property(p => p.DumpingQty).Readonly();
                    View.Property(p => p.BeginSn).Readonly();
                    View.Property(p => p.EndSn).Readonly();
                    View.Property(p => p.SingleQty);
                    View.Property(p => p.PageCount);
                }

                using (View.DeclareGroup("打印控制"))
                {
                    View.Property(p => p.PrintControl);
                }
            }
        }
    }
}