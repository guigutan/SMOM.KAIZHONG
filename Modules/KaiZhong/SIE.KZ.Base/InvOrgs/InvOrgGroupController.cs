using SIE.Domain;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.InvOrgs
{
    /// <summary>
    /// 
    /// </summary>
    public class InvOrgGroupController : InvOrgController
    {
        public virtual EntityList<InvOrgGroup> GetInvOrgGroupsByUserId(double userId)
        {
            return Query<InvOrgGroup>().Join<UserInInvOrg>(((o, ou) => ou.InvOrgId == o.Id && ou.UserId == userId)).ToList();

        }
    }
}
