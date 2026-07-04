using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Checker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Checker
{
    /// <summary>
    /// 检具维护控制器
    /// </summary>
    public class CheckerUpholdController : DomainController
    {
        /// <summary>
        /// 根据编码获取检具维护
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<CheckerUphold> GetCheckerUpholdsByCodes(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<CheckerUphold>().Where(p => c.Contains(p.CheckerCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据编码获取检具维护
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual CheckerUphold GetCheckerUpholdByCode(string code)
        {
            var first = Query<CheckerUphold>().Where(p => p.CheckerCode == code).FirstOrDefault();
            return first;
        }

        /// <summary>
        /// 查询检具维护
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<CheckerUphold> CriterialCheckerUphold(CheckerUpholdCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("检具查询实体异常！".L10N());
            }
            var q = Query<CheckerUphold>();
            if (criterial.ProcessId.HasValue)
            {
                q.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (criterial.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == criterial.FactoryId);
            }
            if (!criterial.CheckerName.IsNullOrEmpty())
            {
                q.Where(m => m.CheckerName.Contains("%" + criterial.CheckerName + "%"));
            }
            if (!criterial.CheckerCode.IsNullOrEmpty())
            {
                q.Where(m => m.CheckerCode.Contains("%" + criterial.CheckerCode + "%"));
            }
            if (!criterial.CheckerType.IsNullOrEmpty())
            {
                q.Where(m => m.CheckerType.Contains("%" + criterial.CheckerType + "%"));
            }

            if (criterial.EffectiveDate.BeginValue != null)
                q.Where(p => p.EffectiveDate >= criterial.EffectiveDate.BeginValue.Value);
            if (criterial.EffectiveDate.EndValue != null)
                q.Where(p => p.CreateDate <= criterial.EffectiveDate.EndValue.Value);
            if (!criterial.DrawingNo.IsNullOrEmpty())
                q.Where(p => p.DrawingNo.Contains(criterial.DrawingNo));

            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 是否又相同的检具编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual bool GetCheckerUpholdBool(string code)
        {
            var q = Query<CheckerUphold>().Where(p => p.CheckerCode == code).ToList();
            if (q.Count > 0)
                return true;
            else
                return false;
        }
    }
}
