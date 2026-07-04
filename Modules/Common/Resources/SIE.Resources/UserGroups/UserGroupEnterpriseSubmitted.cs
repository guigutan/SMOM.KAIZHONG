using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    public class UserGroupEnterpriseSubmitted : OnSubmitted<UserGroupEnterprise>
    {
        protected override void Invoke(UserGroupEnterprise entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Factory, UserGroupLogState.Add, entity.Enterprise.Name + $"[{entity.Enterprise.Code}]");
            }
            if (e.Action == SubmitAction.Delete)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Factory, UserGroupLogState.Delete, entity.Enterprise.Name + $"[{entity.Enterprise.Code}]");
            }
        }
    }
}
