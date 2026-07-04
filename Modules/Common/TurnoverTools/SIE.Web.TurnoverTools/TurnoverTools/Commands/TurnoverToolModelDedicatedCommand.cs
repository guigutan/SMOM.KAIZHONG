using SIE.TurnoverTools.TurnoverTools;
using SIE.Web.Command;

namespace SIE.Web.Elec.MES.TurnoverTools.Commands
{
    /// <summary>
    /// 设为专用
    /// </summary>
    [JsCommand("SIE.Web.Elec.MES.TurnoverTools.Commands.SetModelDedicatedCommand")]
    public class SetModelDedicatedCommand : ViewCommand
    {
        /// <summary>
        /// 设为专用
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>返回信息</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<KitTurnoverToolController>();
            var modelIds = args.SelectedIds;
            ctl.SetTurnoverToolModelDedicated(modelIds, true);
            return true;
        }
    }

    /// <summary>
    /// 取消专用
    /// </summary>
    [JsCommand("SIE.Web.Elec.MES.TurnoverTools.Commands.CancelModelDedicatedCommand")]
    public class CancelModelDedicatedCommand : ViewCommand
    {
        /// <summary>
        /// 取消专用
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>返回信息</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<KitTurnoverToolController>();
            var modelIds = args.SelectedIds;
            ctl.SetTurnoverToolModelDedicated(modelIds, false);
            return true;
        }
    }
}
