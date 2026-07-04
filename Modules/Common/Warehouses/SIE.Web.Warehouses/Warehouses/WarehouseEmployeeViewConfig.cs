using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 仓库与员工关系视图配置
    /// </summary>
    public class WarehouseEmployeeViewConfig : WebViewConfig<WarehouseEmployee>
    {
        public const string EmployeeSelectView = "EmployeeReadOnlyView";
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("仓库与员工关系").HasDelegate(WarehouseEmployee.EmployeeIdProperty);
            View.DeclareExtendViewGroup(new string[] { EmployeeSelectView });
            if (ViewGroup == EmployeeSelectView)
            {
                ConfigEmpReadOnlyView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(WarehouseEmployeeLookupCommand).FullName, WebCommandNames.Delete);
            View.Property(p => p.EmployeeCode).HasLabel("工号");
            View.Property(p => p.EmployeeName).HasLabel("姓名");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.EmployeeCode).HasLabel("工号");
            View.Property(p => p.EmployeeName).HasLabel("姓名");
        }

        /// <summary>
        /// 员工查看仓库视图
        /// </summary>
        protected void ConfigEmpReadOnlyView()
        {
            View.FormEdit();
            View.UseCommands(typeof(EmployeeWarehouseCommand).FullName, WebCommandNames.Delete, WebCommandNames.ExportXls);
            View.Property(p => p.WarehouseCode).Show();
            View.Property(p => p.WarehouseName).Show();
        }
    }
}