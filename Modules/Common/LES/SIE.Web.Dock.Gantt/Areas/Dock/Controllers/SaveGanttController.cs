using Microsoft.AspNetCore.Mvc;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Dock.Gantt.Areas.Dock.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Dock.Gantt.Areas.Dock.Controllers
{
    /// <summary>
    /// 保存
    /// </summary>
    [Area("Dock")]
    public class SaveGanttController : Controller
    {

        /// <summary>
        /// 甘特图视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Load()
        {
            return View();
        }

        /// <summary>
        /// 保存WEB甘特图传过来的数据
        /// </summary>
        /// <param name="contents">Json数据</param>      
        /// <returns>1:成功 0：没有需要保存的数据 -1：失败 -2："保存失败，数据冲突[{0}]存在删除与添加数据"</returns>
        public string SaveOrderGanttData(string contents)
        {
            var appoints = contents.ToJsonObject<List<SaveAppointModel>>();

            if (appoints.Any(f => f.resourceId == 0))
                throw new ValidationException("没有传入月台Id,请检查http请求resouceId".L10N());

            if (appoints.Any(f => f.operateType != "delete" && !RT.Service.Resolve<DockMaintainService>().PraseDate(f.startDate)))
            {
                return new SaveReturnMsg() { Message = "不能操作开始时间小于等于当前时间的数据或时间格式错误", Success = false }.ToJsonString();
            }
            //身份证号码为必填项 会出现重复的数据 地图表找不到原因 在后端过滤掉身份证号码为空的数据
            appoints = appoints.Where(f=>!f.identity.IsNullOrEmpty()).ToList();
            //数据校验
            if (appoints.Any(f => f.operateType != "delete" && !RT.Service.Resolve<DockMaintainService>().PraseDate(f.endDate)))
            {
                return new SaveReturnMsg() { Message = "不能操作结束时间小于等于当前时间的数据或时间格式错误", Success = false }.ToJsonString();
            }
            if (appoints.Any(f => f.operateType == "delete" && !RT.Service.Resolve<DockMaintainService>().PraseDate(f.endDateRange)))
            {
                return new SaveReturnMsg() { Message = "不能删除范围时段小于等于当前时间的数据或时间格式错误", Success = false }.ToJsonString();
            }

            List<DockAppoint> dockAppoints = new List<DockAppoint>();
            var dockIds = appoints.Where(f => f.resourceId > 0).Select(f => f.resourceId).Distinct().ToList();
            var docks = RT.Service.Resolve<DockMaintainService>().GetDockMaintains(dockIds);
            var zoneIds = docks.Select(f => f.YardZoneId).Distinct().ToList();
            var handles = RT.Service.Resolve<DockHandlingService>().GetDockHandlings(zoneIds);
            appoints.Where(f => f.id.Contains("generated")).ForEach(f => f.operateType = "");
            var rst = HandleDeleteData(appoints, dockAppoints);
            if (docks.Any(f=>f.State ==State.Disable))
            {
               return new SaveReturnMsg() { Message = "已禁用的月台无法新增预约", Success = false }.ToJsonString();
            }
            if (!rst.Success)
            {
                return rst.ToJsonString();
            }

            rst = HandleEditData(appoints, dockAppoints, docks, handles);
            if (!rst.Success)
            {
                return rst.ToJsonString();
            }
            rst = HandleAddData(appoints, dockAppoints, docks, handles);
            if (!rst.Success)
            {
                return rst.ToJsonString();
            }
            try
            {
                var result = RT.Service.Resolve<DockAppointService>().SaveAppoints(dockAppoints);
                if (result.ErrorMsg.IsNotEmpty())
                {
                    rst.Success = false;
                    rst.Message = result.ErrorMsg;
                }
                else
                {
                    rst.Success = true;
                    rst.Message = "操作成功！".L10N();
                }
            }
            catch (Exception ex)
            {
                rst.Success = false;
                var exb = ex.GetBaseException();
                if (exb is ValidationException)
                    rst.Message = exb.Message;
                else
                    rst.Message = ex.Message;
            }
            return rst.ToJsonString();
        }


        /// <summary>
        /// 处理删除的数据
        /// </summary>       
        private SaveReturnMsg HandleDeleteData(List<SaveAppointModel> appoints, List<DockAppoint> dockAppoints)
        {
            SaveReturnMsg rst = new SaveReturnMsg();
            var deleteAppoint = appoints.Where(f => f.operateType == "delete").ToList();
            for (int i = 0; i < deleteAppoint.Count; i++)
            {
                var p = deleteAppoint[i];
                double id = 0;
                DockAppoint dockAppoint = new DockAppoint();
                if (!double.TryParse(p.id, out id))
                {
                    rst.Message = "操作类型是删除的，Id的值类型不正确[预约号{0}]".L10nFormat(p.orderNo);
                    rst.Success = false;
                    return rst;
                }
                else
                {
                    dockAppoint.Id = id;
                    dockAppoint.PersistenceStatus = PersistenceStatus.Deleted;
                    dockAppoints.Add(dockAppoint);
                }

            }
            rst.Success = true;
            return rst;
        }

        /// <summary>
        /// 处理修改的数据
        /// </summary>       
        private SaveReturnMsg HandleEditData(List<SaveAppointModel> appoints, List<DockAppoint> dockAppoints, EntityList<DockMaintain> docks, EntityList<DockHandling> hands)
        {
            SaveReturnMsg rst = new SaveReturnMsg();
            var opAppoints = appoints.Where(f => f.operateType == "modify").ToList();
            List<double> edits = new List<double>();
            for (int i = 0; i < opAppoints.Count; i++)
            {
                var p = opAppoints[i];
                double id = 0;
                if (!double.TryParse(p.id, out id))
                {
                    rst.Message = "操作类型是修改的，Id的值类型不正确[预约号{0}]".L10nFormat(p.orderNo);
                    rst.Success = false;
                    return rst;
                }
                else
                {
                    edits.Add(id);
                }
            }
            var editAppoints = RT.Service.Resolve<DockAppointService>().GetDockAppointList(edits);
            for (int i = 0; i < opAppoints.Count; i++)
            {
                var p = opAppoints[i];
                double id = double.Parse(p.id);
                var dock = docks.FirstOrDefault(a => a.Id == p.resourceId);

                DateTime start = DateTime.Parse(p.startDate);
                DateTime end = DateTime.Parse(p.endDate);
                var editAppoint = editAppoints.FirstOrDefault(a => a.Id == id);
                editAppoint.CompanyName = p.name;
                //不在原时间范围内，暂定园区不能变

                var hand = hands.FirstOrDefault(f => f.YardZoneId == dock.YardZoneId
                && DateTime.Parse(f.BeginTime).TimeOfDay <= start.TimeOfDay && DateTime.Parse(f.EndTime).TimeOfDay >= end.TimeOfDay);
                if (hand == null)
                {
                    rst.Message = "当前园区没有符合的时间段[开始时间{0}]".L10nFormat(start.ToString());
                    rst.Success = false;
                    return rst;
                }
                else
                {
                    //有在时段，但是占用时间超长，后面统一处理   
                    editAppoint.AppointStartDate = start.Date + DateTime.Parse(hand.BeginTime).TimeOfDay;
                    editAppoint.AppointEndDate = end.Date + DateTime.Parse(hand.EndTime).TimeOfDay;
                    editAppoint.AppointDate = start.Date;
                }

                editAppoint.UseHours = p.duration;
                editAppoint.DockMaintainId = p.resourceId;
                editAppoint.YardZoneId = dock.YardZoneId;
                editAppoint.BillNo = p.billNo;
                editAppoint.CarNum = p.carNumber;
                editAppoint.Contacts = p.contacts;
                editAppoint.ContactNum = p.phone;
                editAppoint.IDNumber = p.identity;
                editAppoint.AppointType = p.orderType == 0 ? AppointType.Delivery : AppointType.PickUp;
                if (editAppoint.IsDirty)
                    dockAppoints.Add(editAppoint);
            }

            rst.Success = true;
            return rst;
        }

        /// <summary>
        /// 处理添加的数据
        /// </summary>      
        private SaveReturnMsg HandleAddData(List<SaveAppointModel> appoints, List<DockAppoint> dockAppoints, EntityList<DockMaintain> docks, EntityList<DockHandling> hands)
        {
            SaveReturnMsg rst = new SaveReturnMsg();
            var addAppoints = appoints.Where(f => f.operateType != "delete" && f.operateType != "modify").ToList();
            for (int i = 0; i < addAppoints.Count; i++)
            {
                var p = addAppoints[i];
                DateTime start = DateTime.Parse(p.startDate);
                DateTime end = DateTime.Parse(p.endDate);
                if (p.orderType != (int)AppointType.PickUp && p.orderType != (int)AppointType.Delivery)
                {
                    rst.Message = "预约类型不正确".L10N();
                    rst.Success = false;
                    return rst;
                }
                var dock = docks.FirstOrDefault(a => a.Id == p.resourceId);
                var useHours = p.duration;
                if (dock == null)
                {
                    rst.Message = "后端找不到月台[前端传入月台的Id参数名：resourceId=【{0}】]".L10nFormat(p.resourceId);
                    rst.Success = false;
                    return rst;
                }
                while (useHours > 0)
                {
                    double curUseHour = 0;//本次用掉的小时数
                    DockAppoint appoint = new DockAppoint();
                    appoint.CompanyName = p.name;
                    if (dock.IsShip && dock.IsReceive)
                    {
                        appoint.AppointType = (AppointType)p.orderType;
                    }
                    else if (dock.IsShip)
                    {
                        appoint.AppointType = AppointType.PickUp;
                    }
                    else
                    {
                        appoint.AppointType = AppointType.Delivery;
                    }

                    var hand = hands.FirstOrDefault(f => f.YardZoneId == dock.YardZoneId
                   && DateTime.Parse(f.BeginTime).TimeOfDay <= start.TimeOfDay && DateTime.Parse(f.EndTime).TimeOfDay >= end.TimeOfDay);
                    if (hand == null)
                    {
                        rst.Message = "当前园区没有符合的时间段[开始时间{0}]".L10nFormat(start.ToString());
                        rst.Success = false;
                        return rst;
                    }
                    else
                    {
                        //有在时段，但是占用时间超长，后面统一处理                    
                        appoint.AppointStartDate = start.Date + DateTime.Parse(hand.BeginTime).TimeOfDay;
                        appoint.AppointEndDate = end.Date + DateTime.Parse(hand.EndTime).TimeOfDay;
                        appoint.AppointDate = start.Date;
                        var sethours = (DateTime.Parse(hand.EndTime).TimeOfDay - DateTime.Parse(hand.BeginTime).TimeOfDay).TotalHours;
                        if (sethours > useHours)
                        {
                            curUseHour = useHours;
                            useHours = 0;
                        }
                        else
                        {
                            curUseHour = sethours;
                            useHours -= sethours;
                            start = appoint.AppointEndDate;
                        }
                    }

                    appoint.UseHours = curUseHour;
                    appoint.DockMaintainId = p.resourceId;
                    appoint.YardZoneId = dock.YardZoneId;
                    appoint.BillNo = p.billNo;
                    appoint.CarNum = p.carNumber;
                    appoint.Contacts = p.contacts;
                    appoint.ContactNum = p.phone;
                    appoint.IDNumber = p.identity;
                    appoint.PersistenceStatus = PersistenceStatus.New;
                    appoint.No = RT.Service.Resolve<DockAppointService>().GetDockAppointNo();
                    dockAppoints.Add(appoint);
                }
            }

            rst.Success = true;
            return rst;
        }
    }


}
