using SIE.Domain;
using System.Linq;

namespace SIE.MES.SpcFromMess
{
    /// <summary>
    /// 统计过程控制-控制器
    /// </summary>
    public class SpcFromMesController: DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SpcFromMes> Fetch(SpcFromMesCriteria criteria)
        {
            var q = Query<SpcFromMes>();

            // 条件过滤：管制编号
            if (!string.IsNullOrEmpty(criteria.No))
            {
                q.Where(p => p.No.Contains(criteria.No));
            }

            // 条件过滤：管制名称
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                q.Where(p => p.Name.Contains(criteria.Name));
            }

            // 条件过滤：产品
            if (criteria.ItemId.HasValue)
            {
                q.Where(p => p.ItemId == criteria.ItemId);
            }

            // 条件过滤：管制项目
            if (!string.IsNullOrEmpty(criteria.Project))
            {
                q.Where(p => p.Project.Contains(criteria.Project));
            }

            // 条件过滤：生产部门
            if (!string.IsNullOrEmpty(criteria.ProductionDept))
            {
                q.Where(p => p.ProductionDept.Contains(criteria.ProductionDept));
            }

            // 条件过滤：机台
            if (criteria.ResourceId.HasValue)
            {
                q.Where(p => p.ResourceId == criteria.ResourceId);
            }

            // 条件过滤：测量人员
            if (criteria.InspectorId.HasValue)
            {
                q.Where(p => p.InspectorId == criteria.InspectorId);
            }

            // 条件过滤：创建时间（日期范围）
            if (criteria.CreateTime.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateTime.BeginValue);
            }
            if (criteria.CreateTime.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateTime.EndValue);
            }

            //// 使用贪婪加载，一次性加载所有主表和子表数据
            //var result = q.ToList(criteria.PagingInfo,
            //    new EagerLoadOptions().LoadWith(SpcFromMes.SpcDetailListProperty));

            // 执行查询，预加载视图属性
            var result = q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return result;
        }
    }
}
