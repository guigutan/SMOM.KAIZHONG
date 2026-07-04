using SIE.Domain;
using SIE.MES.BatchWIP.Products;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次采集工序明细视图配置
    /// </summary>
    public class BatchWipProductProcessDetailViewConfig : WebViewConfig<BatchWipProductProcessDetail>
    {
        /// <summary>
        /// 出站明细视图
        /// </summary>
        public static readonly string OutDetailView = "OutDetailView";

        /// <summary>
        /// 默认试图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(OutDetailView);

            if (ViewGroup == OutDetailView)
                ConfigOutDetailView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.MES.BatchWIP.Products.BatchWipProductProcessDtlBehavior");
            View.AssignAuthorize(typeof(BatchWipProductVersion));
            View.ClearCommands();
            View.UseLayoutSize(-4, -6);
            View.Property(p => p.BatchNo).Readonly().FixColumn().ShowInList(150);
            View.Property(p => p.SubBatchNo).Readonly().FixColumn().ShowInList(150);
            View.Property(p => p.ContainerNo).Readonly().FixColumn();
            View.Property(p => p.PlugType).Readonly().UseEnumEditor();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.RemainQty).Readonly();
            View.Property(p => p.ScrapQty).Readonly();
            View.Property(p => p.ResultType).Readonly().UseEnumEditor();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.OperateByName).Readonly();
            View.Property(p => p.BatchState).Readonly().UseEnumEditor();
            View.Property(p => p.InputDate).Readonly();
            View.Property(p => p.OutputDate).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(BatchWipProductProcessDetail), (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var entity = args.Parent as BatchWipProductProcessDetail;
                var processDetail = RF.GetById<BatchWipProductProcessDetail>(entity.Id);
                return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductProcessOutDetail(processDetail, args.PagingInfo);
            }, OutDetailView, allowPaging: true).HasLabel("出站明细").OrderNo = 1;
            View.ChildrenProperty(p => p.KeyItemList).HasLabel("关键件").Show(ChildShowInWhere.Hide);

        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        void ConfigOutDetailView()
        {
            View.AssignAuthorize(typeof(BatchWipProductProcessDetail));
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
