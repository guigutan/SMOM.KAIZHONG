using SIE.LES.MaterialReceives;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReceives.Commands
{
    /// <summary>
    /// 拒收(整行)
    /// </summary>
    public class RejectDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = args.Data.ToJsonObject<List<MaterialReceiveDetail>>();
            var receiveId = list.FirstOrDefault()?.MaterialReceiveId ?? 0;
            var receiveDetailIds = list.Select(p => p.Id).Distinct().ToList();
            RT.Service.Resolve<MaterialReceiveController>().MaterialReceive(receiveId, receiveDetailIds, true);
            return true;
        }
    }
}
