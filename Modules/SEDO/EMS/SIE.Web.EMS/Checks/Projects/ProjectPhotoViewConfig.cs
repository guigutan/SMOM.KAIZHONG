using SIE.EMS.Checks.Projects;

namespace SIE.Web.EMS.Checks.Projects
{
    /// <summary>
    /// 项目图片-界面
    /// </summary>
    internal class ProjectPhotoViewConfig : WebViewConfig<ProjectPhoto>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Photo);
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Photo);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Photo);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Photo);
        }
    }
}
