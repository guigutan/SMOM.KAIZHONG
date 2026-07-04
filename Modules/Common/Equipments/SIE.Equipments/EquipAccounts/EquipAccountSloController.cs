using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 缸槽控制器
    /// </summary>
    public class EquipAccountSloController : DomainController
    {
        /// <summary>
        /// 获取缸槽实体列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSlot> GetPcbSlots(EquipAccountSloCriteria criteria, PagingInfo info)
        {
            var query = GetPcbSlotsQuery(criteria);
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取缸槽查询条件
        /// </summary>
        /// <param name="criteria">缸槽查询实体</param>
        /// <returns></returns>
        public virtual IEntityQueryer<EquipAccountSlot> GetPcbSlotsQuery(EquipAccountSloCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria), "查询实体参数不能为空".L10N());
            var query = base.Query<EquipAccountSlot>().Join<EquipAccountSlot, EquipAccount>((p, e) => p.EquipAccountId == e.Id);

            if (criteria.UseStateList != null && criteria.UseStateList.Count > 0)
            {
                query.Where<EquipAccount>((p, e) => criteria.UseStateList.Contains(e.UseState));
            }
            if (criteria.StateList != null && criteria.StateList.Count > 0)
            {
                query.Where<EquipAccount>((p, e) => criteria.StateList.Contains(e.State));
            }
            if (criteria.OrderInfoList != null && criteria.OrderInfoList.Count > 0)
                query.OrderBy(criteria.OrderInfoList);
            return query;
        }

        /// <summary>
        /// 根据设备台账ID获取钢槽列表
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="orderInfos"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSlot> GetEquipAccountSlots(double equipId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountSlot>();

            query.Where(p => p.EquipAccountId == equipId);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
