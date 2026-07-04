using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Dao;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockQueues.Configs;
using SIE.Dock.DockQueues.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.DockQueues.Dao
{
    /// <summary>
    /// 月台排队Dao
    /// </summary>
    public class DockQueueDao : BaseDao<DockQueue>
    {
        /// <summary>
        /// 获取月台排队数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueues(DockQueueCriteria criteria)
        {
            var query = Query();
            query.LeftJoin<DockAppoint>((x, y) => x.DockAppointId == y.Id);
            query.LeftJoin<DockMaintain>((x, d) => x.AssignDockId == d.Id);

            //排队号
            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            //状态
            if (criteria.QueueState.HasValue)
            {
                query.Where(p => p.QueueState == criteria.QueueState.Value);
            }
            //排队类型
            if (criteria.AppointType.HasValue)
            {
                query.Where(p => p.AppointType == criteria.AppointType.Value);
            }
            //排队地点
            if (criteria.YardZoneId.HasValue)
            {
                query.Where(p => p.YardZoneId == criteria.YardZoneId.Value);
            }
            //单据号
            if (criteria.BillNo.IsNotEmpty())
            {
                query.Where(p => p.BillNo.Contains(criteria.BillNo));
            }
            //预约号
            if (criteria.DockAppointNo.IsNotEmpty())
            {
                query.Where<DockAppoint>((x, y) => y.No.Contains(criteria.DockAppointNo));
            }
            //分配月台编码
            if (criteria.DockMaintainCode.IsNotEmpty())
            {
                query.Where<DockMaintain>((x, d) => d.Code.Contains(criteria.DockMaintainCode));
            }
            //联系人
            if (criteria.Contacts.IsNotEmpty())
            {
                query.Where(p => p.Contacts.Contains(criteria.Contacts));
            }
            //车牌号
            if (criteria.CarNum.IsNotEmpty())
            {
                query.Where(p => p.CarNum == criteria.CarNum);
            }
            //创建时间
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取月台排队配置的编码规则
        /// </summary>
        /// <returns>返回编码规则</returns>
        public virtual DockQueueNumberConfigValue GetDockQueueNumberRule()
        {
            var config = ConfigService.GetConfig(new DockQueueNumberConfig(), typeof(DockQueue));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到月台排队号生成规则,请检查规则配置".L10N());
            return config;
        }

        /// <summary>
        /// 获取月台排队的排队号
        /// </summary>
        /// <returns>月台排队号</returns>
        public virtual string GetDockQueueNo()
        {
            var config = GetDockQueueNumberRule();
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取月台排队数据
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueList(List<double> ids)
        {
            return ids.SplitContains(tmpIds =>
            {
                return Query().Where(p => tmpIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据月台查询月台排队数据
        /// </summary>
        /// <param name="dockIds">月台ID集合</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueByDockIds(List<double> dockIds)
        {
            return dockIds.SplitContains(tmpIds =>
            {
                return Query().Where(p => p.AssignDockId != null && tmpIds.Contains((double)p.AssignDockId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取根据月台预约ID查询月台排队数据
        /// </summary>
        /// <param name="dockAppointIds">月台预约ID集合</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueByDockAppointIds(List<double> dockAppointIds)
        {
            return dockAppointIds.SplitContains(tmpIds =>
            {
                return Query().Where(p => p.DockAppointId != null && tmpIds.Contains((double)p.DockAppointId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取对应状态的月台排队数据
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueByStates(QueueState state)
        {
            return Query().Where(p => p.QueueState == state).ToList();
        }

        /// <summary>
        /// 获取月台排队用于调度
        /// </summary>
        /// <returns>月台排队数据</returns>
        public virtual EntityList<DockQueue> GetDockQueueForJob()
        {
            return Query().Where(p => p.QueueState == QueueState.Waiting).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 月台排队推迟
        /// </summary>
        /// <param name="delayIdList">需要推迟的排队号Id</param>
        public virtual void DelayQueue(List<double> delayIdList)
        {
            var queueList = GetDockQueueList(delayIdList);
            if (queueList.Any(a => a.QueueState != QueueState.Handling))
                throw new ValidationException("排队状态不是[装卸中]，不能推迟".L10N());

            var config = GetDockQueueNumberRule();
            if (queueList.Any(p => p.DelayNum >= config?.MaxDelayNum))
                throw new ValidationException("推迟次数大于或等于配置项的最大推迟次数，不能推迟".L10N());

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                double? assignDockId = null;
                DateTime? nullTime = null;
                delayIdList.SplitDataExecute(tmpIds =>
                {
                    Update().Set(p => p.QueueState, QueueState.Waiting)
                        .Set(p => p.QueuePriority, QueuePriority.Normal)
                        .Set(p => p.DelayNum, p => p.DelayNum + 1)
                        .Set(p => p.LastDistriTime, p => p.DistributionTime)
                        .Set(p => p.AssignDockId, assignDockId)
                        .Set(p => p.DistributionTime, nullTime)
                        .Set(p => p.CheckInTime, nullTime)
                        .Where(p => tmpIds.Contains(p.Id)).Execute();
                });

                //TODO:推送微信信息(暂未实现)

                tran.Complete();
            }
        }

        /// <summary>
        /// 月台排队签到
        /// </summary>
        /// <param name="inIdList">需要签到的排队号Id</param>
        public virtual void CheckInQueue(List<double> inIdList)
        {
            EntityList<DockQueue> queueList = RT.Service.Resolve<DockQueueService>().GetDockQueueList(inIdList);
            if (queueList.Any(a => a.QueueState != QueueState.Handling || a.CheckInTime != null))
                throw new ValidationException("排队状态不是[装卸中]或签到时间不为空，不能签到".L10N());

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                inIdList.SplitDataExecute(tmpIds =>
                {
                    Update().Set(p => p.CheckInTime, DateTime.Now)
                            .Where(p => tmpIds.Contains(p.Id)).Execute();
                });

                tran.Complete();
            }
        }

        /// <summary>
        /// 月台排队签出
        /// </summary>
        /// <param name="outIdList">需要签出的排队号Id</param>
        public virtual void CheckOutQueue(List<double> outIdList)
        {
            EntityList<DockQueue> queueList = RT.Service.Resolve<DockQueueService>().GetDockQueueList(outIdList);
            if (queueList.Any(a => a.QueueState != QueueState.Handling || a.CheckInTime == null))
                throw new ValidationException("排队状态不是[装卸中]或签到时间为空，不能签出".L10N());

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                outIdList.SplitDataExecute(tmpIds =>
                {
                    Update().Set(p => p.CheckOutTime, DateTime.Now)
                            .Set(p => p.QueueState, QueueState.Finish)
                            .Where(p => tmpIds.Contains(p.Id)).Execute();
                });

                //更新作业时间
                var signOutDockQueues = GetDockQueueList(outIdList);
                signOutDockQueues.ForEach(p =>
                {
                    if (p.CheckOutTime.HasValue && p.CheckInTime.HasValue)
                    {
                        p.JobTime = (decimal)Math.Round((p.CheckOutTime.Value - p.CheckInTime.Value).TotalMinutes, 3);
                    }
                });

                RF.Save(signOutDockQueues);

                tran.Complete();
            }
        }

        /// <summary>
        /// 月台排队升级
        /// </summary>
        /// <param name="upQueueId">需要升级的排队号Id</param>
        public virtual void UpDockQueueData(double upQueueId)
        {
            var upQueue = RF.GetById<DockQueue>(upQueueId);
            int priority = (int)upQueue.QueuePriority;
            if (upQueue.QueuePriority == QueuePriority.Highest)
                return;
            priority = priority + 1;
            

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                Update().Set(p => p.QueuePriority, (QueuePriority)priority)
                        .Where(p => p.Id == upQueueId).Execute();

                tran.Complete();
            }
        }

        /// <summary>
        /// 月台排队降级
        /// </summary>
        /// <param name="downQueueId">需要降级的排队号Id</param>
        public virtual void DownDockQueueData(double downQueueId)
        {
            var downQueue = RF.GetById<DockQueue>(downQueueId);
            int priority = (int)downQueue.QueuePriority;
            if (downQueue.QueuePriority == QueuePriority.Last)
                return;            
            priority = priority - 1;

            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                Update().Set(p => p.QueuePriority, (QueuePriority)priority)
                        .Where(p => p.Id == downQueueId).Execute();

                tran.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageNum">页号</param>
        /// <param name="PageSize">页码</param>
        /// <param name="keyWord">查询关键字</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public virtual EntityList<DockQueue> GetDockQueuesList(int PageNum, int PageSize, string keyWord, DateTime? date)
        {
            var query = Query().Where(p => p.QueueState == QueueState.Handling || p.QueueState == QueueState.Waiting);
            if (!keyWord.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(keyWord) || p.CompanyName.Contains(keyWord) || p.CarNum.Contains(keyWord) || p.ContactNum.Contains(keyWord));
            }
            if (date.HasValue)
            {
                query.Where(p => p.CreateDate >= date && p.CreateDate < date);
            }
            query.OrderByDescending(p => p.QueuePriority).OrderBy(p=>p.DelayNum).OrderBy(p=>p.CreateDate);
            return query.ToList(new PagingInfo(PageNum, PageSize), new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取排队数据
        /// </summary>
        /// <param name="Code">月台编码</param>
        /// <param name="QueueState">排队状态</param>
        /// <returns></returns>
        public virtual EntityList<DockQueue> GetDockQueueByState(string Code, QueueState QueueState)
        {
            return Query().Where(p => p.AssignDock.Code == Code && p.QueueState == QueueState).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

    
    }
}
