using SIE.Domain;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    public class UserInUserGroupSubmitted : OnSubmitted<UserInUserGroup>
    {
        protected override void Invoke(UserInUserGroup entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //当新增用户的时候更新对应用户的资源、工序、工厂、库存组织
                RT.Service.Resolve<UserGroupsController>().InsertSyncEmployee(entity);
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.User, UserGroupLogState.Add, entity.User.Employee.Name + $"[{entity.User.Employee.Code}]");
            }
            if (e.Action == SubmitAction.Delete)
            {
                //当删除的时候，把用户组这边对应资源、工序、工厂删除、库存组织
                RT.Service.Resolve<UserGroupsController>().DeleteSyncEmployee(entity);
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.User, UserGroupLogState.Delete, entity.User.Employee.Name + $"[{entity.User.Employee.Code}]");
            }
        }
    }
}
