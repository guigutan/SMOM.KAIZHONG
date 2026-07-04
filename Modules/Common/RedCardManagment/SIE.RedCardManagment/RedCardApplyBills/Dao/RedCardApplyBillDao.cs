using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.RedCardApplyBills.Dao
{
    /// <summary>
    /// 红牌申请单Dao
    /// </summary>
    public class RedCardApplyBillDao : BaseDao<RedCardApplyBill>
    {
        /// <summary>
        /// 获取红牌申请单列表
        /// </summary>
        /// <param name="criteria">红牌申请单查询实体</param>
        /// <returns>红牌申请单列表</returns>
        public virtual EntityList<RedCardApplyBill> GetApplyBills(RedCardApplyBillCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria), "查询实体参数不能为空".L10N());
            var query = Query();
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.ItemId.HasValue)
                query.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.SupplierId.HasValue)
                query.Where(p => p.SupplierId == criteria.SupplierId);
            if (criteria.BillStatus.HasValue)
                query.Where(p => p.BillStatus == criteria.BillStatus);
            if (criteria.ApplyType.HasValue)
                query.Where(p => p.ApplyType == criteria.ApplyType);
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            if (criteria.OrderInfoList != null && criteria.OrderInfoList.Count > 0)
                query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
