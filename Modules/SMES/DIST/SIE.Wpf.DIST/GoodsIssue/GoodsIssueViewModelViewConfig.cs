using SIE.DIST;
using SIE.Wpf.Common;
using SIE.Wpf.Resources;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 载具关联视图模型视图配置
    /// </summary>
    class GoodsIssueViewModelViewConfig : WPFViewConfig<GoodsIssueViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(GoodsIssue));
            View.UseDetail(columnCount: 4, columnMaxWidth: 220);
            View.UseCommands(typeof(SubmitCommand), typeof(RestartCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseTipsEditor().ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseErrorEditor().ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("请扫描产品条码"))
                {
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 2, height: 0);
                }

                using (View.DeclareGroup("工单信息", detailColumnCount: 4, collapsable: true))
                {
                    View.Property(p => p.GoodsIssue.WorkOrder.No).HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.Item.Code).HasLabel("物料编码").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.Item.Name).HasLabel("物料名称").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.Item.Model.Name).HasLabel("物料规格").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.Qty).HasLabel("仓库发货数").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.RemainderQty).HasLabel("剩余数量").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.DistributionQty).HasLabel("累计配送数").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.DefectQty).HasLabel("缺陷数量").ShowInDetail().Readonly();
                    View.Property(p => p.GoodsIssue.Unit.Name).HasLabel("单位").Show(ShowInWhere.All);
                    View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 0).ShowInDetail().Readonly(GoodsIssueViewModel.QtyReadOnlyProperty);
                    View.Property(p => p.Resource).UseEnterpriseEquipmentResourceEditor().Show(ShowInWhere.All);
                }
            }
        }
    }
}
