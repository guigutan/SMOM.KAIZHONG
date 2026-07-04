using SIE.Domain;
using SIE.EMS.Checks.Records;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Records
{
    /// <summary>
    /// 点检记录查询实体视图配置
    /// </summary>
    internal class CheckRecordCriteriaViewConfig : WebViewConfig<CheckRecordCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.CheckPlanNo);
            View.Property(p => p.EquipAccount).UseDataSource((e, c, r) =>
            {
                return RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(c, r);
            });
            View.Property(p => p.Workshop).UseDataSource((e, c, r) =>
            {
                var criteria = e as CheckRecordCriteria;
                if (criteria == null)
                    return new EntityList<Enterprise>();
                var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                return workShopList;
            }).Show(ShowInWhere.All).HasLabel("车间");
            View.Property(p => p.LineId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var lines = RT.Service.Resolve<EnterpriseController>().GetLines(pagingInfo, keyword, null);
                if (lines == null || lines.Count <= 0)
                    return new EntityList<Enterprise>();
                lines.ForEach(p => p.TreePId = null);
                return lines;
            }).UsePagingLookUpEditor().Show(ShowInWhere.All).HasLabel("产线");
            View.Property(p => p.ExeState).DefaultValue(null);
            View.Property(p => p.DepartmentId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                if (departments == null || departments.Count <= 0)
                    return new EntityList<Enterprise>();
                departments.ForEach(p => p.TreePId = null);
                return departments;
            }).UsePagingLookUpEditor().Show(ShowInWhere.All).HasLabel("责任部门");
            View.Property(p => p.CheckEmployee).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(p, k);
            }).Show(ShowInWhere.All);
            View.Property(p => p.ExeResult).DefaultValue(null);
            View.Property(p => p.ConfirmResult).DefaultValue(null);
            View.Property(p => p.PlanCheckDate).UseDateRangeEditor(p =>
            {
                p.DateFormat = "Y/m/d";
                p.DateRangeType = ObjectModel.DateRangeType.Today;
            });
        }
    }
}
