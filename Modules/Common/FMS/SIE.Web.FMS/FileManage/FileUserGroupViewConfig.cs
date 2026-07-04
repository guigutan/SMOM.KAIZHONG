using SIE.FMS;
using SIE.FMS.FileManages;
using SIE.Web.FMS.FileManage.Commands;

namespace SIE.Web.FMS.FileManage
{
    /// <summary>
    /// 文件用户组-界面
    /// </summary>
    internal class FileUserGroupViewConfig : WebViewConfig<FileUserGroup>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseCommand(typeof(SetAdminCommand).FullName);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsAdmin);
            View.AttachChildrenProperty(typeof(UserInFileUserGroup), (w =>
            {
                var args = w as ChildPagingDataArgs;
                var fileUserGroup = w.Parent as FileUserGroup;
                var groupUser = RT.Service.Resolve<FileManageController>().GetUserInFileUserGroups(fileUserGroup.Id, args.SortInfo, args.PagingInfo);
                return groupUser;
            })).HasLabel("员工");
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
