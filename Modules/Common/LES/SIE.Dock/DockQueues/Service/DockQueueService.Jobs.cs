using SIE.Core;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.DockQueues.Configs;
using SIE.Dock.DockRunMts.Service;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Dock.DockQueues.Service
{
    /// <summary>
    /// 用于调度Job的服务
    /// </summary>
    public partial class DockQueueService
    {
        /// <summary>
        /// 执行分配月台排队数据
        /// </summary>
        /// <param name="dockQueues">月台排队数据</param>
        public virtual void ExecuteAssignDockQueues(EntityList<DockQueue> dockQueues)
        {
            DateTime dt = DateTime.Now;
            var config = _dockQueueDao.GetDockQueueNumberRule();

            //对筛选到的数据判定其预约优先系数
            var dockAppointQueueIds = dockQueues.Where(p => p.TakeNoWay == TakeNoWay.Appoint && dt >= p.AppointStartDate && dt <= p.AppointEndDate).Select(p => p.Id).ToList();
            dockQueues.Where(p => dockAppointQueueIds.Contains(p.Id)).ForEach(p => p.PrioritySeq = 0);
            dockQueues.Where(p => !dockAppointQueueIds.Contains(p.Id)).ForEach(p => p.PrioritySeq = 1);

            //筛选可用的且运行中的月台数据
            var dockList = RT.Service.Resolve<DockMaintainService>().GetEnabelDockMaintains(null, null);

            //过滤掉在月台排队模块查到关联“装卸中”状态排队数据的月台
            var handlingQueues = GetDockQueueByStates(QueueState.Handling);
            List<double> assignDockIds = handlingQueues.Where(p => p.AssignDockId != null).Select(p => p.AssignDockId.Value).ToList();

            //获取供给月台池数据
            List<DockMaintain> docks = dockList.Where(p => !assignDockIds.Contains(p.Id)).ToList();
            List<double> dockIds = docks.Select(p => p.Id).ToList();
            var dockRuns = RT.Service.Resolve<DockRunMtService>().GetDockRunMtByDockIds(dockIds);
            List<double> dockRunDockIds = dockRuns.Select(p => p.DockMaintainId).Distinct().ToList();
            List<double> hasRunDockIds = docks.Where(p => !dockRunDockIds.Contains(p.Id)).Select(p => p.Id).ToList();

            var dtHm = dt.ToString(DateTimeFormat.HHmm);
            if (dockRuns.Any())
            {
                foreach (var dockRun in dockRuns)
                {
                    var excepTimes = dockRun.ExcepTimeList.Where(p => dt >= p.BeginTime && dt <= p.EndTime).ToList();
                    if (excepTimes.Any())
                    {
                        if (excepTimes.All(p => p.ExcepType == DockRunMts.ExcepType.Enable))
                        {
                            hasRunDockIds.Add(dockRun.DockMaintainId);
                        }
                        else if (excepTimes.All(p => p.ExcepType == DockRunMts.ExcepType.Disable))
                        {
                            hasRunDockIds.Remove(dockRun.DockMaintainId);
                        }
                    }
                    else
                    {
                        if (!dockRun.WorkTimeList.Any())
                        {
                            hasRunDockIds.Add(dockRun.DockMaintainId);
                        }
                        else if (dockRun.WorkTimeList.Any(p => DateTime.Parse(dtHm) >= DateTime.Parse(p.BeginTime.ToString(DateTimeFormat.HHmm)) && DateTime.Parse(dtHm) <= DateTime.Parse(p.EndTime.ToString(DateTimeFormat.HHmm))))
                        {
                            hasRunDockIds.Add(dockRun.DockMaintainId);
                        }
                        else
                        {
                            hasRunDockIds.Remove(dockRun.DockMaintainId);
                        }
                    }
                }
            }
            else
            {
                hasRunDockIds.AddRange(dockIds);
            }

            hasRunDockIds = hasRunDockIds.Distinct().ToList();
            docks = docks.Where(p => hasRunDockIds.Contains(p.Id)).ToList();

            //是否有月台数据
            if (!docks.Any())
            {
                return;
            }

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                //对排队数据按顺序从上到下逐个处理
                foreach (var dockQueue in dockQueues.OrderByDescending(t => t.QueuePriority).ThenBy(t => t.DelayNum).ThenBy(t => t.PrioritySeq).ThenBy(t => t.CreateDate))
                {
                    if (!docks.Any())
                    {
                        break;
                    }

                    //处理分配月台
                    bool isClear = HandleAssignDock(dockQueue, docks, config);

                    if (!isClear)
                    {
                        continue;
                    }

                    docks.Remove(dockQueue.AssignDock);
                }

                RF.Save(dockQueues);

                ////TODO:推送微信信息(暂未实现)

                tran.Complete();
            }
        }

        /// <summary>
        /// 处理分配月台
        /// </summary>
        /// <param name="dockQueue"></param>
        /// <param name="docks"></param>
        /// <param name="config"></param>
        private bool HandleAssignDock(DockQueue dockQueue, List<DockMaintain> docks, DockQueueNumberConfigValue config)
        {
            bool isClear = true;

            //排队数据是否维护了分配月台
            if (!dockQueue.AssignDockId.HasValue)
            {
                DockMaintain assignDock = null;

                //对排队数据按顺序分配月台
                List<DockMaintain> assignDocks = docks.Where(p => p.YardZoneId == dockQueue.YardZoneId).ToList();

                if (dockQueue.AppointType == DockAppoints.AppointType.Delivery)
                {
                    assignDock = assignDocks.Where(p => p.IsReceive).OrderBy(t => t.RecPriority).FirstOrDefault();
                }
                else
                {
                    assignDock = assignDocks.Where(p => p.IsShip).OrderBy(t => t.ShipPriority).FirstOrDefault();
                }

                if (assignDock == null)
                {
                    isClear = false;
                    return isClear;
                }

                //更新排队信息
                dockQueue.AssignDockId = assignDock.Id;
                dockQueue.AssignDock = assignDock;
                dockQueue.LastDistriTime = dockQueue.DistributionTime;
                dockQueue.DistributionTime = DateTime.Now;
                dockQueue.QueueState = QueueState.Handling;
                if (config.AutoCheckIn)
                {
                    dockQueue.CheckInTime = DateTime.Now;
                }
            }
            else
            {
                //分配月台是否存在于供给月台池中
                if (docks.Any(t => t.Id == dockQueue.AssignDockId.Value))
                {
                    //更新排队信息
                    dockQueue.QueueState = QueueState.Handling;
                    if (config.AutoCheckIn)
                    {
                        dockQueue.CheckInTime = DateTime.Now;
                    }
                }
            }

            return isClear;
        }

        /// <summary>
        /// 执行推迟和签出调度
        /// </summary>
        /// <param name="dt">当前时间</param>
        public virtual void ExecuteDelayAndCheckOutData(DateTime dt)
        {
            var dockQueues = DB.Query<DockQueue>().Where(p => p.QueueState == QueueState.Handling).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var config = _dockQueueDao.GetDockQueueNumberRule();

            //1）筛选所有状态为“装卸中”且“当前服务器时间-签到时间＞配置项的作业超时强制签出时间(H)”的数据。这部分数据就是作业超时的数据。
            var jobOverTimeQueues = dockQueues.Where(p => p.CheckInTime != null && (dt - p.CheckInTime.Value).TotalHours > config.CheckOutTimeOut).ToList();

            //2）筛选所有状态为“装卸中”且“签到时间为空”且“当前服务器时间-分配时间＞配置项的签到超时强制推迟时间(Min)”的数据。这部分数据就是签到超时的数据
            var checkInOverTimeQueues = dockQueues.Where(p => p.CheckInTime == null && p.DistributionTime != null && (dt - p.DistributionTime.Value).TotalMinutes > config.CheckOutDelay).ToList();

            //3.是否有排队数据
            if (!jobOverTimeQueues.Any() && !checkInOverTimeQueues.Any())
            {
                return;
            }

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                //更新作业超时的排队数据
                List<double> jobOverTimeQueueIds = jobOverTimeQueues.Select(p => p.Id).ToList();
                DateTime curDt = DateTime.Now;
                jobOverTimeQueueIds.SplitDataExecute(tmpIds =>
                {
                    DB.Update<DockQueue>().Set(p => p.QueueState, QueueState.Finish)
                      .Set(p => p.CheckOutTime, curDt)
                      .Where(p => tmpIds.Contains(p.Id))
                      .Execute();
                });

                //更新作业时间
                var signOutDockQueues = GetDockQueueList(jobOverTimeQueueIds);
                UpdateDockQueueJobTimes(signOutDockQueues);

                //签到超时的数据中“推迟次数＜配置项的最大推迟次数”的部分
                double? assignDockId = null;
                DateTime? clearTime = null;
                List<double> delayCheckInOverTimeIds = checkInOverTimeQueues.Where(p => p.DelayNum < config.MaxDelayNum).Select(p => p.Id).ToList();
                delayCheckInOverTimeIds.SplitDataExecute(tmpIds =>
                {
                    DB.Update<DockQueue>()
                      .Set(p => p.QueueState, QueueState.Waiting)
                      .Set(p => p.QueuePriority, QueuePriority.Normal)
                      .Set(p => p.DelayNum, p => p.DelayNum + 1)
                      .Set(p => p.LastDistriTime, p => p.DistributionTime)
                      .Set(p => p.AssignDockId, assignDockId)
                      .Set(p => p.DistributionTime, clearTime)
                      .Set(p => p.CheckInTime, clearTime)
                      .Where(p => tmpIds.Contains(p.Id))
                      .Execute();
                });

                //对于签到超时的数据中“推迟次数≥配置项的最大推迟次数”的部分
                List<double> cancelQueueIds = checkInOverTimeQueues.Where(p => p.DelayNum >= config.MaxDelayNum).Select(p => p.Id).ToList();
                cancelQueueIds.SplitDataExecute(tmpIds =>
                {
                    DB.Update<DockQueue>().Set(p => p.QueueState, QueueState.Cancel)
                      .Set(p => p.CancelEmployeeId, RT.IdentityId)
                      .Set(p => p.CancelTime, DateTime.Now)
                      .Where(p => tmpIds.Contains(p.Id))
                      .Execute();
                });

                ////TODO:推送微信信息(暂未实现)

                tran.Complete();
            }
        }
    }
}