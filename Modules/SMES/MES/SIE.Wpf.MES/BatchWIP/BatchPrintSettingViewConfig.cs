using SIE.Barcodes.Printables;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.Wpf.Common;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Moves;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 批次打印设置视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BatchPrintSettingViewConfig : WPFViewConfig<BatchPrintSetting>
    {
        #region 条码规则明细 BarcodeRuleDtl
        /// <summary>
        /// 条码规则明细
        /// </summary>      
        public static readonly Property<string> BarcodeRuleDtlProperty = P<BatchPrintSetting>.RegisterExtensionReadOnly("BarcodeRuleDtl", typeof(BatchPrintSettingViewConfig),
            GetBarcodeRuleDtl, BatchPrintSetting.NumberRuleProperty);

        /// <summary>
        /// 条码规则明细
        /// </summary>
        public static string GetBarcodeRuleDtl(BatchPrintSetting me)
        {
            var detail = new System.Text.StringBuilder();  //条码明细
            if (me.NumberRule == null)
                return string.Empty;
            foreach (var ruleDtl in me.NumberRule.DetailList)
            {
                detail.Append( "[" + ruleDtl.Segment.Name + "]" + " ");
            }
            return detail.ToString();
        }
        #endregion

        #region 条码预览 Preview
        /// <summary>
        /// 条码预览
        /// </summary> 
        public static readonly Property<string> PreviewProperty = P<BatchPrintSetting>.RegisterExtensionReadOnly("Preview", typeof(BatchPrintSettingViewConfig),
            GetPreview, BatchPrintSetting.NumberRuleProperty);

        /// <summary>
        /// 条码预览
        /// </summary>
        public static string GetPreview(BatchPrintSetting me)
        {
            var sn = string.Empty;
            if (me.WorkOrder == null || !me.NumberRuleId.HasValue)
                return sn;
            sn = RT.Service.Resolve<NumberRuleController>().GetStartSegment(me.NumberRuleId.Value, me.WorkOrder);
            return sn;
        }
        #endregion


        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(Entity.IdProperty);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(BatchAssemblyViewModel), typeof(BatchInspectViewModel), typeof(BatchMoveViewModel));
            using (View.DeclareGroup("拆合批次条码规则", 2))
            {
                View.Property(p => p.NumberRule);
                View.Property(BarcodeRuleDtlProperty).HasLabel("规则明细").Readonly();
                View.Property(p => p.Qty).Show(ShowInWhere.Hide).UseSpinEditor(e => { e.MinValue = 1; });
            }
            using (View.DeclareGroup("打印信息", 2))
            {
                View.Property(p => p.Printer).UsePrinterEditor();
                View.Property(p => p.PrintTemplate).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).UseDataSource((e, p, s) =>
                {
                    var setting = e as BatchPrintSetting;
                    var template = new EntityList<PrintTemplate>();
                    if (setting.NumberRule == null) return template;
                    template.AddRange(setting.NumberRule.TemplateList.Select(f => f.Template).Where(f => f.State == State.Enable && f.EntityType == typeof(WipBatchPrintable).GetQualifiedName()));
                    return template;
                });
                View.Property(p => p.PageCount).UseSpinEditor(e => { e.MinValue = 1; });
                View.Property(PreviewProperty).Show(ShowInWhere.Hide).HasLabel("条码预览").Readonly();
                View.Property(p => p.PrintControl);
            }
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}