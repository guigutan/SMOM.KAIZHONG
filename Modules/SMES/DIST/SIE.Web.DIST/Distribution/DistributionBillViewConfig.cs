using SIE.DIST;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class DistributionBillViewConfig : WebViewConfig<DistributionBill>
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
            View.UseCommands("SIE.Web.DIST.Distribution.Commands.BillLabelPrintCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ContainerNo).ShowInList(150);
                View.Property(p => p.No).ShowInList(150).FixColumn(true);
                View.Property(p => p.WorkOrderNo).ShowInList(150).FixColumn(true);
                View.Property(p => p.ItemName).ShowInList(150).FixColumn(true);
                View.Property(p => p.Qty);
                View.Property(p => p.OkQty);
                View.Property(p => p.ReturnQty);
                View.Property(p => p.NgReturnQty);
                View.Property(p => p.PrintQty);
                View.Property(p => p.RemainderQty);
                View.Property(p => p.BindingDate).ShowInList(150);
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
                View.Property(p => p.No).Show(ShowInWhere.All).FixColumn(true);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All).FixColumn(true);
                View.Property(p => p.ItemName).Show(ShowInWhere.All).FixColumn(true);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.OkQty).Show(ShowInWhere.All);
                View.Property(p => p.ReturnQty).Show(ShowInWhere.All);
                View.Property(p => p.NgReturnQty).Show(ShowInWhere.All);
                View.Property(p => p.PrintQty).Show(ShowInWhere.All);
                View.Property(p => p.RemainderQty).Show(ShowInWhere.All);
                View.Property(p => p.BindingDate).Show(ShowInWhere.All);
                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.BindingBy).Show(ShowInWhere.All);
                View.Property(p => p.CreateBy).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.All);
            }
        }
    }
}
