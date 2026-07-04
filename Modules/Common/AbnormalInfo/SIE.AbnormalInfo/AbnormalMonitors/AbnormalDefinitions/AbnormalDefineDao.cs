using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常定义Dao
    /// </summary>
    public class AbnormalDefineDao : BaseDao<AbnormalDefine>
    {
        /// <summary>
        /// 获取异常定义列表
        /// </summary>
        /// <param name="criteria">异常定义查询实体</param>
        /// <returns>异常定义实体列表</returns>
        public virtual EntityList<AbnormalDefine> GetAbnormalDefines(AbnormalDefineCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria), "查询实体参数不能为空".L10N());
            var query = Query();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.AbnomalSourceId.HasValue)
                query.Join<AbnormalDecisionRule>((x, y) => x.AbnormalRuleId == y.Id).Where<AbnormalDecisionRule>((x, y) => y.AbnomalSourceId == criteria.AbnomalSourceId);
            if (criteria.AbnormalRuleId.HasValue)
                query.Where(p => p.AbnormalRuleId== criteria.AbnormalRuleId);
            if (criteria.AbnormalWarnDefineId.HasValue)
                query.Where(p => p.AbnormalWarnDefineId == criteria.AbnormalWarnDefineId);
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
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
