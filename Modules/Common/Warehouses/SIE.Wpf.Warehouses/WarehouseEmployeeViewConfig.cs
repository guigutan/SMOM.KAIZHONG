using SIE.Warehouses;
using SIE.Wpf.Command;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 仓库与员工关系视图配置
    /// </summary>
    internal class WarehouseEmployeeViewConfig : WPFViewConfig<WarehouseEmployee>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("仓库与员工关系").HasDelegate(WarehouseEmployee.EmployeeIdProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(LookupCommand), typeof(ListDeleteCommand));
            View.Property(p => p.Employee.Code).HasLabel("工号");
            View.Property(p => p.Employee.Name).HasLabel("姓名");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Employee.Code).HasLabel("工号");
            View.Property(p => p.Employee.Name).HasLabel("姓名");
        }
    }
}