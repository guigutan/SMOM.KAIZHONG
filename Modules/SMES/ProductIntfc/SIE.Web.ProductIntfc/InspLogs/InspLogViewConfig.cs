using SIE.MetaModel.View;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.ProductInsps;
using SIE.Web.Common;
using SIE.Web.ProductIntfc.InspLogs.Commands;

namespace SIE.Web.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检日志视图配置
    /// </summary>
    public class InspLogViewConfig : WebViewConfig<InspLog>
    {
        /// <summary>
        /// 报检日志自定义ViewGroup
        /// </summary>
        public const string ShippingInspLogListView = "ShippingInspLogListView";

        /// <summary>
        /// 审核视图
        /// </summary>
        public const string ExamineViewStr = "ExamineViewStr";
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProductInsp));
            View.DeclareExtendViewGroup(ShippingInspLogListView, ExamineViewStr);
            if (ViewGroup == ShippingInspLogListView)
                ConfigShippingInspLogListView();
            if (ViewGroup == ExamineViewStr)
            {
                ExamineView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.InspNo).FixColumn(true)
                .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}编码", "成品报检单号", "报检日志"); });
            View.Property(p => p.InspType).UseEnumEditor().FixColumn(true);
            View.Property(p => p.CustomerName);
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.WorkOrderType).UseEnumEditor().HasLabel("工单类型");
            View.Property(p => p.WorkOrderProductCode);
            View.Property(p => p.WorkOrderProductName);
            View.Property(p => p.ResourceCode);
            View.Property(p => p.InspectionResult).UseEnumEditor();
            View.Property(p => p.OperateByName);
            View.Property(p => p.InspectionQty);
            View.Property(p => p.InspectDate).ShowInList(width: 150);
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
            View.UseCommands(WebCommandNames.ExportXls, "SIE.Web.ProductIntfc.InspLogs.Commands.InspLogExamineCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.InspNo).Readonly().FixColumn(true).ShowInList(150).HasOrderNo(10);
                View.Property(p => p.InspectionDate).Readonly().ShowInList(width: 150).HasOrderNo(20);
                View.Property(p => p.CheckNo).Readonly().ShowInList(width: 150).HasOrderNo(30);
                View.Property(p => p.InspectionQty).Readonly().ShowInList(width: 150).HasOrderNo(30);
                View.Property(p => p.InspectionResult).Readonly().UseEnumEditor().ShowInList().HasOrderNo(40);
                View.Property(p => p.InspectionStatus).Readonly().UseEnumEditor().ShowInList().HasOrderNo(40);
                View.Property(p => p.InspectDate).Readonly().ShowInList(width: 150).HasOrderNo(50);
                View.Property(p => p.ProcessMode).Readonly().ShowInList().HasOrderNo(60);
                View.Property(p => p.Remark).Readonly().ShowInList().HasOrderNo(70);
                View.Property(p => p.IsCall).Readonly().ShowInList().HasOrderNo(70);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InspBarcodeLogList);
            }
        }

        /// <summary>
        /// 审核视图
        /// </summary>
        void ExamineView()
        {
            View.FormEdit();
            View.HasDetailColumnsCount(3);
            View.ClearCommands();
            View.UseCommands(typeof(InspLogExamineSubmitCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspNo).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.WorkOrderNo).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.WorkOrderProductCode).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.WorkOrderProductName).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.WorkOrderType).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.InspectionQty).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.InspectionResult).Cascade(p => p.ProcessMode, null).ShowInDetail(columnSpan: 1);
                View.Property(p => p.ProcessMode).Readonly(p => p.InspectionResult != SIE.Common.InspectionResult.Fail).UseCatalogEditor(p => { p.CatalogReloadData = true; p.CatalogType = InspLog.ProcessModeCataStr; }).ShowInDetail(columnSpan: 1);
            }
        }
    }
}
