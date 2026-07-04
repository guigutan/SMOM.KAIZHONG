using SIE.Core.Common.Dao;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Items;
using SIE.LES.Interfaces;
using SIE.ShipPlan;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.StockPlans.Dao
{
    /// <summary>
    /// 备货计划DAO
    /// </summary>
    public class StockPlansDao : BaseDao<StockPlan>
    {
        /// <summary>
        /// 获取备货计划数据
        /// </summary>
        /// <param name="criteria">备货计划查询实体</param>
        /// <returns>备货计划数据</returns>
        public virtual EntityList<StockPlan> GetStockPlans(StockPlanCriteria criteria)
        {
            var query = Query().Where(p => p.OrderType == OrderType.WorkFeed && p.SourceType == DeliverySourceType.External);

            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.WarehouseId.HasValue)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
            }
            if(criteria.TargetWarehouseId.HasValue)
            {
                query.Where(p => p.TargetWarehouseId == criteria.TargetWarehouseId.Value);
            }
            if (!string.IsNullOrEmpty(criteria.State))
            {
                var criteriaState = new List<int>();
                criteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.State));
            }
            if (criteria.OrderType.HasValue)
            {
                query.Where(p => p.OrderType == criteria.OrderType.Value);
            }
            if (criteria.EnterpriseId.HasValue)
            {
                query.Where(p => p.EnterpriseId == criteria.EnterpriseId.Value);
            }
            if (criteria.CustomerId.HasValue)
            {
                query.Where(p => p.CustomerId == criteria.CustomerId.Value);
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
            }
            if (criteria.ItemCode.IsNotEmpty() || criteria.ItemName.IsNotEmpty())
            {
                query.Join<Item>((x, y) => y.Id == x.ItemId)
                     .WhereIf<Item>(criteria.ItemCode.IsNotEmpty(), (x, y) => y.Code.Contains(criteria.ItemCode))
                     .WhereIf<Item>(criteria.ItemName.IsNotEmpty(), (x, y) => y.Name.Contains(criteria.ItemName));
            }
            if (criteria.DeliveryDate.BeginValue.HasValue)
            {
                query.Where(p => p.DeliveryDate >= criteria.DeliveryDate.BeginValue.Value);
            }
            if (criteria.DeliveryDate.EndValue.HasValue)
            {
                query.Where(p => p.DeliveryDate <= criteria.DeliveryDate.EndValue.Value);
            }
            if (criteria.IsFilter)
            {
                query.Where(p => p.State != DeliveryState.Cancel && p.State != DeliveryState.Finished);
            }
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取备料计划
        /// </summary>
        /// <param name="nos">单号</param>
        /// <returns>备料计划</returns>
        public virtual EntityList<StockPlan> GetStockPlans(List<string> nos)
        {
            return Query().Where(f => nos.Contains(f.No)).ToList();
        }

        /// <summary>
        /// 备料计划ID集合
        /// </summary>
        /// <param name="ids">备料ID</param>
        /// <returns></returns>
        public virtual EntityList<StockPlan> GetStockPlansByIds(List<double> ids)
        {
            return RT.Service.Resolve<StockOrderController>().GetStockPlansByIds(ids);
        }

        /// <summary>
        /// 获取备料计划
        /// </summary>
        /// <param name="no">计划号</param>
        /// <param name="elo">贪婪</param>
        /// <returns>备料计划</returns>
        public virtual StockPlan GetStockPlan(string no, EagerLoadOptions elo)
        {
            return Query().Where(p => p.No == no).FirstOrDefault(elo);
        }
    }
}
