using SIE.Inventory.Transactions;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Transactions.Commands;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// 仓库与员工关系视图配置
    /// </summary>
    public class FunctionEmployeeViewConfig : WebViewConfig<FunctionEmployee>
    {
      
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("禁用员工").HasDelegate(FunctionEmployee.EmployeeIdProperty);            
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(FunctionEmployeeLookupCommand).FullName, WebCommandNames.Delete);
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
    }
}