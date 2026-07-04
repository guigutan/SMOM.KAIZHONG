using SIE.Domain;
using SIE.TurnoverTools.TurnoverTools;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Elec.MES.TurnoverTools.Commands
{
    /// <summary>
    /// 周转工具删除命令
    /// </summary>
    [JsCommand("SIE.Web.Elec.MES.TurnoverTools.Commands.TurnoverToolDeleteCommand")]
    public class TurnoverToolDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 选择物料
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>返回信息</returns>
        protected override object Excute(double[] args, string scope)
        {
            var turnoverToolIds = args.ToList();
            RT.Service.Resolve<KitTurnoverToolController>().CheckTurnoverToolDeleteState(turnoverToolIds);
            return true;
        }
    }

}
