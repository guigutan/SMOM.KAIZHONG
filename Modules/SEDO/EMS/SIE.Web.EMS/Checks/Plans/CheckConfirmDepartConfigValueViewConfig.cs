using SIE.EMS.Checks.Configs;
using SIE.EMS.Common;

namespace SIE.Web.EMS.Checks.Plans
{

    /// <summary>
    /// 视图配置
    /// </summary>
    public class CheckConfirmDepartConfigValueViewConfig : WebViewConfig<CheckConfirmDepartConfigValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.DepartmentNames).UsePagingLookUpGridPopupEditor(p =>
            {
                p.Model = typeof(DepartmentSelect).FullName;
                p.LinkField = CheckConfirmDepartConfigValue.DepartmentIdsProperty.Name;
                p.DisplayField = DepartmentSelect.NameProperty.Name;
                //部门多选控件
                p.XType = "emsMultiDepartmentComboPopup";
                p.Editable = false;
                p.Separator = ",";
            }).Show();
            View.Property(p => p.IsMarkScore).Show();
        }
    }
}
