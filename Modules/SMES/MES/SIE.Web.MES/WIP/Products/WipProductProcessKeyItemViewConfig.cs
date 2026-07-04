using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品生产关键件视图配置
    /// </summary>
    internal class WipProductProcessKeyItemViewConfig : WebViewConfig<WipProductProcessKeyItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
            View.ClearCommands();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.SourceCode).Readonly();
            View.Property(p => p.SourceType).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.ItemExtPropName).Readonly();
            View.Property(p => p.ItemDescription).Readonly();
            View.Property(p => p.ItemUnitName).Readonly();
            View.Property(p => p.IsUnbound).Readonly();
            View.Property(p => p.CreateDate).HasLabel("操作时间").Readonly().ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            //View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide);
        }
    }
}
