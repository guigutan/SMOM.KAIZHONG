using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Enums;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ERPInterface.Logs.Commands
{
    /// <summary>
    /// 事务上传重传命令
    /// </summary>
    public class ReUploadCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //事务Id列表
            RT.Service.Resolve<UploadBaseController>().AdjustTransation(idlist, ProcessState.Retry);

            return true;
        }
    }
}


