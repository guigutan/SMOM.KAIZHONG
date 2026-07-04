using SIE.EMS.SpareParts;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 备件入库添加命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Commands.AddStoreDetailCommand")]
    public class AddStoreDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>工作任务号</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var No = RT.Service.Resolve<SparePartController>().GetDetailCode();
            return No;
        }
    }
}
