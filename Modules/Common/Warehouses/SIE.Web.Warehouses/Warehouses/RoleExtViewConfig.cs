using SIE.Rbac.Roles;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Warehouses
{
    internal class RoleExtViewConfig : WebViewConfig<Role>
    {
        protected override void ConfigListView()
        {
            View.Property(RoleExt.IsAllWarehouseProperty).HasLabel("全仓库权限").Show().UseListSetting(x => x.HelpInfo = "WMS全仓库可使用标记").HasOrderNo(10);
        }
    }
}
