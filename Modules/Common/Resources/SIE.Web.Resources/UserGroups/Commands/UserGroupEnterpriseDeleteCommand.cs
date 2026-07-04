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
    /// <summary>
    /// 用户组与工厂关系删除命令
    /// </summary>
    public class UserGroupEnterpriseDeleteCommand : DeleteCommand
    {
        protected override void DoSave(EntityList data)
        {
            EntityList<UserGroupEnterprise> list = data.DeletedList.OfType<UserGroupEnterprise>().AsEntityList();
            base.DoSave(data);
            RT.Service.Resolve<UserGroupsController>().DeleteUserGroupEnterpriseSyncEmployee(list);
        }
    }
}
