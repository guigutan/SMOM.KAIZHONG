using SIE.EMS.DevicePurs;
using SIE.MetaModel.View;
using SIE.Web.EMS.DevicePurs.Commands;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 使用部门-界面
    /// </summary>
    internal class DeviceUseDepartmentViewConfig : WebViewConfig<DeviceUseDepartment>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelUseDepaCommand).FullName, WebCommandNames.Delete);
            View.Property(p => p.EnterpriseCode).Readonly();
            View.Property(p => p.EnterpriseName).Readonly();
        }
    }
}
