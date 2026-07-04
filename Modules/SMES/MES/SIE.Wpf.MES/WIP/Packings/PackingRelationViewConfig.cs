using SIE.Packages;
using SIE.Wpf.MES.WIP.Packings.Commands;
using SIE.Wpf.MES.WIP.Packings.ViewBehaviors;
using SIE.Wpf.MES.WIP.PackRecombine;
using SIE.Wpf.MES.WIP.PackRecombine.Commands;
using SIE.Wpf.MES.WIP.PackRecombine.ViewBehaviors;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装关系视图配置
    /// </summary>
    internal class PackingRelationViewConfig : WPFViewConfig<PackingRelation>
    {
        /// <summary>
        /// 包装视图
        /// </summary>
        public const string PackingView = "Packing";

        /// <summary>
        /// 新包装视图
        /// </summary>
        public const string NewPackingView = "NewPacking";

        /// <summary>
        /// 包装拆合视图
        /// </summary>
        public const string RecombineView = "RecombineView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PackingView, RecombineView, NewPackingView);

            if (ViewGroup == PackingView)
                ConfigPackingView();
            else if (ViewGroup == RecombineView)
                ConfigRecombineView();
            else if (ViewGroup == NewPackingView)
                ConfigNewPackingView();
        }

        /// <summary>
        /// 配置包装拆合视图
        /// </summary>
        private void ConfigRecombineView()
        {
            View.AssignAuthorize(typeof(PackRecombineBaseViewModel));
            View.ClearCommands().UseCommands(typeof(PrintPackingNoCommand));
            View.AddBehavior(typeof(PackRecombineViewBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo).HasLabel("包装条码").ShowInList(150).Show(ShowInWhere.All);
                View.Property(p => p.PackageUnit.Name).HasLabel("单位").ShowInList(40).Show(ShowInWhere.All);
                View.Property(p => p.PackedQty).ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.ItemQty).HasLabel("产品数").ShowInList(60).Show(ShowInWhere.All);

            }
        }

        /// <summary>
        /// 包装采集包装清单视图
        /// </summary>
        void ConfigPackingView()
        {
            View.AssignAuthorize(typeof(PackingViewModel));
            View.AddBehavior(typeof(PackageRelationViewBehavior));
            using (View.OrderProperties())
            {
                View.AssignAuthorize(typeof(PackingViewModel));
                View.ClearCommands(false).UseCommands(typeof(PrintBarcodeCommand), typeof(PackingCommand));
                View.RemoveCommands(WPFCommandNames.CustomizeUI);
                View.Property(p => p.State).ShowInList(60).HasLabel("条码状态").Show(ShowInWhere.All);
                View.Property(p => p.PackageNo).HasLabel("包装条码").ShowInList(150).Show(ShowInWhere.All);
                View.Property(p => p.PackageUnit.Name).HasLabel("单位").ShowInList(40).Show(ShowInWhere.All);
                View.Property(p => p.PackedQty).ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.ItemQty).HasLabel("产品数").ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 新包装采集包装清单视图
        /// </summary>
        void ConfigNewPackingView()
        {
            View.AssignAuthorize(typeof(NewPackingViewModel));
            View.AssignAuthorize(typeof(JoinPackingViewModel));
            View.AddBehavior(typeof(NewPackageRelationViewBehavior));
            using (View.OrderProperties())
            {
                View.ClearCommands(false).UseCommands(typeof(NewPrintBarcodeCommand), typeof(NewPackingCommand));
                View.RemoveCommands(WPFCommandNames.CustomizeUI);
                View.Property(p => p.State).ShowInList(60).HasLabel("条码状态").Show(ShowInWhere.All);
                View.Property(p => p.PackageNo).HasLabel("包装条码").ShowInList(150).Show(ShowInWhere.All);
                View.Property(p => p.PackageUnit.Name).HasLabel("单位").ShowInList(40).Show(ShowInWhere.All);
                View.Property(p => p.PackedQty).ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.ItemQty).HasLabel("产品数").ShowInList(60).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}