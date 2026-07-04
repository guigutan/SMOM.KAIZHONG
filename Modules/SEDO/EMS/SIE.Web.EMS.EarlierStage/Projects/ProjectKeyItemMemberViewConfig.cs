using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 事项成员视图配置
    /// </summary>
    internal class ProjectKeyItemMemberViewConfig : WebViewConfig<ProjectKeyItemMember>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProjectKeyItem));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete, WebCommandNames.Save);
            
            View.Property(p => p.EmployeeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as ProjectKeyItemMember;
                if (model == null)
                    return new EntityList<Employee>();
                return RT.Service.Resolve<ProjectController>().GetKeyItemMemberEmployees(model.ProjectKeyItemId, pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EmployeeCode), nameof(e.Employee.Code));
                keyValues.Add(nameof(e.EmployeeName), nameof(e.Employee.Name));
                keyValues.Add(nameof(e.Phone), nameof(e.Employee.Phone));
                m.DicLinkField = keyValues;
                m.DisplayField = Employee.CodeProperty.Name;
                m.BindDisplayField = ProjectMember.EmployeeCodeProperty.Name;
            }).HasLabel("工号").ShowInList(80);
            View.Property(p => p.EmployeeName).HasLabel("姓名").ShowInList(140).Readonly();
            View.Property(p => p.Position).ShowInList(120);
            View.Property(p => p.Phone).Readonly().ShowInList(120);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}