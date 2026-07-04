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
    /// 取消备料需求明细
    /// </summary>
    public class WithDrawPreDetailCommand : ViewCommand
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
            var detailIds = args.SelectedIds.ToList();
            RT.Service.Resolve<MaterialPreparationController>().WithDrawPreDetail(detailIds);
            return true;
        }
    }
}
