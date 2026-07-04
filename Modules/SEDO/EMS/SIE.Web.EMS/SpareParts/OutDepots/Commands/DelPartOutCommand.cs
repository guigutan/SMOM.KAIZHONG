using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.OutDepots.Commands
{
    /// <summary>
    /// 删除出库明细
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.DelPartOutCommand")]
    public class DelPartOutCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
