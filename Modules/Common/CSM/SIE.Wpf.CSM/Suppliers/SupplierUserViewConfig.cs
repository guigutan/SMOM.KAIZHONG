using SIE.CSM.Suppliers;
using SIE.Wpf.CSM.Suppliers.Commonds;

namespace SIE.Wpf.CSM.Suppliers
{
    /// <summary>
    /// 供应商与用户关系视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class SupplierUserViewConfig : WPFViewConfig<SupplierUser>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.UseCommands(typeof(DetaisViewSelectUserCommand), WPFCommandNames.ListDelete);
            View.DeclareExtendViewGroup(nameof(SupplierUser));
            ConfigChildrenUser();
        }

        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.User.Employee.Code).HasLabel("账号");
            View.Property(p => p.User.Employee.Name).HasLabel("员工");
        }

        /// <summary>
        /// 供应商关联的用户视图配置
        /// </summary>
        void ConfigChildrenUser()
        {
            View.DomainName("供应商关联的用户");
            using (View.OrderProperties())
            {
                View.Property(p => p.User.Employee.Code).HasLabel("工号").Show(ShowInWhere.List);
                View.Property(p => p.User.Employee.Name).HasLabel("姓名").Show(ShowInWhere.List);
                View.Property(p => p.UserCode).HasLabel("用户账号").Show(ShowInWhere.List);
                View.Property(p => p.UserState).UseListSetting(w => w.ListGridWidth = 60).HasLabel("用户状态").Show(ShowInWhere.List);
            }
        }
    }
}
