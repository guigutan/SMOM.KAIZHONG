using SIE.EMS.EquipLends;
using SIE.EMS.Equipments;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;

namespace SIE.Web.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理查询实体视图配置
    /// </summary>
    public class EquipLendManageCriteriaViewConfig : WebViewConfig<EquipLendManageCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipCode).Show();
            View.Property(p => p.FixCode).Show();
            View.Property(p => p.RFID).Show();
            View.Property(p => p.EquipModel).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipController>().GetEquipModels(p, null, k);
            }).Show();
            View.Property(p => p.State).Show();
            View.Property(p => p.LendObject).Show();
            View.Property(p => p.LendEnterprise).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EnterpriseController>().GetDepartmentsNoTree(p, k);
            }).Show();
            View.Property(p => p.LendEmployee).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetJobEmployees(p, k);
            }).Show();
            View.Property(p => p.LendDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
        }
    }
}
