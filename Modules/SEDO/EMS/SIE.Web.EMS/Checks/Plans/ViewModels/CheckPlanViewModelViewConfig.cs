using SIE.EMS.Checks.Plans.ViewModels;
using SIE.Web.EMS.Checks.Plans.Commands;
using System;

namespace SIE.Web.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 保养计划视图配置
    /// </summary>
    internal class CheckPlanViewModelViewConfig : WebViewConfig<CheckPlanViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.FormEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Checks.Plans.Scripts.CheckPlanBehavior");
            View.UseCommands(typeof(AddCheckPlanCommand).FullName, typeof(BatchAddCheckPlanCommand).FullName);
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.YearAndMonth)
                    .UseDateEditor(d => d.Format = "Y-m")
                    .DefaultValue(DateTime.Now)
                    .FixColumn().ShowInList(width: 75).HasOrderNo(1);
                View.Property(p => p.EquipAccountCode).FixColumn().ShowInList(width: 105).HasOrderNo(2);
                View.Property(p => p.EquipAccountName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.EquipModelName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.EquipTypeName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.UseState).FixColumn().ShowInList(width: 105);
                View.Property(p => p.WorkShopName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.ResourceName).FixColumn().ShowInList(width: 105);
                View.Property(p => p.ProcessName).FixColumn().ShowInList(width: 105);
            }
        }
    }
}
