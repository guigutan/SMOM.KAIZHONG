using SIE.Domain;
using SIE.Resources.WipResources;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Resources.WipResources.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [JsCommand("SIE.Web.Resources.WipResources.Commands.WipResourceStopCommand")]
    public class WipResourceStopCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var resources = args.Data.ToJsonObject<List<WipResource>>();
            RT.Service.Resolve<WipResourceController>().DisableWipResource(resources);
            resources.ForEach(x => { x.ResourceState = ResourceState.Stop; x.PersistenceStatus = PersistenceStatus.Unchanged; });
            return true;
        }
    }
}
