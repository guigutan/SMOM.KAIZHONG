using SIE.Common.Prints;
using SIE.MES.WorkOrders;
using SIE.Packages.Packages;
using SIE.Wpf.MES.BatchWIP.Packings;
using SIE.Wpf.MES.BatchWIP.PackRecombine;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装规则明细视图配置
    /// </summary>
    internal class PackageRuleDetailViewConfig : WPFViewConfig<WorkOrderPackageRuleDetail>
    {
        /// <summary>
        /// 包装视图
        /// </summary>
        public const string PackingView = "Packing";

        /// <summary>
        /// 包装拆合视图
        /// </summary>
        public const string RecombineView = "RecombineView";

        /// <summary>
        /// 包装视图
        /// </summary>
        public const string BatchPackingView = "BatchPacking";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PackingView, BatchPackingView, RecombineView);
            if (ViewGroup == PackingView || ViewGroup == RecombineView)
            {
                ConfigPackingView();
            }
            else if (ViewGroup == BatchPackingView)
            {
                ConfigBatchPackingView();
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 配置包装视图
        /// </summary>
        private void ConfigPackingView()
        {
            View.AssignAuthorize(typeof(BatchPackRecombineViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.PackageUnit).UsePagingLookUpEditor(p => p.DisplayMember = PackingUnit.NameProperty.Name).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Description).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPackage).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.IsOutStockLabel).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.IsInStockLabel).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.Weight).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Height).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Volume).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Length).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Width).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.PrintTemplate).Show(ShowInWhere.All).UsePagingLookUpEditor(p => p.DisplayMember = PrintTemplate.FileNameProperty.Name);
                View.ChildrenProperty(P => P.WorkOrderProcessPackingUnitList).HasLabel("工序权限").Readonly().Show(ChildShowInWhere.Detail);
            }
        }

        /// <summary>
        /// 批次包装采集包装规则视图
        /// </summary>
        void ConfigBatchPackingView()
        {
            View.AssignAuthorize(typeof(BatchPackingViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands(false);
                View.AssignAuthorize(typeof(BatchPackingViewModel));
                View.Property(p => p.PackageUnitName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Description).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPackage).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.IsOutStockLabel).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.IsInStockLabel).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.Weight).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Height).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Volume).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Length).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Width).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.NumberRule).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPrint).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.PrintTemplate).Show(ShowInWhere.All).UsePagingLookUpEditor(p => p.DisplayMember = PrintTemplate.FileNameProperty.Name);
                View.ChildrenProperty(P => P.WorkOrderProcessPackingUnitList).HasLabel("工序权限").Readonly().Show(ChildShowInWhere.Hide);
            }
        }
    }
}
