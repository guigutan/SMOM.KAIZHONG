using SIE.Resources.Employees;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class EmployeeGroupViewConfig : WebViewConfig<EmployeeGroup>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Employee));
        }

        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
