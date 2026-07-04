using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ESop.EngDocuments.Daos
{
    /// <summary>
    /// Dao层
    /// </summary>
    public class EngDocumentDao : BaseDao<EngDocument>
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="engDocCriteria"></param>
        /// <returns></returns>
        public EntityList EngDocCriteriaFetch(EngDocCriteria engDocCriteria)
        {
            var query = Query();
            if (engDocCriteria == null)
            {
                throw new ValidationException("查询实体异常，请刷新界面！".L10N());
            }
            if (engDocCriteria.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Code.Contains(engDocCriteria.ProductCode));
            }
            if (engDocCriteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(engDocCriteria.ProductName));
            }
            if (engDocCriteria.WoNo.IsNotEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(engDocCriteria.WoNo));
            }
            if (engDocCriteria.Type.HasValue)
            {
                query.Where(p => p.Type == engDocCriteria.Type.Value);
            }
            if (engDocCriteria.DocCode.IsNotEmpty() || engDocCriteria.DocName.IsNotEmpty())
            {
                query.Join<EngDocumentDetail>((x, y) => y.EngDocumentId == x.Id).Distinct()
                    .WhereIf<EngDocumentDetail>(engDocCriteria.DocCode.IsNotEmpty(), (x, y) => y.DocCode.Contains(engDocCriteria.DocCode))
                    .WhereIf<EngDocumentDetail>(engDocCriteria.DocName.IsNotEmpty(), (x, y) => y.DocName.Contains(engDocCriteria.DocName));
            }
            return query.OrderBy(engDocCriteria.OrderInfoList).ToList(engDocCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品Ids获取数据
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<EngDocument> GetEngDocumentByProductIds(List<double> productIds, List<double> ids)
        {
            return productIds.SplitContains(tempIds =>
            {
                return Query().Where(p => p.ProductId != null && tempIds.Contains((double)p.ProductId) && !ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据工单Ids获取数据
        /// </summary>
        /// <param name="woIds"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<EngDocument> GetEngDocumentByWoIds(List<double> woIds, List<double> ids)
        {
            return woIds.SplitContains(tempIds =>
            {
                return Query().Where(p => p.WorkOrderId != null && tempIds.Contains((double)p.WorkOrderId) && !ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

    }
}
