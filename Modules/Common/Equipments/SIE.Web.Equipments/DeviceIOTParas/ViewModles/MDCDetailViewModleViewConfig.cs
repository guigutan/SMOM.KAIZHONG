using SIE.Equipments.DeviceIOTParas.ViewModles;

namespace SIE.Web.Equipments.DeviceIOTParas.ViewModles
{
    /// <summary>
    /// 明细视图配置
    /// </summary>
    public class MDCDetailViewModleViewConfig : WebViewConfig<MDCDetailViewModle>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.EquipmentCode);
            View.Property(p => p.MesDeviceName);
            View.Property(p => p.MesModel);
        }
    }
}
