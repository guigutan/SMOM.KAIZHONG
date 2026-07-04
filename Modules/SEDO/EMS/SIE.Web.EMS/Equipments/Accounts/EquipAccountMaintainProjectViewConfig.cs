using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.Equipments.Accounts.Commands;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账保养项目视图配置
    /// </summary>
    public class EquipAccountMaintainProjectViewConfig : WebViewConfig<EquipAccountMaintainProject>
    {
        /// <summary>
        /// 保养计划添加保养项目视图
        /// </summary>
        public const string AddMaintainProject = "AddMaintainProject";

        /// <summary>
        ///设备表单视图
        /// </summary>
        public const string EquipAccountDetailViewGroup = "EquipAccountDetailViewGroup";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddMaintainProject, EquipAccountDetailViewGroup);
            if (ViewGroup == AddMaintainProject)
                AddMaintainProjectView();
            if (ViewGroup == EquipAccountDetailViewGroup)
            {
                ConfigListView();
                //View.RemoveCommands(WebCommandNames.Save);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipAccount));

            View.ClearCommands();
            View.UseCommands(typeof(SelAccountMaintainCommand).FullName,  typeof(ImmediateDeleteCommand).FullName,
                WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).Show();
                View.Property(p => p.CycleType).Show();
                View.Property(p => p.DepartmentId).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = EquipAccountMaintainProject.DepartmentNameViewProperty.Name;
                    m.BindDisplayField = EquipAccountMaintainProject.DepartmentNameViewProperty.Name;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                    if (departments == null || departments.Count <= 0)
                        return new EntityList<Enterprise>();
                    departments.ForEach(p => p.TreePId = null);
                    return departments;
                }).UsePagingLookUpEditor().Show();
                View.Property(p => p.Part).Show();
                View.Property(p => p.Consumable).Show();
                View.Property(p => p.Method).Show();
                View.Property(p => p.Standard).Show();
                View.Property(p => p.MinValue).UseSpinEditor(p => { p.MinValue = 0;p.AllowNegative = false; }).Show();
                View.Property(p => p.MaxValue).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).Show();
                View.Property(p => p.Unit).Show();
                View.Property(p => p.UseTime).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).Show();
                View.Property(p => p.LastMaintainDate).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 保养计划添加保养项目视图
        /// </summary>
        protected void AddMaintainProjectView()
        {
            View.FormEdit();
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.UseCommands("SIE.Web.EMS.Equipments.Accounts.Commands.SelMaintainPlanProjectCommand", "SIE.Web.EMS.Equipments.Accounts.Commands.DeleteSelCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountCode).HasLabel("设备编码").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).HasLabel("设备名称").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ProjectName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ProjectType).Readonly().Show(ShowInWhere.All);                
                View.Property(p => p.CycleType).Readonly().Show(ShowInWhere.All);                
                View.Property(p => p.Part).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Consumable).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Method).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Standard).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.MinValue).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.MaxValue).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Unit).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.UseTime).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.LastMaintainDate).Readonly().Show(ShowInWhere.All);
            }
        }
    }
}
