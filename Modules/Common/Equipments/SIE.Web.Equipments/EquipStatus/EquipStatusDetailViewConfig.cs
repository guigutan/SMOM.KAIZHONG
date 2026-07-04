using SIE.Equipments.EquipStatus;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Equipments.EquipStatus
{
    public class EquipStatusDetailViewConfig : WebViewConfig<EquipStatusDetail>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Status).Show().Readonly();
                View.Property(p => p.EndTime).Show().Readonly();
                View.Property(p => p.BeginTime).Show().Readonly();
                View.Property(p => p.Minute).Show().Readonly();
            }
        }
    }
}
