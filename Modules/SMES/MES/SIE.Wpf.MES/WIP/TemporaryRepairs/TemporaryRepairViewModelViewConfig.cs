using SIE.Wpf.Common;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using SIE.Wpf.MES.WIP.TemporaryRepairs.Commands;
using System;

namespace SIE.Wpf.MES.Wip.TemporaryRepairs
{
    /// <summary>
    /// 维修采集视图配置
    /// </summary>
    internal class TemporaryRepairViewModelViewConfig : WPFViewConfig<TemporaryRepairViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {

            View.AssignAuthorize(typeof(TemporaryRepairViewModel));

            View.UseCommands(typeof(CollectRestartCommand), typeof(ChangeWorkOrderCommand),
                typeof(SaveRepairRecordCommand), typeof(SubmitCommand), typeof(CollectProjectCardCommand));

            View.UseDetail(columnCount: 3);

            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                    View.Property(p => p.IsLoadItem).UseBoolSwitchEditor(e => e.DisplayName = new string[] { "上料", "维修" }).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    //View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                }
            }
        }
    }
}