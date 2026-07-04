using SIE.MetaModel.View;
using SIE.Rbac.Users;
using SIE.Resources.UserGroups;
using SIE.Tech.Processs;
using SIE.Web.Tech.Processs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.Processs
{
    public class ProcessUserGroupViewConfig : WebViewConfig<ProcessUserGroup>
    {
        protected override void ConfigListView()
        {
            View.FormEdit();
            //View.UseCommands(typeof(ProcessEmployeeImportCommand).FullName, typeof(ProcessEmployeeDLTemplateCommand).FullName);
            View.UseCommands(typeof(SelectProcessUserGroupCommand).FullName, typeof(ProcessUserGroupDeleteCommand).FullName);
            View.Property(p => p.ProductFamilyCategory).Show();
            View.Property(p => p.Process).Show();
        }
    }

    public class UserGroupViewConfig : WebViewConfig<UserGroup>
    {
        protected override void ConfigListView()
        {
            View.AttachChildrenProperty(typeof(ProcessUserGroup), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var w = e.Parent as UserGroup;
                return RT.Service.Resolve<ProcessController>().GetProcessUserGroupsByUserGroupId(w.Id, args.PagingInfo);
            });
        }
    }
}
