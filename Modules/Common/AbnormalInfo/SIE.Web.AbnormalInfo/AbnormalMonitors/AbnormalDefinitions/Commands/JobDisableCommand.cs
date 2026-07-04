using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Common.Schdules;
using SIE.Domain;
using SIE.Web.Command;
using SIE.Web.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands
{
    /// <summary>
    /// 禁用任务
    /// </summary>
    public class JobDisableCommand : DisableCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            if (scope != meta.EntityType.GetQualifiedName())
                throw new System.Security.SecurityException("参数type[{0}]与令牌不一致".L10nFormat(args.Type));
            return RT.Service.Resolve<AbnormalDefineService>().Disable(args.SelectedIds.ToList());
        }
    }
}
