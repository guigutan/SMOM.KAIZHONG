using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Andon.Commands
{
    public class ThresholdDLTemplateCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
