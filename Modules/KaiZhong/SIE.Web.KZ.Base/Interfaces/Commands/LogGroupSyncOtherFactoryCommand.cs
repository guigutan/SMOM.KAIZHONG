using SIE.KZ.Base.Interfaces.ViewModels;
using SIE.KZ.Base.SmomControl;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.Commands
{
    /// <summary>
    /// 同步到其他工厂
    /// </summary>
    public class LogGroupSyncOtherFactoryCommand : ViewCommand<LogGroupSyncOtherFactoryViewModel>
    {
        protected override object Excute(LogGroupSyncOtherFactoryViewModel args, string scope)
        {
            RT.Service.Resolve<InfNcDataLogGroupController>().LogGroupSyncOtherFactory(args);
            return "处理完成";
        }
    }
}
