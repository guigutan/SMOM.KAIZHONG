using SIE.Packages.ItemLabels;
using SIE.Wpf.MES.BatchWIP.PackRecombine;
using SIE.Wpf.MES.WIP.PackRecombine;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 物料标签视图配置
    /// </summary>
    internal class ItemLabelViewConfig : WPFViewConfig<ItemLabel>
    {
        /// <summary>
        /// 包装ViewGroup
        /// </summary>
        public const string PackingView = "Packing";

        /// <summary>
        /// 包装ViewGroup
        /// </summary>
        public const string PackRecombineView = "PackRecombine";

        /// <summary>
        /// 批次包装ViewGroup
        /// </summary>
        public const string BatchPackRecombineView = "BatchPackRecombine";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PackingView, PackRecombineView, BatchPackRecombineView);
            switch (ViewGroup)
            {
                case PackingView:
                    PackingConfigView(); break;
                case PackRecombineView:
                    PackRecombineConfigView(); break;
                case BatchPackRecombineView:
                    BatchPackRecombineConfigView(); break;
            }
        }

        /// <summary>
        /// 包装视图配置
        /// </summary>
        void PackingConfigView()
        {
            using (View.OrderProperties())
            {
                View.AssignAuthorize(typeof(PackingViewModel));
                View.AssignAuthorize(typeof(NewPackingViewModel));
                View.AssignAuthorize(typeof(JoinPackingViewModel));
                View.ClearCommands();
                View.Property(p => p.Label).Show(ShowInWhere.All);
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 包装管理视图配置
        /// </summary>
        void PackRecombineConfigView()
        {
            using (View.OrderProperties())
            {
                View.AssignAuthorize(typeof(PackRecombineViewModel));
                View.ClearCommands();
                View.Property(p => p.Label).Show(ShowInWhere.All);
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 批次包装管理视图配置
        /// </summary>
        void BatchPackRecombineConfigView()
        {
            using (View.OrderProperties())
            {
                View.AssignAuthorize(typeof(BatchPackRecombineViewModel));
                View.ClearCommands();
                View.Property(p => p.Label).Show(ShowInWhere.All);
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
