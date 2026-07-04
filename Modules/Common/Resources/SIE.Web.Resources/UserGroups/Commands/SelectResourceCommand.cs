using SIE.Domain;
using SIE.Rbac.Users;
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
    internal class SelectResourceCommand: ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var userGroupResourceList = args.Data.ToJsonObject<List<UserGroupResource>>();
            Check.NotNullOrEmpty(userGroupResourceList, nameof(userGroupResourceList));

            if (null == userGroupResourceList || userGroupResourceList.Count == 0)
                throw new ArgumentNullException(nameof(userGroupResourceList));

            RT.Service.Resolve<UserGroupsController>().SaveUserGroupResource(userGroupResourceList);

            return true;
        }
    }
}
