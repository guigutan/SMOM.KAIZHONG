using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Threshold
{
    /// <summary>
    /// 阈值控制器
    /// </summary>
    public class ThresholdController : DomainController
    {
        /// <summary>
        /// 根据产品编码、工序编码获取可疑品阈值
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="processCodes"></param>
        /// <returns></returns>
        public virtual EntityList<Threshold> GetThresholdsByCodeProcess(List<string> itemCodes,List<string> processCodes)
        {
            var thresholds = processCodes.SplitContains(pcs =>
            {
                return itemCodes.SplitContains(ics =>
                {
                    return Query<Threshold>().Where(p => ics.Contains(p.Item.Code) && pcs.Contains(p.Process.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return thresholds;
        }

        /// <summary>
        /// 查询阈值
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<Threshold> CriterialThreshold(ThresholdCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("阈值查询实体异常！".L10N());
            }
            var q = Query<Threshold>();
            if (criterial.ProcessId.HasValue)
            {
                q.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    q.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            if (!criterial.ItemName.IsNullOrEmpty())
            {
                q.Where(m => m.Item.Name.Contains("%" + criterial.ItemName + "%"));
            }
            if (!criterial.ThresholdValue.IsNullOrEmpty())
            {
                q.Where(m => m.ThresholdValue.Contains("%" + criterial.ThresholdValue + "%"));
            }

            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 查询阈值
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Threshold> GetThresholdByItem(double itemId, double processId)
        {
           
            var q = Query<Threshold>().Where(p=>p.ItemId== itemId && p.ProcessId==processId);
            
            return q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
