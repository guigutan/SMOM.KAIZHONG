using SIE.EMS.RunStandards;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.RunStandards.Commands;

namespace SIE.Web.EMS.RunStandards
{
    /// <summary>
    /// 
    /// </summary>
    public class RunStandardProjectViewConfig : WebViewConfig<RunStandardProject>
    {

        /// <summary>
		/// 编辑视图
		/// </summary>
		public const string EditView = "EditView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                DetailListView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected  void DetailListView()
        {
            View.UseCommands(typeof(SelRunStandardProjectCommand).FullName, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectDetailId).HasLabel("项目名称").ShowInList(80);
                View.Property(p => p.DepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var ctl = RT.Service.Resolve<EnterpriseController>();
                    return ctl.GetAllDepartmentsWithFactoryOrAll(null,pagingInfo, keyword);
                }).UsePagingLookUpEditor().ShowInList(80);
                View.Property(p => p.Part).ShowInList(120);
                View.Property(p => p.Consumable).ShowInList(80);
                View.Property(p => p.Method).ShowInList(120);
                View.Property(p => p.Standard).ShowInList(100);
                View.Property(p => p.MinValue).ShowInList(80);
                View.Property(p => p.MaxValue).ShowInList(80);
                View.Property(p => p.Unit).ShowInList(60);
                View.Property(p => p.UseTime).ShowInList(100);
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectDetailId).HasLabel("项目名称");
                View.Property(p => p.DepartmentId).ShowInList(80);
                View.Property(p => p.Part).ShowInList(120);
                View.Property(p => p.Consumable).ShowInList(80);
                View.Property(p => p.Method).ShowInList(120);
                View.Property(p => p.Standard).ShowInList(100);
                View.Property(p => p.MinValue).ShowInList(80);
                View.Property(p => p.MaxValue).ShowInList(80);
                View.Property(p => p.Unit).ShowInList(60);
                View.Property(p => p.UseTime).ShowInList(100);
            }
        }
    }
}
