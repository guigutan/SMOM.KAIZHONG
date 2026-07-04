using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TeamManagement.OnLoans
{
    public class WorkGroupOnLoanController : DomainController
    {
        public virtual EntityList<WorkGroupOnLoan> GetWorkGroupOnLoans(WorkGroupOnLoanCriteria criteria) { 
            var q= Query<WorkGroupOnLoan>();
            if (criteria.No.IsNotEmpty()) {
                q.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.InitiatorId.HasValue)
            {
                q.Where(p => p.InitiatorId == criteria.InitiatorId);
            }
            if (criteria.GroupInId.HasValue)
            {
                q.Where(p => p.GroupInId == criteria.GroupInId);
            }
            if (criteria.GroupOutId.HasValue)
            {
                q.Where(p => p.GroupOutId == criteria.GroupOutId);
            }
            if (criteria.ApproverId.HasValue)
            {
                q.Where(p => p.InitiatorId == criteria.InitiatorId);
            }
            if (criteria.State.HasValue)
            {
                q.Where(p => p.State==criteria.State.Value);
            }
            return q.ToList(criteria.PagingInfo,new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
