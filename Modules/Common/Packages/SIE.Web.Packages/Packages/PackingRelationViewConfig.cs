using SIE.Packages;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// 包装关系视图配置
    /// </summary>
    internal class PackingRelationViewConfig : WebViewConfig<PackingRelation>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.InlineEdit()
                .UseDefaultCommands();
            View.Property(p => p.PackageNo);
            View.Property(p => p.PackageUnitName);
            View.Property(p => p.PackedQty);
            View.Property(p => p.ItemQty);
        }
    }
}
