using Amazon.Runtime.Internal.Auth;
using SIE.Equipments.EquipStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Equipments.EquipStatus
{
    public class EquipStatusCriteriaViewConfig : WebViewConfig<EquipStatusCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountCode).Show();
                View.Property(p => p.EquipAccountName).Show();
                View.Property(p => p.Status).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.LastMonth).Show();
            }
        }
    }
}
