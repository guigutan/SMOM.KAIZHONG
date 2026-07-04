using SIE.Dock.DockAppoints;
using SIE.Domain;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;

namespace SIE.Web.Dock.DockAppoints
{
    /// <summary>
    /// 月台预约查询实体视图配置
    /// </summary>
    internal class DockAppointCriteriaViewConfig : WebViewConfig<DockAppointCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.BillNo).Show();
                View.Property(p => p.DockMaintainCode).Show();
                View.Property(p => p.DockMaintainName).Show();
                View.Property(p => p.YardZoneId).UseDataSource((o, c, r) =>
                {
                    var criteria = o as DockAppointCriteria;
                    if (criteria == null)
                    {
                        return new EntityList<YardZone>();
                    }

                    return RT.Service.Resolve<YardZoneService>().GetEnableYardZones(r, c);
                }).Show();
                View.Property(p => p.Contacts).Show();
                View.Property(p => p.CarNum).Show();
                View.Property(p => p.AppointType).Show();
                View.Property(p => p.AppointDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}