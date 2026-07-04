using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.BatchWIP.Commands;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP;
using System;

namespace SIE.Wpf.MES.BatchWIP.Assemblys
{
    /// <summary>
    /// 批次采集视图配置
    /// </summary>
    internal class BatchAssemblyViewModelViewConfig : WPFViewConfig<BatchAssemblyViewModel>
    {
        protected override void ConfigView()
        {
            View.UseDetail(columnCount: 3);
            View.UseCommands(typeof(BatchRestartCommand), typeof(WorkSwitchCommand), typeof(BatchSelectCommand), typeof(PrintSettingCommand), typeof(CollectProjectCardCommand));
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
                    View.Property(p => p.IsLoadItem).UseEditor(EnumButtonEditor.EditorName).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                }
                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").ShowInDetail().Readonly(true);
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.DisplayBarCode).HasLabel("条码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.ProjectMaintainCode).HasLabel("项目号").ShowInDetail().Readonly();
                }
            }
        }
    }
}