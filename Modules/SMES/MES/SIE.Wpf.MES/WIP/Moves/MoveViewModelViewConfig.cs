using SIE.Wpf.Common.Editors;
using System;

namespace SIE.Wpf.MES.WIP.Moves
{
    /// <summary>
    /// 过站采集视图配置
    /// </summary>
    public class MoveViewModelViewConfig : WPFViewConfig<MoveViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MoveViewModel));
            View.UseCommands(typeof(CollectRestartCommand), typeof(ChangeWorkOrderCommand));
            View.UseDetail(columnCount: 6);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 5, height: 0, hideLabel: true);
                    View.Property(p => p.Qualified)
                        .UseCheckResultBoolEditor()
                        .ShowInDetail(columnSpan: 1, height: 0, hideLabel: true)
                        .Visibility(DataCollectionViewModel.HaveFailParameterProperty);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail(columnSpan: 2).Readonly();

                    //View.Property(p => p.QtyFinish).UseEditor("IntegerUpDownEditor").ShowInDetail().Readonly();
                }
            }
        }
    }
}
