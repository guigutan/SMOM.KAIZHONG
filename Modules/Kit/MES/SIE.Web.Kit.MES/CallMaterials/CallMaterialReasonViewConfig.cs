using SIE.Kit.MES.CallMaterials;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料原因视图配置
    /// </summary>
    internal class CallMaterialReasonViewConfig : WebViewConfig<CallMaterialReason>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().UseImportCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
        }
    }
}
