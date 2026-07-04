using SIE.MetaModel.View;
using SIE.Resources.UserGroups;
using SIE.Web.Resources.Employees.Commands;
using SIE.Web.Resources.UserGroups.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups
{
    public class UserGroupEnterpriseViewConfig : WebViewConfig<UserGroupEnterprise>
    {
        protected override void ConfigListView()
        {
            //View.InlineEdit();
            View.FormEdit();
            View.UseCommands(typeof(SIE.Web.Resources.UserGroups.Commands.SelectEnterpriseCommand).FullName, typeof(UserGroupEnterpriseDeleteCommand).FullName);
            View.Property(p => p.EnterpriseCode).HasLabel("编码");
            View.Property(p => p.EnterpriseName).HasLabel("名称");
        }
    }
}
