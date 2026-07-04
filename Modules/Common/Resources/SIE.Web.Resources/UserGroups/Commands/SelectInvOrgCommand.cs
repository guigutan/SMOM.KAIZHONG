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
    internal class SelectInvOrgCommand: ViewCommand
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
            var userGroupInvOrgs = args.Data.ToJsonObject<List<UserGroupInvOrg>>();
            Check.NotNullOrEmpty(userGroupInvOrgs, nameof(userGroupInvOrgs));

            if (null == userGroupInvOrgs || userGroupInvOrgs.Count == 0)
                throw new ArgumentNullException(nameof(userGroupInvOrgs));

            RT.Service.Resolve<UserGroupsController>().SaveUserGroupInvOrg(userGroupInvOrgs);

            return true;
        }
    }
}
