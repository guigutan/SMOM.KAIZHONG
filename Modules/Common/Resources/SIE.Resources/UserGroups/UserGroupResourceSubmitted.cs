using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    public class UserGroupResourceSubmitted : OnSubmitted<UserGroupResource>
    {
        protected override void Invoke(UserGroupResource entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Resource, UserGroupLogState.Add, entity.Resource.Name + $"[{entity.Resource.Code}]");
            }
            if (e.Action == SubmitAction.Delete)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Resource, UserGroupLogState.Delete, entity.Resource.Name + $"[{entity.Resource.Code}]");
            }
        }
    }
}
