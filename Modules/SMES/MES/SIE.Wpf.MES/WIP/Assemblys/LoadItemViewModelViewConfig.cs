using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.LoadItems.Commands;
using SIE.Wpf.MES.WIP.Repairs;

namespace SIE.Wpf.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料视图配置
    /// </summary>
    internal class LoadItemViewModelViewConfig : WPFViewConfig<LoadItemViewModel>
    {
        /// <summary>
        /// 批次上料采集视图
        /// </summary>
        public const string BatchLoadItemView = "BatchLoadItemView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("上料明细");
            View.DeclareExtendViewGroup(BatchLoadItemView);
            if (ViewGroup == BatchLoadItemView)
                ConfigBatchLoadItemView();
        }

        /// <summary>
        /// 默认列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel));
            View.AssignAuthorize(typeof(RepairViewModel));
            View.UseCommands(typeof(UnloadItemCommand), typeof(UnloadDefectItemCommand), typeof(RefreshLoadItemCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.LoadItem.Item.Code).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.SourceCode).HasLabel("标签号").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.SourceType).HasLabel("来源类型").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.LoadQty).HasLabel("上料数量").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.Qty).HasLabel("剩余数量").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.CreateDate).HasLabel("上料时间").Show(ShowInWhere.All).Readonly();
            }
        }

        /// <summary>
        /// 默认表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(RepairViewModel));
            View.UseCommands(typeof(UnloadItemCommand), typeof(UnloadDefectItemCommand), typeof(RefreshLoadItemCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.LoadItem.Item.Code).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.SourceCode).HasLabel("标签号").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.SourceType).HasLabel("来源类型").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.LoadQty).HasLabel("上料数量").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.Qty).HasLabel("剩余数量").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.CreateDate).HasLabel("上料时间").Show(ShowInWhere.All).Readonly();
            }
        }

        /// <summary>
        /// 批次上料采集视图
        /// </summary>
        void ConfigBatchLoadItemView()
        {
            View.AssignAuthorize(typeof(BatchAssemblyViewModel));
            View.UseCommands(typeof(UnloadItemCommand), typeof(UnloadDefectItemCommand), typeof(RefreshLoadItemCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.LoadItem.Item.Code).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.SourceCode).HasLabel("标签号").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.SourceType).HasLabel("来源类型").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.LoadQty).HasLabel("上料数量").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.Qty).HasLabel("剩余数量").Show(ShowInWhere.All);
                View.Property(p => p.LoadItem.CreateDate).HasLabel("上料时间").Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
