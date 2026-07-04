using SIE.TurnoverTools.TurnoverTools;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TurnoverTools.Commands
{
    /// <summary>
    /// 回收周转工具
    /// </summary>   
    [JsCommand("SIE.Web.MES.TurnoverTools.Commands.TurnoverToolRecoveryCommand")]
    public class TurnoverToolRecoveryCommand : ViewCommand
    {
        /// <summary>
        /// 恢复工单执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<KitTurnoverToolController>().TurnoverToolStateAction(Convert.ToDouble(args.Data), TurnoverToolState.Unused, TurnoverToolAction.Recycle);
            return true;
        }
    }

    /// <summary>
    /// 维修周转工具
    /// </summary>    
    [JsCommand("SIE.Web.MES.TurnoverTools.Commands.TurnoverToolRepairCommand")]
    public class TurnoverToolRepairCommand : ViewCommand
    {
        /// <summary>
        /// 维修周转工具
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<KitTurnoverToolController>().TurnoverToolStateAction(Convert.ToDouble(args.Data), TurnoverToolState.Repair, TurnoverToolAction.Repair);
            return true;
        }
    }

    /// <summary>
    /// 报废周转工具
    /// </summary>  
    [JsCommand("SIE.Web.MES.TurnoverTools.Commands.TurnoverToolScrapCommand")]
    public class TurnoverToolScrapCommand : ViewCommand
    {
        /// <summary>
        /// 报废周转工具执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">scope</param>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<KitTurnoverToolController>().TurnoverToolStateAction(Convert.ToDouble(args.Data), TurnoverToolState.Scrap, TurnoverToolAction.Scrap);
            return true;
        }
    }
}