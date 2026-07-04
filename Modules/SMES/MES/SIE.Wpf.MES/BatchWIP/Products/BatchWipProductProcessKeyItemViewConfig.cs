using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品生产关键件视图配置
    /// </summary>
    internal class BatchWipProductProcessKeyItemViewConfig : WPFViewConfig<BatchWipProductProcessKeyItem>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion), typeof(BatchWipProductRouting));
            View.ClearCommands();
            View.Property(p => p.ProcessName);
            View.Property(p => p.StationName);
            View.Property(p => p.SourceCode);
            View.Property(p => p.SourceType).UseEnumEditor();
            View.Property(p => p.Qty);
            View.Property(p => p.SingleQty);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.ItemDesc);
            View.Property(p => p.ItemNnitName);
            View.Property(p => p.CreateDate).HasLabel("操作时间").ShowInList(gridWidth: 150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ValueList).Show(ChildShowInWhere.Hide);
        }
    }
}
