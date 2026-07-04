using Castle.Core.Logging;
using NPOI.SS.Formula.Functions;
using SIE.Core.Common;
using SIE.CSM.Common;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EventMessages;
using SIE.EventMessages.QMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 检验特性控制器
    /// </summary>
    public class ItemInspCharacteristicsController : DomainController
    {
        /// <summary>
        /// 获取物料检验特性信息列表
        /// </summary>
        /// <param name="criteria">物料检验特性查询实体</param>
        /// <returns>物料检验特性信息列表</returns>
        public virtual EntityList QueryItemInspCharacteristis(ItemInspCharacteristicsCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = Query<ItemInspCharacteristics>();
            if (criteria.ItemId.HasValue)
                query.Where(p => p.ItemId == criteria.ItemId);
            if (!criteria.ItemName.IsNullOrEmpty())
                query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            if (criteria.SupplierId.HasValue)
                query.Where(p => p.SupplierId == criteria.SupplierId);
            if (!criteria.SupplierName.IsNullOrEmpty())
                query.Where(p => p.Supplier.Name.Contains(criteria.SupplierName));
            if (criteria.SupplierState.HasValue)
                query.Where(p => p.SupplierState == criteria.SupplierState);

            if (criteria.OrderInfoList != null && criteria.OrderInfoList.Count > 0)
                query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 供应商选择物料后往物料检验特性表插入数据
        /// </summary>
        /// <param name="data">供应商选择的物料列表</param>
        public virtual void SaveItemInspCharacteristics(EntityList data)
        {
            foreach (var item in data)
            {
                SupplierItem supplierItem = item as SupplierItem;
                var result = Query<ItemInspCharacteristics>().Where(p => p.SupplierId == supplierItem.SupplierId && p.ItemId == supplierItem.ItemId).FirstOrDefault();
                if (result != null)
                {
                    if (result.SupplierState != State.Enable)
                    {
                        result.SupplierState = State.Enable;
                        RepositoryFactory.Save(result);
                    }

                    continue;
                }

                ItemInspCharacteristics itemInspCharacteristics = new ItemInspCharacteristics();
                itemInspCharacteristics.SupplierId = supplierItem.SupplierId;
                itemInspCharacteristics.ItemId = supplierItem.ItemId;

                Supplier supplier = AppRuntime.Service.Resolve<SupplierController>().GetSupplier(supplierItem.SupplierId);
                itemInspCharacteristics.SupplierState = supplier.State;
                RepositoryFactory.Save(itemInspCharacteristics);
            }
        }

        /// <summary>
        /// 保存批量设置信息
        /// </summary>
        /// <param name="list">需要批量设置的物料检验特性维护列表</param>
        /// <param name="data">物料检验特性维护</param>
        public virtual void SaveBatchSettings(List<ItemInspCharacteristics> list, ItemInspCharacteristics data)
        {
            if (data.RecurringInspection)
                list.ForEach(a => { a.RecurringInspection = data.RecurringInspection; a.PeriodType = data.PeriodType; a.IntervalPeriod = data.IntervalPeriod; a.FactoryInspection = false; a.ConfirmInspection = false; a.ForceSupplierShipBill = data.ForceSupplierShipBill; a.PersistenceStatus = PersistenceStatus.Modified; RepositoryFactory.Save(a); });
            else if (data.FactoryInspection)
                list.ForEach(a => { a.FactoryInspection = data.FactoryInspection; a.RecurringInspection = false; a.PeriodType = null; a.IntervalPeriod = null; a.ConfirmInspection = false; a.ForceSupplierShipBill = data.ForceSupplierShipBill; a.PersistenceStatus = PersistenceStatus.Modified; RepositoryFactory.Save(a); });
            else if (data.ConfirmInspection)
                list.ForEach(a => { a.ConfirmInspection = data.ConfirmInspection; a.RecurringInspection = false; a.PeriodType = null; a.IntervalPeriod = null; a.FactoryInspection = false; a.ForceSupplierShipBill = data.ForceSupplierShipBill; a.PersistenceStatus = PersistenceStatus.Modified; RepositoryFactory.Save(a); });
        }

        /// <summary>
        /// 根据供应商+物料获取检验特性信息
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>与供应商+物料关联的检验特性信息</returns>
        public virtual ItemInspCharacteristics GetInspCharacteristics(double supplierId, double itemId)
        {
            return Query<ItemInspCharacteristics>().Where(p => p.SupplierId == supplierId && p.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 根据供应商+物料获取检验特性信息
        /// </summary>
        /// <param name="supplierIds">供应商Id</param>
        /// <param name="itemIds">物料Id</param>
        /// <returns>与供应商+物料关联的检验特性信息</returns>
        public virtual EntityList<ItemInspCharacteristics> GetInspCharacteristics(List<double> supplierIds, List<double> itemIds)
        {
            supplierIds = supplierIds.Distinct().ToList();
            return itemIds.Distinct().SplitContains(ids =>
             {
                 return Query<ItemInspCharacteristics>().Where(p => supplierIds.Contains(p.SupplierId) && ids.Contains(p.ItemId)).ToList();
             });
        }

        /// <summary>
        /// 获取免检清单物料
        /// </summary>
        /// <param name="ItemIds">物料ID集合</param>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="elo">是否贪婪加载</param>
        /// <returns></returns>
        public virtual List<SimpleNoIqcItem> GetNoIqcItems(List<double> ItemIds, double supplierId, EagerLoadOptions elo = null)
        {
            //var rst = new List<double>();
            if (ItemIds.Count == 0)
            {
                return new List<SimpleNoIqcItem>();
            }
            return DataProcessEx.SplitContains(ItemIds, ids =>
            {
                var query = Query<ItemInspCharacteristics>().Where(p => ids.Contains(p.ItemId) && p.SupplierId == supplierId && p.InspectionFree).Select((x) => new
                {
                    x.ItemId,
                    x.EffectiveStartTime,
                    x.EffectiveEndTime,
                    x.InspectionFree
                });
                return query.ToList<SimpleNoIqcItem>().ToList();
            });
        }

        /// <summary>
        /// 根据供应商获取检验特性信息
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <returns>与供应商+物料关联的检验特性信息</returns>
        public virtual EntityList<ItemInspCharacteristics> GetInspCharacteristicsList(double supplierId)
        {
            return Query<ItemInspCharacteristics>().Where(p => p.SupplierId == supplierId).ToList();
        }
    }
}