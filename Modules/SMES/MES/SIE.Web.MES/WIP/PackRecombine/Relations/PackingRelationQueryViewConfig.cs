using SIE.MES.WIP.PackRecombine.Relations;
using SIE.MetaModel.View;

namespace SIE.Web.MES.WIP.PackRecombine.Relations
{
    /// <summary>
    /// 包装关系查询视图配置
    /// </summary>
    internal class PackingRelationQueryViewConfig : WebViewConfig<PackingRelationQuery>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DraggableForTree();
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.PackageNo).ShowInList(width: 300).Readonly();
            View.Property(p => p.PackageUnitName).ShowInList(width: 80).Readonly();
            View.Property(p => p.PackedQty).Readonly();
            View.Property(p => p.ItemQty).ShowInList(width: 80).Readonly();
            View.Property(p => p.IsPacked).ShowInList(width: 80).Readonly();
            View.Property(p => p.WorkOrderNo).ShowInList(width: 150).Readonly();
            View.Property(p => p.Batch).ShowInList(width: 150).Readonly();
            View.Property(p => p.ProductName).ShowInList(width: 150).Readonly();
            View.Property(p => p.ResourceName).ShowInList(width: 120).Readonly();
            View.Property(p => p.ProcessName).ShowInList(width: 100).Readonly();
            View.Property(p => p.StationName).ShowInList(width: 100).Readonly();
            View.Property(p => p.PackedDate).ShowInList(width: 150).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
