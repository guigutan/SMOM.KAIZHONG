using SIE.EMS.Equipments.Accounts;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账保养项目查询视图配置
    /// </summary>
    internal class MaintainProjectCriteriaViewConfig : WebViewConfig<EquipAccountMaintainProjectCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All);
                View.Property(p => p.ProjectName).Show(ShowInWhere.All);
                View.Property(p => p.Category).Show(ShowInWhere.All);
                View.Property(p => p.CycleType).Show(ShowInWhere.All);
            }
        }
    }
}
