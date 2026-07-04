using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账选择视图
    /// </summary>
    public class EquipAccountSelectController : DomainController
    {

        /// <summary>
        /// 查询设备台账列表
        /// </summary>
        /// <param name="criteria">设备台账查询对象</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccountSelect> GetEquipAccountsByCriteria(EquipAccountSelectCriteria criteria)
        {
            if (criteria.IsLoadAll)
            {
                //(不过滤设备权限）
                using (SIE.DataAuth.DataAuths.LoadAll())
                {
                    return GetEquipAccountsByCriteriaInner(criteria);
                }
            }
            else
            {
                return GetEquipAccountsByCriteriaInner(criteria);
            }
        }

        /// <summary>
        /// 查询设备台账列表
        /// </summary>
        /// <param name="criteria">设备台账查询对象</param>
        /// <returns>设备台账列表</returns>
        private EntityList<EquipAccountSelect> GetEquipAccountsByCriteriaInner(EquipAccountSelectCriteria criteria)
        {
            var query = Query<EquipAccountSelect>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.ModelCode.IsNotEmpty() || criteria.ModelName.IsNotEmpty() || criteria.TypeCategory.IsNotEmpty())
            {
                query.Join<EquipModel>((x, y) => x.EquipModelId == y.Id)
                .WhereIf<EquipModel>(criteria.ModelCode.IsNotEmpty(), (x, y) => y.Code.Contains(criteria.ModelCode))
                .WhereIf<EquipModel>(criteria.ModelName.IsNotEmpty(), (x, y) => y.Name.Contains(criteria.ModelName))
                .WhereIf<EquipModel>(criteria.TypeCategory.IsNotEmpty(), (x, y) => y.TypeCategory == criteria.TypeCategory);
            }

            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State);
            }
            if (criteria.AccountUseState.HasValue)
            {
                query.Where(p => p.UseState == criteria.AccountUseState);
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.ResourceId.HasValue && criteria.ResourceId != 0)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId);
            }
            if (criteria.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            if (criteria.EnableUseState.IsNotEmpty())
            {
                List<AccountUseState> useStateList = new List<AccountUseState>();
                var stateString = criteria.EnableUseState.Split(';').ToList();
                foreach ( var item in stateString)
                {
                    var value = int.Parse(item);
                    useStateList.Add((AccountUseState)value);
                }
                query.Where(p => useStateList.Contains(p.UseState));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询需点检的设备台账
        /// </summary>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccountSelect> GetCheckPlanEquipAccounts(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountSelect>().Where(p => p.UseState == AccountUseState.Using);

            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 查询需点检的设备台账
        /// </summary>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccountSelect> GetAllCheckPlanEquipAccounts(string keyword, PagingInfo pagingInfo)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var query = Query<EquipAccountSelect>().Where(p => p.UseState == AccountUseState.Using);

                if (!keyword.IsNullOrEmpty())
                {
                    query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }

                var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

                return list;
            }
        }

        /// <summary>
        /// 查询需点检的设备台账
        /// </summary>
        /// <param name="states">设备台账状态</param>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> GetCheckPlanEquipAccounts(List<AccountUseState> states, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountSelect>().Where(p => states.Contains(p.UseState));

            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 查询需点检的设备台账
        /// </summary>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备台账列表</returns>
        public virtual EntityList<EquipAccountSelect> GetEquipAccounts(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipAccountSelect>();
            
            if (!keyword.IsNullOrEmpty())
            {
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        
    }
}
