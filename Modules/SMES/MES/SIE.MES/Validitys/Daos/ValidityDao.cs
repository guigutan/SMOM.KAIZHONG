using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Validitys.Daos
{
    /// <summary>
    /// 有效期标准维护Dao
    /// </summary>
    public class ValidityDao : BaseDao<ValidityStandard>
    {
        /// <summary>
        /// 查询有效期标准维护
        /// </summary>
        /// <returns></returns>
        public EntityList QueryValidityStandards(ValidityStandardCriteria criteria)
        {
            var query = Query();
            if (criteria == null)
            {
                return new EntityList<ValidityStandard>();
            }
            if (criteria.ItemId.HasValue)
            {
                query.Where(p => p.ItemId == criteria.ItemId);
            }
            if (criteria.ItemType.HasValue) 
            {
                query.Where(p => p.Item.Type == criteria.ItemType);
            }
            if (criteria.Effective.BeginValue.HasValue)
            {
                query.Where(p => p.Effective >= criteria.Effective.BeginValue);
            }
            if (criteria.Effective.EndValue.HasValue)
            {
                query.Where(p => p.Effective <= criteria.Effective.EndValue);
            }
            if (criteria.Expiration.BeginValue.HasValue)
            {
                query.Where(p => p.Expiration >= criteria.Expiration.BeginValue);
            }
            if (criteria.Expiration.EndValue.HasValue)
            {
                query.Where(p => p.Expiration <= criteria.Expiration.EndValue);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料Ids获取有效期标准规则
        /// </summary>
        /// <param name="itemIds">物料ids</param>
        /// <param name="ids">数据ids</param>
        /// <returns></returns>
        public EntityList<ValidityStandard> GetValidityStandardByItemIds(List<double> itemIds, List<double> ids)
        {
            EntityList<ValidityStandard> validityStandards = new EntityList<ValidityStandard>();
            itemIds.SplitDataExecute(tempItemIds =>
            {
                ids.SplitDataExecute(tempIds =>
                {
                    var query = Query().Where(p => tempItemIds.Contains(p.ItemId) && !tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    validityStandards.AddRange(query);
                });
            });
            return validityStandards;
        }

        /// <summary>
        /// 根据物料id和扩展属性获取当前时间的有效期
        /// </summary>
        /// <param name="itemId">物料id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        public ValidityStandard GetValidityStandard(double itemId, string itemExtProp)
        {
            var now = DateTime.Now;
            return Query().Where(p => p.ItemId == itemId && p.ItemExtProp ==  itemExtProp && ((p.Expiration == null && p.Effective <= now) || (p.Expiration != null && p.Effective <= now && p.Expiration >= now))).FirstOrDefault();
        }
    }
}
