using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次采集工序明细视图配置
    /// </summary>
    internal class BatchWipProductProcessDetailViewConfig : WPFViewConfig<BatchWipProductProcessDetail>
    {
        /// <summary>
        /// 出站明细视图
        /// </summary>
        public const string OutDetailView = "OutDetailView";

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
            View.AssignAuthorize(typeof(BatchWipProductVersion));
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-4, -6);
            View.Property(p => p.BatchNo);
            View.Property(p => p.SubBatchNo);
            View.Property(p => p.ContainerNo);
            View.Property(p => p.PlugType).UseEnumEditor();
            View.Property(p => p.Qty);
            View.Property(p => p.RemainQty);
            View.Property(p => p.ScrapQty);
            View.Property(p => p.ResultType).UseEnumEditor();
            View.Property(p => p.StationName);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ResourceName);
            View.Property(p => p.OperateByName);
            View.Property(p => p.BatchState).UseEnumEditor();
            View.Property(p => p.InputDate);
            View.Property(p => p.OutputDate);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(BatchWipProductProcessDetail), (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var entity = args.Parent as BatchWipProductProcessDetail;
                return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductProcessOutDetail(entity, args.PagingInfo);
            }, OutDetailView, allowPaging: true).HasLabel("出站明细").OrderNo = 1;
            View.ChildrenProperty(p => p.KeyItemList).HasLabel("关键件").Show(ChildShowInWhere.Hide);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        void ConfigOutDetailView()
        {
            View.AddBehavior(typeof(ProcessDetailBehavior));
            View.AssignAuthorize(typeof(BatchWipProductProcessDetail));
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-5, -5);
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.SubBatchNo).Show();
                View.Property(p => p.ContainerNo).Show();
                View.Property(p => p.PlugType).UseEnumEditor().Show();
                View.Property(p => p.Qty).Show();
                View.Property(p => p.RemainQty).Show();
                View.Property(p => p.ScrapQty).Show();
                View.Property(p => p.ResultType).UseEnumEditor().Show();
                View.Property(p => p.Station).Show();
                View.Property(p => p.Process).Show();
                View.Property(p => p.Resource).Show();
                View.Property(p => p.OperateBy).Show();
                View.Property(p => p.BatchState).UseEnumEditor().Show();
                View.Property(p => p.InputDate).Show();
                View.Property(p => p.OutputDate).Show();
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }

            View.ChildrenProperty(p => p.KeyItemList).HasLabel("关键件");
        }
    }
}