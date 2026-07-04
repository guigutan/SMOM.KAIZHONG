using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Resources.UserGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.UserGroups
{
    public class UserGroupLogViewConfig : WebViewConfig<UserGroupLog>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.UserGroup).Show().Readonly();
                View.Property(p => p.Type).Show().Readonly();
                View.Property(p => p.State).Show().Readonly();
                View.Property(p => p.OperateData).ShowInList(width: 180).Readonly();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.UserGroup).Show();
                View.Property(p => p.Type).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.CreateDate).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.LastMonth);
            }
        }
    }
}
