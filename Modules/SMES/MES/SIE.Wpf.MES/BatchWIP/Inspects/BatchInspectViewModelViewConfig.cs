using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.BatchWIP.Commands;
using SIE.Wpf.MES.BatchWIP.Inspects.Commands;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP;
using System;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验采集视图配置类
    /// </summary>
    internal class BatchInspectViewModelViewConfig : WPFViewConfig<BatchInspectViewModel>
    {
        /// <summary>
        /// 批次检验采集视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDetail(columnCount: 3); ////BatchRestartCommand; PrintSettingCommand
            View.UseCommands(typeof(BatchRestartCommandInspect), typeof(WorkSwitchCommand), typeof(BatchSelectCommand), typeof(PrintSettingCommandInspect), typeof(CollectProjectCardCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                    View.Property(p => p.IsMoveIn).UseEditor(BatchMoveEditor.EditorName).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").ShowInDetail().Readonly(true);
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.QtyFaild).UseHighlightEditor("#000000", "#############################").ShowInDetail().Readonly(true);
                    View.Property(p => p.DisplayBarCode).HasLabel("条码").ShowInDetail().Readonly();
                }
            }
        }
    }
}
