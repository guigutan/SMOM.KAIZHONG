using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ApiModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands
{
    /// <summary>
    /// 工艺资料产品Bom引用标准BOM命令
    /// </summary>
    public class TreeBomInitCommand : ViewCommand
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
            var data = args.SelectedIds;
            RT.Service.Resolve<ProjectDesignController>().InitDesignTreeBoms(data);
            return true;
        }
    }
}
