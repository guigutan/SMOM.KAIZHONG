using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.Editors;
using System;

namespace SIE.Wpf.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料采集视图配置
    /// </summary>
    public class AssemblyViewModelViewConfig : WPFViewConfig<AssemblyViewModel>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel));
            View.UseDetail(columnCount: 12);
            View.UseCommands(typeof(CollectRestartCommand), typeof(ChangeWorkOrderCommand), typeof(CollectProjectCardCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor(TipsEditor.EditorName).ShowInDetail(columnSpan: 12, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor(ErrorEditor.EditorName).ShowInDetail(columnSpan: 12, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 9, height: 0, hideLabel: true);
                    View.Property(p => p.Qualified)
                        .UseCheckResultBoolEditor()
                        .ShowInDetail(columnSpan: 1, height: 0, hideLabel: true)
                        .Visibility(DataCollectionViewModel.HaveFailParameterProperty);
                    View.Property(p => p.IsLoadItem)
                        .UseBoolSwitchEditor(e => e.DisplayName = new string[] { "上料", "装配采集" })
                        .ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail(columnSpan: 4).Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 4).Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").ShowInDetail(columnSpan: 4).Readonly(true);
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 4).Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail(columnSpan: 4).Readonly();
                    View.Property(p => p.DisplayBarCode).HasLabel("条码").ShowInDetail(columnSpan: 4).Readonly();
                    View.Property(p => p.WorkOrder.ProjectMaintainCode).HasLabel("项目号").ShowInDetail(columnSpan: 4).Readonly();
                }
            }
        }
    }
}
