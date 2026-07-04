using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Daos
{
    /// <summary>
    /// QT标准维护Dao层
    /// </summary>
    public class QTimeStandardDao : BaseDao<QTimeStandard>
    {
        /// <summary>
        /// QT标准维护查询
        /// </summary>
        /// <param name="qTimeStandardCriteria"></param>
        /// <returns></returns>
        public EntityList<QTimeStandard> QueryQTimeEntityList(QTimeStandardCriteria qTimeStandardCriteria)
        {
            var query = Query();
            if (qTimeStandardCriteria == null)
            {
                return new EntityList<QTimeStandard>();
            }
            if (qTimeStandardCriteria.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Code.Contains(qTimeStandardCriteria.ProductCode));
            }
            if (qTimeStandardCriteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(qTimeStandardCriteria.ProductName));
            }
            if (qTimeStandardCriteria.WipResourceId != null && qTimeStandardCriteria.WipResourceId != 0)
            {
                query.Where(p => p.WipResourceId ==  qTimeStandardCriteria.WipResourceId);
            }
            if (qTimeStandardCriteria.StartProcessId != null && qTimeStandardCriteria.StartProcessId != 0)
            {
                query.Where(p => p.StartProcessId == qTimeStandardCriteria.StartProcessId);
            }
            if (qTimeStandardCriteria.EndProcessId != null && qTimeStandardCriteria.EndProcessId != 0)
            {
                query.Where(p => p.EndProcessId == qTimeStandardCriteria.EndProcessId);
            }
            if (qTimeStandardCriteria.State.HasValue)
            {
                query.WhereIf(qTimeStandardCriteria.State == State.Enable,p => p.State)
                    .WhereIf(qTimeStandardCriteria.State == State.Disable, p => !p.State);
            }
            return query.OrderBy(qTimeStandardCriteria.OrderInfoList).ToList(qTimeStandardCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 新增保存时获取同产品产线数据
        /// </summary>
        /// <param name="ids">数据Ids</param>
        /// <param name="productIds">产品Ids</param>
        /// <param name="wipIds">产线Ids</param>
        /// <returns></returns>
        public EntityList<QTimeStandard> GetQTByProductAndWip(List<double> ids, List<double?> productIds, List<double?> wipIds)
        {
            var query = Query().Where(p => !ids.Contains(p.Id));
            if (productIds.Any(p => p == null))
            {
                productIds.RemoveAll(p => p == null);
                query.Where(p => productIds.Contains(p.ProductId) || p.ProductId == null);
            }
            else
            {
                query.Where(p => productIds.Contains(p.ProductId));
            }
            if (wipIds.Any(p => p == null))
            {
                wipIds.RemoveAll(p => p == null);
                query.Where(p => wipIds.Contains(p.WipResourceId) || p.WipResourceId == null);
            }
            else
            {
                query.Where(p => wipIds.Contains(p.WipResourceId));
            }
            return query.ToList();
        }

        /// <summary>
        /// 按优先级获取QT标准规则
        /// </summary>
        /// <returns></returns>
        public EntityList<QTimeStandard> GetQTByPriority()
        {
            EntityList<QTimeStandard> qTimeStandards = new EntityList<QTimeStandard>();
            // 相同开始工序、开始状态、结束工序、结束状态规则，按照以下优先级取规则
            // 1.产品+产线
            // 2.产品
            // 3.产线
            var queryList = Query().Where(p => p.State).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var groupList = queryList.GroupBy(p => new {p.StartProcessId,p.StartState,p.EndProcessId,p.EndState}).ToList();
            foreach ( var group in groupList)
            {
                var priorityOne = group.FirstOrDefault(p => p.ProductId != null && p.WipResourceId != null);
                if (priorityOne != null)
                {
                    qTimeStandards.Add(priorityOne);
                    continue;
                }
                var priorityTwo = group.FirstOrDefault(p => p.ProductId != null);
                if (priorityTwo != null)
                {
                    qTimeStandards.Add(priorityTwo);
                    continue;
                }
                var priorityThree = group.FirstOrDefault(p => p.WipResourceId != null);
                if (priorityThree != null)
                {
                    qTimeStandards.Add(priorityThree);
                    continue;
                }
            }
            return qTimeStandards;
        }
    }
}
