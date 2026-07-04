using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Checker;
using SIE.MES.ItemLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Fixture
{
    /// <summary>
    /// 工装维护控制器
    /// </summary>
    public class FixtureUpholdController : DomainController
    {

        /// <summary>
        /// 根据编码获取检具维护
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureUphold> GetFixtureUpholdsByCodes(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<FixtureUphold>().Where(p => c.Contains(p.FixtureCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据编码获取检具维护
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual FixtureUphold GetFixtureUpholdByCode(string code)
        {
            var first = Query<FixtureUphold>().Where(p => p.FixtureCode == code).FirstOrDefault();
            return first;
        }

        /// <summary>
        /// 查询工装维护
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<FixtureUphold> CriterialFixtureUphold(FixtureUpholdCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("工装查询实体异常！".L10N());
            }
            var q = Query<FixtureUphold>();
            if (criterial.ProcessId.HasValue)
            {
                q.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (criterial.FactoryId.HasValue)
            {
                q.Where(p => p.FactoryId == criterial.FactoryId);
            }
            if (!criterial.FixtureName.IsNullOrEmpty())
            {
                q.Where(m => m.FixtureName.Contains("%" + criterial.FixtureName + "%"));
            }
            if (!criterial.FixtureCode.IsNullOrEmpty())
            {
                q.Where(m => m.FixtureCode.Contains("%" + criterial.FixtureCode + "%"));
            }
            if (!criterial.FixtureType.IsNullOrEmpty())
            {
                q.Where(m => m.FixtureType.Contains("%" + criterial.FixtureType + "%"));
            }
            if (!criterial.FixtureState.IsNullOrEmpty())
            {
                q.Where(m => m.FixtureState.Contains("%" + criterial.FixtureState + "%"));
            }
            if (!criterial.Drawn.IsNullOrEmpty())
            {
                q.Where(p => p.Drawn.Contains(criterial.Drawn));
            }
            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 是否有相同的工装编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual bool GetFixtureUpholdBool(string code)
        {
            var q = Query<FixtureUphold>().Where(p => p.FixtureCode == code).ToList();
            if (q.Count > 0)
                return true;
            else
                return false;
        }
    }
}
