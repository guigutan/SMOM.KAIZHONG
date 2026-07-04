using SIE.Recheck.Common.ItemRecheck;
using SIE.Web.Command;
using SIE.Web.Common.Sort.Commands;

namespace SIE.Web.Recheck.Common.ItemRecheck.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>   
    public class ItemRecheckProgramAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ItemRecheckProgram>();
            data.Code = AppRuntime.Service.Resolve<ItemRecheckProgramController>().GetItemRecheckProgramCode();
            return data;
        }
    }

    /// <summary>
    /// 物料复检方案保存命令
    /// </summary>
    [JsCommand("SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramSaveCommand")]
    public class ItemRecheckProgramSaveCommand : SaveCommand
    {
    }

    /// <summary>
    /// 置顶
    /// </summary>
    [JsCommand("SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveTop")]
    public class RecheckProgramDetailMoveTop : MoveTopCommand
    {
    }

    /// <summary>
    /// 上移
    /// </summary>
    [JsCommand("SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveUp")]
    public class RecheckProgramDetailMoveUp : MoveUpCommand
    {
    }

    /// <summary>
    /// 下移
    /// </summary>
    [JsCommand("SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveDown")]
    public class RecheckProgramDetailMoveDown : MoveDownCommand
    {
    }

    /// <summary>
    /// 置底
    /// </summary>
    [JsCommand("SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveBottom")]
    public class RecheckProgramDetailMoveBottom : MoveBottomCommand
    {
    }

    /// <summary>
    /// 命令
    /// </summary>
    public static class ItemRecheckCommands
    {
        /// <summary>
        /// 方案保存命令
        /// </summary>
        public static readonly string ItemRecheckProgramSaveCommand = "SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramSaveCommand";

        /// <summary>
        /// 方案添加命令
        /// </summary>
        public static readonly string ItemRecheckProgramAddCommand = "SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramAddCommand";

        /// <summary>
        /// 方案删除命令
        /// </summary>
        public static readonly string ItemRecheckProgramDeleteCommand = "SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramDeleteCommand";

        /// <summary>
        /// 明细添加命令
        /// </summary>
        public static readonly string RecheckProgramDetailAddCommand = "SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailAddCommand";

        /// <summary>
        /// 置顶命令
        /// </summary>
        public static readonly string RecheckProgramDetailMoveTop = "SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveTop";

        /// <summary>
        /// 上移命令
        /// </summary>
        public static readonly string RecheckProgramDetailMoveUp = "SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveUp";

        /// <summary>
        /// 下移命令
        /// </summary>
        public static readonly string RecheckProgramDetailMoveDown = "SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveDown";

        /// <summary>
        /// 置底命令
        /// </summary>
        public static readonly string RecheckProgramDetailMoveBottom = "SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveBottom";
    }
}
