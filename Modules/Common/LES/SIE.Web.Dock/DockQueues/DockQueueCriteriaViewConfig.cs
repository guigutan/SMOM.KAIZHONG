using SIE.Dock.DockQueues;
using SIE.Dock.YardZones.Service;
using SIE.Dock.YardZones;
using SIE.Domain;

namespace SIE.Web.Dock.DockQueues
{
    /// <summary>
    /// 月台排队查询视图配置
    /// </summary>
    internal class DockQueueCriteriaViewConfig : WebViewConfig<DockQueueCriteria>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.QueueState).Show();
                View.Property(p => p.AppointType).Show();
                View.Property(p => p.YardZoneId).UseDataSource((o, c, r) =>
                {
                    var criteria  = o as DockQueueCriteria;
                    if (criteria == null)
                    {
                        return new EntityList<YardZone>();
                    }

                    return RT.Service.Resolve<YardZoneService>().GetEnableYardZones(r, c);
                }).Show();
                View.Property(p => p.BillNo).Show();
                View.Property(p => p.DockAppointNo).Show();
                View.Property(p => p.DockMaintainCode).Show();
                View.Property(p => p.Contacts).Show();
                View.Property(p => p.CarNum).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}
