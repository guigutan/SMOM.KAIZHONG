using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.BatchWIP.Commands;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP.Packings;
using System;

namespace SIE.Wpf.MES.BatchWIP.Packings
{
    /// <summary>
    /// 批次包装视图配置
    /// </summary>
    public class BatchPackingModelViewConfig : WPFViewConfig<BatchPackingViewModel>
    {
        /// <summary>
        /// 包装采集视图
        /// </summary>
        public const string PackingView = "BatchPacking";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PackingView);
            if (ViewGroup == PackingView)
                PackingConfigView();
        }

        /// <summary>
        /// 包装视图配置
        /// </summary>
        void PackingConfigView()
        {
            View.AssignAuthorize(typeof(BatchPackingViewModel));
            View.UseCommands(typeof(BatchRestartCommand), typeof(WorkSwitchCommand));
            View.UseDetail(columnCount: 6);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor(TipsEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor(ErrorEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                }
                using (View.DeclareGroup("扫描信息".L10N(), detailColumnCount: 8))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 5, height: 0, hideLabel: true);
                    View.Property(p => p.WeightInfo).UseEditor(WeighEditor.EditorName).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                    View.Property(p => p.ScanMode).UseEditor(EnumButtonEditor.EditorName).ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                }
                using (View.DeclareGroup("控制选项".L10N(), detailColumnCount: 6))
                {
                    View.Property(p => p.AutoDoPackingMode).ShowInDetail(columnSpan: 4, height: 0, hideLabel: true).UseEditor(EnumButtonEditor.EditorName);
                    View.Property(p => p.IsAutoPrintLabel).ShowInDetail(height: 0, hideLabel: true).UseEditor(BoolToggleButtonEditor.EditorName);
                    View.Property(p => p.Printer).ShowInDetail(height: 0, hideLabel: true)
                       .UseEditor(PrinterEditor.EditorName);
                }
                using (View.DeclareGroup("工单信息".L10N(), detailColumnCount: 5, collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly(true);
                }
                View.ChildrenProperty(p => p.PackingRelationList).Show(ChildShowInWhere.All).HasLabel("包装关系").Readonly().UseViewGroup(BatchPackingRelationViewConfig.BatchPackingView);
                View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.All).HasLabel("包装规则").Readonly().UseViewGroup(PackageRuleDetailViewConfig.BatchPackingView);
                View.ChildrenProperty(p => p.InputBatchList).Show(ChildShowInWhere.All).UseViewGroup(InputBatchViewConfig.BatchPackingView);
                View.ChildrenProperty(p => p.OutputBatchList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
