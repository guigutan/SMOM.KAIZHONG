using SIE.Domain;
using SIE.EMS.Lubrications;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Enterprises;
using System.Linq;

namespace SIE.Web.EMS.Lubrications
{
    /// <summary>
    /// 润滑记录查询视图
    /// </summary>
    internal class LubricationCriteriaViewConfig : WebViewConfig<LubricationCriteria>
    {
        /// <summary>
        /// 主体
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.LubricationNo).Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountId).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EquipAccountController>().GetAllEquipAccounts(p, k);
                }).HasLabel("设备编码").Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All);
                View.Property(p => p.EquipTypeId).HasLabel("设备类型").Show(ShowInWhere.All);
                View.Property(p => p.EquipModelId).HasLabel("设备型号").Show(ShowInWhere.All);
                View.Property(p => p.LubricationStatus).HasLabel("润滑状态").Show(ShowInWhere.All);
                View.Property(p => p.WorkShopId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as LubricationCriteria;
                    if (criteria == null)
                    {
                        return new EntityList<Enterprise>();
                    }
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                    workShopList.ForEach(enterprise =>
                    {
                        enterprise.TreePId = null;
                    });
                    return workShopList;
                }).HasLabel("车间").Show(ShowInWhere.All);
                View.Property(p => p.UseDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var ctl = RT.Service.Resolve<EnterpriseController>();
                    var DepartmentList = ctl.GetDepartments(pagingInfo, keyword);
                    DepartmentList.ForEach(enterprise =>
                    {
                        enterprise.TreePId = null;
                    });
                    return DepartmentList;
                }).UsePagingLookUpEditor().HasLabel("部门").Show(ShowInWhere.All);
                View.Property(p => p.IsOverdue).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PlanDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                }).Show(ShowInWhere.All);
                View.Property(p => p.StartDateTime).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).HasLabel("润滑日期").Show(ShowInWhere.All);
            }
        }
    }
}
