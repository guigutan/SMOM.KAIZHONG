using SIE.Domain;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.Equipments.Accounts.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.ExtensionViewConfig
{
    /// <summary>
    /// 设备台账点检项目视图配置
    /// </summary>
    public class MeteringEquipAccountCheckProjectViewConfig : WebViewConfig<MeteringEquipAccountCheckProject>
    {
        /// <summary>
        ///新增点检计划-项目列表
        /// </summary>
        public const string CheckPlanProjectList = "CheckPlanProjectList";

        /// <summary>
        ///设备表单视图
        /// </summary>
        public const string EquipAccountDetailViewGroup = "EquipAccountDetailViewGroup";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CheckPlanProjectList, EquipAccountDetailViewGroup);
            if (ViewGroup == CheckPlanProjectList)
                CheckPlanProjectListView();
            if (ViewGroup == EquipAccountDetailViewGroup)
            {
                ConfigListView();
               // View.RemoveCommands(WebCommandNames.Save);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(MeteringEquipmentAccount));

            View.ClearCommands();
            View.UseCommands(typeof(SelAccountCheckCommand).FullName, typeof(ImmediateDeleteCommand).FullName, WebCommandNames.Save);
           
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).Readonly().Show();
                View.Property(p => p.CycleType).Show();
                View.Property(p => p.DepartmentId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.DepartmentNameView), nameof(e.Department.Name));

                    m.DicLinkField = dic;
                    m.DisplayField = MeteringEquipAccountCheckProject.DepartmentNameViewProperty.Name;
                    m.BindDisplayField = MeteringEquipAccountCheckProject.DepartmentNameViewProperty.Name;
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
                View.Property(p => p.MinValue).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Show();
                View.Property(p => p.MaxValue).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Show();
                View.Property(p => p.Unit).Show();
                View.Property(p => p.UseTime).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Show();
                View.Property(p => p.LastCheckDate).Show();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProjectName);
            View.Property(p => p.ProjectType);            
        }

        /// <summary>
        /// 配置新增点检计划-项目列表
        /// </summary>
        protected void CheckPlanProjectListView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.UseCommands("SIE.Web.EMS.Equipments.Accounts.Commands.SelCheckPlanProjectCommand");
            View.UseCommands("SIE.Web.EMS.Equipments.Accounts.Commands.DeleteSelCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All);
                View.Property(p => p.ProjectName).Show(ShowInWhere.All);
                View.Property(p => p.ProjectType).Show(ShowInWhere.All);                
                View.Property(p => p.CycleType).Show(ShowInWhere.All);                
                View.Property(p => p.Part).Show(ShowInWhere.All);
                View.Property(p => p.Consumable).Show(ShowInWhere.All);
                View.Property(p => p.Method).Show(ShowInWhere.All);
                View.Property(p => p.Standard).Show(ShowInWhere.All);
                View.Property(p => p.MinValue).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Show(ShowInWhere.All);
                View.Property(p => p.MaxValue).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Show(ShowInWhere.All);
                View.Property(p => p.Unit).Show(ShowInWhere.All);
                View.Property(p => p.UseTime).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Show(ShowInWhere.All);
                View.Property(p => p.LastCheckDate).Show(ShowInWhere.All);
            }
        }
    }
}
