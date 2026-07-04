using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检控制器
    /// </summary>
    public class StationCheckController : DomainController
    {
        /// <summary>
        /// 获取工位点检设备列表
        /// </summary> 
        /// <param name="stationId">工位ID</param>  
        /// <param name="pagingInfo">分页信息</param> 
        /// <returns>工位点检设备列表</returns>
        public virtual EntityList<CheckEquipment> GetCheckEquipmentList(double stationId, PagingInfo pagingInfo)
        {
            return Query<CheckEquipment>().Where(p => p.StationId == stationId).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取某工位点检设备列表
        /// </summary>       
        /// <param name="stationId">工位ID</param>  
        /// <returns>工位点检设备列表</returns>
        public virtual EntityList<CheckEquipment> GetCheckEquipmentList(double stationId)
        {
            return Query<CheckEquipment>().Where(p => p.StationId == stationId).ToList();
        }

        /// <summary>
        /// 获取所有工位点检设备明细列表
        /// </summary>         
        /// <param name="stationId">工位ID</param>  
        /// <returns>工位点检设备明细列表</returns>
        private EntityList<CheckEquipmentDetail> GetCheckEquipmentDetails(double stationId)
        {
            var allCheckEquipments = GetCheckEquipmentList(stationId);
            var allCheckEquipmentIds = allCheckEquipments.Select(p => p.Id).ToList();
            return GetCheckEquipmentDetailList(allCheckEquipmentIds);
        }

        /// <summary>
        /// 获取工位点检设备明细列表
        /// </summary>
        /// <param name="checkEquipmentIds">点检设备Id</param>
        /// <returns>工位点检设备明细</returns>
        public virtual EntityList<CheckEquipmentDetail> GetCheckEquipmentDetailList(IEnumerable<double> checkEquipmentIds)
        {
            var eagerLoad = new EagerLoadOptions().LoadWith(CheckEquipmentDetail.ProjectProperty);
            return Query<CheckEquipmentDetail>().Where(p => checkEquipmentIds.Contains(p.CheckEquipmentId)).ToList(null, eagerLoad);
        }

        /// <summary>
        /// 获取所有设备点检信息
        /// </summary>
        /// <param name="stationId">工位ID</param>  
        /// <param name="stationCheckInfos">点检集合</param>
        private void GetEquipmentChecks(double stationId, ObservableCollection<StationCheck> stationCheckInfos, DateTime data)
        {
            var checkEquipmentDetails = GetCheckEquipmentDetails(stationId);
            foreach (var checkEquipmentDetail in checkEquipmentDetails)
            {
                bool state = false;
                var stationCheckResult = GetStationCheckResult(checkEquipmentDetail.Id, stationId, CheckType.Equipment, data);
                if (stationCheckResult != null)
                    state = stationCheckResult.IsCheck;
                var stationCheck = new StationCheck()
                {
                    StationId = stationId,
                    CheckItemId = checkEquipmentDetail.Id,
                    WorkType = "设备工器具点检",
                    CheckType = CheckType.Equipment,
                    Code = checkEquipmentDetail.Project?.Code,
                    Name = checkEquipmentDetail.Project?.Name,
                    DemandQty = null,
                    State = state,
                };
                stationCheckInfos.Add(stationCheck);
            }
        }

        /// <summary>
        /// 获取所有物料点检信息
        /// </summary>
        /// <param name="stationId">工位ID</param>  
        /// <param name="stationCheckInfos">所有点检集合</param>
        private void GetItemChecks(double stationId, ObservableCollection<StationCheck> stationCheckInfos, DateTime data)
        {
            var checkItems = GetCheckItemList(stationId);
            foreach (var checkItem in checkItems)
            {
                bool state = false;
                var item = checkItem.Item;
                var stationCheckResult = GetStationCheckResult(checkItem.Id, stationId, CheckType.Item, data);
                if (stationCheckResult != null)
                    state = stationCheckResult.IsCheck;
                var stationCheck = new StationCheck()
                {
                    StationId = stationId,
                    CheckItemId = checkItem.Id,
                    WorkType = "物料点检",
                    CheckType = CheckType.Item,
                    Code = item?.Code,
                    Name = item?.Name,
                    DemandQty = checkItem.DemandQty,
                    State = state,
                };
                stationCheckInfos.Add(stationCheck);
            }
        }

        /// <summary>
        /// 获取某工位点检集合
        /// </summary>
        /// <param name="stationId">工位ID</param>  
        /// <returns>所有工位点检集合</returns>
        public virtual ObservableCollection<StationCheck> GetStationChecks(double stationId)
        {
            ObservableCollection<StationCheck> stationCheckInfos = new ObservableCollection<StationCheck>();
            var data = RF.Find<CheckItem>().GetDbTime();
            GetItemChecks(stationId, stationCheckInfos, data.Date);
            GetEquipmentChecks(stationId, stationCheckInfos, data.Date);

            return stationCheckInfos;
        }

        /// <summary>
        /// 获取工位物料点检列表
        /// </summary> 
        /// <param name="stationId">工位ID</param>  
        /// <param name="pagingInfo">分页信息</param> 
        /// <returns>工位物料点检列表</returns>
        public virtual EntityList<CheckItem> GetCheckItemList(double stationId, PagingInfo pagingInfo)
        {
            return Query<CheckItem>().Where(p => p.StationId == stationId).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取工位物料点检列表
        /// </summary>    
        /// <param name="stationId">工位ID</param>  
        /// <returns>工位物料点检列表（贪婪加载物料信息）</returns>
        public virtual EntityList<CheckItem> GetCheckItemList(double stationId)
        {
            var eagerLoad = new EagerLoadOptions().LoadWith(CheckItem.ItemProperty);
            return Query<CheckItem>().Where(p => p.StationId == stationId).ToList(null, eagerLoad);
        }

        /// <summary>
        /// 保存工位点检状态
        /// </summary>
        /// <param name="stationCheck">工位点检数据</param>
        /// <param name="state">状态</param>
        public virtual void SaveCheckItemResult(StationCheck stationCheck, bool? state)
        {
            if (stationCheck == null)
                return;
            var date = RF.Find<CheckItem>().GetDbTime();
            StationCheckResult checkRsult = null;
            checkRsult = GetStationCheckResult(stationCheck.CheckItemId, stationCheck.StationId, stationCheck.CheckType, date.Date);
            if (checkRsult == null)
            {
                checkRsult = new StationCheckResult()
                {
                    CheckItemId = stationCheck.CheckItemId,
                    StationId = stationCheck.StationId,
                    CheckType = stationCheck.CheckType,
                    CheckDate = date.Date,
                    IsCheck = state.Value,
                    PersistenceStatus = PersistenceStatus.New
                };
            }
            else
            {
                checkRsult.IsCheck = state.Value;
                checkRsult.PersistenceStatus = PersistenceStatus.Modified;
            }

            RF.Save(checkRsult);
        }

        /// <summary>
        /// 获取工位点检结果
        /// </summary>
        /// <param name="checkItemId">点检项目Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="checkType">检验类型</param>
        /// <param name="data">日期</param>
        /// <returns>工位点检结果</returns>
        public virtual StationCheckResult GetStationCheckResult(double checkItemId, double stationId, CheckType checkType, DateTime data)
        {
            return Query<StationCheckResult>().Where(p => p.CheckItemId == checkItemId && p.StationId == stationId && p.CheckType == checkType && p.CheckDate == data).FirstOrDefault();
        }

        public virtual EntityList<StationCheckResult> GetStationCheckResults(double stationId, DateTime date)
        {
            return Query<StationCheckResult>().Where(p => p.StationId == stationId && p.CheckDate == date).ToList();
        }

        public virtual void SaveStationOnDuty(StationOnDuty onduty)
        {
            var result = GetStationOnDuty(onduty.OnDutyId, onduty.StationId, onduty.OnDutyDate);
            if (result == null)
                result = new StationOnDuty() { StationId = onduty.StationId, ActualOnDutyId = onduty.ActualOnDutyId, OnDutyId = onduty.OnDutyId, OnDutyDate = onduty.OnDutyDate };
            result.ActualOnDutyId = onduty.ActualOnDutyId;
            RF.Save(result);
        }

        /// <summary>
        /// 根据当班员工获取出勤信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="stationId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual StationOnDuty GetStationOnDuty(double? employeeId, double stationId, DateTime date)
        {
            return Query<StationOnDuty>().Where(p => p.OnDutyId == employeeId && p.StationId == stationId && p.OnDutyDate == date).FirstOrDefault();
        }
    }
}
