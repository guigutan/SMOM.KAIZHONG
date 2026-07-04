using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Report.BatchWipProducts;

namespace SIE.Web.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次采集工序明细视图配置
    /// </summary>
    public class BatchWipProductProcessDetailReportViewConfig : WebViewConfig<BatchWipProductProcessDetailReport>
    {
        /// <summary>
        /// 出站明细视图
        /// </summary>
        public const string OutDetailView = "ReportOutDetailView";

        /// <summary>
        /// 报表视图
        /// </summary>
        public const string ReportDetailView = "ReportDetailView";

        /// <summary>
        /// 默认试图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(OutDetailView, ReportDetailView);
            if (ViewGroup == OutDetailView)
                ConfigOutDetailView();
            if (ViewGroup == ReportDetailView)
                ConfigReportDetailView();
        }

        ///<summary>
        /// 配置报表视图
        /// </summary>
        void ConfigReportDetailView()
        {
            View.AddBehavior("SIE.Web.MES.Report.BatchWipProducts.BatchWipProductProcessDtlBehavior");
            View.AssignAuthorize(typeof(BatchWipProductVersionReport));
            View.ClearCommands();
            View.UseLayoutSize(-4, -6);
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().FixColumn().ShowInList(150);
                View.Property(p => p.SubBatchNo).Readonly().FixColumn().ShowInList(150);
                View.Property(p => p.ContainerNo).Readonly().FixColumn().Show();
                View.Property(p => p.PlugType).Readonly().UseEnumEditor().Show();
                View.Property(p => p.Qty).Readonly().Show();
                View.Property(p => p.RemainQty).Readonly().Show();
                View.Property(p => p.ScrapQty).Readonly().Show();
                View.Property(p => p.ResultType).Readonly().UseEnumEditor().Show();
                View.Property(p => p.StationName).Readonly().Show();
                View.Property(p => p.ProcessName).Readonly().Show();
                View.Property(p => p.ResourceName).Readonly().Show();
                View.Property(p => p.OperateByName).Readonly().Show();
                View.Property(p => p.BatchState).Readonly().UseEnumEditor().Show();
                View.Property(p => p.InputDate).Readonly().Show();
                View.Property(p => p.OutputDate).Readonly().Show();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
            View.AttachChildrenProperty(typeof(BatchWipProductProcessDetailReport), (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var entity = args.Parent as BatchWipProductProcessDetailReport;
                var processDetail = RF.GetById<BatchWipProductProcessDetailReport>(entity.Id);
                return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductProcessOutDetail(processDetail, args.PagingInfo);
            }, OutDetailView, allowPaging: true).HasLabel("出站明细").OrderNo = 1;
            View.ChildrenProperty(p => p.KeyItemList).HasLabel("关键件").Show(ChildShowInWhere.Hide);

        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        void ConfigOutDetailView()
        {
            View.AssignAuthorize(typeof(BatchWipProductProcessDetailReport));
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().ShowInList(150).FixColumn();
                View.Property(p => p.SubBatchNo).Readonly().ShowInList(150).FixColumn();
                View.Property(p => p.ContainerNo).Readonly().Show().FixColumn();
                View.Property(p => p.PlugType).Readonly().UseEnumEditor().Show();
                View.Property(p => p.Qty).Readonly().Show();
                View.Property(p => p.RemainQty).Readonly().Show();
                View.Property(p => p.ScrapQty).Readonly().Show();
                View.Property(p => p.ResultType).Readonly().UseEnumEditor().Show();
                View.Property(p => p.Station).Readonly().Show();
                View.Property(p => p.Process).Readonly().Show();
                View.Property(p => p.Resource).Readonly().Show();
                View.Property(p => p.OperateBy).Readonly().Show();
                View.Property(p => p.BatchState).Readonly().UseEnumEditor().Show();
                View.Property(p => p.InputDate).Readonly().Show();
                View.Property(p => p.OutputDate).Readonly().Show();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.KeyItemList).HasLabel("关键件");
        }
    }
}
