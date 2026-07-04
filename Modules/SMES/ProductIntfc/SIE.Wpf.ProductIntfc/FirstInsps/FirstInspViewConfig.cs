using SIE.ProductIntfc.FirstInsps;
using SIE.Wpf.ProductIntfc.FirstInsps.Commands;

namespace SIE.Wpf.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首检查询实体视图配置
    /// </summary>
    public class FirstInspViewConfig : WPFViewConfig<FirstInsp>
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
            View.UseCommands(WPFCommandNames.Export, typeof(FirstInspRuleCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.InspNo).HasLabel("报检单号").Show();
                View.Property(p => p.WorkOrderNo).HasLabel("工单").Show();
                View.Property(p => p.WorkOrderProductCode).Show();
                View.Property(p => p.WorkOrderProductName).Show();
                View.Property(p => p.InspectionQty).Show();
                View.Property(p => p.WorkOrderType).Show();
                View.Property(p => p.WorkOrderState).UseEnumEditor().HasLabel("工单状态").Show();
                View.Property(p => p.ShopName).HasLabel("车间").Show();
                View.Property(p => p.ResourceName).HasLabel("资源").Show();
                View.Property(p => p.InspectionDate).Show();
                View.Property(p => p.InspectionResult).Show();
                View.Property(p => p.ProcessMode).HasLabel("处理建议").Show();
                View.Property(p => p.CheckNo).HasLabel("首件检验单号").Show();
                View.Property(p => p.InspectDate).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InspBarcodeLogList).Show(ChildShowInWhere.All).UseViewGroup(FirstInspDetailViewConfig.FirstInspDetailView).HasLabel("首检条码");
            }
        }
    }
}
