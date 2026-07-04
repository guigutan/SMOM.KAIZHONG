using SIE.Packages;
using SIE.Wpf.MES.BatchWIP.Packings.Commands;
using SIE.Wpf.MES.BatchWIP.Packings.ViewBehaviors;
using SIE.Wpf.MES.BatchWIP.PackRecombine;
using SIE.Wpf.MES.WIP.PackRecombine.Commands;

namespace SIE.Wpf.MES.BatchWIP.Packings
{
    [ManagedProperty.CompiledPropertyDeclarer]
    class BatchPackingRelationViewConfig : WPFViewConfig<BatchPackingRelation>
    {
        /// <summary>
        /// 批次包装视图
        /// </summary>
        public const string BatchPackingView = "BatchPacking";

        /// <summary>
        /// 批次包装拆合视图
        /// </summary>
        public const string PackRecombineView = "PackRecombineView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchPackingView, PackRecombineView);

            if (ViewGroup == BatchPackingView)
            {
                ConfigBatchPackingView();
            }
            else if (ViewGroup == PackRecombineView)
                ConfigPackRecombineView();
        }

        /// <summary>
        /// 配置批次包装拆合视图
        /// </summary>
        private void ConfigPackRecombineView()
        {
            View.AssignAuthorize(typeof(BatchPackRecombineViewModel));
            View.ClearCommands().UseCommands(typeof(PrintPackingNoCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo).HasLabel("包装条码").Show(ShowInWhere.All);
                View.Property(p => p.PackageUnit.Name).HasLabel("单位").Show(ShowInWhere.All);
                View.Property(p => p.PackedQty).ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.ItemQty).HasLabel("产品数").Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).HasLabel("生产批次").Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 批次包装采集包装清单视图
        /// </summary>
        void ConfigBatchPackingView()
        {
            View.ClearCommands();
            View.AssignAuthorize(typeof(BatchPackingViewModel));
            View.UseCommands(typeof(BatchPkgBarcodePrintCommand), typeof(PkgBatchPackingCommand), typeof(OpenPackageCommand));
            View.AddBehavior(typeof(HideListEditBehavior));
            View.AddBehavior(typeof(Behaviors.GridRowDoubleClickViewBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo).HasLabel("包装条码").Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).HasLabel("生产批次").Show(ShowInWhere.All);
                View.Property(p => p.State).ShowInList(60).HasLabel("条码状态").Show(ShowInWhere.All);
                View.Property(p => p.PackageUnitName).HasLabel("单位").Show(ShowInWhere.All);
                View.Property(p => p.PackedQty).ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.ItemQty).HasLabel("产品数").Show(ShowInWhere.All);
            }
        }
    }
}
