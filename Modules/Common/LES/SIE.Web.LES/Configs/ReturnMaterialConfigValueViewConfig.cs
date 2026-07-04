using SIE.LES.RetreatItemManage.Configs;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Web.Common;

namespace SIE.Web.LES.Configs
{
    /// <summary>
    /// 
    /// </summary>
    public class ReturnMaterialConfigValueViewConfig : WebViewConfig<ReturnMaterialConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.ReturnMaterialCodeRule).Show(ShowInWhere.All);
            View.Property(p => p.ReasonDefault).UseCatalogEditor(p => { p.CatalogType = MaterialReturn.ReasonMaterialReturn; p.CatalogReloadData = true; }).Show(ShowInWhere.All);
            View.Property(p => p.ReasonRequired).Show(ShowInWhere.All);
        }
    }
}
