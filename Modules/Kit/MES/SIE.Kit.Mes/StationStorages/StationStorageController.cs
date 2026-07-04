using Newtonsoft.Json;
using SIE.Core.Logs;
using SIE.Data;
using SIE.Domain;
using SIE.EventMessages.StationStorage;
using System;
using System.Linq;
using System.Text;

namespace SIE.Kit.MES.StationStorages
{
    /// <summary>
    /// 工位库存控制器
    /// </summary>
    public class StationStorageController : DomainController, IStationItemStore
    {
        /// <summary>
        /// 获取工位库存列表
        /// </summary>
        /// <param name="criteria">工位库存查询实体</param>
        /// <returns>工位库存列表</returns>
        public virtual EntityList<StationStorage> GetStationStorages(StationStorageCriteria criteria)
        {
            var query = Query<StationStorage>();
            if (criteria.StationId.HasValue)
                query.Where(p => p.StationId == criteria.StationId);
            if (criteria.WorkOrderId.HasValue)
                query.Exists<WoStationStorage>((x, y) => y.Where(p => p.StationStorageId == x.Id && p.WorkOrderId == criteria.WorkOrderId));
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更新工位物料库存
        /// </summary>
        /// <param name="storeEvent">工位物料库存事件</param>
        public void UpdateStationStorage(StationItemStoreEvent storeEvent)
        {
            SaveUpdateStationStorageLog(storeEvent);
            ////工位工单库存
            var woStorage = GetWoStationStorage(storeEvent.WorkOrderId, storeEvent.StationId);
            if (woStorage == null)
            {
                var storage = GetStationStorage(storeEvent.StationId);
                if (storage == null)
                {
                    storage = new StationStorage() { StationId = storeEvent.StationId };
                    storage.GenerateId();
                    RF.Save(storage);
                }
                woStorage = new WoStationStorage() { WorkOrderId = storeEvent.WorkOrderId, StationStorage = storage };
                woStorage.GenerateId();
                RF.Save(woStorage);
            }
            if (UpdateStationItemStorage(storeEvent, woStorage.Id) == 0)
            {
                //更新失败，可能没有工位库存或者没有工位物料库存，新增
                RF.Save(new StationItemStorage()
                {
                    ItemId = storeEvent.ItemId,
                    ActStoreQty = storeEvent.ActStoreQty,
                    BudgetQty = storeEvent.BudgetQty,
                    SendingQty = storeEvent.SendingQty,
                    WoStorage = woStorage
                });
            }
        }

        /// <summary>
        /// 保存更新工位物料库存日志
        /// </summary>
        /// <param name="storeEvent">工位物料库存事件</param>
        private void SaveUpdateStationStorageLog(StationItemStoreEvent storeEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(storeEvent);
                var inputValue = "工位物料库存事件:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IStationItemStore",
                    Method = "UpdateStationStorage",
                    ControllerName = "StationStorageController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新工位物料库存
        /// </summary>
        /// <param name="storeEvent">工位物料库存事件</param>
        /// <param name="woStorageId">工单工位库存ID</param>
        /// <returns>更新结果</returns>
        public virtual int UpdateStationItemStorage(StationItemStoreEvent storeEvent, double woStorageId)
        {
            int updateCount = 0;
            if (storeEvent == null)
            {
                return updateCount;
            }    
            using (var dba = DbAccesserFactory.Create(MesCoreEntityDataProvider.ConnectionStringName))
            {                
                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE WIP_ITEM_STORAGE SET ");
                sb.Append("ACT_STORE_QTY = ACT_STORE_QTY + " + storeEvent.ActStoreQty + ",");
                sb.Append("BUDGET_QTY = BUDGET_QTY + " + storeEvent.BudgetQty + ",");
                sb.Append("SENDING_QTY = (CASE WHEN SENDING_QTY + " + storeEvent.SendingQty + " < 0 THEN 0 ELSE  SENDING_QTY + " + storeEvent.SendingQty + " END),");
                sb.Append("UPDATE_BY = " + RT.IdentityId + ",");
                sb.Append("UPDATE_DATE = " + dba.SqlDialect.DbTimeValueSql() + " ");
                sb.Append(" WHERE WO_STORAGE_ID = "+ woStorageId + " AND ITEM_ID = "+ storeEvent.ItemId + " AND IS_PHANTOM = 0 ");
                if(RT.InvOrg!= null)
                {
                    sb.Append(" AND INV_ORG_ID = " + RT.InvOrg + " ");
                }
                updateCount = dba.ExecuteNonQuery(sb.ToString());
            }
            return updateCount;

        }

        /// <summary>
        /// 获取工位库存
        /// </summary>
        /// <param name="stationId">工位ID</param> 
        /// <returns>工位库存</returns>
        public virtual StationStorage GetStationStorage(double stationId)
        {
            return Query<StationStorage>().Where(p => p.StationId == stationId).FirstOrDefault();
        }

        /// <summary>
        /// 获取工位库存
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="stationId">工位ID</param> 
        /// <returns>工位库存</returns>
        public virtual WoStationStorage GetWoStationStorage(double workOrderId, double stationId)
        {
            return Query<WoStationStorage>()
                .Join<StationStorage>((x, y) => x.StationStorageId == y.Id && y.StationId == stationId)
                .Where(p => p.WorkOrderId == workOrderId)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取工位物料库存
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>工位物料库存</returns>
        public virtual StationItemStorage GetStationItemStorage(double workOrderId, double stationId, double itemId)
        {
            return Query<StationItemStorage>()
                .Join<WoStationStorage>((x, y) => x.WoStorageId == y.Id && y.WorkOrderId == workOrderId)
                .Join<WoStationStorage, StationStorage>((x, y) => x.StationStorageId == y.Id && y.StationId == stationId)
                .Where(p => p.ItemId == itemId)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取或创建工位物料库存
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>工位物料库存</returns>
        public virtual StationItemStorage GetOrCreateStationItemStorage(double workOrderId, double stationId, double itemId)
        {
            var itemStorage = GetStationItemStorage(workOrderId, stationId, itemId);
            if (itemStorage == null)
            {
                ////工位库存
                var storage = GetStationStorage(stationId);
                if (storage == null)
                {
                    storage = new StationStorage() { StationId = stationId };
                    storage.GenerateId();
                }
                ////工单工位库存
                var woStorage = storage.WoStorageList.FirstOrDefault(p => p.WorkOrderId == workOrderId);
                if (woStorage == null)
                {
                    woStorage = new WoStationStorage() { StationStorage = storage, WorkOrderId = workOrderId };
                    woStorage.GenerateId();
                }
                ////工位工单物料库存
                itemStorage = new StationItemStorage()
                {
                    ItemId = itemId,
                    WoStorage = woStorage,
                    ActStoreQty = 0,
                    SendingQty = 0,
                    BudgetQty = 0,
                };
                itemStorage.GenerateId();
                RF.Save(storage);
                RF.Save(woStorage);
                RF.Save(itemStorage);
                return RF.GetById<StationItemStorage>(itemStorage.Id);
            }
            return itemStorage;
        }

        /// <summary>
        /// 判断是否存在相同的工位物料库存
        /// </summary>
        /// <param name="excludeId">排除工位物料库存ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistSameItemStorage(double excludeId, double stationId, double workOrderId, double itemId)
        {
            return Query<StationItemStorage>()
                .Join<WoStationStorage>((x, y) => x.WoStorageId == y.Id && y.WorkOrderId == workOrderId)
                .Join<WoStationStorage, StationStorage>((x, y) => x.StationStorageId == y.Id && y.StationId == stationId)
                .Where(p => p.ItemId == itemId && p.Id != excludeId)
                .Count() > 0;
        }
    }
}