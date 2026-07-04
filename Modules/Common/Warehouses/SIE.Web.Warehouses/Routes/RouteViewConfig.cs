using SIE.MetaModel.View;
using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 路径基础信息视图配置
    /// </summary>
    internal class RouteViewConfig : WebViewConfig<Route>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.SrcWhCode);
            View.Property(p => p.SrcAdd).ShowInList(150);
            View.Property(p => p.DesWhCode);
            View.Property(p => p.DesAdd).ShowInList(150);
            View.Property(p => p.Docks).ShowInList(150);
            View.Property(p => p.State);
            View.Property(p => p.Remark);

        }

        /// <summary>
        /// 导入模板
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.SrcWhCode).Show();
            View.Property(p => p.SrcAdd).Show();
            View.Property(p => p.DesWhCode).Show();
            View.Property(p => p.DesAdd).Show();
            View.Property(p => p.Docks).Show();
            View.Property(p => p.Remark).Show();
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.SrcWhCode).Show();
            View.Property(p => p.SrcAdd).Show();
            View.Property(p => p.DesWhCode).Show();
            View.Property(p => p.DesAdd).Show();
            View.Property(p => p.Docks).Show();
        }
    }
}
