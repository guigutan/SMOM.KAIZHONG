using Nest;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Dao;
using SIE.Dock.DockAppoints.Configs;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockQueues;
using SIE.Dock.DockQueues.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Dock.DockAppoints.Dao
{
    /// <summary>
    /// 月台预约DAO
    /// </summary>
    public class DockAppointDao : BaseDao<DockAppoint>
    {
        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppoints(DockAppointCriteria criteria)
        {
            var query = Query();
            query.LeftJoin<DockMaintain>((x, y) => x.DockMaintainId == y.Id);

            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.BillNo.IsNotEmpty())
            {
                query.Where(p => p.BillNo.Contains(criteria.BillNo));
            }

            if (criteria.DockMaintainCode.IsNotEmpty())
            {
                query.Where(p => p.DockMaintain.Code.Contains(criteria.DockMaintainCode));
            }

            if (criteria.DockMaintainName.IsNotEmpty())
            {
                query.Where(p => p.DockMaintain.Name.Contains(criteria.DockMaintainName));
            }

            if (criteria.YardZoneId.HasValue)
            {
                query.Where(p => p.YardZoneId == criteria.YardZoneId.Value);
            }
            if (criteria.Contacts.IsNotEmpty())
            {
                query.Where(p => p.Contacts.Contains(criteria.Contacts));
            }
            if (criteria.CarNum.IsNotEmpty())
            {
                query.Where(p => p.CarNum == criteria.CarNum);
            }
            if (criteria.AppointType.HasValue)
            {
                query.Where(p => p.AppointType == criteria.AppointType.Value);
            }
            if (criteria.AppointDate.BeginValue.HasValue)
            {
                query.Where(p => p.AppointStartDate >= criteria.AppointDate.BeginValue.Value);
            }
            if (criteria.AppointDate.EndValue.HasValue)
            {
                query.Where(p => p.AppointEndDate <= criteria.AppointDate.EndValue.Value);
            }

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取发运单配置的编码规则
        /// </summary>
        /// <returns>返回编码规则</returns>
        public virtual DockAppointNoConfigValue GetDockAppointNumberRule()
        {
            var config = ConfigService.GetConfig(new DockAppointNoConfig(), typeof(DockAppoint));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到月台预约单号生成规则,请检查规则配置".L10N());
            return config;
        }

        /// <summary>
        /// 获取月台预约单号
        /// </summary>
        /// <returns>月台预约单号</returns>
        public virtual string GetDockAppointNo()
        {
            var config = GetDockAppointNumberRule();
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppointList(List<double> ids, EagerLoadOptions elo)
        {
            return ids.SplitContains(tmpIds =>
            {
                return Query().Where(p => tmpIds.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取月台预约数据
        /// </summary>
        /// <param name="dockIds">月台维护ID集合</param>
        /// <param name="appointDate">大于等于预约日期</param>
        /// <returns>月台预约数据</returns>
        public virtual EntityList<DockAppoint> GetDockAppointByDockIds(List<double> dockIds, DateTime? appointDate = null)
        {
            return dockIds.SplitContains(tmpIds =>
            {
                var query = Query();
                if (appointDate.HasValue)
                {
                    query.Where(p => p.AppointDate >= appointDate.Value);
                }

                return query.Where(p => tmpIds.Contains(p.DockMaintainId) && !p.IsCancelAppoint).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
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
            return dockIds.SplitContains(tmpIds =>
            {
                var query = Query().Where(p => !p.IsCancelAppoint);
                if (beginDate.HasValue)
                    query.Where(p => p.AppointDate >= beginDate);
                if (endDate.HasValue)
                    query.Where(p => p.AppointDate < endDate.Value.AddDays(1));
                if (appointNo.IsNotEmpty())
                    query.Where(p => p.No.Contains(appointNo));
                if (billNo.IsNotEmpty())
                    query.Where(p => p.BillNo.Contains(billNo));

                return query.Where(p => tmpIds.Contains(p.DockMaintainId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取预约数据
        /// </summary>
        /// <param name="keyword">查询关键子</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>预约数据</returns>
        public virtual EntityList<DockAppoint> GetSelectDockAppoints(string keyword, PagingInfo pagingInfo)
        {
            var query = Query().Where(p => !p.IsCancelAppoint);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(keyword) || p.BillNo.Contains(keyword) || p.CompanyName.Contains(keyword) || p.CarNum.Contains(keyword) || p.Contacts.Contains(keyword) || p.ContactNum.Contains(keyword) || p.IDNumber.Contains(keyword));
            }

            var dockAppoints = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var tmpDockAppoints = dockAppoints.Where(p => p.AppointEndDate.Date >= DateTime.Now.AddDays(-1)).ToList();

            List<double> dockAppointIds = tmpDockAppoints.Select(p => p.Id).Distinct().ToList();
            var dockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueByDockAppointIds(dockAppointIds);
            List<double> selectDockApponintIds = dockQueues.Where(p => p.QueueState == QueueState.Waiting || p.QueueState == QueueState.Handling || p.QueueState == QueueState.Finish).Select(p => p.DockAppointId.Value).Distinct().ToList();

            var selectDockAppoints = tmpDockAppoints.Where(p => !selectDockApponintIds.Contains(p.Id)).AsEntityList();
            selectDockAppoints.SetTotalCount(selectDockAppoints.Count);

            return selectDockAppoints;
        }

        /// <summary>
        /// 获取预约日期范围的预约月台数据
        /// </summary>
        /// <param name="yardZoneId">园区ID</param>
        /// <param name="appointType">预约类型</param>
        /// <param name="beginTime">开始时间集合</param>
        /// <param name="endTime">结束时间集合</param>
        /// <param name="dockList">园区下的月台</param>
        /// <returns>预约月台数据</returns>
        public virtual EntityList<DockAppoint> GetAppointDateDockAppoints(double yardZoneId, AppointType? appointType, DateTime? beginTime, DateTime? endTime, EntityList<DockMaintain> dockList)
        {
            
            var query = Query().Where(p => p.YardZoneId == yardZoneId && !p.IsCancelAppoint);
            if (beginTime.HasValue)
            {
                query.Where(p => p.AppointStartDate >= beginTime.Value);
            }

            if (endTime.HasValue)
            {
                query.Where(p => p.AppointEndDate <= endTime.Value);
            }
            if (appointType.HasValue)
            {
                query.Where(p => p.AppointType == appointType);
            }
            if (dockList.Count > 0)
            {
                var DockIds = dockList.Select(p => p.Id).Distinct().ToList();
                query.Where(p => DockIds.Contains(p.DockMaintainId));
            }
            
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 预约ID
        /// </summary>
        /// <param name="Id">预约ID</param>
        /// <returns></returns>
        public virtual DockAppoint GetDockAppointById(double Id)
        {
            return Query().Where(p => p.Id == Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
        }
    }
}
