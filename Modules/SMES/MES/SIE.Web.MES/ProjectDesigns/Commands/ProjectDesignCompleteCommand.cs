using SIE.MES.ProjectDesigns;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.Commands
{
    /// <summary>
    /// 设计完成命令
    /// </summary>
    public class ProjectDesignCompleteCommand : ViewCommand
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
            var selIds = args.SelectedIds.ToList();
            RT.Service.Resolve<ProjectDesignController>().DesignComplete(selIds);
            return true;
        }
    }
}
