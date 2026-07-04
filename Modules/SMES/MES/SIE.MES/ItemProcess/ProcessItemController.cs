using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemProcess
{
    /// <summary>
    /// 产品与产线关系控制器
    /// </summary>
    public class ProcessItemController : DomainController
    {
        #region 查询物料与工序关系

        /// <summary>
        /// 根据工序查询工序与物料的关系
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ProcessItem> GetProcessItemsByProcessId(List<double> processIds)
        {
            var list = processIds.SplitContains(ids =>
            {
                return Query<ProcessItem>().Where(p => ids.Contains((double)p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 查询产品与产线关系
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<ProcessItem> CriterialProcessItem(ProcessItemCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("产品与产线关系查询实体异常！".L10N());
            }
            var query = Query<ProcessItem>();
            if (criterial.State.HasValue)
            {
                query.Where(p => p.State == criterial.State.Value);
            }
            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    query.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            if (criterial.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (!criterial.ItemName.IsNullOrEmpty())
            {
                query.Where(m => m.Item.Name.Contains("%" + criterial.ItemName + "%"));
            }
            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 启用产品与产线关系
        /// <summary>
        /// 启用产品与产线关系
        /// </summary>
        /// <returns></returns>
        public virtual void EnableProcessItem(List<double> LineIds)
        {
            var lineList = LineIds.SplitContains(tempIds =>
            {
                return Query<ProcessItem>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            lineList.ForEach(p =>
            {
                p.State = State.Enable;
            });
            RF.Save(lineList);
        }
        #endregion

        #region 禁用产品与产线关系
        /// <summary>
        /// 禁用产品与产线关系
        /// </summary>
        /// <param name="LineIds"></param>
        public virtual void DisableProcessItem(List<double> LineIds)
        {
            var lineList = LineIds.SplitContains(tempIds =>
            {
                return Query<ProcessItem>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            lineList.ForEach(p =>
            {
                p.State = State.Disable;
            });
            RF.Save(lineList);
        }
        #endregion

        #region 是否有相同的数据
        /// <summary>
        /// 是否有相同的数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="processId"></param>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual bool GetProcessItemBool(double itemId, double processId)
        {
            var query = Query<ProcessItem>().Where(p => p.ItemId == itemId && p.ProcessId == processId).ToList();
            if (query.Count > 0)

                return true;
            else
                return false;
        }
        #endregion
    }
}
