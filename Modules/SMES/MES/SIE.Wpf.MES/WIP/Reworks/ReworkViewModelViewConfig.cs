using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP.ViewBehaviors;
using System;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// 返工视图配置类
    /// </summary>
    public class ReworkViewModelViewConfig : WPFViewConfig<ReworkViewModel>
    {
        /// <summary>
        /// 返工视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ReworkViewModel));
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(ReworkViewBehavior));
            View.UseDetail(columnCount: 3);
            View.UseCommands(typeof(CollectRestartCommand), typeof(SubmitCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor(TipsEditor.EditorName).ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor(ErrorEditor.EditorName).ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                    View.Property(p => p.ReworkOperate).UseEditor(EnumButtonEditor.EditorName).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").ShowInDetail().Readonly(true);
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品机型").ShowInDetail().Readonly();
                }
            }
        }
    }
}
