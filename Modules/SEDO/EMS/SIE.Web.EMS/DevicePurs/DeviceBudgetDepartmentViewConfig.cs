using SIE.EMS.DevicePurs;
using SIE.MetaModel.View;
using SIE.Web.EMS.DevicePurs.Commands;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 预算部门-界面
    /// </summary>
    internal class DeviceBudgetDepartmentViewConfig : WebViewConfig<DeviceBudgetDepartment>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelBudgetDepartmentCommand).FullName,
                 WebCommandNames.Delete);

            View.Property(p => p.EnterpriseCode).Readonly();
            View.Property(p => p.EnterpriseName).Readonly();
        }
    }
}
