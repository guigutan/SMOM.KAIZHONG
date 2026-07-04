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
    /// 部分接收
    /// </summary>
    public class PartReceiveDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<MaterialReceiveDetail>();
            
            RT.Service.Resolve<MaterialReceiveController>().MaterialReceive(new List<MaterialReceiveDetail> { data });
            return true;
        }
    }
}
