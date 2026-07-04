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
    /// 工艺资料-产品工艺路线设置更新版本命令
    /// </summary>
    public class TreeRoutingUpDateCommand : ViewCommand
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
            var data = args.Data.ToJsonObject<TreeBomInfo>();
            RT.Service.Resolve<ProjectDesignController>().TreeRoutingUpDateVersion(data);
            return true;
        }
    }
}
