using DevExpress.CodeParser;
using DevExpress.DataAccess.DataFederation;
using SIE.EMS.EarlierStage.Projects;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.EMS.EarlierStage.Projects.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目成员视图配置
    /// </summary>
    public class ProjectMemberViewConfig : WebViewConfig<ProjectMember>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(ProjectMemberImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.EmployeeCode).ShowInList(140);
            View.Property(p => p.EmployeeName).ShowInList(80);
            View.Property(p => p.Position).ShowInList(120);
            View.Property(p => p.Phone).ShowInList(120);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.MemberStatus).ShowInList(80);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Project.Code).HasLabel("项目编码");
            View.PropertyRef(p => p.Employee.Code).HasLabel("工号");
            View.Property(p => p.Position);
            View.Property(p => p.Remark);
            View.Property(p => p.MemberStatus);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        void ConfigEditView()
        {
            View.AssignAuthorize(typeof(Project));
            View.UseCommands("SIE.Web.EMS.EarlierStage.Projects.Commands.SelectProjectMemberCommand", WebCommandNames.Add, WebCommandNames.Delete);
            using (View.OrderProperties())
            {                
                View.Property(p => p.Employee).UseDataSource(
                    (source, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                    }
                    ).UsePagingLookUpEditor((m, e) =>
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
                View.Property(p => p.MemberStatus).Readonly().ShowInList(80);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}