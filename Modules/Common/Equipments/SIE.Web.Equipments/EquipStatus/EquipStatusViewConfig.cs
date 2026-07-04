using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Equipments.EquipStatus
{
    public class EquipStatusViewConfig : WebViewConfig<SIE.Equipments.EquipStatus.EquipStatus>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SIE.Equipments.EquipStatus.EquipStatus));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountCode).Show().Readonly();
                View.Property(p => p.EquipAccountName).Show().Readonly();
                View.Property(p => p.Status).Show().Readonly();
                View.Property(p => p.Factory).Show().Readonly();
                View.ChildrenProperty(p => p.EquipStatusDetailList).Show(ChildShowInWhere.All);
            }
        }
    }
}
