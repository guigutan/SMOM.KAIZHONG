using SIE.MES.Workbench;
using SIE.MES.Workbench.AlertLights;
using SIE.Resources.Employees;

namespace SIE.Wpf.MES.Workbench.AlertLights
{
    /// <summary>
    /// 班组扩展属性试图--安灯预警员工设置
    /// </summary>
    public class WorkGroupExtPrpyAlertLightCallSetViewConfig : WPFViewConfig<WorkGroup>
    {
        /// <summary>
        /// 安灯预警员工设置扩展试图
        /// </summary>
        protected override void ConfigView()
        {
            //View.UseChildrenAsHorizontal();
            View.AssociateChildrenProperty(WorkGroupExtPrpyAlertLightCallSet.EmpCallSettingExtProperty, (obj) =>
            {
                var wkGroup = obj.Parent as WorkGroup;
                var arg = obj as ChildPagingDataArgs;
                return RT.Service.Resolve<AlertLightsController>().GetEmpCallSettings(wkGroup.Id, arg.PagingInfo);
            }).HasLabel("安灯预警员工设置").OrderNo = 5;
        }
    }
}
