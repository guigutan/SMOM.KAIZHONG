using SIE.EMS.Equipments.Models;
using SIE.Resources.Enterprises;

namespace SIE.Web.EMS.Equipments.Models
{
    /// <summary>
    /// 维修项目查询条件视图
    /// </summary>
    public class EquipModelRepairProjectCriteriaViewConfig : WebViewConfig<EquipModelRepairProjectCriteria>
    {
        /// <summary>
        /// 配置查询条件
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProjectName).Show();
            View.Property(p => p.Part).Show();            
        }
    }
}
