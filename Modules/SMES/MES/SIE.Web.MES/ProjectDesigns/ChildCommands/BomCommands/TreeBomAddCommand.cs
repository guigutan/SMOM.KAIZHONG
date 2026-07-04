using SIE.MES.ProjectDesigns;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands
{
    /// <summary>
    /// 工艺资料-产品Bom添加命令
    /// </summary>
    public class TreeBomAddCommand : ViewCommand
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
            var version = string.Empty;
            var versions = RT.Service.Resolve<ProjectDesignController>().GetBomVersions(1);
            if (versions.Any())
            {
                version = versions.First();
            }
            return version;
        }
    }
}
