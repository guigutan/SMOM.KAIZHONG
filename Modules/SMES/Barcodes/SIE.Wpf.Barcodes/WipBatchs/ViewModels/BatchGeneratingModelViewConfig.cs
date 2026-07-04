using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.Barcodes.WipBatchs.Commands;
using SIE.Wpf.Common;

namespace SIE.Wpf.Barcodes.WipBatchs.ViewModels
{
    /// <summary>
    /// 批次生成ViewModel视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BatchGeneratingModelViewConfig : WPFViewConfig<BatchGeneratingViewModel>
    {
        #region 子批次生成只读 ChildBatchReadonly
        /// <summary>
        /// 子批次生成只读
        /// </summary>
        public static readonly Property<bool> ChildBatchReadonlyProperty = P<BatchGeneratingViewModel>.RegisterExtensionReadOnly("ChildBatchReadonly", typeof(BatchGeneratingModelViewConfig),
            GetChildBatchReadonly, BatchGeneratingViewModel.BatchQtyProperty, BatchGeneratingViewModel.BatchRuleProperty);

        /// <summary>
        /// 子批次生成只读
        /// </summary>
        /// <param name="me">批次生成视图模型</param>
        /// <returns>载具分批或者批次数量等于0返回true，否则返回false</returns>
        public static bool GetChildBatchReadonly(BatchGeneratingViewModel me)
        {
            if (me.BatchRule == Core.Items.BatchRule.Vehicle || me.BatchQty == 0)
            {
                me.GenerateChildren = false;
                return true;
            }

            return false;
        }
        #endregion

        #region 子批次信息只读 ChildBthInfoReadlonly
        /// <summary>
        /// 子批次信息只读
        /// </summary>
        [Label("子批次信息只读")]
        public static readonly Property<bool> ChildBthInfoReadlonlyProperty = P<BatchGeneratingViewModel>.RegisterExtensionReadOnly("ChildBthInfoReadlonly", typeof(BatchGeneratingModelViewConfig),
            GetChildBthInfoReadlonly, BatchGeneratingViewModel.GenerateChildrenProperty);

        /// <summary>
        /// 子批次信息只读
        /// </summary>
        /// <param name="me">批次生成视图模型</param>
        /// <returns>非生成子批次返回true，否则返回true</returns>
        public static bool GetChildBthInfoReadlonly(BatchGeneratingViewModel me)
        {
            return me.GenerateChildren;
        }
        #endregion

        /// <summary>
        /// 表单试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(BatchWorkOrder));
            View.UseCommands(typeof(GenerateCommand), typeof(GenerateAndPrintCommand));

            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工单信息", 3))
                {
                    View.Property(p => p.BatchWo.No).HasLabel("工单号").Readonly();
                    View.Property(p => p.BatchWo.PlanQty).HasLabel("计划数量").Readonly();
                    View.Property(p => p.BatchWo.PlanBeginDate).HasLabel("计划日期").Readonly();
                }

                using (View.DeclareGroup("批次信息", 3))
                {
                    View.Property(p => p.BatchRule).HasLabel("批次数量").UseItemBatchRuleLookUpEditor();
                    View.Property(p => p.NumberRule).UseNumberRuleBarcodeLookUpEditor();
                    View.Property(p => p.BarcodeRuleDtl).Readonly();
                    View.Property(p => p.GeneratedQty).Readonly();
                    View.Property(p => p.NotGenerateQty).Readonly();
                    View.Property(p => p.GenerateingQty).UseSpinEditor(p => p.MinValue = 0);
                    View.Property(p => p.BeginSn).Readonly();
                    View.Property(p => p.EndSn).Readonly();
                }

                using (View.DeclareGroup("子批信息", 3))
                {
                    View.Property(p => p.GenerateChildren).UseCheckEditor().Readonly(ChildBatchReadonlyProperty);
                    View.Property(p => p.ChildNumberRule).UseNumberRuleBarcodeLookUpEditor().Readonly(ChildBthInfoReadlonlyProperty);
                    View.Property(p => p.ChildBarcodeRuleDtl).Readonly();
                    View.Property(p => p.ChildBatchQty).UseSpinEditor(p => p.MinValue = 0).Readonly(ChildBthInfoReadlonlyProperty);
                    View.Property(p => p.ChildBeginSn).Readonly();
                    View.Property(p => p.ChildEndSn).Readonly();
                }

                using (View.DeclareGroup("打印信息", 3))
                {
                    View.Property(p => p.Printer).UsePrinterEditor();
                    View.Property(p => p.Template).UseBatchPrintTemplateLookUpEditor(p => p.ReloadDataOnPopping = true);
                    View.Property(p => p.PageCount);
                }

                using (View.DeclareGroup("打印控制"))
                {
                    View.Property(p => p.PrintControl).UseCheckEditor();
                }
            }
        }
    }
}