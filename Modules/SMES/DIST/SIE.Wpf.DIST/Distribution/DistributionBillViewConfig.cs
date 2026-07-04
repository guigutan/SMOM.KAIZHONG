using SIE.DIST;
using SIE.MetaModel.View;
using SIE.Wpf.DIST.Distribution;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class DistributionBillViewConfig : WPFViewConfig<DistributionBill>
    {
        /// <summary>
        /// 载具关联视图组
        /// </summary>
        public const string Distribution = "Distribution";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(Distribution);
            if (ViewGroup == Distribution)
            {
                CollectViewConfig();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(BillLabelPrintCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.ContainerNo);
                View.Property(p => p.No).FixColumn(ColumnFixedStyle.Left);
                View.Property(p => p.WorkOrderNo).FixColumn(ColumnFixedStyle.Left);
                View.Property(p => p.ItemName).FixColumn(ColumnFixedStyle.Left);
                View.Property(p => p.Qty);
                View.Property(p => p.OkQty);
                View.Property(p => p.ReturnQty);
                View.Property(p => p.NgReturnQty);
                View.Property(p => p.PrintQty);
                View.Property(p => p.RemainderQty);
                View.Property(p => p.BindingDate);
                View.Property(p => p.State);
                View.Property(p => p.BindingBy);
                View.ChildrenProperty(p => p.DetailList);
                View.ChildrenProperty(p => p.PropertyValueList);
            }
        }

        /// <summary>
        /// 载具关联视图
        /// </summary>
        void CollectViewConfig()
        {
            View.AssignAuthorize(typeof(GoodsIssue));
            View.UseChildrenAsHorizontal(true);
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ContainerNo).Show(ShowInWhere.All);
                View.Property(p => p.No).Show(ShowInWhere.All).FixColumn(ColumnFixedStyle.Left);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All).FixColumn(ColumnFixedStyle.Left);
                View.Property(p => p.ItemName).Show(ShowInWhere.All).FixColumn(ColumnFixedStyle.Left);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.OkQty).Show(ShowInWhere.All);
                View.Property(p => p.ReturnQty).Show(ShowInWhere.All);
                View.Property(p => p.NgReturnQty).Show(ShowInWhere.All);
                View.Property(p => p.PrintQty).Show(ShowInWhere.All);
                View.Property(p => p.RemainderQty).Show(ShowInWhere.All);
                View.Property(p => p.BindingDate).Show(ShowInWhere.All);
                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.BindingBy).Show(ShowInWhere.All);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.List);
                View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.List);
            }
        }
    }
}
