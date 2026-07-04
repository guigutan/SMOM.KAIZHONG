using SIE.Domain;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警控制器
    /// </summary>
    public class AlarmController : DomainController
    {
        /// <summary>
        /// 查询报警统计
        /// </summary>
        /// <param name="criteria">报警统计实体</param>
        /// <returns>报警统计列表</returns>
        public virtual EntityList<AlarmCount> CriteriaAlarmCount(AlarmCountCriteria criteria)
        {
            var q = Query<EquipAlarmRecord>();
            if (criteria.EquipAccountId != 0)
            {
                q.Where(p => p.EquipAccountId == criteria.EquipAccountId.Value);
            }

            if (criteria.EquipModelId != 0)
            {
                q.Where(p => p.EquipAccount.EquipModelId == criteria.EquipModelId.Value);
            }

            if (criteria.EquipTypeId != 0)
            {
                q.Where(p => p.EquipAccount.EquipModel.EquipTypeId == criteria.EquipTypeId.Value);
            }

            if (criteria.AlarmTime.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.AlarmTime.BeginValue);
            }

            if (criteria.AlarmTime.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.AlarmTime.EndValue);
            }

            var allList = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var idList = q.Select(p => p.EquipAccountId).Distinct().ToList();

            EntityList<AlarmCount> list = new EntityList<AlarmCount>();

            foreach (var item in idList)
            {
                var entity = allList.FirstOrDefault(p => p.EquipAccountId == item.EquipAccountId);

                list.Add(new AlarmCount()
                {
                    EquipAccountId = entity.EquipAccountId,
                    AlarmEquipAccountCode = entity.EquipAccountCode,
                    AlarmEquipAccountName = entity.EquipAccountName,
                    AlarmEquipModelCode = entity.EquipModelCode,
                    AlarmEquipModelName = entity.EquipModelName,
                    AlarmEquipTypeeCode = entity.EquipTypeeCode,
                    AlarmEquipTypeeName = entity.EquipTypeeName,
                    AlarmSum = allList.Count(p => p.EquipAccountId == item.EquipAccountId),
                    Serious = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmLevel == Enums.AlarmLevel.Serious),
                    Major = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmLevel == Enums.AlarmLevel.Major),
                    Medium = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmLevel == Enums.AlarmLevel.Medium),
                    Minor = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmLevel == Enums.AlarmLevel.Minor),
                    Info = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmLevel == Enums.AlarmLevel.Info),
                    Alarm = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmState == Enums.AlarmState.Alarm),
                    Close = allList.Count(p => p.EquipAccountId == item.EquipAccountId && p.AlarmState == Enums.AlarmState.Close)
                });
            }

            EntityList<AlarmCount> listCopy = new EntityList<AlarmCount>();
            listCopy.AddRange(list.OrderByDescending(p => p.Alarm).ThenByDescending(p => p.AlarmSum).ToList());
            return listCopy;
        }

        /// <summary>
        /// 获取报警值
        /// </summary>
        /// <param name="parentId">设备报警记录ID</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<AlarmRecordValueViewModel> GetAlarmRecordValueViewModels(double parentId,
            IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            var equipAlarmRecord = RF.GetById<EquipAlarmRecord>(parentId);


            var alarmRecordValues = Query<AlarmRecordValue>().Where(x => x.EquipAlarmRecordId == parentId).ToList();

            if (equipAlarmRecord == null || alarmRecordValues == null || !alarmRecordValues.Any())
            {
                return new EntityList<AlarmRecordValueViewModel>();
            }

            var facilityDetails = Query<FacilityDetail>()
                .Where(x => x.EquipAccountId == equipAlarmRecord.EquipAccountId)
                .ToList();

            var deviceIOTParaIds = facilityDetails.Select(x => x.DeviceIOTParaId).Distinct().ToList();

            var physicalUnions = deviceIOTParaIds.SplitContains(tempIds =>
             {
                 return Query<PhysicalUnion>().Where(x => tempIds.Contains(x.DeviceIOTParaId)).ToList();
             });



            EntityList<AlarmRecordValueViewModel> alarmRecordValueViewModels = new EntityList<AlarmRecordValueViewModel>();

            foreach (var alarmRecordValue in alarmRecordValues)
            {
                var alarmRecordValueViewModel = new AlarmRecordValueViewModel()
                {
                    FullTagName = alarmRecordValue.FullTagName,
                    AlarmValue = alarmRecordValue.AlarmValue,
                    RecoveryValue = alarmRecordValue.RecoveryValue,
                };

                // "IO.Simulator1.IOTag9"
                var tagNamsArr = alarmRecordValue.FullTagName.Split('.');

                //最后部分
                var shorTagName = tagNamsArr[tagNamsArr.Length - 1];

                var physicalUnion = physicalUnions.FirstOrDefault(x => x.MDCVariableName == shorTagName);
                if (physicalUnion != null)
                {
                    alarmRecordValueViewModel.MDCVariableName = physicalUnion.MDCVariableName;
                    alarmRecordValueViewModel.PararCode = physicalUnion.PararCode;
                    alarmRecordValueViewModel.ParaName = physicalUnion.ParaName;
                }

                alarmRecordValueViewModels.Add(alarmRecordValueViewModel);
            }

            return alarmRecordValueViewModels;
        }


        /// <summary>
        /// 查询报警明细
        /// </summary>
        /// <param name="criteria">报警明细实体</param>
        /// <returns>报警明细列表</returns>
        public virtual EntityList<EquipAlarmRecord> CriteriaAlarmDetail(EquipAlarmRecordCriteria criteria)
        {
            var q = Query<EquipAlarmRecord>();
            if (criteria.EquipAccountId != 0)
                q.Where(p => p.EquipAccountId == criteria.EquipAccountId.Value);
            if (!string.IsNullOrEmpty(criteria.ViewEquipAccountName))
            {
                q.Join<EquipAccount>((d, s) => d.EquipAccountId == s.Id && s.Name.Contains("%" + criteria.ViewEquipAccountName + "%"));
            }
            if (criteria.EquipModelId != 0)
                q.Where(p => p.EquipAccount.EquipModelId == criteria.EquipModelId.Value);
            if (criteria.EquipTypeId != 0)
                q.Where(p => p.EquipAccount.EquipModel.EquipTypeId == criteria.EquipTypeId.Value);
            if (criteria.AlarmTime.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criteria.AlarmTime.BeginValue);
            if (criteria.AlarmLevel.HasValue)
                q.Where(p => p.AlarmLevel == criteria.AlarmLevel.Value);
            if (!string.IsNullOrEmpty(criteria.AlarmType))
                q.Where(p => p.AlarmType.Contains("%" + criteria.AlarmType + "%"));
            if (criteria.AlarmState.HasValue)
                q.Where(p => p.AlarmState == criteria.AlarmState.Value);
            if (!string.IsNullOrEmpty(criteria.AlarmContent))
                q.Where(p => p.AlarmContent.Contains("%" + criteria.AlarmContent + "%"));
            if (criteria.AlarmTime.EndValue.HasValue)
                q.Where(p => p.CreateDate <= criteria.AlarmTime.EndValue);
            var list = q.OrderByDescending(p => p.AlarmTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (EquipAlarmRecord item in list)
            {
                if (item.AlarmState == Enums.AlarmState.Alarm)
                {
                    item.Duration = CalculateDate(item.AlarmTime, DateTime.Now);
                }
                else if (item.AlarmState == Enums.AlarmState.Close)
                {
                    if (item.CloseTime != null)
                        item.Duration = CalculateDate(item.AlarmTime, Convert.ToDateTime(item.CloseTime));
                }
            }
            return list;
        }

        /// <summary>
        /// 时间计算
        /// </summary>
        /// <param name="begTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual string CalculateDate(DateTime begTime, DateTime endTime)
        {
            DateTime dt1 = Convert.ToDateTime(endTime);
            DateTime dt2 = Convert.ToDateTime(begTime);
            TimeSpan ts1 = dt1.Subtract(dt2);
            int day = ts1.Days;//天
            int hour = ts1.Hours;//小时
            int minute = ts1.Minutes;//分钟
            if (day != 0)
            {
                return day + "天".L10N() + hour + "小时".L10N() + minute + "分钟".L10N();
            }
            if (hour != 0)
            {
                return hour + "小时".L10N() + minute + "分钟".L10N();
            }
            if (minute != 0)
            {
                return minute + "分钟".L10N();
            }

            return null;
        }
    }
}
