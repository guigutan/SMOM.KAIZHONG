using SIE.EMS.Warehouses;
using SIE.Warehouses;

namespace SIE.Web.EMS.Warehouses
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WarehouseExtensionViewConfig : WebViewConfig<Warehouse>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(WarehouseExtension.IsZeroCostProperty)
                .HasLabel("不计成本")
                .HasOrderNo(11)
                .UseListSetting(x=>x.HelpInfo = "EDO用来标识是否计算成本");
        }
    }
}
