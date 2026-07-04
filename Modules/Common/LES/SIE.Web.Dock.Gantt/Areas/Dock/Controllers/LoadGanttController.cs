using Microsoft.AspNetCore.Mvc;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.DockQueues;
using SIE.Dock.DockQueues.Service;
using SIE.Dock.YardMaintains;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Warehouses;
using SIE.Web.Dock.Gantt.Areas.Dock.ViewModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Dock.Gantt.Areas.Dock.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Dock")]
    public class LoadGanttController : Controller
    {
        #region 属性

        #endregion


        #region 方法
        /// <summary>
        /// 甘特图视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Load()
        {
            return View();
        }


        /// <summary>
        /// 加载数据
        /// </summary>        
        public LoadData LoadData(double? yardMaintainId, double? warehouseId, int? state, DateTime beginDate, DateTime endDate, string appointNo, string billNo)
        {
            //前端 null 空白  1可用 0禁用
            LoadData rst = new LoadData();
            //if (state == 1)
            //{
            //    state = null;
            //}
            //else if (state == 2)
            //{
            //    state = 1;
            //}
            //else
            //    state = 0;

            var docks = RT.Service.Resolve<DockMaintainService>().GetDockMaintains(yardMaintainId, warehouseId, state, new EagerLoadOptions().LoadWithViewProperty());
            rst.success = true;
            if (docks.Count == 0)
            { //没月台数据需要回传默认的日历
                return SetDefaultVal();
            }

            if (appointNo.IsNotEmpty())
            {
                appointNo = "%" + appointNo + "%";
            }

            if(billNo.IsNotEmpty())
            {
                billNo = "%" + billNo + "%";
            }

            var appoints = RT.Service.Resolve<DockAppointService>().GetDockAppointByDockIds(docks.Select(f => f.Id).ToList(), beginDate.Date, endDate.Date, appointNo, billNo);
            var dockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueByDockAppointIds(appoints.Select(f=>f.Id).ToList());
            var zoneIds = docks.Select(f => f.YardZoneId).Distinct().ToList();
            var handles = RT.Service.Resolve<DockHandlingService>().GetDockHandlings(zoneIds);
            var serDbNow = RF.Find<DockAppoint>().GetDbTime();
            docks.OrderBy(f => f.YardZoneCode).GroupBy(f => f.YardZoneId).ForEach(a =>
              {

                  var yardZoneName = a.First().YardZoneName;

                  a.OrderBy(b => b.Code).ForEach(f =>
                  {
                      DockData dock = new DockData()
                      {
                          id = f.Id,
                          name = f.Name,
                          groupName = yardZoneName,
                          calendar = "parkWeek" + a.Key,
                          state = f.State == State.Enable ? "ON" : "OFF",
                      };

                      if (f.IsShip && f.IsReceive)
                          dock.type = "all";
                      else if (f.IsShip)
                          dock.type = "out";
                      else if (f.IsReceive)
                          dock.type = "in";

                      rst.resources.rows.Add(dock);
                      var dockAppoints = appoints.Where(p => p.DockMaintainId == f.Id && p.UseHours > 0);
                      if (dockAppoints.Any())
                      {
                          dockAppoints.GroupBy(b => new { b.AppointStartDate, b.AppointEndDate }).ForEach(b =>
                           {
                               DateTime dt = b.Key.AppointStartDate;
                               b.OrderBy(p => p.CreateDate).ForEach(p =>
                               {
                                   
                                   SaveAppointModel apponit = new SaveAppointModel()
                                   {
                                       id = p.Id.ToString(),
                                       name = p.CompanyName,
                                       startDate = dt.ToString("yyyy-MM-dd HH:mm:ss"),
                                       durationUnit = "hour",
                                       duration = p.UseHours,
                                       startDateRange = p.AppointStartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                       endDateRange = p.AppointEndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                       eventColor = p.AppointType == AppointType.Delivery ? "blue" : "yellow",
                                       orderType = (int)p.AppointType,
                                       resourceId = p.DockMaintainId,
                                       billNo = p.BillNo,
                                       orderPlace = yardZoneName,
                                       orderNo = p.No,
                                       carNumber = p.CarNum,
                                       contacts = p.Contacts,
                                       phone = p.ContactNum,
                                       identity = p.IDNumber,
                                       orderDate = p.AppointDate.ToString("yyyy/MM/dd"),
                                       orderTime = p.AppointStartDate.ToString("HH:mm") + "-" + p.AppointEndDate.ToString("HH:mm"),
                                       operateType = "UnChanged",
                                       locked = p.AppointEndDate <= serDbNow || f.State == State.Disable,
                                   };
                                   var queues = dockQueues.FirstOrDefault(x => x.DockAppointNo == p.No);
                                   if (queues != null)
                                   {
                                       apponit.isQueue = true;
                                       apponit.QueueState = queues.QueueState;
                                       //已完工不能修改
                                       if (apponit.QueueState == QueueState.Finish)
                                       {
                                           apponit.locked = true;
                                       }
                                   }
                                   apponit.draggable = true;
                                   apponit.resizable = true;
                                   if (apponit.locked)
                                   {
                                       //locked true draggable false resizable false才是锁定
                                       apponit.draggable = false;
                                       apponit.resizable = false;
                                   }
                                   rst.events.rows.Add(apponit);

                                   dt = dt.AddHours(p.UseHours);
                               });

                           });

                      }

                  });
                  var hands = handles.Where(f => f.YardZoneId == a.Key).ToList();

                  WorkTime workTime = new WorkTime()
                  {
                      id = "parkWeek" + a.Key,
                      name = "parkWeek" + a.Key,
                      unspecifiedTimeIsWorking = false,
                  };

                  hands.OrderBy(f => f.BeginTime).ForEach(f =>
                    {
                        Intervals handitem = new Intervals()
                        {
                            recurrentStartDate = "at " + f.BeginTime,
                            recurrentEndDate = "at " + f.EndTime,
                            isWorking = true,
                        };

                        workTime.intervals.Add(handitem);
                    });


                  rst.calendars.rows.Add(workTime);
              });

            return rst;
        }

        private LoadData SetDefaultVal()
        {
            LoadData rst = new LoadData();
            WorkTime workTime = new WorkTime()
            {
                id = "parkWeekDef",
                name = "parkWeekDef",
                unspecifiedTimeIsWorking = false,
            };
            Intervals handitem = new Intervals()
            {
                recurrentStartDate = "at 08:00",
                recurrentEndDate = "at 23:00",
                isWorking = true,
            };
            workTime.intervals.Add(handitem);
            rst.calendars.rows.Add(workTime);
            rst.success = true;
            return rst;
        }

        /// <summary>
        /// 返回数据类
        /// </summary>
        /// <returns></returns>
        public string GetGanttInfo()
        {
            //LoadData();
            return "";
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        /// <returns></returns>
        public string GetWarehouses()
        {
            List<DropValue> list = new List<DropValue>();
            var whs = RT.Service.Resolve<WarehouseController>().GetWarehouse();
            whs.ForEach(p =>
            {
                DropValue item = new DropValue()
                {
                    value = p.Id,
                    text = p.Name,
                };
                list.Add(item);
            });
            if (list.Count > 0)
                list.Insert(0, new DropValue() { value = null, text = "" });
            return list.ToJsonString();
        }

        /// <summary>
        /// 获取园区信息
        /// </summary>
        /// <returns></returns>
        public string GetParks()
        {
            List<DropValue> list = new List<DropValue>();
            var parks = RF.GetAll<YardMaintain>();
            parks.ForEach(p =>
            {
                DropValue item = new DropValue()
                {
                    value = p.Id,
                    text = p.Name,
                };
                list.Add(item);
            });
            if (list.Count > 0)
                list.Insert(0, new DropValue() { value = null, text = "" });
            return list.ToJsonString();
        }


        /// <summary>
        /// 获取预约类型
        /// </summary>
        /// <returns></returns>
        public string GetOrderTypeData()
        {
            List<DropValue> list = new List<DropValue>();
            foreach (AppointType t in Enum.GetValues(typeof(AppointType)))
            {
                DropValue item = new DropValue()
                {
                    text = t.ToLabel(),
                    value = (int)t
                };

                list.Add(item);
            }
            return list.ToJsonString();
        }

        #endregion

    }
}
