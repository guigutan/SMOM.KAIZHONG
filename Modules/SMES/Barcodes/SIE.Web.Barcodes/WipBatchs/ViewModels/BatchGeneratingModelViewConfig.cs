using SIE.Barcodes.WipBatchs;
using SIE.ObjectModel;
using SIE.Web.Items._Extentions_;
using System;
using System.Linq.Expressions;

namespace SIE.Web.Barcodes.WipBatchs.ViewModels
{
    /// <summary>
    /// 批次生成ViewModel视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BatchGeneratingModelViewConfig : WebViewConfig<BatchGeneratingViewModel>
    {
        #region 子批次生成只读 IsChildBatchReadonly
        /// <summary>
        /// 子批次生成只读
        /// </summary>
        [Label("子批次生成只读")]
        public static Expression<Func<BatchGeneratingViewModel, bool>> IsChildBatchReadonly { get; }
            = p => p.BatchRule == Core.Items.BatchRule.Vehicle || p.BatchQty == 0;
        #endregion

        #region 子批次信息只读 IsChildBatchInfoReadonly
        /// <summary>
        /// 子批次信息只读
        /// </summary>
        [Label("子批次信息只读")]
        public static Expression<Func<BatchGeneratingViewModel, bool>> IsChildBatchInfoReadonly { get; }
            = p => !p.GenerateChildren;
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(BatchWorkOrder));
        }

        /// <summary>
        /// 表单试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands("SIE.Web.Barcodes.WipBatchs.GenerateCommand", "SIE.Web.Barcodes.WipBatchs.GenerateAndPrintCommand");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工单信息", 3))
                {
                    View.Property(p => p.BatchWoNo).HasLabel("工单号").Readonly();
                    View.Property(p => p.PlanQty).HasLabel("计划数量").Readonly();
                    View.Property(p => p.PlanBeginDate).HasLabel("计划日期").Readonly();
                }

                using (View.DeclareGroup("批次信息", 3))
                {
                    View.Property(p => p.BatchRule).HasLabel("批次数量").UseDisplayEditor(p => p.XType = "BatchRuleAndQtyEditor");
                    View.Property(p => p.NumberRule).UseNumberRuleBarcodeLookUpEditor()
                        .UseListSetting(e => { e.HelpInfo = "显示规则类型为产品条码的编码规则"; });
                    View.Property(p => p.BarcodeRuleDtl).Readonly();
                    View.Property(p => p.GeneratedQty).Readonly();
                    View.Property(p => p.NotGenerateQty).Readonly();
                    View.Property(p => p.GenerateingQty).UseItemUnitEditor(p =>
                    {
                        p.MinValue = 1;
                        p.ItemIdField= "ProductId";
                        //p.AllowDecimals = false;
                    });
                    View.Property(p => p.BeginSn).Readonly();
                    View.Property(p => p.EndSn).Readonly();
                }

                //using (View.DeclareGroup("子批信息", 3))
                //{
                //    View.Property(p => p.GenerateChildren).UseCheckEditor();
                //    View.Property(p => p.ChildNumberRule).UseNumberRuleBarcodeLookUpEditor().Readonly(IsChildBatchInfoReadonly)
                //        .UseListSetting(e => { e.HelpInfo = "显示规则类型为产品条码的编码规则,生成子批次可编辑"; });
                //    View.Property(p => p.ChildBarcodeRuleDtl).Readonly();
                //    View.Property(p => p.ChildBatchQty).UseItemUnitEditor(p =>
                //    {
                //        p.ItemIdField = "ProductId";
                //        p.MinValue = 1;
                //        //p.AllowDecimals = false; 
                //    }).Readonly(IsChildBatchInfoReadonly)
                //        .UseListSetting(e => { e.HelpInfo = "生成子批次可编辑"; });
                //    View.Property(p => p.ChildBeginSn).Readonly();
                //    View.Property(p => p.ChildEndSn).Readonly();
                //}

                using (View.DeclareGroup("打印信息", 3))
                {
                    View.Property(p => p.Template).UseBatchPrintTemplateLookUpEditor();
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