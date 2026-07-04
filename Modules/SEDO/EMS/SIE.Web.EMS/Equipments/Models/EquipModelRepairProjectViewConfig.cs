using SIE.EMS.Equipments.Models;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.Equipments.Models.Commands;

namespace SIE.Web.EMS.Equipments.Models
{
    /// <summary>
    /// 维修项目视图
    /// </summary>
    public class EquipModelRepairProjectViewConfig : WebViewConfig<EquipModelRepairProject>
    {
        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.WithoutPaging();
            View.UseCommands(typeof(SelRepairProjectCommand).FullName,WebCommandNames.Delete);
            View.Property(p => p.ProjectDetailId).HasLabel("项目名称");
            View.Property(p => p.DepartmentId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var ctl = RT.Service.Resolve<EnterpriseController>();
                return ctl.GetDepartmentsWithParent(pagingInfo, keyword);
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
}
