using SIE.Domain;

namespace SIE.Equipments.FinancialCategorys
{
    /// <summary>
    /// 财务分类控制器
    /// </summary>
    public class FinancialCategoryController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual  EntityList<FinancialCategory> Fetch(FinancialCategoryCriteria criteria)
        {
            var query = Query<FinancialCategory>();
            if (!string.IsNullOrEmpty(criteria.Code))
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (!string.IsNullOrEmpty(criteria.Desc))
            {
                query.Where(p => p.Desc.Contains(criteria.Desc));
            }
            if (criteria.CreationDate != null && criteria.CreationDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreationDate.BeginValue.Value);
            }
            if (criteria.CreationDate != null && criteria.CreationDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreationDate.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
