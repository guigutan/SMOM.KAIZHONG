using SIE.Core.Common.Service;
using SIE.Dock.Common;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockQueues.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Shipment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Dock.DockQueues.Service
{
    /// <summary>
    /// 月台排队Service
    /// </summary>
    public partial class DockQueueService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台排队数据访问
        /// </summary>
        private readonly DockQueueDao _dockQueueDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DockQueueService(DockQueueDao dockQueueDao)
        {
            _dockQueueDao = dockQueueDao;
        }
        #endregion

        /// <summary>
        /// 获取月台排队数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueues(DockQueueCriteria criteria)
        {
            return _dockQueueDao.GetDockQueues(criteria);
        }

        /// <summary>
        /// 获取月台排队的排队号
        /// </summary>
        /// <returns>月台排队号</returns>
        public virtual string GetDockQueueNo()
        {
            return _dockQueueDao.GetDockQueueNo();
        }

        /// <summary>
        /// 获取月台排队数据
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueList(List<double> ids)
        {
            return _dockQueueDao.GetDockQueueList(ids);
        }

        /// <summary>
        /// 根据月台查询月台排队数据
        /// </summary>
        /// <param name="dockIds">月台ID集合</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueByDockIds(List<double> dockIds)
        {
            return _dockQueueDao.GetDockQueueByDockIds(dockIds);
        }

        /// <summary>
        /// 获取根据月台预约ID查询月台排队数据
        /// </summary>
        /// <param name="dockAppointIds">月台预约ID集合</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueByDockAppointIds(List<double> dockAppointIds)
        {
            return _dockQueueDao.GetDockQueueByDockAppointIds(dockAppointIds);
        }

        /// <summary>
        /// 获取月台排队用于调度
        /// </summary>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueForJob()
        {
            return _dockQueueDao.GetDockQueueForJob();
        }

        /// <summary>
        /// 获取对应状态的月台排队数据
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueByStates(QueueState state)
        {
            return _dockQueueDao.GetDockQueueByStates(state);
        }

        /// <summary>
        /// 验证月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        private void ValidDockQueueData(DockQueue data)
        {
            //单据号必填
            if (data.BillNo.IsNullOrEmpty())
            {
                throw new ValidationException("单据号不能为空".L10N());
            }

            if (data.YardZoneId <= 0)
            {
                throw new ValidationException("排队地点不能为空".L10N());
            }

            if (data.CarNum.IsNullOrEmpty())
            {
                throw new ValidationException("车牌号不能为空".L10N());
            }

            if (data.Contacts.IsNullOrEmpty())
            {
                throw new ValidationException("联系人不能为空".L10N());
            }

            if (data.ContactNum.IsNullOrEmpty())
            {
                throw new ValidationException("联系电话不能为空".L10N());
            }

            if (data.IDNumber.IsNullOrEmpty())
            {
                throw new ValidationException("身份证号不能为空".L10N());
            }

            if (data.TakeNoWay == TakeNoWay.Appoint && !data.DockAppointId.HasValue)
            {
                throw new ValidationException("当取号方式是[预约取号]时,预约号不能为空".L10N());
            }
        }

        /// <summary>
        /// 保存月台预约数据
        /// </summary>
        /// <param name="data">月台预约数据</param>
        public virtual void SaveDockQueueDatas(DockQueue data)
        {
            var WxSenderController = RT.Service.Resolve<WechatSenderController>();
            //验证数据
            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                ValidDockQueueData(data);
                DockQueue dockQueue = new DockQueue();
                dockQueue.QueueState = QueueState.Waiting;
                dockQueue.No = data.No;
                dockQueue.TakeNoWay = data.TakeNoWay;
                dockQueue.AppointType = data.AppointType;
                dockQueue.BillNo = data.BillNo;
                dockQueue.YardZoneId = data.YardZoneId;
                dockQueue.CompanyName = data.CompanyName;
                dockQueue.CarNum = data.CarNum;
                dockQueue.Contacts = data.Contacts;
                dockQueue.ContactNum = data.ContactNum;
                dockQueue.IDNumber = data.IDNumber;
                dockQueue.WeChatID = data.WeChatID;
                dockQueue.Remark = data.Remark;
                if (data.DockAppointId.HasValue)
                {
                    dockQueue.DockAppointId = data.DockAppointId.Value;
                }

                int priorityType = 0;
                if (dockQueue.AppointType == DockAppoints.AppointType.Delivery)
                {
                    var asn = RT.Service.Resolve<IReceipt>().GetAsnDataByAsnNos(new List<string> { data.BillNo }).FirstOrDefault();
                    priorityType = asn?.PriorityType ?? 0;

                }
                else
                {
                    var so = RT.Service.Resolve<IShippingOrder>().GetSoDatas(new List<string> { data.BillNo }).FirstOrDefault();
                    priorityType = so?.PriorityType ?? 0;
                }

                switch (priorityType)
                {
                    case 0:
                        dockQueue.QueuePriority = QueuePriority.Normal;
                        break;
                    case 1:
                        dockQueue.QueuePriority = QueuePriority.Urgent;
                        break;
                    case 2:
                        dockQueue.QueuePriority = QueuePriority.Surpass;
                        break;
                    default:
                        dockQueue.QueuePriority = QueuePriority.Normal;
                        break;
                }
                RF.Save(dockQueue);
                if (!dockQueue.WeChatID.IsNullOrEmpty())
                {
                    var sender = WxSenderController.GetWxSenderDataByQueue(dockQueue, dockQueue.WeChatID);
                    //微信ID/openID不为空的情况下推送微信信息
                    RT.Service.Resolve<WechatSenderController>().SendMessage(sender);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 取消月台排队数据
        /// </summary>
        /// <param name="dockQueueIds">月台排队ID集合</param>
        /// <param name="reasonDes">原因描述</param>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual void CancelDockQueueData(List<double> dockQueueIds, string reasonDes)
        {
            var WxSenderController = RT.Service.Resolve<WechatSenderController>();
            var dockQueues = GetDockQueueList(dockQueueIds);
            if (dockQueues.Any(t => t.QueueState != QueueState.Waiting && t.QueueState != QueueState.Handling))
            {
                throw new ValidationException("月台排队状态不是[等候、装卸中]".L10N());
            }

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                dockQueueIds.SplitDataExecute(tmpIds =>
                {
                    DB.Update<DockQueue>().Set(p => p.QueueState, QueueState.Cancel)
                          .Set(p => p.CancelEmployeeId, RT.IdentityId)
                          .Set(p => p.CancelTime, DateTime.Now)
                          .Set(p => p.CancelReason, reasonDes)
                          .Where(p => tmpIds.Contains(p.Id))
                          .Execute();
                });
                //前面已经把排队状态都更新为取消了
                //TODO:推送微信信息(暂未实现)
                dockQueues.ForEach(p => {
                    p.QueueState = QueueState.Cancel;
                    if (!p.WeChatID.IsNullOrEmpty())
                    {
                        var sender = WxSenderController.GetWxSenderDataByQueue(p, p.WeChatID);
                        //微信ID/openID不为空的情况下推送微信信息
                        RT.Service.Resolve<WechatSenderController>().SendMessage(sender);
                    }
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 分配月台
        /// </summary>
        /// <param name="dockQueueId">月台排队ID</param>
        /// <param name="dockId">月台ID</param>
        /// <param name="isAtOnceAssign">是否立即分配</param>
        public virtual void AssignDockData(double dockQueueId, double dockId, bool isAtOnceAssign)
        {
            var WxSenderController = RT.Service.Resolve<WechatSenderController>();
            var dockQueue = RF.GetById<DockQueue>(dockQueueId,new EagerLoadOptions().LoadWithViewProperty());
            var dock = RF.GetById<DockMaintain>(dockId);
            //验证数据
            if (dockQueue == null)
            {
                throw new ValidationException("选择的月台排队数据不存在".L10N());
            }
            if (dockQueue.AppointType == AppointType.Delivery && !dock.IsReceive || dockQueue.AppointType == AppointType.PickUp && !dock.IsShip)
            {
                throw new ValidationException("月台排队号:[{0}]排队类型:[{1}]与月台：[{2}]的月台类型不一致".L10nFormat(dockQueue.No, dockQueue.AppointType.ToLabel(),dock.Code));
            }
            //所选数据的状态为“等候、装卸中”
            if (dockQueue.QueueState != QueueState.Waiting && dockQueue.QueueState != QueueState.Handling)
            {
                throw new ValidationException("月台排队号:[{0}]的状态:[{1}]不是[等候、装卸中]".L10nFormat(dockQueue.No, dockQueue.QueueState.ToLabel()));
            }

            var dockQueues = GetDockQueueByDockIds(new List<double> { dockId });

            //如果所选数据的状态为“装卸中”,不是立即分配的，根据选择的月台编码查询月台排队的数据，查询不到状态为“装卸中”的数据
            if (dockQueue.QueueState == QueueState.Handling && !isAtOnceAssign && dockQueues.Any(t => t.QueueState == QueueState.Handling))
            {
                throw new ValidationException("月台排队号:[{0}]的状态:[装卸中]时，不是立即分配的，当前选择的月台:[{1}]在月台排队数据不能存在状态为[装卸中]的数据".L10nFormat(dockQueue.No, dock.Code));
            }

            //如果所选数据的状态为“装卸中”，是立即分配的,则，选择的月台和所选数据的分配月台不能一样
            if (dockQueue.QueueState == QueueState.Handling && isAtOnceAssign && dockQueue.AssignDockId == dockId)
            {
                throw new ValidationException("月台排队号:[{0}]的状态:[装卸中]时，是立即分配的，当前选择的月台:[{1}]和所选数据的分配月台不能一样".L10nFormat(dockQueue.No, dock.Code));
            }

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                //如果是立即分配的，先将装卸中的排队数据处理
                if (isAtOnceAssign && dockQueues.Any(t => t.QueueState == QueueState.Handling))
                {
                    UpdateDockQueueData(dockQueues);
                }

                //先将选择的数据更新
                var query = DB.Update<DockQueue>();
                query.Set(p => p.LastDistriTime, p => p.DistributionTime)
                     .Set(p => p.DistributionTime, DateTime.Now)
                     .Set(p => p.AssignDockId, dockId);

                if (isAtOnceAssign)
                {
                    query.Set(p => p.QueueState, QueueState.Handling);
                }

                var config = _dockQueueDao.GetDockQueueNumberRule();
                if (!dockQueue.CheckInTime.HasValue && config?.AutoCheckIn == true)
                {
                    query.Set(p => p.CheckInTime, DateTime.Now);
                }

                query.Where(p => p.Id == dockQueueId).Execute();

                //TODO:推送微信信息(暂未实现)
                if (!dockQueue.WeChatID.IsNullOrEmpty())
                {
                    //如果微信号不为空的时候才进行推送
                    var sender = WxSenderController.GetWxSenderDataByAssign(dockQueue, dockQueue.WeChatID, dock.Name);
                    WxSenderController.SendMessage(sender);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dockQueues">月台排队数据</param>
        private void UpdateDockQueueData(EntityList<DockQueue> dockQueues)
        {
            //更新签到时间没有的数据,没有签到更新为取消，有签到的更新为完工
            List<double> signInDockIds = dockQueues.Where(x => x.QueueState == QueueState.Handling && x.CheckInTime == null).Select(p => p.Id).ToList();
            signInDockIds.SplitDataExecute(tmpIds =>
            {
                DB.Update<DockQueue>().Set(p => p.CheckInTime, DateTime.Now)
                                      .Set(p => p.CheckOutTime, DateTime.Now)
                                      .Set(p => p.QueueState, QueueState.Cancel)
                                      .Where(p => tmpIds.Contains(p.Id))
                                      .Execute();
            });

            //更新签到时间有就更新签出时间数据
            List<double> signOutDockIds = dockQueues.Where(x => x.QueueState == QueueState.Handling && x.CheckInTime != null).Select(p => p.Id).ToList();
            signOutDockIds.SplitDataExecute(tmpIds =>
            {
                DB.Update<DockQueue>().Set(p => p.CheckOutTime, DateTime.Now)
                                      .Set(p => p.QueueState, QueueState.Finish)
                                      .Where(p => tmpIds.Contains(p.Id))
                                      .Execute();
            });

            //更新作业时间
            var signOutDockQueues = GetDockQueueList(signOutDockIds);
            UpdateDockQueueJobTimes(signOutDockQueues);
        }

        /// <summary>
        /// 更新作业时间
        /// </summary>
        /// <param name="dockQueues"></param>
        private void UpdateDockQueueJobTimes(EntityList<DockQueue> dockQueues)
        {
            dockQueues.ForEach(p =>
            {
                if (p.CheckOutTime.HasValue && p.CheckInTime.HasValue)
                {
                    p.JobTime = (decimal)Math.Round((p.CheckOutTime.Value - p.CheckInTime.Value).TotalMinutes, 3);
                }
            });

            RF.Save(dockQueues);
        }

        /// <summary>
        /// 月台排队推迟
        /// </summary>
        /// <param name="delayIdList">需要推迟的排队号Id</param>
        public virtual void DelayQueue(List<double> delayIdList)
        {
            _dockQueueDao.DelayQueue(delayIdList);
        }

        /// <summary>
        /// 月台排队签到
        /// </summary>
        /// <param name="inIdList">需要签到的排队号Id</param>
        public virtual void CheckInQueue(List<double> inIdList)
        {
            _dockQueueDao.CheckInQueue(inIdList);
        }

        /// <summary>
        /// 月台排队签出
        /// </summary>
        /// <param name="outIdList">需要签出的排队号Id</param>
        public virtual void CheckOutQueue(List<double> outIdList)
        {
            _dockQueueDao.CheckOutQueue(outIdList);
        }

        /// <summary>
        /// 月台排队升级
        /// </summary>
        /// <param name="upQueueId">需要升级的排队号Id</param>
        public virtual void UpDockQueueData(double upQueueId)
        {
            _dockQueueDao.UpDockQueueData(upQueueId);
        }

        /// <summary>
        /// 月台排队降级
        /// </summary>
        /// <param name="downQueueId">需要降级的排队号Id</param>
        public virtual void DownDockQueueData(double downQueueId)
        {
            _dockQueueDao.DownDockQueueData(downQueueId);
        }

        /// <summary>
        /// 获取月台排队数据
        /// </summary>
        /// <param name="PageNum">页号</param>
        /// <param name="PageSize">页码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public virtual EntityList<DockQueue> GetDockQueuesList(int PageNum, int PageSize, string keyword, DateTime? date)
        {
            return _dockQueueDao.GetDockQueuesList(PageNum, PageSize, keyword, date);
        }

        /// <summary>
        /// 获取月台数据
        /// </summary>
        /// <param name="Code">月台编码</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<DockQueue> GetDockQueueByState(string Code, QueueState state)
        {
            return _dockQueueDao.GetDockQueueByState(Code, state);
        }
    }
}
