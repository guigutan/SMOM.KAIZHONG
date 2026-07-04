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
    /// 工艺资料-产品Bom更新版本命令
    /// </summary>
    public class TreeBomUpdateCommand : ViewCommand
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
            var projectMaintainId = args.Data.ToJsonObject<double>();
            RT.Service.Resolve<ProjectDesignController>().UpdateDesignTreeBom(selIds, projectMaintainId);
            return true;
        }
    }
}
