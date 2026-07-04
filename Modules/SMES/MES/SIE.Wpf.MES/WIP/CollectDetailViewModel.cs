using SIE.MES.WIP;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Inspects;
using SIE.Wpf.MES.WIP.Moves;
using SIE.Wpf.MES.WIP.Repairs;
using SIE.Wpf.MES.WIP.Reworks;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 采集明细 视图配置
    /// </summary>
    class CollectDetailViewModelViewConfig : WPFViewConfig<CollectDetailViewModel>
    {
        /// <summary>
        /// 批次采集ViewGroup
        /// </summary>
        public static readonly string BatchViewGroup = "BatchCollectionView";

        /// <summary>
        /// 批次采集维修ViewGroup
        /// </summary>
        public static readonly string BatchRepairGroup = "BatchRepairGroup";

        /// <summary>
        /// 返工采集ViewGroup
        /// </summary>
        public static readonly string ReworkViewGroup = "ReworkViewGroup";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchViewGroup, BatchRepairGroup, ReworkViewGroup, CollectionUITemplate.CollectionUIViewGroup);
            View.DomainName("采集操作记录");
            if (ViewGroup == CollectionUITemplate.CollectionUIViewGroup)
                CollectionView();
            else if (ViewGroup == BatchViewGroup)
                BatchCollectionView();
            else if (ViewGroup == BatchRepairGroup)
                BatchRepairView();
            else if (ViewGroup == ReworkViewGroup)
                ReworkCollcetionView();
        }

        /// <summary>
        /// 批次维修采集视图模型
        /// </summary>
        private void BatchRepairView()
        {
            View.AssignAuthorize(typeof(BatchRepairViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.BatchNo).Show(ShowInWhere.All).HasLabel("生产批号");
                View.Property(p => p.ContainerNo).Show(ShowInWhere.All);
                View.Property(p => p.Qty).Show(ShowInWhere.All).HasLabel("批次数量");
                View.Property(p => p.Result).Show(ShowInWhere.All);
                View.Property(p => p.ScrapQty).Show(ShowInWhere.All).HasLabel("报废数量");
                View.Property(p => p.OptTme).UseListSetting(e => e.ListGridWidth = 160).Show(ShowInWhere.All);
                View.Property(p => p.BatchState).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 批次采集视图模型
        /// </summary>
        private void BatchCollectionView()
        {
            View.AssignAuthorize(typeof(BatchAssemblyViewModel), typeof(BatchInspectViewModel), typeof(BatchMoveViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.BatchNo).Show(ShowInWhere.All).HasLabel("批次号");
                View.Property(p => p.ContainerNo).Show(ShowInWhere.All);
                View.Property(p => p.PlugType).Show(ShowInWhere.All);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.SplitQty).Show(ShowInWhere.Hide);
                View.Property(p => p.NgQty).Show(ShowInWhere.All);
                View.Property(p => p.ScrapQty).HasLabel("报废数量").Show(ShowInWhere.All);
                View.Property(p => p.Result).Show(ShowInWhere.All);
                View.Property(p => p.OptTme).UseListSetting(e => e.ListGridWidth = 160).Show(ShowInWhere.All);
                View.Property(p => p.BatchState).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 返工采集视图配置
        /// </summary>
        protected void ReworkCollcetionView()
        {
            View.AssignAuthorize(typeof(ReworkViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.Barcode).Show(ShowInWhere.All).HasLabel("生产条码");
                View.Property(p => p.ContainerNo).Show(ShowInWhere.All);
                View.Property(p => p.BarcodeType).Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).Show(ShowInWhere.All).HasLabel("原条码");
                View.Property(p => p.Result).Show(ShowInWhere.All);
                View.Property(p => p.OptTme).UseListSetting(e => e.ListGridWidth = 160).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置CollectionView视图
        /// </summary>
        protected void CollectionView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(MoveViewModel), typeof(InspectViewModel), typeof(RepairViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.Barcode).Show(ShowInWhere.All);
                View.Property(p => p.BarcodeType).Show(ShowInWhere.All);
                View.Property(p => p.Result).Show(ShowInWhere.All);
                View.Property(p => p.CollectDate).UseListSetting(e => e.ListGridWidth = 150).Show(ShowInWhere.All);
            }
        }
    }
}