using SIE.Andon.Andons;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AndonTypeMessageAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var andonTypeMessage = args.Data.ToJsonObject<AndonTypeMessageSend>();
            var andonTypeConfigPushPlug = RT.Service.Resolve<AndonTypeController>().GetAndonTypeConfigPushPlug();
            if (andonTypeConfigPushPlug != null)
            {
                andonTypeMessage.PushPlug = andonTypeConfigPushPlug;
                andonTypeMessage.PushPlugName = andonTypeMessage.PushPlug.Name;
            }
            return andonTypeMessage;
        }
    }
}
