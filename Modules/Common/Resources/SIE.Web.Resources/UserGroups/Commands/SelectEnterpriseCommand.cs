using SIE.Resources.Employees;
using SIE.Resources.UserGroups;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups.Commands
{
    public class SelectEnterpriseCommand: ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var userGroupEnterpriseList = args.Data.ToJsonObject<List<UserGroupEnterprise>>();
            RT.Service.Resolve<UserGroupsController>().SaveUserGroupEnterprise(userGroupEnterpriseList);
            return true;
        }
    }
}
