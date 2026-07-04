using SIE.Andon.AndonBulletinBoard.APIModels;
using SIE.Andon.Andons;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.Tech.Stations;
using SIE.Warehouses.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Andon.AndonBulletinBoard
{
    /// <summary>
    /// 安灯看板服务
    /// </summary>
    public class AndonBoardService : DomainService
    {
        /// <summary>
        /// 根据车间id获取安灯状态集合
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual List<AndonBoardStateInfo> GetAndonStateData(double workShopId)
        {
            if (workShopId < 0)
            {
                throw new ValidationException("车间信息异常！".L10N());
            }
            List<AndonBoardStateInfo> andonBoardStateInfos = new List<AndonBoardStateInfo>();
            var andonManageList = DB.Query<AndonManage>().Where(p => p.WorkShopId == workShopId && p.State != Andons.Enum.AndonManageState.Cancel).ToList();
            var groupByState = andonManageList.GroupBy(p => p.State).Select(p => new { state = p.Key.ToLabel(), count = p.Count() }).ToDictionary(p => p.state, p => p.count);
            groupByState.ForEach(item =>
            {
                AndonBoardStateInfo andonBoardStateInfo = new AndonBoardStateInfo()
                {
                    State = item.Key,
                    Count = item.Value,
                };
                andonBoardStateInfos.Add(andonBoardStateInfo);
            });
            return andonBoardStateInfos;
        }

        /// <summary>
        /// 根据车间获取安灯大类统计
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual List<AndonBoardClassInfo> GetAndonClassData(double workShopId)
        {
            if (workShopId < 0)
            {
                throw new ValidationException("车间信息异常！".L10N());
            }
            List<AndonBoardClassInfo> andonBoardClassInfos = new List<AndonBoardClassInfo>();
            var andonManageList = DB.Query<AndonManage>().Where(p => p.WorkShopId == workShopId && p.State != Andons.Enum.AndonManageState.Cancel).ToList();
            var groupByClass = andonManageList.GroupBy(p => p.AndonManageClass).Select(p => new { andonclass = p.Key.ToLabel(), count = p.Count() }).ToDictionary(p => p.andonclass, p => p.count);
            groupByClass.ForEach(item =>
            {
                AndonBoardClassInfo andonBoardClassInfo = new AndonBoardClassInfo()
                {
                    AndonClass = item.Key,
                    Count = item.Value,
                };
                andonBoardClassInfos.Add(andonBoardClassInfo);
            });
            return andonBoardClassInfos;
        }

        /// <summary>
        /// 获取安灯停线信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual AndonLineStop GetAndonLineStop(double workShopId)
        {
            if (workShopId < 0)
            {
                throw new ValidationException("车间信息异常！".L10N());
            }
            AndonLineStop andonLineStop = new AndonLineStop();
            var andonManageList = DB.Query<AndonManage>()
                .Where(p => p.WorkShopId == workShopId && p.State != Andons.Enum.AndonManageState.Cancel).ToList();
            andonLineStop.AndonCount = andonManageList.Count;
            andonLineStop.StandbyCount = andonManageList.Count(p => p.State == Andons.Enum.AndonManageState.Standby);
            andonLineStop.ProcessingCount = andonManageList.Count(p => p.State == Andons.Enum.AndonManageState.Processing);
            andonLineStop.ToAcceptedCount = andonManageList.Count(p => p.State == Andons.Enum.AndonManageState.ToAccepted);
            andonLineStop.ClosedCount = andonManageList.Count(p => p.State == Andons.Enum.AndonManageState.Closed);
            andonLineStop.StopTimes = andonManageList.Count(p => p.LineStop);
            var abnormalCauseList = DB.Query<AbnormalCause>()
                .Where(p => p.SourceType == ExceptionStopSourceType.AlertLight && p.ShopId == workShopId).ToList();
            if (abnormalCauseList.Count > 0)
            {
                var stopLineMax = abnormalCauseList.OrderByDescending(p => p.EndDate.HasValue ? p.EndDate.Value - p.BeginDate : DateTime.Now - p.BeginDate).FirstOrDefault();
                if (stopLineMax != null)
                {
                    andonLineStop.MaxStopLine = stopLineMax.Resource?.Name;
                    andonLineStop.MaxStopHour = (decimal)Math.Round((stopLineMax.EndDate.HasValue ? stopLineMax.EndDate.Value - stopLineMax.BeginDate : DateTime.Now - stopLineMax.BeginDate).TotalHours, 0);
                    andonLineStop.TotalStop = (decimal)Math.Round(abnormalCauseList.Sum(p => (p.EndDate.HasValue ? p.EndDate.Value - p.BeginDate : DateTime.Now - p.BeginDate).TotalHours), 0);
                }
            }
            
            return andonLineStop;
        }

        /// <summary>
        /// 获取安灯类型柏拉图统计
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual AndonTypePlato GetAndonTypePlato(double workShopId)
        {
            var andonManageList = DB.Query<AndonManage>().Where(p => p.WorkShopId == workShopId).ToList();
            var typeGroup = andonManageList.GroupBy(p => p.AndonTypeId).Select(p => new { type = p.Key, count = p.Count() }).OrderByDescending(p => p.count).ToDictionary(p => p.type, p => p.count);
            if (typeGroup.Count == 0)
            {
                return new AndonTypePlato();
            }
            if (typeGroup.Count == 1)
            {
                var andonTypeInfo1 = new AndonTypeInfo
                {
                    AndonType = RF.GetById<AndonType>(typeGroup.Keys.FirstOrDefault()).AndonTypeName,
                    Count = typeGroup.Values.FirstOrDefault(),
                };
                var platoList1 = new List<decimal>() { 100 };
                var andonTypePlato1 = new AndonTypePlato
                {
                    AndonTypeInfos = new List<AndonTypeInfo>() { andonTypeInfo1 },
                    Plato = platoList1,
                };
                return andonTypePlato1;
            }
            // 安灯类型前五
            var typeGroupFive = typeGroup.Count > 5 ? typeGroup.Take(5) : typeGroup.Take(typeGroup.Count - 1);

            // 安灯类型总数
            var typeGroupTotalCount = typeGroup.Sum(p => p.Value);
            // 除去前五类型总数
            var SixthCount = typeGroupTotalCount;

            // 柏拉图折线
            var platoList = new List<decimal>();
            // 前五累计统计数
            double platoCount = 0;

            var andonTypeInfos = new List<AndonTypeInfo>();
            var andonTypeInfoIds = typeGroupFive.Select(m => m.Key).Distinct().ToList();
            var andonTypeInfoList = andonTypeInfoIds.SplitContains(ids => { return DB.Query<AndonType>().Where(m => ids.Contains(m.Id)).ToList(); });
            typeGroupFive.ForEach(item =>
            {
                AndonTypeInfo andonTypeInfo = new AndonTypeInfo
                {
                    AndonType = andonTypeInfoList.FirstOrDefault(m => m.Id == item.Key)?.AndonTypeName,
                    Count = item.Value,
                };
                SixthCount -= item.Value;
                platoCount += item.Value;
                platoList.Add((decimal)(Math.Round(platoCount / typeGroupTotalCount, 2) * 100));
                andonTypeInfos.Add(andonTypeInfo);
            });
            andonTypeInfos.Add(new AndonTypeInfo { AndonType = "其他", Count = SixthCount });
            platoList.Add(100);
            var andonTypePlato = new AndonTypePlato
            {
                AndonTypeInfos = andonTypeInfos,
                Plato = platoList,
            };
            return andonTypePlato;

        }

        /// <summary>
        /// 根据车间获取安灯管理信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <param name="requestType">1车间返回产线，2产线返回工位</param>
        /// <returns></returns>
        public virtual List<AndonManageInfo> GetAndonManageListByWS(double workShopId, int requestType)
        {
            var andonManageList = DB.Query<AndonManage>()
                .Where(p => p.WorkShopId == workShopId && p.State != Andons.Enum.AndonManageState.Closed && p.State != Andons.Enum.AndonManageState.Cancel).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<AndonManageInfo> andonManageInfos = new List<AndonManageInfo>();
            andonManageList.ForEach(andonManage =>
            {
                var andonManageInfo = CreateAndonManageInfo(andonManage, requestType);
                andonManageInfos.Add(andonManageInfo);
            });
            return andonManageInfos;
        }

        /// <summary>
        /// 根据产线获取安灯管理信息
        /// </summary>
        /// <param name="wipId"></param>
        /// <param name="requestType">1车间返回产线，2产线返回工位</param>
        /// <returns></returns>
        public virtual List<AndonManageInfo> GetAndonManageListByWip(double wipId, int requestType)
        {
            var andonManageList = DB.Query<AndonManage>()
                .Where(p => p.WipResourceId == wipId && p.State != Andons.Enum.AndonManageState.Closed && p.State != Andons.Enum.AndonManageState.Cancel).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<AndonManageInfo> andonManageInfos = new List<AndonManageInfo>();
            andonManageList.ForEach(andonManage =>
            {
                var andonManageInfo = CreateAndonManageInfo(andonManage, requestType);
                andonManageInfos.Add(andonManageInfo);
            });
            return andonManageInfos;
        }

        /// <summary>
        /// 获取安灯信息
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="requestType">1车间返回产线，2产线返回工位</param>
        /// <returns></returns>
        private AndonManageInfo CreateAndonManageInfo(AndonManage andonManage, int requestType)
        {
            var dateTimeNow = RF.Find<AndonManage>().GetDbTime();
            var lastMinute = Math.Round((dateTimeNow - andonManage.TriggerTime).TotalMinutes % 60, 0);
            var lastHour = Math.Round((dateTimeNow - andonManage.TriggerTime).TotalMinutes / 60, 0);
            AndonManageInfo andonManageInfo = new AndonManageInfo
            {
                State = andonManage.State.ToLabel(),
                AndonManageCode = andonManage.AndonManageCode,
                AndonType = andonManage.AndonType.AndonTypeName,
                Andon = andonManage.Andon.AndonName,
                WipResource = requestType == 1 ? andonManage.WipResourceName : andonManage.StationName,
                EquipAccount = andonManage.EquipAccountName,
                TriggerTime = andonManage.TriggerTime.ToString(),
                Department = andonManage.Department,
                Handler = andonManage.Handler?.Name,
                LastTime = lastHour + "小时" + lastMinute + "分",
            };
            return andonManageInfo;
        }

        /// <summary>
        /// 根据产线获取安灯工位信息
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual List<AndonStationInfo> GetAndonManageStation(double wipId)
        {
            var andonManage = DB.Query<AndonManage>().Where(p => p.WipResourceId == wipId && p.State != Andons.Enum.AndonManageState.Closed && p.State != Andons.Enum.AndonManageState.Cancel).ToList();
            var groupByStation = andonManage.GroupBy(p => p.StationId).ToList();
            List<AndonStationInfo> andonStationInfos = new List<AndonStationInfo>();
            var stationIds = groupByStation.Where(m => m.Key != null).Select(m => m.Key).Distinct().ToList();
            var stations = stationIds.SplitContains(ids =>
            {
                return DB.Query<Tech.Stations.Station>().Where(m => ids.Contains(m.Id)).ToList();
            });

            groupByStation.ForEach(item =>
            {
                if (item.Key != null)
                {
                    var stationId = item.Key;
                    var station = stations.FirstOrDefault(m => m.Id == stationId)?.Name;
                    var standby = item.Count(p => p.State == Andons.Enum.AndonManageState.Standby);
                    var processing = item.Count(p => p.State == Andons.Enum.AndonManageState.Processing);
                    var toAccepted = item.Count(p => p.State == Andons.Enum.AndonManageState.ToAccepted);
                    AndonStationInfo andonStationInfo = new AndonStationInfo
                    {
                        StationName = station,
                        Standby = standby,
                        Processing = processing,
                        ToAccepted = toAccepted,
                    };
                    andonStationInfos.Add(andonStationInfo);
                }
            });
            return andonStationInfos;
        }
    }
}
