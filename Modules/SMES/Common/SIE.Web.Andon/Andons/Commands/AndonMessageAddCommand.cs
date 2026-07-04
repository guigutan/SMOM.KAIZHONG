using SIE.Andon.Andons;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    public class AndonMessageAddCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var andonMessage = args.Data.ToJsonObject<AndonMessageSend>();
            var andonPushPlugConfig = RT.Service.Resolve<AndonTypeController>().GetAndonConfigPushPlug();
            if(andonPushPlugConfig != null)
            {
                andonMessage.PushPlug = andonPushPlugConfig;
                andonMessage.PushPlugName = andonMessage.PushPlug.Name;
            }
            return andonMessage;
        }
    }
}
