using Newtonsoft.Json.Linq;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.AlarmStates;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.SMDC.Equipments.Infos;
using SIE.SMDC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备警报状态控制器
    /// </summary>
    public partial class EquipController : DomainController
    {
        /// <summary>
        /// 同步设备报警状态记录
        /// </summary>
        public virtual void SyncEquipAlarmRecord()
        {
            var now = RF.Find<EquipAlarmRecord>().GetDbTime();
            var lastRecord_Alarm = Query<EquipAlarmRecord>().Where(p => p.AlarmState == AlarmState.Alarm && p.MdcUid != null).OrderBy(p => p.AlarmTime).FirstOrDefault();//最早一条没有关闭的记录
            var startTime = lastRecord_Alarm == null ? now.AddDays(-1) : lastRecord_Alarm.AlarmTime;
            var endTime = now;

            AlarmRecordInfo[] rtn;

            try
            {
                rtn = RT.Service.Resolve<EquipmentSmdcController>().GetAlarmRecords(startTime, endTime, int.MaxValue, false);
            }
            catch (Exception)
            {
                throw new ValidationException("接口异常：".L10N() + "从MDC获取报警历史数据失败!".L10N());
            }

            // 处理MDC返回结果
            SyncEquipmentTag();
            Dictionary<string, EquipAlarmRecord> equipAlarmRecordsDictionary = ProcessMdcReturnResults(rtn);

            var mdcUids = equipAlarmRecordsDictionary.Keys.Distinct().ToList();

            var existsRecords = mdcUids.SplitContains((tempIds) =>
            {
                return Query<EquipAlarmRecord>().Where(p => tempIds.Contains(p.MdcUid)).ToList();
            });

            // 插入新的报警记录
            var existsUidS = new Dictionary<string, int>();
            foreach (var item in existsRecords)
            {
                existsUidS.Add(item.MdcUid, 0);
            }

            var newEquipAlarmRecords = new EntityList<EquipAlarmRecord>();
            foreach (var equipAlarmRecord in equipAlarmRecordsDictionary.Values)
            {
                if (!existsUidS.ContainsKey(equipAlarmRecord.MdcUid))
                {
                    equipAlarmRecord.PersistenceStatus = PersistenceStatus.New;
                    newEquipAlarmRecords.Add(equipAlarmRecord);
                }
            }

            // 更新要关闭的记录
            List<EquipAlarmRecord> equipAlarmRecordsClose = new List<EquipAlarmRecord>();
            var needCloseRecords = existsRecords.Where(x => x.AlarmState == AlarmState.Alarm).ToList();

            // 值列表
            var needCloseRecordIds = needCloseRecords.Select(x => x.Id).Distinct().ToList();

            EntityList<AlarmRecordValue> existsRecordValues = needCloseRecordIds.SplitContains((tmpIds) =>
            {
                return Query<AlarmRecordValue>().Where(p => tmpIds.Contains(p.Id)).ToList();
            });

            var existsRecordValuesDictionary = existsRecordValues
                .GroupBy(x => x.EquipAlarmRecordId).ToDictionary(x => x.Key, x => x.ToList());

            EntityList<AlarmRecordValue> modifyRecordValues = new EntityList<AlarmRecordValue>();
            foreach (var equipAlarmRecord in needCloseRecords
                .Where(x => equipAlarmRecordsDictionary.ContainsKey(x.MdcUid)))
            {
                var equipAlarmRecordFromMdc = equipAlarmRecordsDictionary[equipAlarmRecord.MdcUid];

                if (equipAlarmRecordFromMdc.AlarmState == AlarmState.Close)
                {
                    equipAlarmRecord.CloseTime = equipAlarmRecordFromMdc.CloseTime;
                    equipAlarmRecord.AlarmState = equipAlarmRecordFromMdc.AlarmState;
                    equipAlarmRecord.Duration = equipAlarmRecordFromMdc.Duration;

                    equipAlarmRecordsClose.Add(equipAlarmRecord);

                    if (existsRecordValuesDictionary.ContainsKey(equipAlarmRecord.Id))
                    {
                        var needCloseRecordValuesOfCurrentRecord = existsRecordValuesDictionary[equipAlarmRecord.Id];

                        foreach (var value in needCloseRecordValuesOfCurrentRecord)
                        {
                            var valueFromMdc = equipAlarmRecordFromMdc.AlarmRecordValueList
                                .FirstOrDefault(x => x.FullTagName == value.FullTagName);

                            if (valueFromMdc != null)
                            {
                                value.RecoveryValue = valueFromMdc.RecoveryValue;
                                modifyRecordValues.Add(value);
                            }
                        }
                    }
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(newEquipAlarmRecords);

                foreach (var needCloseRecord in needCloseRecords)
                {
                    DB.Update<EquipAlarmRecord>()
                        .Set(x => x.CloseTime, needCloseRecord.CloseTime)
                        .Set(x => x.AlarmState, needCloseRecord.AlarmState)
                        .Set(x => x.Duration, needCloseRecord.Duration)
                        .Execute();
                }

                RF.Save(modifyRecordValues);

                trans.Complete();
            }

        }

        /// <summary>
        /// 同步物联参数
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public virtual void SyncEquipmentTag()
        {
            List<EquipmentTag> equipmentTags = new List<EquipmentTag>();

            var equipAccountList = Query<EquipAccount>().ToList();

            foreach (var equipAccount in equipAccountList)
            {
                try
                {
                    var rs = RT.Service.Resolve<EquipmentSmdcController>().GetEquipmentTagFullNames(equipAccount.Code);
                    if (rs.Data == null)
                    {
                        continue;
                    }

                    foreach (var equipmentTag in rs.Data)
                    {
                        equipmentTags.Add(new EquipmentTag()
                        {
                            FullName = equipmentTag.FullName,
                            MaxValue = equipmentTag.MaxValue,
                            MinValue = equipmentTag.MaxValue,
                            Description = equipmentTag.Description,
                            EquipAccountId = equipAccount.Id,
                        });
                    }
                }
                catch (Exception)
                {
                    throw new ValidationException("接口异常：".L10N() + "从MDC获取设备的所有tag全路径的数据失败!".L10N());
                }
            }

            if (!equipmentTags.Any())
            {
                return;
            }

            var equipmentIds = equipmentTags.Select(x => x.EquipAccountId).Distinct().ToList();

            var expression = equipmentIds.CreateContainsExpression<EquipmentTag>("x", EquipmentTag.EquipAccountIdProperty.Name);

            var query = Query<EquipmentTag>();

            if (expression == null)
            {
                return;
            }

            var equipmentTagsOfExists = query.Where(expression).ToList();

            EntityList<EquipmentTag> equipmentTagModifyList = new EntityList<EquipmentTag>();

            foreach (var equipmentTag in equipmentTags)
            {
                var equipmentTagOfExists = equipmentTagsOfExists
                    .FirstOrDefault(x => x.EquipAccountId == equipmentTag.EquipAccountId
                        && x.FullName == equipmentTag.FullName);

                if (equipmentTagOfExists != null)
                {
                    equipmentTagOfExists.MaxValue = equipmentTag.MaxValue;
                    equipmentTagOfExists.MinValue = equipmentTag.MaxValue;
                    equipmentTagOfExists.Description = equipmentTag.Description;
                    equipmentTagModifyList.Add(equipmentTagOfExists);
                }
                else
                {
                    equipmentTagModifyList.Add(new EquipmentTag()
                    {
                        FullName = equipmentTag.FullName,
                        MaxValue = equipmentTag.MaxValue,
                        MinValue = equipmentTag.MaxValue,
                        Description = equipmentTag.Description,
                        EquipAccountId = equipmentTag.EquipAccountId,
                    });
                }
            }

            RF.Save(equipmentTagModifyList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alarmRecordInfos"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private Dictionary<string, EquipAlarmRecord> ProcessMdcReturnResults(AlarmRecordInfo[] alarmRecordInfos)
        {
            var equipAlarmRecordDictionary = new Dictionary<string, EquipAlarmRecord>();
            if (alarmRecordInfos == null || !alarmRecordInfos.Any())
            {
                return equipAlarmRecordDictionary;
            }

            var linkTagFullNames = alarmRecordInfos.Select(x => x.LinkTagFullName).Distinct().ToList();

            var expQueryEquipmentTag = linkTagFullNames.CreateContainsExpression<EquipmentTag>("x", EquipmentTag.FullNameProperty.Name);
            if (expQueryEquipmentTag == null)
            {
                return equipAlarmRecordDictionary;
            }
            var equipmentTags = Query<EquipmentTag>().Where(expQueryEquipmentTag).ToList();
            Dictionary<string, double> equipmentTagDictionary = new Dictionary<string, Double>();
            foreach (var equipmentTag in equipmentTags)
            {
                if (!equipmentTagDictionary.ContainsKey(equipmentTag.FullName))
                {
                    equipmentTagDictionary.Add(equipmentTag.FullName, equipmentTag.EquipAccountId);
                }
            }

            var equipAccountIds = equipmentTags.Select(x => x.EquipAccountId).Distinct().ToList();
            var expQueryEquipAccount = equipAccountIds.CreateContainsExpression<EquipAccount>("x", EquipAccount.IdProperty.Name);
            if (expQueryEquipAccount == null)
            {
                return equipAlarmRecordDictionary;
            }
            var equipAccounts = Query<EquipAccount>().Where(expQueryEquipAccount).ToList();
            var equipAccountsDictionary = equipAccounts.ToDictionary(x => x.Id);

            foreach (var alarmRecordInfo in alarmRecordInfos)
            {
                var linkTagFullName = alarmRecordInfo.LinkTagFullName;

                var linkTagFullNameSplit = linkTagFullName.Split('.');

                if (!equipmentTagDictionary.ContainsKey(linkTagFullName))
                {
                    continue;
                }

                var equipAccountId = equipmentTagDictionary[linkTagFullName];

                if (!equipAccountsDictionary.ContainsKey(equipAccountId))
                {
                    continue;
                }

                var equipAccount = equipAccountsDictionary[equipAccountId];

                var equipAlarmRecord = new EquipAlarmRecord()
                {
                    MdcUid = alarmRecordInfo.Uid,
                    Code = "[" + equipAccount.Code + "]" + alarmRecordInfo.LinkAlarmPath + "." + (alarmRecordInfo.TriggerTime).ToString("yyyyMMddHHmmss"),
                    AlarmId = alarmRecordInfo.AlarmId,
                    LinkAlarmPath = alarmRecordInfo.LinkAlarmPath,
                    AlarmName = alarmRecordInfo.LinkAlarmPath,
                    EquipAccountId = equipAccount.Id,
                    LinkTagFullName = linkTagFullName,
                    TagName = linkTagFullNameSplit[linkTagFullNameSplit.Length - 1],
                    AlarmReason = alarmRecordInfo.AlarmReason,
                    AlarmType = alarmRecordInfo.AlarmType,
                    AlarmContent = alarmRecordInfo.AlarmContent,
                    AlarmDescription = alarmRecordInfo.Description,
                    AlarmRemark = alarmRecordInfo.Remark,
                    LimitValue = alarmRecordInfo.LimitValue,

                    AlarmState = !alarmRecordInfo.RecoveryValue.HasValue ? AlarmState.Alarm : AlarmState.Close,
                    AlarmTime = alarmRecordInfo.TriggerTime,
                    CloseTime = !alarmRecordInfo.RecoveryValue.HasValue ? null : alarmRecordInfo.RecoveryTime,
                    Duration = !alarmRecordInfo.RecoveryValue.HasValue ? null : formatSecond((alarmRecordInfo.RecoveryTime.Value - alarmRecordInfo.TriggerTime).TotalSeconds),
                };

                //"{\"IO.Simulator1.IOTag2\":335\\,\"IO.Simulator1.IOTag3\":335\\,\"IO.Simulator1.IOTag4\":335\\,\"IO.Simulator1.IOTag5\":335\\,\"IO.Simulator1.IOTag6\":335}"
                var alarmChildrenValue = alarmRecordInfo.AlarmChildrenValue.Replace("\\", String.Empty)
                    .Replace("\"", string.Empty)
                    .Replace("{", string.Empty)
                    .Replace("}", string.Empty);

                var alarmChildrenValueArray = alarmChildrenValue.Split(',');
                foreach (var val in alarmChildrenValueArray)
                {
                    var strArr = val.Split(':');
                    double value = 0;
                    double.TryParse(strArr[1], out value);

                    equipAlarmRecord.AlarmRecordValueList.Add(new AlarmRecordValue()
                    {
                        FullTagName = strArr[0],
                        AlarmValue = value,
                    });
                }

                equipAlarmRecordDictionary.Add(alarmRecordInfo.Uid, equipAlarmRecord);
            }

            return equipAlarmRecordDictionary;
        }

        /// <summary>
        /// 将秒数转换成天具体的天时分秒
        /// 比如172800S转换成2天0时0分0秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        private String formatSecond(Double? second)
        {
            String timeStr = "0秒";
            if (second != null)
            {
                Double s = (Double)second;
                String format;
                Object[] array;
                int days = (int)(s / (60 * 60 * 24));
                int hours = (int)(s / (60 * 60) - days * 24);
                int minutes = (int)(s / 60 - hours * 60 - days * 24 * 60);
                int seconds = (int)(s - minutes * 60 - hours * 60 * 60 - days * 24 * 60 * 60);
                if (days > 0)
                {
                    format = "{0}天{1}时{2}分{3}秒";
                    array = new Object[] { days, hours, minutes, seconds };
                }
                else if (hours > 0)
                {
                    format = "{0}时{1}分{2}秒";
                    array = new Object[] { hours, minutes, seconds };
                }
                else if (minutes > 0)
                {
                    format = "{0}分{1}秒";
                    array = new Object[] { minutes, seconds };
                }
                else
                {
                    format = "{0}秒";
                    array = new Object[] { seconds };
                }
                timeStr = String.Format(format, array);
            }
            return timeStr;
        }
    }
}