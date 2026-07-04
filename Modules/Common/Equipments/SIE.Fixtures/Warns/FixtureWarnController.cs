using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Fixtures.Warns
{
    /// <summary>
    /// 工治具保养预警控制器
    /// </summary>
    public class FixtureWarnController : DomainController
    {
        /// <summary>
        /// 工治具保养预警查询实体方法
        /// </summary>
        /// <param name="criteria">工治具保养预警查询实体</param>
        /// <returns>工治具保养预警列表</returns>
        public virtual EntityList<FixtureWarn> QueryFixtureWarn(FixtureWarnCriteria criteria)
        {

            var q = Query<FixtureWarn>();
            if (criteria.FixtureTypeId.HasValue)
                q.Where(w => w.FixtureAccount.FixtureEncode.FixtureModel.FixtureTypeId == criteria.FixtureTypeId);
            if (criteria.ModelCode.IsNotEmpty())
                q.Where(w => w.FixtureAccount.FixtureEncode.FixtureModel.Code.Contains(criteria.ModelCode));
            if (criteria.EncodeCode.IsNotEmpty())
                q.Where(w => w.FixtureAccount.FixtureEncode.Code.Contains(criteria.EncodeCode));
            q.Where(p => p.FixtureAccount.MaintainedHour > p.FixtureAccount.FixtureEncode.FixtureModel.WarnHour
            || p.FixtureAccount.MaintainedNum > p.FixtureAccount.FixtureEncode.FixtureModel.WarnNum);

            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
