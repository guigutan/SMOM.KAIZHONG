using SIE.EventMessages.WebApis;
using SIE.KZ.Base.Interfaces;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.Commands
{
    public class InfNcDataLogGroupReUploadCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            RT.Service.Resolve<LogGroupController>().InfNcDataLogGroupReUpload(args);
            return "重新完成!".L10N();
        }
    }
}
