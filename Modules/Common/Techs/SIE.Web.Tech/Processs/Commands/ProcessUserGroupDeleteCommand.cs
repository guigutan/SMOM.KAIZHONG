using SIE.Common;
using SIE.Domain;
using SIE.Resources.UserGroups;
using SIE.Tech.Processs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.Processs.Commands
{
    public class ProcessUserGroupDeleteCommand : DeleteCommand
    {
        protected override void DoSave(EntityList data)
        {
            EntityList<ProcessUserGroup> list = data.DeletedList.OfType<ProcessUserGroup>().AsEntityList();
            base.DoSave(data);
            RT.Service.Resolve<ProcessController>().DeleteProcessUserGroupSyncEmployee(list);
        }

    }
}
