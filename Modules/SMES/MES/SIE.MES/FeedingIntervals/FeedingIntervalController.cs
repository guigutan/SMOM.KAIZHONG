using SIE.Domain;
using System;
using SIE.Domain.Validation;

namespace SIE.MES.FeedingIntervals
{
    internal class FeedingIntervalController : DomainController
    {

        public virtual EntityList<FeedingInterval> Fetch(FeedingIntervalCriteria feedingIntervalCriteria)
        {          

            if (feedingIntervalCriteria == null)
            {
                throw new ValidationException("上料间隔时间查询实体异常！".L10N());
            }
            var query = Query<FeedingInterval>();
            if (feedingIntervalCriteria.ItemId.HasValue)
            {
                if (feedingIntervalCriteria.ItemId > 0)
                {
                    query.Where(p => p.ItemId == feedingIntervalCriteria.ItemId);
                }
            }

            return query.OrderBy(feedingIntervalCriteria.OrderInfoList)
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

          
        }
    }
}
