using SIE.Equipments.DeviceControls;
using SIE.ObjectModel;

namespace SIE.Web.Equipments.DeviceControls
{
    /// <summary>
    /// 设备控制记录视图配置
    /// </summary>
    class DeviceControlCriteriaViewConfig : WebViewConfig<DeviceControlCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccount).ShowInDetail();
                View.Property(p => p.SourceControl).ShowInDetail();
                View.Property(p => p.IsStop).UseCheckDropDownEditor().ShowInDetail();
                View.Property(p => p.OpearDateTime).UseDateRangeEditor(p => p.DateRangeType = DateRangeType.Today).ShowInDetail();
                View.Property(p => p.IsEffective).UseCheckDropDownEditor().ShowInDetail();

            }
        }

    }
}
