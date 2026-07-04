using SIE.Domain;
using SIE.Rbac.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    public class UserGroupInRoleSubmitted : OnSubmitted<UserGroupInRole>
    {
        protected override void Invoke(UserGroupInRole entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Role, UserGroupLogState.Add, entity.Role.Name + $"[{entity.Role.Code}]");
            }
            if (e.Action == SubmitAction.Delete)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Role, UserGroupLogState.Delete, entity.Role.Name + $"[{entity.Role.Code}]");
            }
        }
    }
}
