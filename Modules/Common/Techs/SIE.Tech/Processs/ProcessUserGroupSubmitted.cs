using SIE.Domain;
using SIE.Resources.UserGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Tech.Processs
{
    public class ProcessUserGroupSubmitted:OnSubmitted<ProcessUserGroup>
    {
        protected override void Invoke(ProcessUserGroup entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Process, UserGroupLogState.Add, entity.Process.Name + $"[{entity.Process.Code}]");
            }
            if (e.Action == SubmitAction.Delete)
            {
                //创建日志
                RT.Service.Resolve<UserGroupsController>().CreateUserGroupLog(entity.UserGroup, UserGroupLogType.Process, UserGroupLogState.Delete, entity.Process.Name + $"[{entity.Process.Code}]");
            }
        }
    }
}
