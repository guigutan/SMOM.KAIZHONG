using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Web.Tech.Processs.Commands;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序与用户视图
    /// </summary>
    public class ProcessEmployeeViewConfig : WebViewConfig<ProcessEmployee>
    {
        /// <summary>
        /// 员工维护ViewGroup
        /// </summary>
        public const string EmplProcessView = "EmplProcessView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EmplProcessView);
            if (ViewGroup == EmplProcessView)
            {
                EmplProcessTabView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.Property(p => p.Process);
            View.Property(p => p.ProcessProductFamilyCode).HasLabel("产品族小类");
            View.Property(p => p.ProcessProductFamilyName).HasLabel("产品族");
            View.Property(p => p.ProcessType).HasLabel("工序类型");
            View.Property(p => p.ProcessSegmentName).HasLabel("工段名称");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ProductFamilyCategory);
            View.Property(p => p.Process);
        }

        /// <summary>
        /// 员工维护 - 工序页签视图
        /// </summary>
        protected void EmplProcessTabView()
        {
            View.FormEdit();
            View.UseCommands(typeof(ProcessEmployeeImportCommand).FullName, typeof(ProcessEmployeeDLTemplateCommand).FullName);
            View.UseCommands(typeof(SelectProcessCommand).FullName, WebCommandNames.Delete);
            View.Property(p => p.ProductFamilyCategory).Show();
            View.Property(p => p.Process).Show();
        }
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Employee.Code).HasLabel("员工工号");
            View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
        }
    }

    /// <summary>
    /// 员工维护 - 工序页签
    /// </summary>
    internal class EmployeeExtViewConfig : WebViewConfig<Employee>
    {

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AttachChildrenProperty(typeof(ProcessEmployee), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var w = e.Parent as Employee;
                return RT.Service.Resolve<ProcessController>().GetProcessEmployees(w.Id, args.PagingInfo);
            }, ProcessEmployeeViewConfig.EmplProcessView);
        }
    }
}
