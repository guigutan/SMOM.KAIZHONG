using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.FixtureRecords
{
    /// <summary>
    /// 工治具出入库记录控制器
    /// </summary>
    public class FixtureRecordController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureRecord> Fetch(FixtureRecordCriteria criteria)
        {
            var query = Query<FixtureRecord>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.FixtureAccountCode.IsNotEmpty())
                query.Where(p => p.FixtureAccount.Code.Contains("%" + criteria.FixtureAccountCode+"%"));
            
            if (criteria.RecordType.HasValue)
                query.Where(p => p.RecordType== criteria.RecordType);

            if (criteria.BusinessType.HasValue)
                query.Where(p => p.BusinessType== criteria.BusinessType);

            if (criteria.FixtureWarehouseId.HasValue)
                query.Where(p => p.FixtureWarehouseId == criteria.FixtureWarehouseId);

            if (criteria.FixtureEncodeId.HasValue)
                query.Where(p => p.FixtureAccount.FixtureEncodeId == criteria.FixtureEncodeId);

            if (criteria.FixtureModelId.HasValue)
                query.Where(p => p.FixtureAccount.FixtureEncode.FixtureModelId == criteria.FixtureModelId);

            if (criteria.ApplyDate != null)
            {
                if (criteria.ApplyDate.BeginValue.HasValue)
                {
                    query.Where(x => x.ApplyDate >= criteria.ApplyDate.BeginValue);
                }
                if (criteria.ApplyDate.EndValue.HasValue)
                {
                    query.Where(x => x.ApplyDate <= criteria.ApplyDate.EndValue);
                }
            }

            if (criteria.ComplyDate != null)
            {
                if (criteria.ComplyDate.BeginValue.HasValue)
                {
                    query.Where(x => x.ComplyDate >= criteria.ComplyDate.BeginValue);
                }
                if (criteria.ApplyDate.EndValue.HasValue)
                {
                    query.Where(x => x.ComplyDate <= criteria.ComplyDate.EndValue);
                }
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
