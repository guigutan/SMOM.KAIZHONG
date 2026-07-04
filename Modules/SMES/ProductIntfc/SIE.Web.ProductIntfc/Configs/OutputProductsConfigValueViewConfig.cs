using SIE.ProductIntfc.Configs;
using SIE.Warehouses;

namespace SIE.Web.ProductIntfc.Configs
{
    /// <summary>
    /// 配置值视图配置
    /// </summary>
    internal class OutputProductsConfigValueViewConfig : WebViewConfig<OutputProductsConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRule).Show(ShowInWhere.All);
            View.Property(p => p.JointWarehouse).UseDataSource((x, y, z) => {
                return RT.Service.Resolve<WarehouseController>().GetWarehouseDataList( LibraryType.Entity,z, y);
            }).Show(ShowInWhere.All);
            View.Property(p => p.ByWarehouse).UseDataSource((x, y, z) => {
                return RT.Service.Resolve<WarehouseController>().GetWarehouseDataList(LibraryType.Entity, z, y);
            }).Show(ShowInWhere.All);

        }
    }
}
