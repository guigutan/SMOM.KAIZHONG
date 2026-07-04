using SIE.EMS.Equipments.Boms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM选择视图
    /// </summary>
    public class EquipBomDetailSelCriteriaViewConfig : WebViewConfig<EquipBomDetailSelCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("设备BOM选择");
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.ModelCode).Show().Readonly(p => p.IsReadOnly);
            }
        }
    }
}
