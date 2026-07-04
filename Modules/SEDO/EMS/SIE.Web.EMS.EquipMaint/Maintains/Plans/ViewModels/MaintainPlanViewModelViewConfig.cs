using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 保养计划视图配置
    /// </summary>
    internal class MaintainPlanViewModelViewConfig : WebViewConfig<MaintainPlanViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.MaintainPlanBehavior");
            View.UseCommands(AddMaintainPlanCommand.CommandName, BatchAddMaintainPlanCommand.CommandName,
                EditEquipMaintainPlanCommand.CommandName,typeof(ImportMaintainPlanCommand).FullName);
            View.UseClientOrder();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountCode).FixColumn().ShowInList(width: 105);
                View.Property(p => p.EquipAccountName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.EquipModelName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.EquipTypeCategory).FixColumn().ShowInList(width: 105);
                View.Property(p => p.WorkShopName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.ResourceName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.UseState).FixColumn().ShowInList(width: 105);
            }
        }
    }
}
