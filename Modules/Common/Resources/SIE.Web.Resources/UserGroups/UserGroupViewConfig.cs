using SIE.Rbac.Users;
using SIE.Resources.UserGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups
{
    public class UserGroupViewConfig : WebViewConfig<UserGroup>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();
            }
        }

        protected override void ConfigListView()
        {
            View.AttachChildrenProperty(typeof(UserGroupResource), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var w = e.Parent as UserGroup;
                return RT.Service.Resolve<UserGroupsController>().GetUserGroupResourcesByUserGroupId(w.Id, args.PagingInfo);
            });
            View.AttachChildrenProperty(typeof(UserGroupEnterprise), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var w = e.Parent as UserGroup;
                return RT.Service.Resolve<UserGroupsController>().GetUserGroupEnterprisesByUserGroupId(w.Id, args.PagingInfo);
            });
            View.AttachChildrenProperty(typeof(UserGroupInvOrg), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var w = e.Parent as UserGroup;
                return RT.Service.Resolve<UserGroupsController>().GetUserGroupInvOrgsByUserGroupId(w.Id, args.PagingInfo);
            });
        }
    }
}
