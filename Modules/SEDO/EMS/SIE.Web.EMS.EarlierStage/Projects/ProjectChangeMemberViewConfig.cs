using SIE.EMS.EarlierStage.Projects;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更成员界面
    /// </summary>
    public class ProjectChangeMemberViewConfig : WebViewConfig<ProjectChangeMember>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadonlyView);
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            View.Property(p => p.EmployeeCode).HasLabel("工号").ShowInList(140).Readonly();
            View.Property(p => p.EmployeeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EmployeeCode), nameof(e.Employee.Code));
                keyValues.Add(nameof(e.Phone), nameof(e.Employee.Phone));
                m.DicLinkField = keyValues;
            }).HasLabel("姓名").ShowInList(80);
            View.Property(p => p.Position).ShowInList(120);
            View.Property(p => p.Phone).Readonly().ShowInList(120);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.MemberStatus).ShowInList(80);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).HasLabel("工号").ShowInList(140).Readonly();
                View.Property(p => p.EmployeeId).HasLabel("姓名").ShowInList(80).Readonly();
                View.Property(p => p.Position).ShowInList(120).Readonly();
                View.Property(p => p.Phone).Readonly().ShowInList(120);
                View.Property(p => p.Remark).ShowInList(200).Readonly();
                View.Property(p => p.MemberStatus).Readonly().ShowInList(80);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
