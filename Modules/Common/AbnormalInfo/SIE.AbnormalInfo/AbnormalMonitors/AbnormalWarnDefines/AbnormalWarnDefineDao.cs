using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 
    /// </summary>
    public class AbnormalWarnDefineDao : BaseDao<AbnormalWarnDefine>
    {

        /// <summary>
        /// 查询异常预警定义
        /// </summary>
        /// <param name="criteria">异常预警定义查询实体</param>
        /// <returns>查询异常预警定义列表</returns>
        public virtual EntityList<AbnormalWarnDefine> GetAbnormalWarnDefines(AbnormalWarnDefineCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria), "查询实体参数不能为空".L10N());
            var query = Query();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);          

            if (criteria.OrderInfoList != null && criteria.OrderInfoList.Count > 0)
                query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual AbnormalWarnDefine Get(string name)
        {
            return Query().Where(p => p.Name == name).FirstOrDefault();
        }
    }
}
