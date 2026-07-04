using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Rbac.Users;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 设备报警记录API控制器
    /// </summary>
    public partial class EquipAlarmRecordController : DomainController
    {
        #region 预警接口
        /// <summary>
        /// 预警-获取部门列表
        /// </summary>
        /// <returns>部门列表</returns>
        [ApiService("预警-获取部门列表")]
        [return: ApiReturn("部门列表")]
        public virtual List<BaseDataInfo> GetUseDepartments()
        {
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var ctl = RT.Service.Resolve<EnterpriseController>();
            var useDepartmentList = ctl.GetDepartmentsWithParent(null, String.Empty);
            useDepartmentList.ForEach(p => infos.Add(new BaseDataInfo
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name
            }));
            return infos;
        }

        /// <summary>
        /// 根据用户获取报警汇总信息
        /// </summary>
        /// <param name="criteria">报警汇总查询信息</param>
        /// <returns>报警汇总信息</returns>
        [ApiService("预警-根据用户获取报警汇总信息")]
        [return: ApiReturn("报警汇总信息 AlarmSummaryInfo")]
        public virtual AlarmSummaryInfo GetAlarmSummaryInfo([ApiParameter("报警汇总查询信息")] AlarmSummaryQueryInfo criteria)
        {
            var info = new AlarmSummaryInfo();
            var equipIds = GetAlarmEquipAccountId(criteria);
            var records = GetEquipAlarmRecordByTime(criteria, equipIds);
            //统计数据
            foreach (var record in records)
            {
                var detailInfo = new AlarmDetailInfo();
                if (record.AlarmState == Enums.AlarmState.Alarm)
                    info.OpenQty++;
                else if (record.AlarmState == Enums.AlarmState.Close)
                    info.CloseQty++;
                if (record.AlarmLevel == Enums.AlarmLevel.Info)
                {
                    detailInfo.AlarmLevel = "提示".L10N();
                    detailInfo.AlarmLevelValue = 4;
                    info.InfoQty++;
                }
                else if (record.AlarmLevel == Enums.AlarmLevel.Minor)
                {
                    detailInfo.AlarmLevel = "轻微".L10N();
                    detailInfo.AlarmLevelValue = 3;
                    info.MinorQty++;
                }
                else if (record.AlarmLevel == Enums.AlarmLevel.Medium)
                {
                    detailInfo.AlarmLevel = "一般".L10N();
                    detailInfo.AlarmLevelValue = 2;
                    info.MediumQty++;
                }
                else if (record.AlarmLevel == Enums.AlarmLevel.Major)
                {
                    detailInfo.AlarmLevel = "严重".L10N();
                    detailInfo.AlarmLevelValue = 1;
                    info.MajorQty++;
                }
                else if (record.AlarmLevel == Enums.AlarmLevel.Serious)
                {
                    detailInfo.AlarmLevel = "紧急".L10N();
                    detailInfo.AlarmLevelValue = 0;
                   info.SeriousQty++;
                }
                detailInfo.EquipAccountId = record.EquipAccountId;
                detailInfo.EquipAlarmRecordId = record.Id;
                detailInfo.EquipAccountCode = record.EquipAccountCode;
                detailInfo.EquipAccountName = record.EquipAccountName;
                detailInfo.EquipModelName = record.EquipModelName;
                detailInfo.Code = record.Code;
                detailInfo.AlarmType = record.AlarmType;
                detailInfo.AlarmContent = record.AlarmContent;
                detailInfo.AlarmTime = record.AlarmTime;
                detailInfo.AlarmState = record.AlarmState.ToLabel();
                detailInfo.AlarmStateValue = record.AlarmState;
                detailInfo.CloseTime = record.CloseTime;
                if (record.CloseTime.HasValue)
                {
                    detailInfo.Duration = record.Duration;
                }
                else
                {
                    detailInfo.Duration = RT.Service.Resolve<AlarmController>().CalculateDate(record.AlarmTime, DateTime.Now);
                }

                info.AlarmDetails.Add(detailInfo);
            }
            //报警类别
            var typeLists = new List<WarningChartInfo>();
            foreach (var group in records.GroupBy(p => p.AlarmType))
            {
                var types = new WarningChartInfo();
                types.Label = group.Key;
                types.Qty = group.Count();
                typeLists.Add(types);
            }
            info.AlarmCategorys = typeLists.OrderByDescending(p => p.Qty).Take(10).ToList();
            //设备报警数
            var qtyLists = new List<WarningChartInfo>();
            foreach (var group in records.GroupBy(p => p.EquipAccountCode))
            {
                var types = new WarningChartInfo();
                types.Label = group.Key;
                types.Qty = group.Count();
                qtyLists.Add(types);
            }
            info.EquipCount = qtyLists.Count;
            info.EquipAlarms = qtyLists.OrderByDescending(p => p.Qty).Take(10).ToList();
            return info;
        }

        /// <summary>
        /// 根据预警信息Id关闭预警
        /// </summary>
        /// <param name="Id">报警Id</param>
        /// <returns></returns>
        [ApiService("预警-根据预警信息Id关闭预警")]
        public virtual void AlarmRecordClose([ApiParameter("报警编号")] double Id)
        {
            var equipAlarmRecord = GetById<EquipAlarmRecord>(Id);
            //更改状态,更新关闭时间,更新报警持续时间
            equipAlarmRecord.AlarmState = Enums.AlarmState.Close;
            equipAlarmRecord.CloseTime = DateTime.Now;
            equipAlarmRecord.Duration = RT.Service.Resolve<AlarmController>().CalculateDate(equipAlarmRecord.AlarmTime, DateTime.Now);
            RF.Save(equipAlarmRecord);
        }

        /// <summary>
        /// 获取报警记录设备Id
        /// </summary>
        /// <param name="criteria">报警汇总查询信息</param>
        /// <returns>报警记录设备Id</returns>
        private IList<double> GetAlarmEquipAccountId(AlarmSummaryQueryInfo criteria)
        {
            var query = Query<EquipAccount>();
            if (criteria.Key.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Key) || p.Name.Contains(criteria.Key));
            }
            if (criteria.EquipTypeId.HasValue)
            {
                query.Join<EquipModel>((d, e) => d.EquipModelId == e.Id && e.EquipTypeId == criteria.EquipTypeId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.UseDepartmentId == criteria.DepartmentId);
            }
            return query.Select(p => p.Id).Distinct().ToList<double>();
        }

        /// <summary>
        /// 获取设备报警记录
        /// </summary>
        /// <param name="criteria">报警汇总查询信息</param>
        /// <param name="equipIds">设备id列表</param>
        /// <returns>设备报警记录</returns>
        private EntityList<EquipAlarmRecord> GetEquipAlarmRecordByTime(AlarmSummaryQueryInfo criteria, IList<double> equipIds)
        {
            var time = GetAlarmQueryTime(criteria.QueryTime);
            var alarmQuery = Query<EquipAlarmRecord>().Where(p => equipIds.Contains(p.EquipAccountId) && p.AlarmTime >= time.Item1 && p.AlarmTime <= time.Item2);
            if (criteria.AlarmLevel.HasValue)
            {
                alarmQuery.Where(p => p.AlarmLevel == criteria.AlarmLevel.Value);
            }
            if (criteria.AlarmState.HasValue)
            {
                alarmQuery.Where(p => p.AlarmState == criteria.AlarmState.Value);
            }
            return alarmQuery.OrderByDescending(p => p.AlarmTime).Distinct().ToList(new PagingInfo() { PageNumber = 1, PageSize = 100 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取开始结束时间
        /// </summary>
        /// <param name="queryTime">查询时间</param>
        /// <returns>开始结束时间</returns>
        private Tuple<DateTime, DateTime> GetAlarmQueryTime(AlarmQueryTime queryTime)
        {
            var today = DateTime.Today;
            var startTime = today;
            var endTime = today.AddDays(1).AddSeconds(-1);
            switch (queryTime)
            {
                case AlarmQueryTime.ThisWeek:
                    startTime = today.AddDays(1 - (int)today.DayOfWeek);
                    endTime = startTime.AddDays(7).AddSeconds(-1);
                    break;
                case AlarmQueryTime.ThisMonth:
                    startTime = today.AddDays(1 - today.Day);
                    endTime = startTime.AddMonths(1).AddSeconds(-1);
                    break;
                case AlarmQueryTime.NearlySevenDays:
                    startTime = today.AddDays(-6);
                    break;
                case AlarmQueryTime.NearlyThirtyDays:
                    startTime = today.AddDays(-29);
                    break;
                case AlarmQueryTime.ThreeMonths:
                    startTime = today.AddMonths(-3);
                    break;
                case AlarmQueryTime.SixMonths:
                    startTime = today.AddMonths(-6);
                    break;
                case AlarmQueryTime.ThisYear:
                    startTime = new DateTime(today.Year, 1, 1);
                    endTime = startTime.AddYears(1).AddSeconds(-1);
                    break;
            }
            return new Tuple<DateTime, DateTime>(startTime, endTime);
        }
        #endregion
    }
}
