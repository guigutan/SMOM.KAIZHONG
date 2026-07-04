using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ApiModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands
{
    /// <summary>
    /// 工艺资料-产品工艺路线设置工艺BOM生成命令
    /// </summary>
    public class TreeRoutingBomInitCommand : ViewCommand
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
            var selIds = args.SelectedIds;
            RT.Service.Resolve<ProjectDesignController>().InitRoutingBomDetail(selIds);
            return true;
        }
    }
}
