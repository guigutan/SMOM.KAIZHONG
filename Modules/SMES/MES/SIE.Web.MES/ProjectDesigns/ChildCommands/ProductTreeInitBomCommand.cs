using SIE.Core.ApiModels;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ApiModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands
{
    /// <summary>
    /// 引用标准Bom命令
    /// </summary>
    public class ProductTreeInitBomCommand : ViewCommand
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
            var data = args.Data.ToJsonObject<ProTreeInfo>();
            RT.Service.Resolve<ProjectDesignController>().InitProductTrees(data);
            return true;
        }
    }
}
