using SIE.Wpf.Common;
using SIE.Wpf.MES.WIP;
using System;

namespace SIE.Wpf.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 维修采集视图配置
    /// </summary>
    public class BatchRepairViewModelViewConfig : WPFViewConfig<BatchRepairViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDetail(columnCount: 3);
            View.UseCommands(typeof(CollectRestartCommand), typeof(CollectProjectCardCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseTipsEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseErrorEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseWorkOrderDetailEditor().HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.QtyPass).HasLabel("当班采集数").UseHighlightEditor("#000000", "#############################").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.DisplayBarCode).HasLabel("条码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.ProjectMaintainCode).HasLabel("项目号").ShowInDetail().Readonly();
                }
            }
        }
    }
}
