using SIE.Barcodes.Panels;
using SIE.Barcodes.Panels.ViewModels;
using SIE.Web.Command;

namespace SIE.Web.Barcodes.Panels.Commands
{
    /// <summary>
    /// 拼板码归属按钮
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.Panels.Commands.PanelBelongCommand")]
    public class PanelBelongCommand : ViewCommand
    {
        /// <summary>
        /// 执行拼板码归属操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>拼板码归属结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var data = args.Data.ToJsonObject<PanelBelongViewModel>();
            errMsg = RT.Service.Resolve<PanelController>().PanelBelong(data);
            if (errMsg.Length == 0)
                return "拼板码归属成功";
            else
                return errMsg;
        }
    }
}
