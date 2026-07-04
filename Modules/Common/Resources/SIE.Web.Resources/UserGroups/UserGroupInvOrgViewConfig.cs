using SIE.Resources.UserGroups;
using SIE.Web.Resources.UserGroups.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups
{
    public class UserGroupInvOrgViewConfig:WebViewConfig<UserGroupInvOrg>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.FormEdit();
                View.UseCommands(typeof(SelectInvOrgCommand).FullName, typeof(UserGroupInvOrgDeleteCommand).FullName);
                View.Property(p => p.InvOrgCode).Show().Readonly();
                View.Property(p => p.InvOrgName).Show().Readonly();
                View.Property(p => p.InvOrgExternalId).Show().Readonly();
                View.Property(p => p.InvOrgRemark).Show().Readonly();
            }
        }
    }
}
