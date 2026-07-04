using SIE.Core.Common.Dao;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using System;

namespace SIE.RedCardManagment.RedCards.Dao
{
    public class RedCardDao : BaseDao<RedCard>
    {
        /// <summary>
        /// 修改红牌状态
        /// </summary>
        /// <param name="redCardId"></param>
        /// <returns></returns>
        public bool AlterRedCardStatus(double redCardId, RedCardState state)
        {
            var result =  DB.Update<RedCard>()
                .Set(c=> c.Status, state)
                .Set(c => c.ApplicantId, RT.IdentityId)
                .Set(c => c.ApplyTime, DateTime.Now)
                .Where(c => c.Id == redCardId).Execute();
            return result > 0;
        }

        public EntityList<RedCard> GetList(RedCardCriteria criteria)
        {
            var query = DB.Query<RedCard>();
            if (criteria.No.IsNotEmpty())
            {
                query.Where(c => c.No.Contains(criteria.No));
            }
            if (criteria.ItemSN.IsNotEmpty())
            {
                query.Join<ItemSnRetroactive>((red, isr) => red.Id == isr.RedCardId && (isr.SN.Contains(criteria.ItemSN) || red.ItemSN.Contains(criteria.ItemSN)));
            }
            if (criteria.ItemBatch.IsNotEmpty())
            {
                query.Where(c => c.ItemBatch.Contains(criteria.ItemBatch));
            }
            if (criteria.ApplyBillNo.IsNotEmpty())
            {
                query.Where(c => c.ApplyBillNo.Contains(criteria.ApplyBillNo));
            }
            query.WhereIf(criteria.Status.HasValue, p => p.Status == criteria.Status);
            query.WhereIf(criteria.AddWay.HasValue, p => p.AddWay == criteria.AddWay);
             if (criteria.ItemName.IsNotEmpty())
            {
                query.Join<Item>((red,item)=>red.ItemId==item.Id && ( item.Code.Contains(criteria.ItemName) || item.Name.Contains(criteria.ItemName)));
            }
            if (criteria.Supplier.IsNotEmpty())
            {
                query.Join<Supplier>((red, su) => red.SupplierId == su.Id && ( su.Code.Contains(criteria.Supplier) || su.Name.Contains(criteria.Supplier)));
            }
            if (criteria.ApplyTime.BeginValue.HasValue)
                query.Where(p => p.ApplyTime >= criteria.ApplyTime.BeginValue);
            if (criteria.ApplyTime.EndValue.HasValue)
                query.Where(p => p.ApplyTime <= criteria.ApplyTime.EndValue);
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

    }
}
