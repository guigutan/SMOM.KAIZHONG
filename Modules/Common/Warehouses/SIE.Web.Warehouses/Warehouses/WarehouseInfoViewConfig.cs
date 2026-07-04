using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 仓库资料视图配置
    /// </summary>
    internal class WarehouseInfoViewConfig : WebViewConfig<WarehouseInfo>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(3);
            View.Property(p => p.Area);
            View.Property(p => p.Volume);
            View.Property(p => p.WarehouseForm).DefaultValue(0);
        }
    }
}
