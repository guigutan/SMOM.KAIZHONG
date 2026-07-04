using SIE.Core;
using SIE.Core.Common.Service;
using SIE.Dock.Common;
using SIE.Dock.Datas;
using SIE.Dock.DockAppoints.Dao;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.DockQueues;
using SIE.Dock.ViewModels;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Dock.DockAppoints.Service
{
    /// <summary>
    /// 月台预约Service
    /// </summary>
    public partial class DockAppointService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台维护数据访问
        /// </summary>
        private readonly DockAppointDao _dockAppointDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DockAppointService(DockAppointDao dockAppointDao)
        {
            _dockAppointDao = dockAppointDao;
        }
        #endregion

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppoints(DockAppointCriteria criteria)
        {
            return _dockAppointDao.GetDockAppoints(criteria);
        }

        /// <summary>
        /// 获取月台预约单号
        /// </summary>
        /// <returns>月台预约单号</returns>
        public virtual string GetDockAppointNo()
        {
            return _dockAppointDao.GetDockAppointNo();
        }

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppointList(List<double> ids, EagerLoadOptions elo = null)
        {
            return _dockAppointDao.GetDockAppointList(ids, elo);
        }

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="dockIds">月台维护ID集合</param>
        /// <param name="appointDate">大于等于预约日期</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppointByDockIds(List<double> dockIds, DateTime? appointDate = null)
        {
            return _dockAppointDao.GetDockAppointByDockIds(dockIds, appointDate);
        }

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="dockIds">月台维护ID集合</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="appointNo">预约号</param>
        /// <param name="billNo">单据号</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppointByDockIds(List<double> dockIds, DateTime? beginDate, DateTime? endDate, string appointNo = "", string billNo = "")
        {
            return _dockAppointDao.GetDockAppointByDockIds(dockIds, beginDate, endDate, appointNo, billNo);
        }

        /// <summary>
        /// 获取预约数据
        /// </summary>
        /// <param name="keyword">查询关键子</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>预约数据</returns>
        public virtual EntityList<DockAppoint> GetSelectDockAppoints(string keyword, PagingInfo pagingInfo)
        {
            return _dockAppointDao.GetSelectDockAppoints(keyword, pagingInfo);
        }

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="yardZoneId">园片区ID</param>
        /// <param name="appointType">预约类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="dockList">园区下的月台</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetAppointDateDockAppoints(double yardZoneId, AppointType? appointType, DateTime? beginTime, DateTime? endTime, EntityList<DockMaintain> dockList)
        {
            return _dockAppointDao.GetAppointDateDockAppoints(yardZoneId, appointType, beginTime, endTime, dockList);
        }

        /// <summary>
        /// 获取月台数据
        /// </summary>
        /// <param name="appointDate">预约时间</param>
        /// <param name="yardZoneId">预约地点ID</param>
        /// <param name="appointType">预约类型</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<SelectAppointDockViewModel> GetSelectAppointDockViewModels(DateTime appointDate, double yardZoneId, AppointType appointType, string keyword, PagingInfo pagingInfo)
        {
            EntityList<SelectAppointDockViewModel> selectDocks = new EntityList<SelectAppointDockViewModel>();

            bool? isRec = null;
            bool? isShip = null;
            if (appointType == AppointType.Delivery)
            {
                isRec = true;
            }
            else
            {
                isShip = true;
            }

            //3）根据预约地点、预约类型筛选月台维护的数据，初步获得可以预约的月台数据，并计算这些月台在各时间段的可用时间.
            var dockList = RT.Service.Resolve<DockMaintainService>().GetDockMaintainByParkIds(yardZoneId, keyword, isRec, isShip, null);

            List<DockHandleData> dockHandlDatas = GetDockHandleDatas(appointDate, yardZoneId, appointType, dockList);
            if (!dockHandlDatas.Any())
            {
                return selectDocks;
            }

            foreach (var dockHandle in dockHandlDatas.Where(p => p.RemainUseTime > 0).OrderBy(p => p.BeginTime))
            {
                var selectDock = new SelectAppointDockViewModel
                {
                    StartDate = dockHandle.BeginTime,
                    EndDate = dockHandle.EndTime,
                    AppointTimeDisplay = dockHandle.BeginTime.ToString(DateTimeFormat.HHmm) + "~" + dockHandle.EndTime.ToString(DateTimeFormat.HHmm) + "  已预约:" + dockHandle.HasAppointCount,
                    AppointUseTime = dockHandle.UseHours,
                    MaxRestTime = dockHandle.MaxRestTime
                };

                selectDocks.Add(selectDock);
            }

            selectDocks.SetTotalCount(selectDocks.Count);

            return selectDocks;
        }

        /// <summary>
        /// 获取预约月台处理数据
        /// </summary>
        /// <param name="appointDate">预约日期</param>
        /// <param name="yardZoneId">园片区维护ID</param>
        /// <param name="appointType">预约类型</param>
        /// <param name="dockList">月台数据</param>
        /// <returns>预约月台处理数据</returns>
        private List<DockHandleData> GetDockHandleDatas(DateTime appointDate, double yardZoneId, AppointType appointType, EntityList<DockMaintain> dockList, bool isCheck = false)
        {
            List<DockHandleData> dockHandlDatas = new List<DockHandleData>();

            // 1）根据预约地点找到园片区维护模块下的“月台装卸能力”数据，该数据即初始的时间段
            var yardZone = RF.GetById<YardZone>(yardZoneId, new EagerLoadOptions().LoadWithViewProperty());
            if (yardZone == null || !yardZone.DockHandlingList.Any())
            {
                return dockHandlDatas;
            }

            DateTime curDt = DateTime.Now;
            //预约时间是当天过滤园片区开始时间小于当前时间的时间段
            if (appointDate.Date == curDt.Date)
            {
                var dtHands = yardZone.DockHandlingList.Where(t => DateTime.Parse(curDt.Date.ToString(DateTimeFormat.YYYMMdd1) +" " + t.BeginTime) > curDt).ToList();
                dockHandlDatas.AddRange(dtHands.Select(p => new DockHandleData
                {
                    BeginTime = DateTime.Parse("2008-01-01 " + p.BeginTime),
                    EndTime = DateTime.Parse("2008-01-01 " + p.EndTime),
                    ShipAppoNum = p.ShipAppoNum,
                    ReceiveAppoNum = p.ReceiveAppoNum,
                }).ToList());
            }
            else
            {
                //不是当天获取原片区的全部的时间段
                var dtHands = yardZone.DockHandlingList.ToList();
                dockHandlDatas.AddRange(dtHands.Select(p => new DockHandleData
                {
                    BeginTime = DateTime.Parse("2008-01-01 " + p.BeginTime),
                    EndTime = DateTime.Parse("2008-01-01 " + p.EndTime),
                    ShipAppoNum = p.ShipAppoNum,
                    ReceiveAppoNum = p.ReceiveAppoNum,
                }).ToList());
            }
            //根据类型过滤可预约的时间段
            if (appointType == AppointType.Delivery)
            {
                dockHandlDatas = dockHandlDatas.Where(p => p.ShipAppoNum > 0).ToList();
            }
            else
            {
                dockHandlDatas = dockHandlDatas.Where(p => p.ReceiveAppoNum > 0).ToList();
            }

            var config = _dockAppointDao.GetDockAppointNumberRule();
            //获取配置项的最大预约占用
            double maxAppointTime = (double)(config.MaxAppointTime ?? 0);

            //4）根据上一步找到的可以预约的月台、预约日期、时间段找到月台预约模块的“是否取消预约=False”的数据，统计找到的数据个数，统计找到数据的预计占用时间的和。举例： 
            foreach (var dockHandle in dockHandlDatas.OrderBy(p => p.BeginTime))
            {
                var beginTime = DateTime.Parse(appointDate.Date.ToString(DateTimeFormat.YYYMMdd2) + " " + dockHandle.BeginTime.ToString(DateTimeFormat.HHmm));
                var endTime = DateTime.Parse(appointDate.ToString(DateTimeFormat.YYYMMdd2) + " " + dockHandle.EndTime.ToString(DateTimeFormat.HHmm));
                AppointType? appoint = appointType;
                if (isCheck)
                    appoint = null;
                //根据类型获取在时间段内的预约单据信息
                var dockAppoints = GetAppointDateDockAppoints(yardZoneId, appoint, beginTime, endTime, dockList);
                dockHandle.UseTime = (dockHandle.EndTime - dockHandle.BeginTime).TotalHours * dockList.Count;
                dockHandle.RemainUseTime = dockHandle.UseTime - dockAppoints.Sum(t => t.UseHours);
                dockHandle.HasAppointCount = dockAppoints.Count;
                if (dockHandle.RemainUseTime <= 0)
                {
                    continue;
                }
                var workTime = (dockHandle.EndTime - dockHandle.BeginTime).TotalHours;
                //获取月台的最大可用时间
                var dockMaxRestTimeDatas = GetDockMaxRestTime(dockList, dockAppoints, workTime);
                double maxRestTime = dockMaxRestTimeDatas.Max(t => t.MaxRestTime) > 0 ? dockMaxRestTimeDatas.Max(t => t.MaxRestTime) : workTime;
                dockHandle.MaxRestTime = maxRestTime;
                dockHandle.DockId = dockMaxRestTimeDatas.FirstOrDefault(t => t.MaxRestTime == maxRestTime)?.DockId ?? dockList.FirstOrDefault()?.Id ?? 0;
                double agvTime = 0;
                if (appointType == AppointType.Delivery)
                {
                    var tmpUseHours = Math.Floor((decimal)(workTime * dockList.Count * 2 / dockHandle.ShipAppoNum));
                    agvTime = (double)tmpUseHours / 2;
                }
                else
                {
                    var tmpUseHours = Math.Floor((decimal)(workTime * dockList.Count * 2 / dockHandle.ReceiveAppoNum));
                    agvTime = (double)tmpUseHours / 2;
                }
                dockHandle.UseHours = Math.Min(maxAppointTime, Math.Min(maxRestTime, agvTime));
            }

            return dockHandlDatas;
        }

        /// <summary>
        /// 获取最大可用时间
        /// </summary>
        /// <param name="dockList">月台数据</param>
        /// <param name="dockAppoints">月台预约数据</param>
        /// <param name="workTime">工作时段</param>
        /// <returns>最大可用时间</returns>
        private List<DockMaxRestTimeData> GetDockMaxRestTime(EntityList<DockMaintain> dockList, EntityList<DockAppoint> dockAppoints, double workTime)
        {
            List<DockMaxRestTimeData> dockMaxes = new List<DockMaxRestTimeData>();
            if (!dockList.Any())
            {
                return dockMaxes;
            }

            foreach (var dock in dockList)
            {
                double tmpMaxRestTime = workTime - dockAppoints.Where(p => p.DockMaintainId == dock.Id).Sum(t => t.UseHours);
                dockMaxes.Add(new DockMaxRestTimeData
                {
                    MaxRestTime = tmpMaxRestTime,
                    DockId = dock.Id,
                });
            }

            return dockMaxes;
        }

        /// <summary>
        /// 验证月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        private void ValidDockAppointData(DockAppoint data)
        {
            //预约日期大于等于当天日期
            if (data.AppointDate.Date < DateTime.Now.Date)
            {
                throw new ValidationException("预约日期:[{0}]必须大于等于当天日期".L10nFormat(data.AppointDate));
            }

            var config = _dockAppointDao.GetDockAppointNumberRule();

            //预计占用时间≤配置项的最大预约时间
            if (config != null && config.MaxAppointTime.HasValue && data.UseHours > (double)(config.MaxAppointTime.Value))
            {
                throw new ValidationException("预计占用时间:[{0}]必须小于等于配置项的最大预约时间:[{1}]".L10nFormat(data.UseHours, config?.MaxAppointTime ?? 0));
            }

            //////如果预约日期为当前服务器日期，则，预约结束时间-预计占用时间＞当前服务器时间
            ////if (data.AppointDate.Date == DateTime.Now.Date && (DateTime.Parse(data.AppointEndDate.ToString(DateTimeFormat.HHmm)).AddHours(-data.UseHours) <= DateTime.Now))
            ////{
            ////    throw new ValidationException("预约结束时间-预计占用时间:[{0}]必须大于当前服务器时间:[{1}]".L10nFormat(DateTime.Parse(data.AppointEndDate.ToString(DateTimeFormat.HHmm)).AddHours(-data.UseHours), DateTime.Now));
            ////}

            bool? isRec = null;
            bool? isShip = null;
            if (data.AppointType == AppointType.Delivery)
            {
                isRec = true;
            }
            else
            {
                isShip = true;
            }

            //3）根据预约地点、预约类型筛选月台维护的数据，初步获得可以预约的月台数据，并计算这些月台在各时间段的可用时间.
            var dockList = RT.Service.Resolve<DockMaintainService>().GetDockMaintainByParkIds(data.YardZoneId, string.Empty, isRec, isShip, null);

            //月台的最大剩余可用时间≥预计占用时间
            List<DockHandleData> dockHandlDatas = GetDockHandleDatas(data.AppointDate, data.YardZoneId, data.AppointType, dockList, true);
            var maxDockDatas = dockHandlDatas.Where(t => t.BeginTime.ToString(DateTimeFormat.HHmm) == data.AppointStartDate.ToString(DateTimeFormat.HHmm) && t.EndTime.ToString(DateTimeFormat.HHmm) == data.AppointEndDate.ToString(DateTimeFormat.HHmm)).ToList();
            double maxRestTime = maxDockDatas.Any() ? maxDockDatas.Max(t => t.MaxRestTime) : 0;
            if (maxRestTime < data.UseHours)
            {
                throw new ValidationException("月台的最大剩余可用时间:[{0}]小于预计占用时间:[{1}]".L10nFormat(maxRestTime, data.UseHours));
            }

            var dockHandData = maxDockDatas.FirstOrDefault(t => t.MaxRestTime == maxRestTime);

            data.DockMaintainId = dockHandData.DockId <= 0 ? dockList.FirstOrDefault()?.Id ?? 0 : dockHandData.DockId;
        }

        /// <summary>
        /// 保存月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        public virtual void SaveDockAppointDatas(DockAppoint data)
        {
            var WxSenderController = RT.Service.Resolve<WechatSenderController>();
            //验证数据
            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                ValidDockAppointData(data);
                DockAppoint dockAppoint = new DockAppoint();
                dockAppoint.No = data.No;
                dockAppoint.AppointType = data.AppointType;
                dockAppoint.BillNo = data.BillNo;
                dockAppoint.YardZoneId = data.YardZoneId;
                dockAppoint.CompanyName = data.CompanyName;
                dockAppoint.CarNum = data.CarNum;
                dockAppoint.Contacts = data.Contacts;
                dockAppoint.ContactNum = data.ContactNum;
                dockAppoint.IDNumber = data.IDNumber;
                dockAppoint.AppointDate = data.AppointDate.Date;
                dockAppoint.DockSourceType = data.DockSourceType;
                dockAppoint.AppointStartDate = DateTime.Parse(data.AppointDate.ToString("yyyy-MM-dd ") + data.AppointStartDate.ToString(DateTimeFormat.HHmm));
                dockAppoint.AppointEndDate = DateTime.Parse(data.AppointDate.ToString("yyyy-MM-dd ") + data.AppointEndDate.ToString(DateTimeFormat.HHmm));
                dockAppoint.DockMaintainId = data.DockMaintainId;
                dockAppoint.UseHours = data.UseHours;
                dockAppoint.WeChatID = data.WeChatID;
                dockAppoint.Remark = data.Remark;
                RF.Save(dockAppoint);
                if (!data.WeChatID.IsNullOrEmpty())
                {

                    var sender = WxSenderController.GetWxSenderDataByAppoint(dockAppoint, dockAppoint.WeChatID);
                    //微信ID/openID不为空的情况下推送微信信息
                    RT.Service.Resolve<WechatSenderController>().SendMessage(sender);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        public virtual void UpdateDockAppointDatas(DockAppoint data)
        {
            DB.Update<DockAppoint>().Set(p => p.BillNo, data.BillNo)
                                    .Set(p => p.CompanyName, data.CompanyName)
                                    .Set(p => p.CarNum, data.CarNum)
                                    .Set(p => p.Contacts, data.Contacts)
                                    .Set(p => p.ContactNum, data.ContactNum)
                                    .Set(p => p.IDNumber, data.IDNumber)
                                    .Where(p => p.Id == data.Id).Execute();
        }

        /// <summary>
        /// 取消预约数据
        /// </summary>
        /// <param name="dockAppointIds">月台预约ID集合</param>
        /// <param name="reasonDes">原因描述</param>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual void CancelDockAppointData(List<double> dockAppointIds, string reasonDes)
        {
            var WxSenderController = RT.Service.Resolve<WechatSenderController>();
            var dockAppoints = GetDockAppointList(dockAppointIds);
            if (dockAppoints.Any(t => t.IsCancelAppoint || t.AppointEndDate <= DateTime.Now))
            {
                throw new ValidationException("月台预约已取消，或预约结束时间小于当前时间".L10N());
            }
            List<double?> dockAppointIdsNull = new List<double?>();
            dockAppointIds.ForEach(a =>
            {
                dockAppointIdsNull.Add(a);
            });
            if (DB.Query<DockQueue>().Where(f => f.DockAppointId > 0 && dockAppointIdsNull.Contains(f.DockAppointId) && f.QueueState != QueueState.Cancel).Count() > 0)
                throw new ValidationException("不能取消在月台排队不是取消状态的预约号".L10N());


            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                dockAppointIds.SplitDataExecute(tmpIds =>
                {
                    DB.Update<DockAppoint>().Set(p => p.IsCancelAppoint, true)
                          .Set(p => p.CancelAppointBy, RT.IdentityId)
                          .Set(p => p.CancelAppointDate, DateTime.Now)
                          .Set(p => p.CancelReason, reasonDes)
                          .Where(p => tmpIds.Contains(p.Id))
                          .Execute();
                });
                dockAppoints.ForEach(p =>
                {
                    p.IsCancelAppoint = true;
                    if (!p.WeChatID.IsNullOrEmpty())
                    {
                        var sender = WxSenderController.GetWxSenderDataByAppoint(p, p.WeChatID);
                        //微信ID/openID不为空的情况下推送微信信息
                        RT.Service.Resolve<WechatSenderController>().SendMessage(sender);
                    }
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新取消预约月台数据来自ASN
        /// </summary>
        /// <param name="dockNo"></param>
        /// <param name="reasonDes"></param>
        public virtual void CancelDockAppointByAsn(string dockNo, string reasonDes)
        {
            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                DB.Update<DockAppoint>().Set(p => p.IsCancelAppoint, true)
                      .Set(p => p.CancelAppointBy, RT.IdentityId)
                      .Set(p => p.CancelAppointDate, DateTime.Now)
                      .Set(p => p.CancelReason, reasonDes)
                      .Where(p => p.No == dockNo && !p.IsCancelAppoint)
                      .Execute();

                tran.Complete();
            }
        }

        /// <summary>
        /// 获取预约数据
        /// </summary>
        /// <param name="Id">预约ID</param>
        /// <returns></returns>
        public virtual DockAppoint GetDockAppointById(double Id)
        {
            return _dockAppointDao.GetDockAppointById(Id);
        }
    }
}