using Newtonsoft.Json.Linq;
using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.Resources.UserGroups;
using SIE.Web.Command;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups.Commands
{
    /// <summary>
    /// 用户与资源删除命令
    /// </summary>
    public class UserGroupResourceDeleteCommand : DeleteCommand
    {

        protected override void DoSave(EntityList data)
        {
            EntityList<UserGroupResource> userGroupResources = data.DeletedList.OfType<UserGroupResource>().AsEntityList();
            base.DoSave(data);
            RT.Service.Resolve<UserGroupsController>().DeleteUserGroupResourceSyncEmployee(userGroupResources);
        }

    }
}
