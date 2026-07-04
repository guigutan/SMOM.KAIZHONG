using SIE.Common;
using SIE.Domain;
using SIE.Resources.UserGroups;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups.Commands
{
    public class UserGroupInvOrgDeleteCommand: DeleteCommand
    {
        protected override void DoSave(EntityList data)
        {
            EntityList<UserGroupInvOrg> userGroupInvOrgs = data.DeletedList.OfType<UserGroupInvOrg>().AsEntityList();
            base.DoSave(data);
            RT.Service.Resolve<UserGroupsController>().DeleteUserGroupInvOrgSyncUser(userGroupInvOrgs);
        }
    }
}
