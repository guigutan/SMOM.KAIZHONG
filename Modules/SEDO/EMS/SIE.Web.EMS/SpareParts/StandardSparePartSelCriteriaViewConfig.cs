using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.SpareParts;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 标准备件基础数据选择 视图
    /// </summary>
    public class StandardSparePartSelCriteriaViewConfig : WebViewConfig<StandardSparePartSelCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("标准备件基础数据选择");
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.ProjectDetailId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as StandardSparePartSelCriteria;
                    if (criteria == null)
                    {
                        return new EntityList<ProjectDetail>();
                    }
                    return RT.Service.Resolve<EquipController>().GetEquipModelRepairProjectList(criteria.EquipModelId, c, r);

                }).HasLabel("项目名称").Show();
                View.Property(p => p.EquipModelId).Show(ShowInWhere.Hide);
            }
        }
    }
}
