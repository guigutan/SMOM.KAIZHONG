using SIE.LES.MaterialPreparations;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.Commands
{
    /// <summary>
    /// 取消备料数
    /// </summary>
    public class WithDrawPrepareCommand : ViewCommand
    {
        /// <summary>
        /// 取消备料数
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<MaterialPreparation>();
            RT.Service.Resolve<MaterialPreparationController>().WithDrawPreparation(data.Id);
            return true;
        }
    }
}
