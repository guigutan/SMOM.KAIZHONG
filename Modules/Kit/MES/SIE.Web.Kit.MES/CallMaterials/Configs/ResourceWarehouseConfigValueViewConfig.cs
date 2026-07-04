using SIE.Kit.MES.CallMaterials.Configs;

namespace SIE.Web.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 叫料管理资源仓库 视图配置
    /// </summary>
    public class ResourceWarehouseConfigValueViewConfig : WebViewConfig<ResourceWarehouse>
    {
        protected override void ConfigView()
        {
            View.WithoutPaging();
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
        }
    }
}
