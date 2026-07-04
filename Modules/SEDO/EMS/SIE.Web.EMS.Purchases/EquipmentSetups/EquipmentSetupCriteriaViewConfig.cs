using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试查询界面
    /// </summary>
    internal class EquipmentSetupCriteriaViewConfig : WebViewConfig<EquipmentSetupCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.No);
            View.Property(p => p.EquipAccountId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipAccountSelectController>().GetEquipAccounts(keyword,pagingInfo);
            });
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.SetupStatus);
            View.Property(p => p.PrincipalId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            });
            View.Property(p => p.Overtime);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
