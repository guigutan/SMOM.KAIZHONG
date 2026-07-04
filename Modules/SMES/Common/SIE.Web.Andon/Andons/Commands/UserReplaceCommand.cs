using SIE.Andon.Andons;
using SIE.Andon.Andons.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 责任人替换
    /// </summary>
    public class UserReplaceCommand : ViewCommand<UserReplaceViewModel>
    {
        protected override object Excute(UserReplaceViewModel args, string scope)
        {
            RT.Service.Resolve<AndonController>().ReplaceDetailUser(args);
            return true;
        }
    }
}
