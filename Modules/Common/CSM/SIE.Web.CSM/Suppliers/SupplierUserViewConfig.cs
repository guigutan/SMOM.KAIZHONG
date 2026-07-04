using SIE.CSM.Suppliers;
using SIE.MetaModel.View;
using SIE.Web.CSM.Suppliers.Commands;

namespace SIE.Web.CSM.Suppliers
{
    /// <summary>
    /// 供应商与用户关系视图配置
    /// </summary>
    internal class SupplierUserViewConfig : WebViewConfig<SupplierUser>
    {
        /// <summary>
        /// 视图列表
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.UseCommands(typeof(SelectUserCommand).FullName, WebCommandNames.Delete);
            View.DeclareExtendViewGroup(nameof(SupplierUser));
            ConfigChildrenUser();
        }
        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.EmployeeCode).Show();
            View.Property(p => p.EmployeeName).Show();
            View.Property(p => p.UserCode).Show();
            View.Property(p => p.UserState).Show();
        }

        /// <summary>
        /// 供应商关联的用户视图配置
        /// </summary>
        void ConfigChildrenUser()
        {
            View.Property(p => p.EmployeeCode).Show();
            View.Property(p => p.EmployeeName).Show();
            View.Property(p => p.UserCode).Show();
            View.Property(p => p.UserState).Show();
        }
    }
}
