using SIE.Domain;
using SIE.Domain.Validation;
using SIE.AbnormalInfo.AbnormalInfos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.AbnormalInfo.Reports
{
    /// <summary>
    /// 异常信息报表控制器
    /// </summary>
    public class AbnormalInfoReportController : DomainController
    {
        /// <summary>
        /// 查询异常信息统计记录，按日期统计
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>异常信息统计记录</returns>
        public virtual List<AbnormalReportCloseDto> GetAbnormalReportRecord(AbnormalInfoReportCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            var q = Query<AbnormalInfor>();
            if (criteria.AbnormalCategoryId.HasValue)
            {
                q.Join<AbnormalInfoDefinition>((infor, def) => infor.AbnormalInfoDefinitionId == def.Id && def.AbnormalCategoryId == criteria.AbnormalCategoryId.Value);
            }
            q.WhereIf(criteria.AbnormalDefinitionId.HasValue, p => p.AbnormalInfoDefinitionId == criteria.AbnormalDefinitionId);

            if (criteria.CreateDate.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            if (criteria.CreateDate.DateRangeType == ObjectModel.DateRangeType.Custom && criteria.CreateDate.BeginValue.HasValue && criteria.CreateDate.EndValue.HasValue && criteria.CreateDate.BeginValue.Value.AddYears(1) < criteria.CreateDate.EndValue.Value)
            {
                throw new ValidationException("异常发生时间范围最大跨度只能是1年".L10N());
            }

            var list = q.Select(p => new { p.AbnormalStatus, p.CreateDate }).ToList<AbnormalReportStateTime>();
            var result = list.GroupBy(p => p.CreateDate.Date).Select(p => new AbnormalReportCloseDto
            {
                Date = p.Key,
                CloseQty = p.Count(k => k.AbnormalStatus == AbnormalStatus.Close),
                TotalQty = p.Count()
            }).ToList();
            if (result?.Count > 0)
                result.ForEach(p => p.Rate = (double)p.CloseQty / p.TotalQty);
            return result;
        }
    }
}
