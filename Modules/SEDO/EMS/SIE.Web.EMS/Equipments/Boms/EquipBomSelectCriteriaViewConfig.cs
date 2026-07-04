using SIE.EMS.Equipments.Boms;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Equipments.Boms
{
    /// <summary>
    /// 选择设备bom视图
    /// </summary>
    public class EquipBomSelectCriteriaViewConfig : WebViewConfig<EquipBomSelectCriteria>
    {
        /// <summary>
        /// 查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExecuteQuery);
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
        }
    }
}
