using SIE.Wpf.Common;
using SIE.Wpf.MES.WIP.Inspects.Commands;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验项目采集 视图配置
    /// </summary>
    internal class InspectByItemViewModelViewConfig : WPFViewConfig<InspectByItemViewModel>
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
        /// 配置列表视图
        /// </summary>
        protected void ConfigCollectionView()
        {
            View.UseCommands(typeof(CollectRestartCommand), typeof(ChangeWorkOrderCommand), typeof(SubmitByItemCommand), typeof(CollectProjectCardCommand));
            using (View.OrderProperties())
            {
                View.UseDetail(columnCount: 3);
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseTipsEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseErrorEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息"))
                {
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息", collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).UseWorkOrderDetailEditor().HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.QtyFaild).UseHighlightEditor("#000000", "#############################").HasLabel("当班不良数").ShowInDetail().Readonly();
                    View.Property(p => p.DisplayBarCode).HasLabel("条码").ShowInDetail().Readonly();
                }
            }
        }
    }
}
