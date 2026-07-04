using SIE.MES.EmpWork;
using SIE.MES.LineAndon;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.LineAndon
{
    /// <summary>
    /// 产线与安灯
    /// </summary>
    public class AndonLineCriterialViewConfig : WebViewConfig<AndonLineCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.MachineName).ShowInList(width: 150);
                View.Property(p => p.MachineCode).ShowInList(width: 150);
                View.Property(p => p.EquipmentNo).ShowInList(width: 150);
                View.Property(p => p.WorkCenter).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList(width: 200).UseFactoryEditor();
                View.Property(p => p.WorkShopCode).ShowInList(width: 200);
                View.Property(p => p.AndonUphold).ShowInList(width: 200);
                View.Property(p => p.AndonCode).ShowInList(width: 150);
            }
        }
    }
}
