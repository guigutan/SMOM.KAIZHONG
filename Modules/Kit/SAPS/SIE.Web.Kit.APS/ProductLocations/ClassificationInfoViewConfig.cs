using SIE.Kit.APS.Common;

namespace SIE.Web.Kit.APS.ProductLocations
{
    /// <summary>
    /// 查询分类值配置视图
    /// </summary>
    public class ClassificationInfoViewConfig : WebViewConfig<ClassificationInfo>
    {
        /// <summary>
        /// 列表逻辑视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Key);
            View.Property(p => p.Value);
            View.WithoutPaging();
        }
    }
}
