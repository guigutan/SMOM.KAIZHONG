using SIE.Barcodes.WipBatchs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.BatchGeneration;
using SIE.MES.OnOffDuty;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.Web.MES.BatchGeneration
{
    /// <summary>
    /// 批次生成ViewModel
    /// </summary>
    [RootEntity, Label("批次生成")]
    public class BatchGeneratingViewModel : ViewModel
    {

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
        public static readonly RefEntityProperty<WOBatchGeneration> BatchWoProperty =
            P<BatchGeneratingViewModel>.RegisterRef(e => e.BatchWo, BatchWoIdProperty);

        /// <summary>
        /// 批次工单
        /// </summary>
        public WOBatchGeneration BatchWo
        {
            get { return this.GetRefEntity(BatchWoProperty); }
            set { this.SetRefEntity(BatchWoProperty, value); }
        }
        #endregion

        #region 工单编号 BatchWoNo
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> BatchWoNoProperty = P<BatchGeneratingViewModel>.Register(e => e.BatchWoNo);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string BatchWoNo
        {
            get { return this.GetProperty(BatchWoNoProperty); }
            set { this.SetProperty(BatchWoNoProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<BatchGeneratingViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<BatchGeneratingViewModel>.Register(e => e.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<BatchGeneratingViewModel>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
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

        #region 打印机信息
        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        //[Required]
        public static readonly Property<string> PrinterProperty = P<BatchGeneratingViewModel>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
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


        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Required]
        [Label("工序Id")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchGeneratingViewModel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchGeneratingViewModel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Required]
        [Label("资源Id")]
        public static readonly IRefIdProperty ResourceIdProperty = P<BatchGeneratingViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<BatchGeneratingViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位Id")]
        public static readonly IRefIdProperty StationIdProperty =
            P<BatchGeneratingViewModel>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty =
            P<BatchGeneratingViewModel>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion


        #region 预览、打印控制
        /// <summary>
        /// 编码规则控制器对象
        /// </summary>
        readonly NumberRuleController controller = RT.Service.Resolve<NumberRuleController>();

        /// <summary>
        /// 打印控制
        /// </summary>
        private void Control()
        {
            if (this.Validate().Count > 0)
            {
                return;
            }
            if (this.NumberRule == null || this.GenerateingQty <= 0 || BatchQty <= 0) //条码规则不空，打印数量大于0
            {
                return;
            }
            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(BatchWo.No);
            int printQty = (int)Math.Ceiling(this.GenerateingQty / BatchQty);
            if (!PrintControl) //默认为正打
            {
                this.BeginSn = controller.GetStartSegment(NumberRuleId.Value, wo);
                this.EndSn = controller.GetEndSegment(NumberRuleId.Value, printQty, wo);
            }
            else
            {
                this.EndSn = controller.GetStartSegment(NumberRuleId.Value, wo);
                this.BeginSn = controller.GetEndSegment(NumberRuleId.Value, printQty, wo);
            }
        }
        #endregion        
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
        }
    }
}