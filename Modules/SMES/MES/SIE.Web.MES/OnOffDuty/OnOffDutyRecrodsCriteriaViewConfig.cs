using SIE.MES.OnOffDuty;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;

namespace SIE.Web.MES.OnOffDuty
{
    internal class OnOffDutyRecrodsCriteriaViewConfig : WebViewConfig<OnOffDutyRecrodsCriteria>
    {
        protected override void ConfigView()
        {
            View.UseClientOrder();

        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Staff).HasLabel("员工号/员工姓名");
            View.Property(p => p.Resource).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(pagingInfo, keyword);
            }).HasLabel("资源");
            View.Property(p => p.Process).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProcessController>().GetProcessList(pagingInfo, keyword);
            }).HasLabel("工序");
            View.Property(p => p.Station).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<StationController>().GetStations(pagingInfo, keyword);
            }).HasLabel("工位");
            View.Property(p => p.OnOffDutyType);
            View.Property(p => p.IsAdditionalRecording);
            View.Property(p => p.OnDutyTime).UseDateRangeEditor(p=>p.DateRangeType= ObjectModel.DateRangeType.All);
            View.Property(p => p.OffDutyTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            View.Property(p => p.CreateTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
