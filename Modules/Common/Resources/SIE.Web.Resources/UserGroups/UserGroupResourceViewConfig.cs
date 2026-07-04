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
    public class UserGroupResourceViewConfig : WebViewConfig<UserGroupResource>
    {
        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.InlineEdit();
            View.FormEdit();
            //View.UseCommands(typeof(EmployeeResourceDLTemplateCommand).FullName, typeof(EmployeeResourceImportCommand).FullName);
            View.UseCommands(typeof(SIE.Web.Resources.UserGroups.Commands.SelectResourceCommand).FullName, typeof(UserGroupResourceDeleteCommand).FullName);
            View.Property(p => p.ResourceCode).HasLabel("编码");
            View.Property(p => p.ResourceName).HasLabel("名称");
        }
    }
}
