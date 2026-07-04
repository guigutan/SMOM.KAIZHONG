using SIE.Web.Command;
using SIE.Web.Common.Sort.Commands;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 下移
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.Commands.ProcessBomAttentMoveDown")]
    public class ProcessBomAttentMoveDownCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法（不执行保存，工单保存按钮统一保存）
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 上移
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.Commands.ProcessBomAttentMoveUp")]
    public class ProcessBomAttentMoveUpCommand : MoveUpCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 置顶
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.Commands.ProcessBomAttentMoveTop")]
    public class ProcessBomAttentMoveTopCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 置底
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.Commands.ProcessBomAttentMoveBottom")]
    public class ProcessBomAttentMoveBottomCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">entityType</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}