using SIE.Domain;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.WCS.Devices
{
    /// <summary>
    /// 路径查询视图配置
    /// </summary>
    internal class RouteCriteriaViewConfig : WebViewConfig<RouteCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("路径");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {                
                View.Property(p => p.Code).Show();
                View.Property(p => p.SrcWhId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();
                    var results = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(pagingInfo, keyword);
                    return results;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.SrcWhCode), nameof(e.SrcWh.Code));
                    m.DicLinkField = dic;
                }).Show();
                View.Property(p => p.SrcAdd).Show();
                View.Property(p => p.DesWhId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();
                    var results = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(pagingInfo, keyword);
                    return results;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.DesWhCode), nameof(e.DesWh.Code));
                    m.DicLinkField = dic;
                }).Show();
                View.Property(p => p.DesAdd).Show();
                View.Property(p => p.Docks).Show();
                View.Property(p => p.State).Show();
            }
        }
    }
}
