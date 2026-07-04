using SIE.LES.MaterialReturnApplys;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReturnApplys.Commands
{
    /// <summary>
    /// 列表提交
    /// </summary>
    public class MaterialReturnListSubmitCommand : ViewCommand
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var selIds = args.SelectedIds.ToList();
            RT.Service.Resolve<MaterialReturnApplyController>().SubmitReturnApply(selIds);
            return true;
        }
    }
}
