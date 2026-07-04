using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.EngDocuments.Commands
{
    /// <summary>
    /// 使用类型维护
    /// </summary>
    public class EngDocUseTypeCommand : ViewCommand
    {
        /// <summary>
        /// 使用类型维护
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
