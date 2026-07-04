using SIE.EMS.SpecialEquipment.RegularInspections.Criterias;
using SIE.Resources.Enterprises;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Criterias
{
    /// <summary>
    /// 检验规程查询视图
    /// </summary>
    internal class RegularInspectionCriteriaViewConfig : WebViewConfig<RegularInspectionCriteria>
    {
        /// <summary>
        /// 主体
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Show(ShowInWhere.All);
                View.Property(p => p.SpecialEquipmentAccount).HasLabel("设备编码").Show(ShowInWhere.All);
                View.Property(p => p.InspectionStatus).Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).Show(ShowInWhere.All);
                View.Property(p => p.UseDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var ctl = RT.Service.Resolve<EnterpriseController>();
                    return ctl.GetDepartmentsWithParent(pagingInfo, keyword);
                }).UsePagingLookUpEditor().HasLabel("使用部门").Show(ShowInWhere.All);
                View.Property(p => p.ResPersonId).HasLabel("资产责任人").Show(ShowInWhere.All);
                View.Property(p => p.Agency).HasLabel("检验机构").Show(ShowInWhere.All);
                View.Property(p => p.PlanInspectionDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                }).Show(ShowInWhere.All);
                View.Property(p => p.ActualInspectionDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
