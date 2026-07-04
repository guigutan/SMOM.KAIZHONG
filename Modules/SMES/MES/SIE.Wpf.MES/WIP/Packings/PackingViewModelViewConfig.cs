using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP.Packings.Commands;
using System;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装ViewModel视图配置
    /// </summary>
    internal class PackingViewModelViewConfig : WPFViewConfig<PackingViewModel>
    {
        /// <summary>
        /// 包装采集视图
        /// </summary>
        public const string PackingView = "Packing";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseCommands(typeof(PackingResetCommand));
            View.UseDetail(columnCount: 6);
            View.DeclareExtendViewGroup(PackingView);
            if (ViewGroup == PackingView)
                PackingConfigView();
        }

        /// <summary>
        /// 包装视图配置
        /// </summary>
        void PackingConfigView()
        {
            View.AssignAuthorize(typeof(PackingViewModel));
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
                using (View.DeclareGroup("工单信息".L10N(), detailColumnCount: 5, collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly(true);
                }
                using (View.DeclareGroup("控制选项".L10N(), detailColumnCount: 6))
                {
                    View.Property(p => p.AutoDoPackingMode).ShowInDetail(columnSpan: 4, height: 0, hideLabel: true).UseEditor(EnumButtonEditor.EditorName);
                    View.Property(p => p.IsAutoPrintLabel).ShowInDetail(height: 0, hideLabel: true).UseEditor(BoolToggleButtonEditor.EditorName);
                    View.Property(p => p.Printer).ShowInDetail(height: 0, hideLabel: true)
                       .UseEditor(PrinterEditor.EditorName);
                }
                View.ChildrenProperty(p => p.PackingRelationList).Show(ChildShowInWhere.Detail).HasLabel("包装关系").Readonly().UseViewGroup(PackingRelationViewConfig.PackingView);
                View.ChildrenProperty(p => p.ItemLabelList).Show(ChildShowInWhere.Detail).HasLabel("产品条码").Readonly().UseViewGroup(ItemLabelViewConfig.PackingView);
                View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").Readonly().UseViewGroup(PackageRuleDetailViewConfig.PackingView);
            }
        }
    }
}