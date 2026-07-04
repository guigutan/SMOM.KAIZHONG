using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 仓库与员工关系视图配置
    /// </summary>
    public class InWarehouseEmployeeViewConfig : WebViewConfig<InWarehouseEmployee>
    {
        public const string EmployeeSelectView = "EmployeeReadOnlyView";
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("可调拨至仓库").HasDelegate(InWarehouseEmployee.EmployeeIdProperty);
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
            View.InlineEdit();
            View.UseCommands(typeof(InEmployeeWarehouseCommand).FullName,WebCommandNames.Save, WebCommandNames.Delete, typeof(SynchronizeToEmployeesCommand).FullName,WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseCode).Show().Readonly();
                View.Property(p => p.WarehouseName).Show().Readonly();
                View.Property(p => p.InvOrgCode).DisableSort().Show().Readonly();
                View.Property(p => p.InvOrgName).DisableSort().Show().Readonly();
                View.Property(p => p.IsDirectAllocate).Show().Readonly(p=>p.InvOrgCode != RT.InvOrg);
                View.Property(p => p.IsTwoAllocate).Show().Readonly(p => p.InvOrgCode != RT.InvOrg);
                View.Property(p => p.IsCrossOrgTransferIn).Show().Readonly();
            }
        }
    }
}