using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    internal class AddAnomalyMonitorsCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var no = RT.Service.Resolve<AbnormalDecisionRuleService>().GetAbnormalDecisionRuleCode();
            return no;
        }
    }
}
