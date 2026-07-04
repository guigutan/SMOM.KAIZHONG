using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.ProductInsps;

namespace SIE.Wpf.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检日志视图配置
    /// </summary>
    internal class InspLogViewConfig : WPFViewConfig<InspLog>
    {
        /// <summary>
        /// 报检日志自定义ViewGroup
        /// </summary>
        public const string ShippingInspLogListView = "ShippingInspLogListView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProductInsp));
            if (ViewGroup == ShippingInspLogListView)
                ConfigShippingInspLogListView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WPFCommandNames.Export);
            View.Property(p => p.InspNo).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
            View.Property(p => p.InspType).UseEnumEditor().FixColumn(MetaModel.View.ColumnFixedStyle.Left);
            View.Property(p => p.CustomerName);
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.WorkOrderType).UseEnumEditor().HasLabel("工单类型");
            View.Property(p => p.WorkOrderProductCode);
            View.Property(p => p.WorkOrderProductName);
            View.Property(p => p.ResourceCode);
            View.Property(p => p.InspectionResult).UseEnumEditor();
            View.Property(p => p.OperateByName);
            View.Property(p => p.InspectionQty);
            View.Property(p => p.InspectDate).UseListSetting(e => e.ListGridWidth = 150);
            View.Property(p => p.CheckNo);
            View.Property(p => p.ProcessMode);
            View.Property(p => p.Remark);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.InspBarcodeLogList).UseViewGroup(InspBarcodeLogViewConfig.LogDetailView);
        }

        /// <summary>
        /// 自定义配置视图列表
        /// </summary>
        private void ConfigShippingInspLogListView()
        {
            View.UseChildrenAsHorizontal().UseLayoutSize(-7, -3);
            View.UseCommands(WPFCommandNames.Export);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspNo).FixColumn(MetaModel.View.ColumnFixedStyle.Left).ShowInList();
                View.Property(p => p.InspectionDate).UseListSetting(e => e.ListGridWidth = 150).ShowInList();
                View.Property(p => p.CheckNo).ShowInList();
                View.Property(p => p.InspectionResult).UseEnumEditor().ShowInList();
                View.Property(p => p.InspectDate).ShowInList();
                View.Property(p => p.ProcessMode).ShowInList();
                View.Property(p => p.Remark).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InspBarcodeLogList);
            }
        }
    }
}