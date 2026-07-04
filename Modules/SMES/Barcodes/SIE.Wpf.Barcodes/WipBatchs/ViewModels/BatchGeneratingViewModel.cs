using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using System;
using System.Linq;
using System.Text;

namespace SIE.Wpf.Barcodes.WipBatchs.ViewModels
{
    /// <summary>
    /// 批次生成ViewModel
    /// </summary>
    [RootEntity, Label("批次生成")]
    public class BatchGeneratingViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数    
        /// </summary>
        /// <param name="batchWorkOrder">批次工单</param>
        public BatchGeneratingViewModel(WorkOrder batchWorkOrder)
        {
            //工单物料对应物料批次规则
            ItemBatchRule batchRule = RT.Service.Resolve<Items.ItemController>().GetBatchRule(batchWorkOrder.ProductId);
            if (batchRule.BatchRule == SIE.Core.Items.BatchRule.WorkOrder)
            {
                var qty = RT.Service.Resolve<WorkOrderController>().GetWipBatch(batchWorkOrder.Id)?.Qty;
                if (qty == null)
                {
                    CRT.MessageService.ShowWarning("工单:{0}未设置生成批次。".L10nFormat(batchWorkOrder.No));
                }

                batchRule.Qty = qty ?? 0;
            }
            else if (batchRule.BatchRule == SIE.Core.Items.BatchRule.Vehicle)
            {
                var qty = RT.Service.Resolve<BoxController>().GetDefaultProductCapacity(batchWorkOrder.ProductId, null).FirstOrDefault()?.Capacity;
                if (qty == null)
                {
                    CRT.MessageService.ShowWarning("物料:{0}未设置默认载具。".L10nFormat(batchWorkOrder.Product.Name));
                }

                batchRule.Qty = qty ?? 0;
            }
            else
            {
                //
            }

            //工单已生成批次数量
            decimal generatedQty = RT.Service.Resolve<WipBatchController>().CountGenerateBarcode(batchWorkOrder.Id);

            BatchWoId = batchWorkOrder.Id;
            BatchRule = batchRule.BatchRule;
            BatchQty = batchRule.Qty;
            GeneratedQty = generatedQty;
            NotGenerateQty = (batchWorkOrder.PlanQty - generatedQty);
            GenerateingQty = (batchWorkOrder.PlanQty - generatedQty);
            NumberRule = batchWorkOrder.Template?.NumberRule;
            ChildNumberRule = batchWorkOrder.Template?.NumberRule;
            ChildBatchQty = 1;

            Printer = Settings.Default.PrinterName;
            Template = batchWorkOrder.Template?.LabelTemplate;
            PageCount = 1;
        }

        #region 批次工单 BatchWo
        /// <summary>
        /// 批次工单Id
        /// </summary>
        [Label("批次工单")]
        public static readonly IRefIdProperty BatchWoIdProperty =
            P<BatchGeneratingViewModel>.RegisterRefId(e => e.BatchWoId, ReferenceType.Normal);

        /// <summary>
        /// 批次工单Id
        /// </summary>
        public double? BatchWoId
        {
            get { return (double?)this.GetRefNullableId(BatchWoIdProperty); }
            set { this.SetRefNullableId(BatchWoIdProperty, value); }
        }

        /// <summary>
        /// 批次工单
        /// </summary>
        public static readonly RefEntityProperty<BatchWorkOrder> BatchWoProperty =
            P<BatchGeneratingViewModel>.RegisterRef(e => e.BatchWo, BatchWoIdProperty);

        /// <summary>
        /// 批次工单
        /// </summary>
        public BatchWorkOrder BatchWo
        {
            get { return this.GetRefEntity(BatchWoProperty); }
            set { this.SetRefEntity(BatchWoProperty, value); }
        }
        #endregion

        #region 批次信息   
        #region 批次规则 BatchRule
        /// <summary>
        /// 批次规则
        /// </summary>
        [Label("批次规则")]
        public static readonly Property<BatchRule?> BatchRuleProperty = P<BatchGeneratingViewModel>.Register(e => e.BatchRule);

        /// <summary>
        /// 批次规则
        /// </summary>
        public BatchRule? BatchRule
        {
            get { return this.GetProperty(BatchRuleProperty); }
            set { this.SetProperty(BatchRuleProperty, value); }
        }
        #endregion

        #region 批次数量 BatchQty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<BatchGeneratingViewModel>.Register(e => e.BatchQty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal BatchQty
        {
            get { return this.GetProperty(BatchQtyProperty); }
            set { this.SetProperty(BatchQtyProperty, value); }
        }
        #endregion

        #region BarcodeRule 编码规则
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        [Required]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<BatchGeneratingViewModel>.RegisterRefId(e => e.NumberRuleId,
        new RegisterRefIdArgs<double>()
        {
            ReferenceType = ReferenceType.Normal
        });

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<BatchGeneratingViewModel>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region BarcodeRuleDtl 编码规则明细
        /// <summary>
        /// 编码规则明细
        /// </summary>
        [Label("规则明细")]
        public static readonly Property<string> BarcodeRuleDtlProperty = P<BatchGeneratingViewModel>.Register(e => e.BarcodeRuleDtl);

        /// <summary>
        /// 编码规则明细
        /// </summary>
        public string BarcodeRuleDtl
        {
            get { return this.GetProperty(BarcodeRuleDtlProperty); }
            set { this.SetProperty(BarcodeRuleDtlProperty, value); }
        }
        #endregion

        #region 剩余数量 NotGenerateQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> NotGenerateQtyProperty = P<BatchGeneratingViewModel>.Register(e => e.NotGenerateQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal NotGenerateQty
        {
            get { return this.GetProperty(NotGenerateQtyProperty); }
            set { this.SetProperty(NotGenerateQtyProperty, value); }
        }
        #endregion

        #region 已生成数量 GeneratedQty
        /// <summary>
        /// 已生成数量
        /// </summary>
        [Label("已生成数量")]
        public static readonly Property<decimal> GeneratedQtyProperty = P<BatchGeneratingViewModel>.Register(e => e.GeneratedQty);

        /// <summary>
        /// 已生成数量
        /// </summary>
        public decimal GeneratedQty
        {
            get { return this.GetProperty(GeneratedQtyProperty); }
            set { this.SetProperty(GeneratedQtyProperty, value); }
        }
        #endregion

        #region 生成数量 GenerateingQty
        /// <summary>
        /// 生成数量
        /// </summary>
        [Label("生成数量")]
        public static readonly Property<decimal> GenerateingQtyProperty = P<BatchGeneratingViewModel>.Register(e => e.GenerateingQty);

        /// <summary>
        /// 生成数量
        /// </summary>
        public decimal GenerateingQty
        {
            get { return this.GetProperty(GenerateingQtyProperty); }
            set { this.SetProperty(GenerateingQtyProperty, value); }
        }
        #endregion

        #region 起始编码 BeginSn
        /// <summary>
        /// 起始编码
        /// </summary>
        [Label("起始编码")]
        public static readonly Property<string> BeginSnProperty = P<BatchGeneratingViewModel>.Register(e => e.BeginSn);

        /// <summary>
        /// 起始编码
        /// </summary>
        public string BeginSn
        {
            get { return this.GetProperty(BeginSnProperty); }
            set { this.SetProperty(BeginSnProperty, value); }
        }
        #endregion

        #region 结束编码 EndSn
        /// <summary>
        /// 结束编码
        /// </summary>
        [Label("结束编码")]
        public static readonly Property<string> EndSnProperty = P<BatchGeneratingViewModel>.Register(e => e.EndSn);

        /// <summary>
        /// 结束编码
        /// </summary>
        public string EndSn
        {
            get { return this.GetProperty(EndSnProperty); }
            set { this.SetProperty(EndSnProperty, value); }
        }
        #endregion
        #endregion

        #region 子批次信息
        #region 生成子批次 GenerateChildren
        /// <summary>
        /// 生成子批次
        /// </summary>
        [Label("生成子批次")]
        public static readonly Property<bool> GenerateChildrenProperty = P<BatchGeneratingViewModel>.Register(e => e.GenerateChildren);

        /// <summary>
        /// 生成子批次
        /// </summary>
        public bool GenerateChildren
        {
            get { return this.GetProperty(GenerateChildrenProperty); }
            set { this.SetProperty(GenerateChildrenProperty, value); }
        }
        #endregion

        #region 子批编码规则 ChildNumberRule
        /// <summary>
        /// 子批编码规则Id
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty ChildNumberRuleIdProperty =
            P<BatchGeneratingViewModel>.RegisterRefId(e => e.ChildNumberRuleId, new RegisterRefIdArgs<double>()
            {
                ReferenceType = ReferenceType.Normal
            });

        /// <summary>
        /// 子批编码规则Id
        /// </summary>
        public double? ChildNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(ChildNumberRuleIdProperty); }
            set { this.SetRefNullableId(ChildNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 子批编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ChildNumberRuleProperty =
            P<BatchGeneratingViewModel>.RegisterRef(e => e.ChildNumberRule, ChildNumberRuleIdProperty);

        /// <summary>
        /// 子批编码规则
        /// </summary>
        public NumberRule ChildNumberRule
        {
            get { return this.GetRefEntity(ChildNumberRuleProperty); }
            set { this.SetRefEntity(ChildNumberRuleProperty, value); }
        }
        #endregion

        #region 子批规则明细 ChildBarcodeRuleDtl
        /// <summary>
        /// 子批规则明细
        /// </summary>
        [Label("规则明细")]
        public static readonly Property<string> ChildBarcodeRuleDtlProperty = P<BatchGeneratingViewModel>.Register(e => e.ChildBarcodeRuleDtl);

        /// <summary>
        /// 子批规则明细
        /// </summary>
        public string ChildBarcodeRuleDtl
        {
            get { return this.GetProperty(ChildBarcodeRuleDtlProperty); }
            set { this.SetProperty(ChildBarcodeRuleDtlProperty, value); }
        }
        #endregion

        #region 子批生成数量 ChildBatchQty
        /// <summary>
        /// 子批生成数量
        /// </summary>
        [Label("子批次数量")]
        public static readonly Property<decimal> ChildBatchQtyProperty = P<BatchGeneratingViewModel>.Register(e => e.ChildBatchQty);

        /// <summary>
        /// 子批生成数量
        /// </summary>
        public decimal ChildBatchQty
        {
            get { return this.GetProperty(ChildBatchQtyProperty); }
            set { this.SetProperty(ChildBatchQtyProperty, value); }
        }
        #endregion

        #region 子批起始编码 ChildBeginSn
        /// <summary>
        /// 子批起始编码
        /// </summary>
        [Label("起始编码")]
        public static readonly Property<string> ChildBeginSnProperty = P<BatchGeneratingViewModel>.Register(e => e.ChildBeginSn);

        /// <summary>
        /// 子批起始编码
        /// </summary>
        public string ChildBeginSn
        {
            get { return this.GetProperty(ChildBeginSnProperty); }
            set { this.SetProperty(ChildBeginSnProperty, value); }
        }
        #endregion

        #region 子批结束编码 ChildEndSn
        /// <summary>
        /// 子批结束编码
        /// </summary>
        [Label("结束编码")]
        public static readonly Property<string> ChildEndSnProperty = P<BatchGeneratingViewModel>.Register(e => e.ChildEndSn);

        /// <summary>
        /// 子批结束编码
        /// </summary>
        public string ChildEndSn
        {
            get { return this.GetProperty(ChildEndSnProperty); }
            set { this.SetProperty(ChildEndSnProperty, value); }
        }
        #endregion
        #endregion

        #region 打印机信息
        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        [Required]
        public static readonly Property<string> PrinterProperty = P<BatchGeneratingViewModel>.Register(e => e.Printer,
        new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as BatchGeneratingViewModel).OnPrinterChanged(o, e)
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
        /// <param name="o">变更对象</param>
        /// <param name="e">变更事件参数</param>
        void OnPrinterChanged(ManagedPropertyObject o, ManagedPropertyChangedEventArgs e)
        {
            Settings.Default.PrinterName = Printer;
            Settings.Default.Save();
        }
        #endregion

        #region 模板 Template
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<BatchGeneratingViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<BatchGeneratingViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region PageCount 打印份数
        /// <summary>
        /// 打印份数
        /// </summary>
        [Label("打印份数")]
        [MinValue(1)]
        public static readonly Property<int> PageCountProperty = P<BatchGeneratingViewModel>.Register(e => e.PageCount);

        /// <summary>
        /// 打印份数
        /// </summary>
        public int PageCount
        {
            get { return this.GetProperty(PageCountProperty); }
            set { this.SetProperty(PageCountProperty, value); }
        }
        #endregion
        #endregion

        #region PrintControl 打印控制（反打）
        /// <summary>
        /// 打印控制
        /// </summary>
        [Label("反打")]
        public static readonly Property<bool> PrintControlProperty = P<BatchGeneratingViewModel>.Register(e => e.PrintControl, new RegisterRefIdArgs<bool>()
        {
            ReferenceType = ReferenceType.Normal,
            PropertyChangedCallBack = (o, e) => (o as BatchGeneratingViewModel).OnPrintControlChanged(e)
        });

        /// <summary>
        /// 勾选（取消）反打事件
        /// </summary>
        /// <param name="e">勾选变更事件参数</param>
        private void OnPrintControlChanged(ManagedPropertyChangedEventArgs e)
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
        readonly NumberRuleController controller = RT.Service.Resolve<NumberRuleController>();

        /// <summary>
        /// 预览
        /// </summary>
        private void Preview()
        {
            if (this.Validate().Any(p => p.Property != PrinterProperty && p.Property != TemplateIdProperty && p.Property != PageCountProperty))
            {
                return;
            }

            if (this.NumberRule == null || this.GenerateingQty <= 0 || BatchQty <= 0) //条码规则不空，打印数量打印0
            {
                return;
            }

            int printQty = (int)Math.Ceiling(this.GenerateingQty / BatchQty);
            this.BeginSn = controller.GetStartSegment(NumberRuleId.Value, BatchWo);
            this.EndSn = controller.GetEndSegment(NumberRuleId.Value, printQty, BatchWo);
        }

        /// <summary>
        /// 子批预览
        /// </summary>
        private void ChildBatchPreview()
        {
            if (this.Validate().Any(p => p.Property != PrinterProperty && p.Property != TemplateIdProperty && p.Property != PageCountProperty))
            {
                return;
            }

            if (this.ChildNumberRule == null || this.GenerateingQty <= 0 || BatchQty <= 0 || this.ChildBatchQty <= 0) //条码规则不空，打印数量打印0
            {
                return;
            }

            int first = (int)Math.Ceiling(GenerateingQty / BatchQty);
            int printQty = (int)Math.Ceiling(BatchQty / ChildBatchQty) * first;

            var mantissaQty = (int)Math.Ceiling(GenerateingQty % BatchQty / ChildBatchQty);
            if (mantissaQty != 0)
            {
                printQty = (int)Math.Ceiling(BatchQty / ChildBatchQty) * (first - 1) + mantissaQty;
            }

            if (ChildNumberRuleId == NumberRuleId)
            {
                this.ChildBeginSn = controller.GetEndSegment(ChildNumberRuleId.Value, first + 1, BatchWo);
                this.ChildEndSn = controller.GetEndSegment(ChildNumberRuleId.Value, printQty + first, BatchWo);
            }
            else
            {
                this.ChildBeginSn = controller.GetStartSegment(ChildNumberRuleId.Value, BatchWo);
                this.ChildEndSn = controller.GetEndSegment(ChildNumberRuleId.Value, printQty, BatchWo);
            }
        }

        /// <summary>
        /// 打印控制
        /// </summary>
        private void Control()
        {
            if (this.Validate().Count > 0)
                return;
            if (this.NumberRule == null || this.GenerateingQty <= 0 || BatchQty <= 0) //条码规则不空，打印数量大于0
                return;

            int printQty = (int)Math.Ceiling(this.GenerateingQty / BatchQty);
            if (!PrintControl) //默认为正打
            {
                this.BeginSn = controller.GetStartSegment(NumberRuleId.Value, BatchWo);
                this.EndSn = controller.GetEndSegment(NumberRuleId.Value, printQty, BatchWo);
            }
            else
            {
                this.EndSn = controller.GetStartSegment(NumberRuleId.Value, BatchWo);
                this.BeginSn = controller.GetEndSegment(NumberRuleId.Value, printQty, BatchWo);
            }

            if (this.ChildNumberRule == null || this.GenerateingQty <= 0 || this.ChildBatchQty <= 0) //条码规则不空，打印数量打印0
                return;

            int first = printQty;
            printQty = (int)Math.Ceiling(BatchQty / ChildBatchQty) * first;
            var mantissaQty = (int)Math.Ceiling(GenerateingQty % BatchQty / ChildBatchQty);
            if (mantissaQty != 0)
            {
                printQty = (int)Math.Ceiling(BatchQty / ChildBatchQty) * (first - 1) + mantissaQty;
            }

            if (!PrintControl) //默认为正打
            {
                if (ChildNumberRuleId == NumberRuleId)
                {
                    this.ChildBeginSn = controller.GetEndSegment(ChildNumberRuleId.Value, first + 1, BatchWo);
                    this.ChildEndSn = controller.GetEndSegment(ChildNumberRuleId.Value, printQty + first, BatchWo);
                }
                else
                {
                    this.ChildBeginSn = controller.GetStartSegment(ChildNumberRuleId.Value, BatchWo);
                    this.ChildEndSn = controller.GetEndSegment(ChildNumberRuleId.Value, printQty, BatchWo);
                }
            }
            else
            {
                if (ChildNumberRuleId == NumberRuleId)
                {
                    this.ChildEndSn = controller.GetEndSegment(ChildNumberRuleId.Value, first + 1, BatchWo);
                    this.ChildBeginSn = controller.GetEndSegment(ChildNumberRuleId.Value, printQty + first, BatchWo);
                }
                else
                {
                    this.ChildEndSn = controller.GetStartSegment(ChildNumberRuleId.Value, BatchWo);
                    this.ChildBeginSn = controller.GetEndSegment(ChildNumberRuleId.Value, printQty, BatchWo);
                }
            }
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == BatchQtyProperty || e.Property == GenerateingQtyProperty)
            {
                Preview();
                ChildBatchPreview();
            }
            else if (e.Property == NumberRuleIdProperty)
            {
                this.BarcodeRuleDtl = string.Empty;  //条码明细
                Template = null;
                if (NumberRule == null)
                {
                    return;
                }

                StringBuilder sb = new StringBuilder();
                foreach (var ruleDtl in NumberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                BarcodeRuleDtl = sb.ToString();
                Preview();
                ChildBatchPreview();
            }
            else if (e.Property == ChildBatchQtyProperty)
            {
                ChildBatchPreview();
            }
            else if (e.Property == ChildNumberRuleIdProperty)
            {
                this.ChildBarcodeRuleDtl = string.Empty;  //条码明细 
                Template = null;
                if (ChildNumberRule == null)
                {
                    return;
                }

                StringBuilder sb = new StringBuilder();
                foreach (var ruleDtl in ChildNumberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                ChildBarcodeRuleDtl = sb.ToString();
                ChildBatchPreview();
            }
            else if (e.Property == BatchRuleProperty)
            {
                if (BatchRule == SIE.Core.Items.BatchRule.Product)
                {
                    BatchQty = RT.Service.Resolve<Items.ItemController>().GetBatchRule(BatchWo.ProductId)?.Qty ?? 1;
                }
                else if (BatchRule == SIE.Core.Items.BatchRule.WorkOrder)
                {
                    var qty = RT.Service.Resolve<WorkOrderController>().GetWipBatch(BatchWo.Id)?.Qty;
                    BatchQty = qty ?? 1;
                }
                else if (BatchRule == SIE.Core.Items.BatchRule.Vehicle)
                {
                    var qty = RT.Service.Resolve<BoxController>().GetDefaultProductCapacity(BatchWo.ProductId, null).FirstOrDefault()?.Capacity;
                    BatchQty = qty ?? 1;
                }
                else
                {
                    //
                }
            }
            else if (e.Property == GenerateChildrenProperty)
            {
                TemplateId = null;
            }
            else
            {
                //
            }
        }
    }

    /// <summary>
    /// 条码范围 实体配置
    /// </summary>
    internal class BatchGeneratingViewModelConfig : EntityConfig<BatchGeneratingViewModel>
    {
        /// <summary>
        /// 增加实体验证规则
        /// </summary>
        /// <param name="rules">实体验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(BatchGeneratingViewModel.TemplateIdProperty, new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var model = o as BatchGeneratingViewModel;
                    if (model.GenerateChildren && model.TemplateId != null && model.Template?.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
                    {
                        e.BrokenDescription = "生成子批次时，打印模板的类型必须是生产子批次".L10N();
                    }
                }
            });

            rules.Add(BatchGeneratingViewModel.GenerateingQtyProperty, new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var model = o as BatchGeneratingViewModel;
                    if (model.GenerateingQty <= 0)
                    {
                        e.BrokenDescription = "生成数量必须大于0".L10N();
                    }
                    else if (model.GenerateingQty > model.NotGenerateQty)
                    {
                        e.BrokenDescription = "生成数量：{0} 不能大于剩余数量：{1}".L10nFormat(model.GenerateingQty, model.NotGenerateQty);
                    }
                    else
                    {
                        //
                    }
                }
            });

            rules.AddRule(BatchGeneratingViewModel.BatchQtyProperty, new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var entity = o.CastTo<BatchGeneratingViewModel>();
                    if (entity.BatchQty <= 0)
                    {
                        e.BrokenDescription = "批次数量必须大于0".L10N();
                    }
                }
            });

            rules.AddRule(BatchGeneratingViewModel.ChildBatchQtyProperty, new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var entity = o.CastTo<BatchGeneratingViewModel>();
                    if (entity.GenerateChildren && entity.ChildBatchQty <= 0)
                    {
                        e.BrokenDescription = "子批次数量必须大于0".L10N();
                    }

                    if (entity.ChildBatchQty > entity.BatchQty)
                    {
                        e.BrokenDescription = "子批次数量{0}不能大于批次数量{1}".L10nFormat(entity.ChildBatchQty, entity.BatchQty);
                    }
                }
            });

            rules.AddRule(BatchGeneratingViewModel.ChildNumberRuleIdProperty, new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var entity = o.CastTo<BatchGeneratingViewModel>();
                    if (entity.GenerateChildren && entity.ChildNumberRuleId == null)
                    {
                        e.BrokenDescription = "子批次编码规则不能为空".L10N();
                    }
                }
            });
        }
    }
}