using SIE.Api;
using SIE.Core;
using SIE.Dock.Common;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Dao;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.DockQueues;
using SIE.Dock.DockQueues.Service;
using SIE.Dock.Interfaces.Datas;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SIE.Dock.Interfaces
{
    /// <summary>
    /// 月台管理接口控制器
    /// </summary>
    public partial class DockManageController : DomainController
    {
        #region 月台公共方法
        /// <summary>
        /// 字符串转化为时间
        /// </summary>
        /// <param name="deliveryTime">交货日期</param>
        /// <returns></returns>
        private DateTime? StringToDateTime(string deliveryTime)
        {
            DateTime? searchDeliveryTime = null;
            if (!string.IsNullOrEmpty(deliveryTime))
            {
                DateTime dt;
                if (DateTime.TryParseExact(deliveryTime, DateTimeFormat.YYYMMdd2, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                {
                    searchDeliveryTime = dt;
                }
                else
                {
                    throw new ValidationException("交货日期格式必须为yyyy-MM-dd".L10N());
                }
            }
            return searchDeliveryTime;
        }

        /// <summary>
        /// 设置月台排队数据
        /// </summary>
        /// <param name="dockQueues">月台排队</param>
        /// <returns>月台排队数据</returns>
        public virtual List<DockQueuesDatas> SetDockQueuesDatas(EntityList<DockQueue> dockQueues)
        {
            List<DockQueuesDatas> datas = new List<DockQueuesDatas>();
            foreach (var item in dockQueues.OrderByDescending(p => p.QueuePriority).ThenBy(p => p.DelayNum).ThenBy(p => p.CreateDate))
            {
                datas.Add(new DockQueuesDatas()
                {
                    Id = item.Id,
                    No = item.No,
                    BillNo = item.BillNo,
                    CompanyName = item.CompanyName,
                    CarNum = item.CarNum,
                    ContactNum = item.ContactNum,
                    QueueState = item.QueueState,
                    QueueStateStr = item.QueueState.ToLabel().L10N(),
                    AppointType = item.AppointType,
                    AppointTypeStr = item.AppointType.ToLabel().L10N(),
                    QueuePriority = item.QueuePriority,
                    QueuePriorityStr = item.QueuePriority.ToLabel().L10N(),
                    DistributionTime = item.DistributionTime,
                    CheckInTime = item.CheckInTime,
                    //MMddHHmm
                    CheckInTimeStr = item.CheckInTime?.ToString(DateTimeFormat.MMddHHmm),
                    CheckOutTime = item.CheckOutTime,
                    CheckOutTimeStr = item.CheckOutTime?.ToString(DateTimeFormat.MMddHHmm),
                    Contacts = item.Contacts,
                    CreateDate = item.CreateDate,
                    MachineTime = DateTime.Now,
                    YardMaintainCode = item.YardZoneCode,
                    YardMaintainId = item.YardZoneId,
                    YardMaintainName = item.YardZoneName,
                    AssignDockName = item.AssignDockName,
                    CreateDateStr = item.CreateDate.ToString(DateTimeFormat.MMddHHmm),
                });
            }

            return datas;
        }

        /// <summary>
        /// 根据经纬度判断当前园区是否显示
        /// </summary>
        /// <param name="zoneLongitude">园片区经度</param>
        /// <param name="zoneLatitude">园片区纬度</param>
        /// <param name="longitude">目的地经度</param>
        /// <param name="latitude">目的地纬度</param>
        /// <param name="Distance">排队取号围栏距离(km)</param>
        /// <returns></returns>
        private bool CheckYardZonesLocations(double zoneLongitude, double zoneLatitude,double longitude, double latitude,double Distance)
        {
            var ActualDistance = MapHelper.GetDistance(zoneLongitude, zoneLatitude, longitude, latitude);
            if (ActualDistance > Distance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 获取等待时间
        /// </summary>
        /// <param name="dockQueues">排队数据</param>
        /// <param name="appointType">排队类型</param>
        /// <param name="dockQueue">当前排队数据</param>
        /// <returns></returns>
        private decimal GetYardZoneAwaitTime(EntityList<DockQueue> dockQueues, AppointType appointType, DockQueue dockQueue)
        {
            //等待时间公式:AgvTime*SeqNum/DockNum
            //根据排队数据所在的排队地点筛选状态为“完工”且排队类型为“送货/提货”的排队数据，计算平均作业时间，假设为数值为AgvTime
            decimal AllWorkTime = (decimal)dockQueues.Where(p => p.QueueState == QueueState.Finish && p.AppointType == appointType).Sum(p => p.JobTime);
            var dockQueuesCount = dockQueues.Count(p => p.QueueState == QueueState.Finish && p.AppointType == appointType);
            decimal AgvTime = 0;
            if (AllWorkTime > 0 && dockQueuesCount > 0)
            {
                AgvTime = AllWorkTime / dockQueuesCount;
            }
            //根据排队数据所在的排队地点筛选状态为“装卸中”的排队数据，并统计这些排队数据的送货月台的个数，“是收送货月台=True，是否发货月台=False”1，“是收送货月台=True，是否发货月台=True”当0.5，假设数值为DockNum
            var assignDockIds = dockQueues.Where(p => p.QueueState == QueueState.Handling && p.AssignDockId != null).Select(p => p.AssignDockId.Value).Distinct().ToList();
            var dockDatas = RT.Service.Resolve<DockMaintainService>().GetDockMaintainsByIds(assignDockIds);
            double dockNum = 0;
            foreach (var item in dockDatas)
            {
                //默认装卸能力都为1
                dockNum = 1;
                //是收送货月台=True，是否发货月台=True 得时候-0.5
                if (item.IsReceive && item.IsShip)
                {
                    dockNum = 0.5;
                }
            }
            //根据排队数据所在的排队地点筛选状态为“等候、装卸中”且排队类型为“送货”且“创建时间小于当前排队数据的创建时间”的排队数据，统计其个数，假设数值为SeqNum
            //如果没有排队数据则比较当前时间
            DateTime dt = dockQueue != null ? dockQueue.CreateDate : DateTime.Now;
            var SeqNum = dockQueues.Count(p => p.AppointType == appointType && p.QueueState == QueueState.Waiting || p.QueueState == QueueState.Handling && p.CreateDate < dt);
            decimal result = 0;
            if (AgvTime > 0 && SeqNum > 0 && dockNum > 0)
            {
                result = AgvTime * SeqNum / (decimal)dockNum;
            }
            return result;
        }

        /// <summary>
        /// 根据园片区ID获取等待时间/排队个数
        /// </summary>
        /// <param name="YardZoneId"></param>
        /// <returns></returns>
        private ZonesAwaitData GetZonesAwaitDataByZoneId(double YardZoneId)
        {
            var result = new ZonesAwaitData();
            //当前排队数据所在园区的送货排队个数：根据排队数据所在的排队地点筛选状态为“等候”且排队类型为“送货”的排队数据，统计其个数
            //当前排队数据所在园区的提货排队个数：根据排队数据所在的排队地点筛选状态为“等候”且排队类型为“提货”的排队数据，统计其个数
            var DockQueueDatas = Query<DockQueue>().Where(p => p.YardZoneId == YardZoneId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            result.ReceiveCount = DockQueueDatas.Count(p => p.AppointType == AppointType.Delivery && p.QueueState == QueueState.Waiting);
            result.ShipCount = DockQueueDatas.Count(p => p.AppointType == AppointType.PickUp && p.QueueState == QueueState.Waiting);
            result.ReceiveAwaitTime = GetYardZoneAwaitTime(DockQueueDatas, AppointType.Delivery, null);
            result.ShipAwaitTime = GetYardZoneAwaitTime(DockQueueDatas, AppointType.PickUp, null);
            return result;
        }
        #endregion

        #region 月台分配
        /// <summary>
        /// 月台分配-获取月台排队列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="queryDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ApiService("月台分配:获取月台排队列表")]
        [return: ApiReturn("获取月台排队列表:List<DockQueuesDatas>")]
        public virtual List<DockQueuesDatas> GetDockQueuesList([ApiParameter("关键字")] string keyword, [ApiParameter("日期")] string queryDate, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            DateTime? dt = StringToDateTime(queryDate);
            var result = new List<DockQueuesDatas>();
            if (!string.IsNullOrEmpty(keyword) && !keyword.EndsWith("%"))
            {
                keyword = keyword + "%";
            }
            var dockQueueList = RT.Service.Resolve<DockQueueService>().GetDockQueuesList(pageNumber, pageSize, keyword, dt);
            result.AddRange(SetDockQueuesDatas(dockQueueList));
            return result;
        }

        /// <summary>
        /// 获取排队地点的月台数据
        /// </summary>
        /// <param name="YardMaintainId">排队地点ID</param>
        /// <param name="Appoint">排队类型</param>
        /// <returns></returns>
        [ApiService("获取排队地点的月台数据")]
        [return: ApiReturn("获取排队地点的月台数据")]
        public virtual List<DockData> GetDockDataByYardMaintain([ApiParameter("排队地点ID")] double YardMaintainId, [ApiParameter("排队类型")] int Appoint)
        {
            var result = new List<DockData>();
            var DockList = RT.Service.Resolve<DockMaintainService>().GetDockMaintainsByZones(YardMaintainId, Appoint);
            foreach (var item in DockList)
            {
                string str = "空闲".L10N();
                var dtls = RT.Service.Resolve<DockQueueService>().GetDockQueueByState(item.Code, QueueState.Handling);
                if (dtls.Count > 0)
                {
                    str = "繁忙".L10N();
                }
                var rst = new DockData()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    State = str,
                };
                result.Add(rst);
            }
            return result;
        }

        /// <summary>
        /// 月台分配
        /// </summary>
        /// <param name="DockQueueId">月台排队ID</param>
        /// <param name="DockCode">月台编码</param>
        /// <param name="IsAtOnceAssign">是否立即分配</param>
        /// <param name="YardZoneId">园片区ID</param>
        /// <param name="YardZoneCode">园片区Code</param>
        [ApiService("月台分配")]
        [return: ApiReturn("月台分配")]
        public virtual void ConfirmAssignDock([ApiParameter("月台排队ID")] double DockQueueId, [ApiParameter("月台编码")] string DockCode, [ApiParameter("是否立即分配")] bool IsAtOnceAssign, [ApiParameter("园片区ID")] double YardZoneId, [ApiParameter("园片区Code")] string YardZoneCode)
        {
            var DockQueueService = RT.Service.Resolve<DockQueueService>();
            var DockMaintainService = RT.Service.Resolve<DockMaintainService>();
            DockMaintainCriteria criteria = new DockMaintainCriteria();
            criteria.Code = DockCode;
            criteria.State = State.Enable;
            var Docks = DockMaintainService.GetDockMaintains(criteria).FirstOrDefault();
            if (Docks == null)
            {
                throw new ValidationException("月台[{0}]不存在或已禁用".L10nFormat(DockCode));
            }
            if (Docks.YardZoneId != YardZoneId)
            {
                throw new ValidationException("月台[{0}]不存在园片区[{1}]中".L10nFormat(DockCode, YardZoneCode));
            }
            DockQueueService.AssignDockData(DockQueueId, Docks.Id, IsAtOnceAssign);
        }
        #endregion

        #region 月台释放
        /// <summary>
        /// 月台释放-获取月台排队列表
        /// </summary>
        /// <param name="dockCode">月台编码</param>
        /// <param name="isAutoRelease">是否自动释放</param>
        /// <returns></returns>
        [ApiService("月台释放:获取月台排队数据")]
        [return: ApiReturn("获取月台排队数据:List<DockQueuesDatas>")]
        public virtual List<DockQueuesDatas> GetReleaseDockQueues([ApiParameter("月台编码")] string dockCode, [ApiParameter("是否自动释放")] bool isAutoRelease)
        {
            List<DockQueuesDatas> datas = new List<DockQueuesDatas>();
            var docks = RT.Service.Resolve<DockMaintainService>().GetEnabelDockMaintains(dockCode, null);
            if (!docks.Any())
            {
                throw new ValidationException("月台编码:[{0}]不存在或已禁用".L10nFormat(dockCode));
            }

            var query = Query<DockQueue>().Exists<DockMaintain>((x, y) => y.Where(p => p.Id == x.AssignDockId && p.Code.Contains(dockCode))).Where(p => p.QueueState == QueueState.Handling);
            var dockQueues = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (!dockQueues.Any())
            {
                return datas;
            }

            if (!isAutoRelease)
            {
                datas.AddRange(SetDockQueuesDatas(dockQueues));
            }
            else
            {
                //执行释放操作
                List<double> dockQueueIds = dockQueues.Select(p => p.Id).ToList();
                datas.AddRange(ReleaseDockQueues(dockQueueIds));
            }

            return datas;
        }

        /// <summary>
        /// 获取月台列表
        /// </summary>
        /// <returns></returns>
        [ApiService("月台释放：获取月台列表数据")]
        [return: ApiReturn("月台列表数据:List<DockData>")]
        public virtual List<DockData> GetDockList([ApiParameter("关键字")] string keyword, [ApiParameter("日期")] string queryDate, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var result = new List<DockData>();
            var query = Query<DockMaintain>().Where(p => p.State == State.Enable);
            if (!string.IsNullOrEmpty(keyword) && !keyword.EndsWith("%"))
            {
                var keywords = keyword + "%";
                query.Where(p => p.Code.Contains(keywords) || p.Name.Contains(keywords));
            }
            if (!queryDate.IsNullOrEmpty())
            {
                DateTime? dt = StringToDateTime(queryDate);
                if (dt.HasValue)
                {
                    query.Where(p => p.CreateDate >= dt && p.CreateDate < dt);
                }
            }
            var DockList = query.ToList(new PagingInfo(pageNumber, pageSize), new EagerLoadOptions().LoadWithViewProperty());
            foreach (var item in DockList)
            {
                var rst = new DockData()
                {
                    Code = item.Code,
                    Name = item.Name,
                    IsReceive = item.IsReceive,
                    IsShip = item.IsShip,
                };
                result.Add(rst);
            }
            return result;
        }

        /// <summary>
        /// 月台释放推迟
        /// </summary>
        /// <param name="dockQueueIds">月台排队ID集合</param>
        [ApiService("月台释放：推迟")]
        [return: ApiReturn("返回月台排队数据:List<DockQueuesDatas>")]
        public virtual List<DockQueuesDatas> DelayDockQueues([ApiParameter("月台排队ID集合")] List<double> dockQueueIds)
        {
            //推迟操作
            RT.Service.Resolve<DockQueueService>().DelayQueue(dockQueueIds);
            var dockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueList(dockQueueIds);

            List<DockQueuesDatas> datas = new List<DockQueuesDatas>();
            datas.AddRange(SetDockQueuesDatas(dockQueues));

            return datas;
        }

        /// <summary>
        /// 月台释放
        /// </summary>
        /// <param name="dockQueueIds">月台排队ID集合</param>
        [ApiService("月台释放：释放操作")]
        [return: ApiReturn("返回月台排队数据:List<DockQueuesDatas>")]
        public virtual List<DockQueuesDatas> ReleaseDockQueues([ApiParameter("月台排队ID集合")] List<double> dockQueueIds)
        {
            //释放其实就是执行签出操作
            var dockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueList(dockQueueIds);
            if (dockQueues.Any(a => a.QueueState != QueueState.Handling))
            {
                throw new ValidationException("排队状态不是[装卸中]，不能推迟".L10N());
            }

            //1）如果排队数据的签到时间栏位有值，执行PC端月台排队模块的签出按钮的验证和数据处理逻辑
            List<double> checkOutIds = dockQueues.Where(p => p.CheckInTime != null).Select(p => p.Id).ToList();
            if (checkOutIds.Any())
            {
                RT.Service.Resolve<DockQueueService>().CheckOutQueue(checkOutIds);
            }

            // 2）如果排队数据的签到时间栏位无值，执行PC端月台排队模块的取消按钮的验证和数据处理逻辑
            List<double> cancelIds = dockQueues.Where(p => p.CheckInTime == null).Select(p => p.Id).ToList();
            if (cancelIds.Any())
            {
                RT.Service.Resolve<DockQueueService>().CancelDockQueueData(cancelIds, string.Empty);
            }

            List<DockQueuesDatas> datas = new List<DockQueuesDatas>();
            var newDockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueList(dockQueueIds);
            datas.AddRange(SetDockQueuesDatas(newDockQueues));

            return datas;
        }
        #endregion

        #region 公众号-预约
        /// <summary>
        /// 公众号-预约：获取历史记录
        /// </summary>
        /// <param name="WeChatID">微信ID/OPenID/UninID</param>
        /// <returns></returns>
        [ApiService("公众号-预约：获取历史记录")]
        [return: ApiReturn("获取历史记录:List<AppointsUserData>")]
        public virtual List<AppointsUserData> GetAppointsUserData([ApiParameter("微信ID/OPenID/UninID")] string WeChatID)
        {
            var datas = new List<AppointsUserData>();
            var DockApps = Query<DockAppoint>().Where(p => p.WeChatID == WeChatID).OrderByDescending(p => p.CreateDate).ToList(new PagingInfo(1, 3), null);
            if (DockApps.Count > 0)
            {
                foreach (var items in DockApps)
                {
                    var item = new AppointsUserData();
                    item.CompanyName = items.CompanyName;
                    item.CarNum = items.CarNum;
                    item.Contacts = items.Contacts;
                    item.ContactNum = items.ContactNum;
                    item.IDNumber = items.IDNumber;
                    //DateTimeFormat.MMddHHmm
                    item.CreateDate = items.CreateDate.ToString("yyyy-MM-dd HH:mm");
                    item.AppointStartDate = items.AppointStartDate.ToString("HH:mm");
                    item.AppointEndDate = items.AppointEndDate.ToString("HH:mm");
                    item.AppointType = items.AppointType;
                    item.AppointTypeStr = items.AppointType.ToLabel();
                    item.AppointDate = items.AppointDate.ToString("yyyy-MM-dd");
                    datas.Add(item);
                }
            }
            return datas;
        }

        /// <summary>
        /// 公众号-预约：获取历史排队记录
        /// </summary>
        /// <param name="WeChatID">微信ID/OPenID/UninID</param>
        /// <returns></returns>
        [ApiService("公众号-预约：获取历史排队记录")]
        [return: ApiReturn("获取历史记录:List<AppointsUserData>")]
        public virtual List<AppointsUserData> GetQueueUserData([ApiParameter("微信ID/OPenID/UninID")] string WeChatID)
        {
            var datas = new List<AppointsUserData>();
            var DockApps = Query<DockQueue>().Where(p => p.WeChatID == WeChatID).OrderByDescending(p => p.CreateDate).ToList(new PagingInfo(1, 3), null);
            if (DockApps.Count > 0)
            {
                foreach (var items in DockApps)
                {
                    var item = new AppointsUserData();
                    item.CompanyName = items.CompanyName;
                    item.CarNum = items.CarNum;
                    item.Contacts = items.Contacts;
                    item.ContactNum = items.ContactNum;
                    item.IDNumber = items.IDNumber;
                    //DateTimeFormat.MMddHHmm
                    item.CreateDate = items.CreateDate.ToString("yyyy-MM-dd HH:mm");
                    item.AppointStartDate = items.AppointStartDate.ToString("HH:mm");
                    item.AppointEndDate = items.AppointEndDate.ToString("HH:mm");
                    item.AppointType = items.AppointType;
                    item.AppointTypeStr = items.AppointType.ToLabel();
                    //item.AppointDate = items.CreateDate.ToString("yyyy-MM-dd");
                    datas.Add(item);
                }
            }
            return datas;
        }

        //public virtual List<T> GetDataByWeChatId<T>(string WechatId,int type)
        //{
        //    List<T> result = new List<T>();
        //    Type type = T as DockAppoint;
        //}

        /// <summary>
        /// 获取园片区数据
        /// </summary>
        /// <returns></returns>
        [ApiService("公众号-预约：获取可用园片区")]
        [return: ApiReturn("获取历史记录:List<AppointsUserData>")]
        public virtual List<YardZoneData> GetEnableZone()
        {
            var datas = new List<YardZoneData>();
            var ZoneList = Query<YardZone>().Where(p => p.State == State.Enable).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var item in ZoneList)
            {
                var zone = new YardZoneData()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                };
                datas.Add(zone);
            }
            return datas;
        }

        /// <summary>
        /// 获取预约时间
        /// </summary>
        /// <returns></returns>
        [ApiService("公众号-预约：获取预约时间")]
        [return: ApiReturn("获取预约时间:List<TimePeriodData>")]
        public virtual List<TimePeriodData> GetTimePeriod([ApiParameter("预约时间")] DateTime AppointDate, [ApiParameter("排队地点ID")] double YardZoneId, [ApiParameter("预约类型")] int AppointType)
        {
            var results = new List<TimePeriodData>();
            var list = RT.Service.Resolve<DockAppointService>().GetSelectAppointDockViewModels(AppointDate, YardZoneId, (AppointType)AppointType, "", null);
            foreach (var items in list)
            {
                var item = new TimePeriodData()
                {
                    StartDate = items.StartDate.ToString(DateTimeFormat.HHmm),
                    EndDate = items.EndDate.ToString(DateTimeFormat.HHmm),
                    AppointTimeDisplay = items.AppointTimeDisplay,
                    AppointUseTime = items.AppointUseTime,
                    MaxRestTime = items.MaxRestTime,
                };
                results.Add(item);
            }
            return results;
        }

        /// <summary>
        /// 获取当前时间和月台最大时间
        /// </summary>
        /// <returns></returns>
        [ApiService("公众号-预约：获取当前时间和月台最大时间")]
        [return: ApiReturn("获取当前时间和月台最大时间")]
        public virtual AppointConfig GetAppointConfig()
        {
            var result = new AppointConfig();
            //DockAppointDao
            var config = RT.Service.Resolve<DockAppointDao>().GetDockAppointNumberRule();
            result.MaxTime = config.MaxAppointTime;
            result.NowDate = DateTime.Now.ToString("yyyy-MM-dd");
            return result;
        }

        /// <summary>
        /// 预约确认
        /// </summary>
        /// <param name="SubmitData">提交的数据</param>
        /// <param name="appId">appId</param>
        /// <param name="secret">微信密码</param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("公众号-预约：确定预约")]
        [return: ApiReturn("确定预约")]
        public virtual void SubmitAppoint([ApiParameter("提交的数据")] DockAppoint SubmitData, [ApiParameter("微信APPID")] string appId, [ApiParameter("微信secret")] string secret)
        {
            if (SubmitData == null)
            {
                throw new ValidationException("不存在提交的数据".L10N());
            }
            SubmitData.No = RT.Service.Resolve<DockAppointService>().GetDockAppointNo();
            //来源类型为微信公众号
            SubmitData.DockSourceType = DockSourceType.WECHAT;
            RT.Service.Resolve<DockAppointService>().SaveDockAppointDatas(SubmitData);
        }

        /// <summary>
        /// 获取预约详情
        /// </summary>
        /// <param name="AppointId">预约ID</param>
        /// <returns></returns>
        [ApiService("公众号-预约：获取预约详情")]
        [return: ApiReturn("获取预约详情")]
        public virtual AppointData GetAppointDtl([ApiParameter("预约ID")] double AppointId)
        {
            var data = Query<DockAppoint>().Where(p => p.Id == AppointId).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
            if (data == null)
            {
                throw new ValidationException("没有找到相应的预约数据".L10N());
            }
            var result = new AppointData()
            {
                DockAppointId = data.Id,
                IDNumber = data.IDNumber,
                No = data.No,
                Contacts = data.Contacts,
                ContactNum = data.ContactNum,
                CompanyName = data.CompanyName,
                AppointType = data.AppointType,
                AppointTypeStr = data.AppointType.ToLabel(),
                CarNum = data.CarNum,
                BillNo = data.BillNo,
                YardZoneId = data.YardZoneId,
                YardZoneName = data.YardZoneName,
                AppointDate = data.AppointDate.ToString(DateTimeFormat.YYYMMdd2),
                AppointStartDate = data.AppointStartDate.ToString(DateTimeFormat.HHmm),
                AppointEndDate = data.AppointEndDate.ToString(DateTimeFormat.HHmm),
                AppointUseTime = data.UseHours,
                CreateDate = data.CreateDate.ToString(DateTimeFormat.YYYMMdd2),
                CancelDate = data.CancelAppointDate?.ToString(DateTimeFormat.YYYMMdd2),
                Remark = data.Remark,
            };
            //当有正常排队数据时，则，状态 = 已排队。有正常排队数据的判断逻辑：根据预约号查询月台排队模块，能查到状态为“等候、装卸中、完工”的排队数据
            var Queue = Query<DockQueue>().Where(p => p.QueueState == QueueState.Waiting || p.QueueState == QueueState.Handling || p.QueueState == QueueState.Finish).Exists<DockAppoint>((x, y) => y.Where(p => p.Id == x.DockAppointId&&p.Id == AppointId)).ToList(null, null).FirstOrDefault();
            if (Queue != null)
            {
                result.ApointStatus = ApointStatus.OnLine;
                result.ApointStatusStr = ApointStatus.OnLine.ToLabel();
            }
            else
            {
                if (data.IsCancelAppoint)
                {
                    result.ApointStatus = ApointStatus.Cancel;
                    result.ApointStatusStr = ApointStatus.Cancel.ToLabel();
                }
                else
                {
                    if (data.AppointEndDate >= DateTime.Now.AddDays(-1))
                    {
                        result.ApointStatus = ApointStatus.Effective;
                        result.ApointStatusStr = ApointStatus.Effective.ToLabel();
                    }
                    else
                    {
                        result.ApointStatus = ApointStatus.Failure;
                        result.ApointStatusStr = ApointStatus.Failure.ToLabel();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 通过经纬度获取园片区数据
        /// </summary>
        /// <param name="Longitude">经度</param>
        /// <param name="Latitude">纬度</param>
        /// <returns></returns>
        [ApiService("公众号-现场排队： 通过经纬度获取园片区数据")]
        [return: ApiReturn(" 通过经纬度获取园片区数据")]
        public virtual List<YardZoneData> GetEnableZoneByLocation([ApiParameter("经度")] double Longitude, [ApiParameter("纬度")] double Latitude)
        {
            var results = new List<YardZoneData>();
            var ZoneLists = RT.Service.Resolve<YardZoneService>().GetEnableYardZones("", null);
            foreach (var Zone in ZoneLists)
            {
                if (Zone.Distance > 0)
                {
                    //如果排队地点的“排队取号围栏距离(km)”不为0的，则，当前位置和排队地点（园片区）的位置的距离小于等于“排队取号围栏距离(km)”。不符合的数据需要过滤掉。
                    var flag = CheckYardZonesLocations(Zone.Longitude,Zone.Latitude,Longitude,Latitude,Zone.Distance);
                    if (flag)
                    {
                        var result = new YardZoneData()
                        {
                            Id = Zone.Id,
                            Code = Zone.Code,
                            Name = Zone.Name,
                        };
                        results.Add(result);
                    }
                }
                else
                {
                    var result = new YardZoneData()
                    {
                        Id = Zone.Id,
                        Code = Zone.Code,
                        Name = Zone.Name,
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DockQueue">排队数据</param>
        /// <param name="appId">appId</param>
        /// <param name="secret">secret</param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("公众号-取号： 现场排队")]
        [return: ApiReturn("现场排队")]
        public virtual void SubmitDockQueue([ApiParameter("排队数据")] DockQueue DockQueue, [ApiParameter("appId")] string appId, [ApiParameter("secret")] string secret)
        {
            if (DockQueue == null)
            {
                throw new ValidationException("提交的数据有误".L10N());
            }
            DockQueue.No = RT.Service.Resolve<DockQueueService>().GetDockQueueNo();
            RT.Service.Resolve<DockQueueService>().SaveDockQueueDatas(DockQueue);
        }

        /// <summary>
        /// 现场预约
        /// </summary>
        /// <param name="appointId">预约ID</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="WechatID">微信ID</param>
        /// <param name="appId">appId</param>
        /// <param name="secret">secret</param>
        [ApiService("公众号-现场预约： 现场预约")]
        [return: ApiReturn("现场预约")]
        public virtual void SubmitAppointQueue([ApiParameter("预约ID")] double appointId, [ApiParameter("经度")] double longitude, [ApiParameter("纬度")] double latitude, [ApiParameter("微信ID")] string WechatID, [ApiParameter("appId")] string appId, [ApiParameter("secret")] string secret)
        {
            var appointData = RT.Service.Resolve<DockAppointService>().GetDockAppointById(appointId);
            if (appointData == null)
            {
                throw new ValidationException("找不到预约数据".L10N());
            }
            if (appointData.Distance > 0)
            {
                var flag = CheckYardZonesLocations(appointData.Longitude, appointData.Latitude, longitude, latitude, appointData.Distance);
                if (!flag)
                {
                    throw new ValidationException("所在位置不在可预约范围内".L10N());
                }
            }
            var submitData = new DockQueue()
            {
                No = RT.Service.Resolve<DockQueueService>().GetDockQueueNo(),
                BillNo = appointData.BillNo,
                YardZoneId = appointData.YardZoneId,
                CompanyName = appointData.CompanyName,
                CarNum = appointData.CarNum,
                Contacts = appointData.Contacts,
                ContactNum = appointData.ContactNum,
                IDNumber = appointData.IDNumber,
                WeChatID = WechatID,
                DockAppointId = appointId,
            };
            RT.Service.Resolve<DockQueueService>().SaveDockQueueDatas(submitData);
        }


        /// <summary>
        /// 获取园片区的送货/提货个数、等候时间
        /// </summary>
        /// <param name="YardZoneId">园片区ID</param>
        /// <returns></returns>
        [ApiService("公众号-现场取号： 获取园片区的送货/提货个数、等候时间")]
        [return: ApiReturn("获取园片区的送货/提货个数、等候时间")]
        public virtual ZonesAwaitData GetZonesAwaitData([ApiParameter("排队地点ID")] double YardZoneId)
        {
            return GetZonesAwaitDataByZoneId(YardZoneId);
        }

        /// <summary>
        /// 通过预约号获取预约数据
        /// </summary>
        /// <param name="AppointCode">预约号</param>
        [ApiService("公众号-预约排队:通过预约号获取预约数据")]
        [return: ApiReturn("通过预约号获取预约数据")]
        public virtual AppointData GetAppointByCode([ApiParameter("预约号")] string AppointCode)
        {
            var result = new AppointData();
            //查询月台预约的数据，能查到唯一的数据，并且该数据的“是否取消预约=False”且“预约结束时间（去掉时分秒）≥当前服务器时间（去掉时分秒）-1天”且在排队模块没有关联“等候、装卸中、完工”状态的排队数据
            var AppointDatas = Query<DockAppoint>().Where(p => !p.IsCancelAppoint && p.AppointEndDate >= DateTime.Now.AddDays(-1) && p.No.Contains(AppointCode)).NotExists<DockQueue>((x, y) => y.Where(p => p.DockAppointId == x.Id && (p.QueueState == QueueState.Waiting || p.QueueState == QueueState.Handling || p.QueueState == QueueState.Finish))).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
            if (AppointDatas == null)
            {
                throw new ValidationException("预约号:[{0}]找不到相应的月台数据或该预约号已排队".L10nFormat(AppointCode));
            }
            result.DockAppointId = AppointDatas.Id;
            result.CarNum = AppointDatas.CarNum;
            result.Contacts = AppointDatas.Contacts;
            result.ContactNum = AppointDatas.ContactNum;
            result.CompanyName = AppointDatas.CompanyName;
            result.AppointType = AppointDatas.AppointType;
            result.AppointTypeStr = AppointDatas.AppointType.ToLabel();
            result.BillNo = AppointDatas.BillNo;
            result.YardZoneId = AppointDatas.YardZoneId;
            result.YardZoneName = AppointDatas.YardZoneName;
            result.AppointDate = AppointDatas.AppointDate.ToString(DateTimeFormat.YYYMMdd2);
            result.AppointStartDate = AppointDatas.AppointStartDate.ToString(DateTimeFormat.HHmm);
            result.AppointEndDate = AppointDatas.AppointEndDate.ToString(DateTimeFormat.HHmm);
            result.AppointUseTime = AppointDatas.UseHours;
            result.Remark = AppointDatas.Remark;
            result.IDNumber = AppointDatas.IDNumber;
            return result;
        }

        /// <summary>
        /// 获取排队数据
        /// </summary>
        /// <param name="id">排队ID</param>
        /// <returns></returns>
        [ApiService("公众号-预约排队:通过预约号获取预约数据")]
        [return: ApiReturn("通过预约号获取预约数据")]
        public virtual DockQueuesDatas GetQueuesDtl([ApiParameter("排队ID")] double id)
        {
            var queueDatas = Query<DockQueue>().Where(p=>p.Id == id).ToList(null,new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
            if (queueDatas == null)
            {
                throw new ValidationException("没有找到排队数据!".L10N());
            }
            var result = new DockQueuesDatas()
            {
                Id = queueDatas.Id,
                No = queueDatas.No,
                BillNo = queueDatas.BillNo,
                CompanyName = queueDatas.CompanyName,
                CarNum = queueDatas.CarNum,
                ContactNum = queueDatas.ContactNum,
                QueueState = queueDatas.QueueState,
                QueueStateStr = queueDatas.QueueState.ToLabel(),
                AppointType = queueDatas.AppointType,
                AppointTypeStr = queueDatas.AppointType.ToLabel(),
                QueuePriority = queueDatas.QueuePriority,
                QueuePriorityStr = queueDatas.QueuePriority.ToLabel(),
                DistributionTime = queueDatas.DistributionTime,
                CheckInTime = queueDatas.CheckInTime,
                //DateTimeFormat
                CheckInTimeStr = queueDatas.CheckInTime?.ToString("MM-dd HH:mm"),
                CheckOutTime = queueDatas.CheckOutTime,
                CheckOutTimeStr = queueDatas.CheckOutTime?.ToString("MM-dd HH:mm"),
                Contacts = queueDatas.Contacts,
                CreateDate = queueDatas.CreateDate,
                MachineTime = DateTime.Now,
                YardMaintainCode = queueDatas.YardZoneCode,
                YardMaintainId = queueDatas.YardZoneId,
                YardMaintainName = queueDatas.YardZoneName,
                AssignDockName = queueDatas.AssignDockName,
                CreateDateStr = queueDatas.CreateDate.ToString("MM-dd HH:mm"),
                IDNumber = queueDatas.IDNumber,
                Remark = queueDatas.Remark,
                AppointNo = queueDatas.DockAppointNo,
            };
            return result;
        }

        /// <summary>
        /// 取消排队/预约
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="type">0-取消排队 1-取消预约</param>
        /// <param name="appId">appId</param>
        /// <param name="secret">secret</param>
        /// <param name="weChatID">weChatID</param>
        [ApiService("公众号-取消排队:取消排队")]
        [return: ApiReturn("取消排队")]
        public virtual void CancelQueue([ApiParameter("排队ID")] List<double> ids, [ApiParameter("类型")] int type, [ApiParameter("appId")] string appId, [ApiParameter("secret")] string secret, [ApiParameter("微信ID/openId")] string weChatID)
        {
            //微信端没有取消原因
            const string reason = "";
            if (type == 0)
            {
                RT.Service.Resolve<DockQueueService>().CancelDockQueueData(ids, reason);
            }
            else
            {
                RT.Service.Resolve<DockAppointService>().CancelDockAppointData(ids, reason);
            }

        }
        #endregion

        #region Web请求
        /// <summary>
        /// postweb请求
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                ////byte[] byteArray = Encoding.UTF8.GetBytes(paramData);
                HttpWebRequest webReq = WebRequest.Create(postUrl) as HttpWebRequest;
                webReq.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
            return ret;
        }
        #endregion
    }
}
