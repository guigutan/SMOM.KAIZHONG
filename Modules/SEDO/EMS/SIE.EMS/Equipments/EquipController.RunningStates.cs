using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.DataAuth;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.RunningStates;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.SMDC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备运行状态控制器
    /// </summary>
    public partial class EquipController : DomainController
    {
        /// <summary>
        /// 同步设备运行状态记录
        /// </summary>
        /// <returns>同步的记录条数</returns>
        public virtual int SyncEquipRunningStateRecord()
        {
            try
            {
                var equipAccountList = Query<EquipAccount>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                var equipAccountCodeList = new List<string>();
                var equipAccountIdAndCode = new Dictionary<string, double>();
                foreach (var item in equipAccountList)
                {
                    equipAccountCodeList.Add(item.Code);
                    equipAccountIdAndCode.Add(item.Code, item.Id);
                }

                var rtn1 = RT.Service.Resolve<EquipmentSmdcController>().GetDeviceRunStateByAssetCode(equipAccountCodeList.ToArray());
                var rtn2 = RT.Service.Resolve<EquipmentSmdcController>().GetDeviceIsOnLineByAssetCode(equipAccountCodeList.ToArray());

                var list = new EntityList<EquipRunningStateRecord>();
                foreach (var item in rtn1)
                {
                    list.Add(new EquipRunningStateRecord()
                    {
                        EquipAccountId = equipAccountIdAndCode[item.Key],
                        EquipOnLineState = rtn2[item.Key] ? EquipOnLineState.OnLine : EquipOnLineState.OffLine,
                        EquipRunningState = (EquipRunningState)item.Value,
                        AtWhatTime = RF.Find<EquipRunningStateRecord>().GetDbTime(),
                        PersistenceStatus = PersistenceStatus.New
                    });
                }
                if (list.Any())
                {
                    using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                    {
                        RF.Save(list);

                        foreach (var equipRunningStateRecord in list)
                        {
                            DB.Update<EquipAccount>()
                                .Set(x => x.EquipOnLineState, equipRunningStateRecord.EquipOnLineState)
                                .Where(x => x.Id == equipRunningStateRecord.EquipAccountId)
                                .Execute();
                        }

                        //提交事务
                        trans.Complete();
                    }
                }
                return list.Count;
            }
            catch (Exception ex)
            {
                throw new ValidationException("接口异常：".L10N()+"MDC接口同步数据失败!".L10N());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipAccountIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRunningStateRecord> GetEquipRunningStateRecords(List<double> equipAccountIds,
            DateTime startDate, DateTime endDate)
        {
            var exp = equipAccountIds.CreateContainsExpression<EquipRunningStateRecord>("x",
                EquipRunningStateRecord.EquipAccountIdProperty.Name);

            var query = Query<EquipRunningStateRecord>()
                .Where(exp)
                .Where(x => x.AtWhatTime >= startDate && x.AtWhatTime <= endDate);

            var runningStateRecords = query.ToList();

            //获取每台设备小于开始时间最晚的一个状态
            foreach (var id in equipAccountIds)
            {
                var equipRunningStateRecord = Query<EquipRunningStateRecord>()
                    .Where(x => x.EquipAccountId == id)
                    .Where(x => x.AtWhatTime < startDate)
                    .OrderByDescending(x => x.AtWhatTime)
                    .FirstOrDefault();

                if (equipRunningStateRecord != null)
                {
                    runningStateRecords.Add(equipRunningStateRecord);
                }
            }

            return runningStateRecords;
        }

        /// <summary>
        /// 查询设备运行状态记录
        /// </summary>
        /// <param name="criteria">设备运行状态记录的查询条件</param>
        /// <returns></returns>
        public virtual EntityList QueryEquipRunningStateRecords(EquipRunningStateRecordCriteria criteria)
        {
            var entityQueryer = Query<EquipRunningStateRecord>();
            if (criteria.EquipAccountId.HasValue)
            {
                entityQueryer.Where(x => x.EquipAccountId == criteria.EquipAccountId.Value);
            }

            if (criteria.EquipOnLineState.HasValue)
            {
                entityQueryer.Where(x => x.EquipOnLineState == criteria.EquipOnLineState.Value);
            }

            if (criteria.EquipRunningState.HasValue)
            {
                entityQueryer.Where(x => x.EquipRunningState == criteria.EquipRunningState.Value);
            }

            var query = entityQueryer.ToQuery();

            if (!criteria.EquipAccountId.HasValue)
            {
                query.QueryWithEquipAccountPermissions(EquipRunningStateRecord.EquipAccountIdProperty.Name);
            }

            return entityQueryer.Repository.QueryList(query, criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取维修定标
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="sortInfo"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountRepairStandard> GetEquipAccountRepairStandard(double id, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<EquipAccountRepairStandard>()
                .Where(p => p.EquipAccountId == id)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}