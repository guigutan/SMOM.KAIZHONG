using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    public class UserGroupInvOrgSubmitted : OnSubmitted<UserGroupInvOrg>
    {
        protected override void Invoke(UserGroupInvOrg entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.InvOrg, UserGroupLogState.Add, entity.Inv.Name + $"[{entity.Inv.Code}]");
            }
            if (e.Action == SubmitAction.Delete)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.InvOrg, UserGroupLogState.Delete, entity.Inv.Name + $"[{entity.Inv.Code}]");
            }
        }
    }
}
