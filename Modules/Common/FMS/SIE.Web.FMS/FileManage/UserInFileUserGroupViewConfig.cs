using SIE.FMS;
using SIE.MetaModel.View;
using SIE.Web.FMS.FileManage.Commands;

namespace SIE.Web.FMS.FileManage
{
    /// <summary>
    /// 文件用户组和用户关系-界面
    /// </summary>
    internal class UserInFileUserGroupViewConfig : WebViewConfig<UserInFileUserGroup>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(AddEmployeeCommand).FullName, WebCommandNames.Delete);
            View.Property(p => p.EmployeeCode);
            View.Property(p => p.EmployeeName);
        }
    }
}
