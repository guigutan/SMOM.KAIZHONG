using SIE;
using SIE.Configuration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ScheduleServer
{
    public class HostApp : DomainApp
    {
        protected override void InitEnvironment()
        {
            RT.Provider.IsDebuggingEnabled = RT.Config.Get(ConfigKeys.IsDebuggingEnabled, false);

            base.InitEnvironment();
        }

        protected override void OnRuntimeStarting()
        {
            base.OnRuntimeStarting();
        }
    }
}
