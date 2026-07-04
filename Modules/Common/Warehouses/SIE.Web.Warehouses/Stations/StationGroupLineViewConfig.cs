using SIE.MetaModel.View;
using SIE.Warehouses.Stations;
using SIE.Web.Warehouses.Stations.Commands;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// 站台组明细视图配置
    /// </summary>
    internal class StationGroupLineViewConfig : WebViewConfig<StationGroupLine>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(SelectStationCommand).FullName);
            View.RemoveCommands(WebCommandNames.Edit, WebCommandNames.Copy, WebCommandNames.Save);
            View.Property(p => p.SequenceNo).ShowInList(width: 50);
            View.Property(p => p.Station).ShowInList(width: 180);
            View.Property(p => p.Note);
            View.Property(p => p.StationName);
            View.Property(p => p.StationType);
            View.Property(p => p.StationState);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.StationFloor);
            View.Property(p => p.RoutewayCode);
        }
    }
}