using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.Resources.Employees;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 接单视图配置
    /// </summary>
    public class TakeOrderViewModelViewConfig : WebViewConfig<TakeOrderViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.MaintenancePerson).Readonly();
            View.Property(p => p.EmployeeName).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Employee).FullName;
                    p.XType = "MultiEmployeeComboPopup";
                    p.ValueField = Employee.IdProperty.Name;
                    p.DisplayField = Employee.NameProperty.Name;
                    p.Editable = false;
                    p.Separator = ",";
                });
            View.Property(p => p.DispatchType).Readonly();
            View.Property(p => p.GuaranteeRange).Readonly();
            View.Property(p => p.GuaranteeRangeType).Readonly();
            View.Property(p => p.ExpectCompleteTime);
        }
    }
}
