using SIE.MetaModel.View;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.Web.ProductIntfc.FirstInsps.Commands;

namespace SIE.Web.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首检查询实体视图配置
    /// </summary>
    public class FirstInspViewConfig : WebViewConfig<FirstInsp>
    {
        /// <summary>
        /// 成品入库视图组
        /// </summary>
        public static string FirstInspView { get; } = "FirstInspViewConfig";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(FirstInspView);
            if (ViewGroup == FirstInspView)
                CusConfigListView();
        }


        /// <summary>
        /// 配置视图
        /// </summary>
        protected void CusConfigListView()
        {
            View.UseCommands("SIE.Web.ProductIntfc.FirstInsps.Commands.FirstInspRuleCommand",typeof(FirstInspCommand).FullName, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspNo).Readonly().HasLabel("报检单号").ShowInList(150);
                View.Property(p => p.WorkOrderNo).Readonly().HasLabel("工单").ShowInList(180);
                View.Property(p => p.WorkOrderProductCode).Readonly().ShowInList(150);
                View.Property(p => p.WorkOrderProductName).Readonly().ShowInList(150);
                View.Property(p => p.InspectionQty).Readonly().Show();
                View.Property(p => p.WorkOrderType).Readonly().Show();
                View.Property(p => p.WorkOrderState).Readonly().UseEnumEditor().HasLabel("工单状态").Show();
                View.Property(p => p.ShopName).Readonly().HasLabel("车间").Show();
                View.Property(p => p.ResourceName).Readonly().HasLabel("资源").Show();
                View.Property(p => p.ProcessId).Readonly().HasLabel("工序").Show();
                View.Property(p => p.DispatchTaskNo).Readonly().HasLabel("任务单").Show();
                View.Property(p => p.InspState).Readonly().HasLabel("报检状态").Show();
                View.Property(p => p.InspectionDate).Readonly().ShowInList(150);
                View.Property(p => p.InspectionResult).Readonly().Show();
                View.Property(p => p.ProcessMode).Readonly().HasLabel("处理建议").Show();
                View.Property(p => p.CheckNo).Readonly().HasLabel("首件检验单号").Show()
                    .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}首件检验单号", "首检单号配置", "首件报检"); });
                View.Property(p => p.InspectDate).Readonly().ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InspBarcodeLogList).HasLabel("首检条码").Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(InspBarcodeLog), (c) =>
                {
                    var child = c as ChildPagingDataArgs;
                    var firstInsp = child.Parent as FirstInsp;
                    return RT.Service.Resolve<InspLogController>().GetBarcodeLogs(firstInsp.Id, child.PagingInfo, child.SortInfo);
                }, FirstInspDetailViewConfig.FirstInspDetailView).HasLabel("首检条码");
            }
        }
    }
}
