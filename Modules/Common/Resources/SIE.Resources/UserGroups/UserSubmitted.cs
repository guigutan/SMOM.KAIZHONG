using SIE.Common.Users;
using SIE.Domain;
using SIE.EventMessages.MES.Andons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    public class UserSubmitted : OnSubmitted<User>
    {
        protected override void Invoke(User entity, EntitySubmittedEventArgs e)
        {
            if ((e.Action == SubmitAction.Update && entity.State == State.Disable) || e.Action == SubmitAction.Delete)
            {
                RT.Service.Resolve<IAndon>().DeleteAndonGroupDetailUser(entity.Id);
            }
        }
    }
}
