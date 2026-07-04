using SIE.Dock.Datas;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Web.Dock.DockMap.Datas;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Dock.DockMap.DataQueryer
{
    /// <summary>
    /// 月台预约图
    /// </summary>
    public class DockMapDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 刷新命令获取月台数据
        /// </summary>
        /// <param name="searchParams">查询参数</param>       
        public List<ZoneData> RefreshGetDocks(DockSearchParams searchParams)
        {
            List<ZoneData> rst = new List<ZoneData>();
            //var docks = RT.Service.Resolve<DockMaintainService>().GetDockMaintains(searchParams.YardZoneId, searchParams.WarehouseId, searchParams.State, new EagerLoadOptions().LoadWithViewProperty());

            //var appoints = RT.Service.Resolve<DockAppointService>().GetDockAppointByDockIds(docks.Select(f => f.Id).ToList(), searchParams.BeginDate, searchParams.EndDate);
            //var zoneIds = docks.Select(f => f.YardZoneId).ToList();
            //var handles = RT.Service.Resolve<DockHandlingService>().GetDockHandlings(zoneIds);
            //docks.GroupBy(f => f.YardZoneId).ForEach(a =>
            //{
            //    ZoneData zoneData = new ZoneData()
            //    {
            //        YardZoneCode = a.First().YardZoneCode,
            //        YardZoneName = a.First().YardZoneName,
            //    };
            //    a.OrderBy(b => b.Code).ForEach(f =>
            //    {
            //        DockData dd = new DockData()
            //        {
            //            Code = f.Code,
            //            Id = f.Id,
            //            IsReceive = f.IsReceive,
            //            IsShip = f.IsShip,
            //            State = f.State,
            //        };
            //        appoints.Where(p => p.DockMaintainId == f.Id).OrderBy(p=>p.CreateDate).ForEach(p =>
            //        {                                             
            //            dd.DockAppoints.Add(p);
            //        });
            //        zoneData.DockDatas.Add(dd);

            //    });
            //    var hands = handles.Where(f => f.YardZoneId == a.Key).ToList();
            //    zoneData.DockHandlings.AddRange(hands);
            //    rst.Add(zoneData);
            //});
            return rst;
        }
        
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dockAppoints">预约数据</param>
        /// <returns>返回数据</returns>
        public DockMapSaveReturnData SaveAppointDatas(List<DockAppoint> dockAppoints)
        {
            return RT.Service.Resolve<DockAppointService>().SaveAppoints(dockAppoints);
        }

        /// <summary>
        /// 检查时间
        /// </summary>
        /// <param name="dockAppoint"></param>
        public bool NewCheckAppointDatas(DockAppoint dockAppoint)
        {
            RT.Service.Resolve<DockAppointService>().NewCheckAppointDatas(dockAppoint);
            return true;
        }       
    } 
}
