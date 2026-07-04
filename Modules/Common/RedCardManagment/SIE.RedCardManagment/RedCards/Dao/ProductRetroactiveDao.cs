using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.RedCardManagment.RedCards.Dao
{
    /// <summary>
    /// 红牌管理关联产品 ProductRetroactiveDao DAO
    /// </summary>
    public class ProductRetroactiveDao : BaseDao<ProductRetroactive>
    {

        /// <summary>
        /// 获取监控实体的，实体仓储
        /// </summary>
        /// <param name="redCardId"></param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns></returns>
        public EntityList<ProductRetroactive> GetList(double redCardId, EagerLoadOptions eagerLoadOptions = null)
        {
            return DB.Query<ProductRetroactive>().Where(c => c.RedCardId == redCardId).ToList(null, eagerLoadOptions);
        }

        /// <summary>
        /// 获取选择的ids产品追溯清单
        /// </summary>
        /// <returns></returns>
        public EntityList<ProductRetroactive> GetProductListFroIds(double[] ids, EagerLoadOptions eagerLoadOptions = null)
        {
            return DB.Query<ProductRetroactive>().Where(c => ids.Contains(c.Id)).ToList(null, eagerLoadOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EntityList<ProductRetroactive> GetList(ProductRetroactiveCriteria criteria)
        {
            var query = DB.Query<ProductRetroactive>()
                .Where(c => c.RedCardId == criteria.RedCardId);
            if (criteria.SN.IsNotEmpty())
            {
                query.Where(c => c.SN == criteria.SN);
            }
            else if (criteria.SnList.IsNotEmpty())
            {
                query.Where(c => criteria.SnList.Contains(c.SN));
            }
            else if (criteria.ItemBatch.IsNotEmpty())
            {
                query.Where(c => c.ItemBatch == criteria.ItemBatch);
            }
            else if (criteria.BatchList.IsNotEmpty())
            {
                query.Where(c => criteria.BatchList.Contains(c.ItemBatch));
            }
            if (criteria.ApplyTime.BeginValue.HasValue)
                query.Where(p => p.ApplyTime >= criteria.ApplyTime.BeginValue);
            if (criteria.ApplyTime.EndValue.HasValue)
                query.Where(p => p.ApplyTime <= criteria.ApplyTime.EndValue);
            query.WhereIf(criteria.ProductSn.IsNotEmpty(), p => p.ProductSn == criteria.ProductSn);
            query.WhereIf(criteria.ProductSn.IsNotEmpty(), p => p.ProductSn == criteria.ProductSn);
            query.WhereIf(criteria.ProductId.HasValue && criteria.ProductId != 0, p => p.ProductId == criteria.ProductId);
            query.WhereIf(criteria.WorkNo.IsNotEmpty(), p => p.WorkNo == criteria.WorkNo);
            query.WhereIf(criteria.Status.HasValue, p => p.Status == criteria.Status);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据红牌ID获取关联表数据
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public EntityList<ProductRetroactive> GetProductRetroactiveInventory(double redCardId)
        {
            return DB.Query<ProductRetroactive>().Where(c => c.RedCardId == redCardId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public bool SetProductRedCardStatus(double productId, RedCardState state)
        {
            var result = DB.Update<BatchRetroactive>().Set(c => c.Status, state).Set(c => c.ApplicantId, RT.IdentityId).Where(c => c.Id == productId).Execute();
            return result > 0;
        }

        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public void SetAllProductRedCardStatus(double redCardId, RedCardState state)
        {
            var result = DB.Query<ProductRetroactive>().Where(c => c.RedCardId == redCardId).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Status = state;
                result[i].ApplicantId = RT.IdentityId;
                result[i].ApplyTime = DateTime.Now;
            }
            RF.Save(result);
        }

        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public void SetProductRedCardStatusForItemSN(List<string> itemSnList, RedCardState state, double redCardId)
        {
            var result = DB.Query<ProductRetroactive>().Where(c => itemSnList.Contains(c.SN) && c.RedCardId == redCardId).ToList();
            if (result == null) return;
            UpDataProductRedCardStatus(result, state);
        }
        
        /// <summary>
        /// 修改产品红牌状态（根据关联锁定SN）
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public void SetProductRedCardStatusAll(RedCardState state, double redCardId)
        {
            var result = DB.Query<ProductRetroactive>().Where(c => c.RedCardId == redCardId).ToList();
            if (result == null) return;
            UpDataProductRedCardStatus(result, state);
        }

        /// <summary>
        /// 根据物料批次修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public void SetProductRedCardStatusForItemBatch(List<string> itemBatchList, RedCardState state, double redCardId)
        {
            var result = DB.Query<ProductRetroactive>().Where(c => itemBatchList.Contains(c.ItemBatch) && c.RedCardId == redCardId).ToList();
            if (result == null) return;
            UpDataProductRedCardStatus(result, state);
            
        }


        /// <summary>
        /// 更新产品红牌状态
        /// </summary>
        /// <param name="result"></param>
        /// <param name="state"></param>
        public void UpDataProductRedCardStatus(EntityList<ProductRetroactive> result, RedCardState state)
        {
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Status = state;
                result[i].ApplicantId = RT.IdentityId;
                result[i].ApplyTime = DateTime.Now;
            }
            RF.Save(result);

        }
    }
}
