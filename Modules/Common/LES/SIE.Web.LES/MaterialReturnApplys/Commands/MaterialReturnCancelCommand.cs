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
    /// 退料申请取消命令
    /// </summary>
    public class MaterialReturnCancelCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<MaterialReturnApply>();
            RT.Service.Resolve<MaterialReturnApplyController>().CancelReturnApply(data.Id);
            return true;
        }
    }
}
