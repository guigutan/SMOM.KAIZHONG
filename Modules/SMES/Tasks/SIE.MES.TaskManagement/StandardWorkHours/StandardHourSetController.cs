using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StandardWorkHours
{
    /// <summary>
    /// 产品标准工时控制器
    /// </summary>
    public class StandardHourSetController : DomainController
    {
        /// <summary>
        /// 界面查询产品标准工时维护
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<StandardHourSet> QueryStandardHourSets(StandardHourSetCriteria criteria)
        {
            if (criteria == null) return new EntityList<StandardHourSet>();
            var q = Query<StandardHourSet>();
            if (criteria.WipResourceId != null && criteria.WipResourceId != 0)
            {
                q.Where(p => p.WipResourceId ==  criteria.WipResourceId);
            }
            if (criteria.ProductModelId != null && criteria.ProductModelId != 0)
            {
                q.Where(p => p.ProductModelId == criteria.ProductModelId);
            }
            if (criteria.ProductId != null && criteria.ProductId != 0)
            {
                q.Where(p => p.ProductId == criteria.ProductId);
            }
            if (criteria.ProductName.IsNotEmpty())
            {
                q.Where(p => p.Product.Name.Contains(criteria.ProductName));
            }
            if (criteria.ProcessId != null && criteria.ProcessId != 0)
            {
                q.Where(p => p.ProcessId == criteria.ProcessId);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存时查询相同工序的产品标准工时维护
        /// </summary>
        /// <param name="processIds"></param>
        /// <param name="editIds"></param>
        /// <returns></returns>
        public virtual EntityList<StandardHourSet> GetStandardHourSets(IEnumerable<double> processIds, IEnumerable<double> editIds)
        {
            EntityList<StandardHourSet> standardHourSets = new EntityList<StandardHourSet>();
            processIds.SplitDataExecute(tempIds1 =>
            {
                editIds.SplitDataExecute(tempIds2 =>
                {
                    var list = Query<StandardHourSet>().Where(p => tempIds1.Contains(p.ProcessId) && !tempIds2.Contains(p.Id)).ToList();
                    standardHourSets.AddRange(list);
                });
            });
            return standardHourSets;
        }

        /// <summary>
        /// 保存时查询相同工序的产品标准工时维护
        /// </summary>
        /// <param name="processIds">工序Ids</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetStandardHourSets(IEnumerable<double> processIds)
        {
            EntityList<StandardHourSet> standardHourSets = new EntityList<StandardHourSet>();
            processIds.SplitDataExecute(tempIds1 =>
            {
                var list = Query<StandardHourSet>().Where(p => tempIds1.Contains(p.ProcessId)).ToList();
                standardHourSets.AddRange(list);
            });
            return standardHourSets.ToDictionary(p => "{0}@{1}@{2}@{3}".FormatArgs(p.WipResourceId, p.ProductModelId, p.ProductId, p.ProcessId), p => p.Id);
        }

        /// <summary>
        /// 校验重复
        /// </summary>
        /// <param name="list"></param>
        protected virtual void ValidateRepeat(EntityList<StandardHourSet> list)
        {
            // 判断 资源+机型+物料+工序 维度唯一
            HashSet<string> repeatHash = new HashSet<string>();
            if (list.Any(p => !repeatHash.Add("{0}@{1}@{2}@{3}".FormatArgs(p.WipResourceId, p.ProductModelId, p.ProductId, p.ProcessId))))
            {
                throw new ValidationException("资源+机型+物料+工序唯一".L10N());
            }
            // 工序必填，只需要查询出数据库中同工序数据即可
            var processIds = list.Select(p => p.ProcessId);
            // 排除编辑数据
            var editIds = list.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id);
            var dbList = GetStandardHourSets(processIds, editIds);
            if (dbList.Any(p => !repeatHash.Add("{0}@{1}@{2}@{3}".FormatArgs(p.WipResourceId, p.ProductModelId, p.ProductId, p.ProcessId))))
            {
                throw new ValidationException("资源+机型+物料+工序唯一".L10N());
            }
        }

        /// <summary>
        /// 验证数值
        /// </summary>
        /// <param name="list"></param>
        protected virtual void ValidateNumber(EntityList<StandardHourSet> list)
        {
            if (list.Any(p => p.StandardMin <= 0))
            {
                throw new ValidationException("工序标准工时必须大于0");
            }
            if (list.Any(p => p.AttachMin != null &&  p.AttachMin <= 0))
            {
                throw new ValidationException("附加合计工时必须大于0");
            }
        }

        /// <summary>
        /// 产品标准工时维护-保存前校验
        /// </summary>
        /// <param name="list">保存数据</param>
        public virtual void ValidateBeforeSave(EntityList<StandardHourSet> list)
        {
            if (!list.Any()) return;
            // 数值校验
            ValidateNumber(list);
            // 重复性校验
            ValidateRepeat(list);
        }
    }
}
