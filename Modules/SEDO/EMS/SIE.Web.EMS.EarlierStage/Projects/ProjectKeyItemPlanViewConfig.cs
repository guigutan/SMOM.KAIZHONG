using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.EMS.EarlierStage.Projects.Commands;
using System;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 事项计划视图配置
    /// </summary>
    internal class ProjectKeyItemPlanViewConfig : WebViewConfig<ProjectKeyItemPlan>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProjectKeyItem));
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectWorkItemBehavior");
            View.UseCommands(WebCommandNames.Add, "SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteKeyItemPlanCommand", WebCommandNames.Save,
                typeof(StartKeyItemPlanCommand).FullName, typeof(CompleteKeyItemPlanCommand).FullName);
            View.Property(p => p.PlanNode).ShowInList(400);
            View.Property(p => p.PlanStart).HasLabel("计划开始时间".L10N()+"*").Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(150);
            View.Property(p => p.PlanEnd).HasLabel("计划结束时间".L10N()+ "*").Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(150);
            View.Property(p => p.ActualSart).ShowInList(150).Readonly();
            View.Property(p => p.ActualStartPeopleId).ShowInList(150).Readonly();
            View.Property(p => p.ActualEnd).ShowInList(150).Readonly();
            View.Property(p => p.ActaulEndPeopleId).ShowInList(150).Readonly();
            View.Property(p => p.WorkStatus).ShowInList(80).Readonly();
            View.Property(p => p.PrincipalId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as ProjectKeyItemPlan;
                if (model == null)
                    return new EntityList<Employee>();
                return RT.Service.Resolve<ProjectController>().GetKeyItemPlanPrincipals(model.ProjectKeyItemId, pagingInfo, keyword);
            }).ShowInList(80).HasLabel("责任人");
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}