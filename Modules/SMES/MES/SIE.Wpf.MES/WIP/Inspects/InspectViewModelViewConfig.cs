using SIE.Wpf.Common;
using SIE.Wpf.MES.WIP.Inspects.Commands;
using System;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验采集 视图配置
    /// </summary>
    internal class InspectViewModelViewConfig : WPFViewConfig<InspectViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CollectionUITemplate.CollectionUIViewGroup);
            if (ViewGroup == CollectionUITemplate.CollectionUIViewGroup)
            {
                ConfigCollectionView();
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected void ConfigCollectionView()
        {
            View.AssignAuthorize(typeof(InspectViewModel));
            View.UseCommands(typeof(CollectRestartCommand), typeof(ChangeWorkOrderCommand), typeof(SubmitCommand), typeof(CollectProjectCardCommand));
            View.UseDetail(columnCount: 3);
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
                    View.Property(p => p.WorkOrder.No).UseWorkOrderDetailEditor().ShowInDetail().Readonly().HasLabel("工单号");
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.QtyFaild).UseHighlightEditor("#000000", "#############################").HasLabel("当班不良数").ShowInDetail().Readonly();
                    View.Property(p => p.DisplayBarCode).HasLabel("条码").ShowInDetail().Readonly();
                }
            }
        }
    }
}
