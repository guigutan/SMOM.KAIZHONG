using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands
{
    /// <summary>
    /// 整单交接命令
    /// </summary>
    public class WholeBillHandoverCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 整单交接操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<OutDepotHandover>();
            RT.Service.Resolve<OutDepotHandoverController>().WholeBillHandover(entity);
            return true;
        }
    }
}
