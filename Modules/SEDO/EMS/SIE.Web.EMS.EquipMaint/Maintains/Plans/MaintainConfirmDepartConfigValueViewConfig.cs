using SIE.EMS.Common;
using SIE.EMS.Maintains.Configs;
using SIE.Resources.Enterprises;

namespace SIE.Web.EMS.Maintains.Plans
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class MaintainConfirmDepartConfigValueViewConfig : WebViewConfig<MaintainConfirmDepartConfigValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.DepartmentNames).UsePagingLookUpGridPopupEditor(p =>
            {
                p.Model = typeof(DepartmentSelect).FullName;
                p.LinkField = MaintainConfirmDepartConfigValue.DepartmentIdsProperty.Name;
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
