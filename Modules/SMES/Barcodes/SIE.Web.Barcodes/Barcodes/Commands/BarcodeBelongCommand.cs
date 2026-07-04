using SIE.Barcodes;
using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Web.Command;

namespace SIE.Web.Barcodes.Barcodes.Commands
{
    /// <summary>
    /// 条码归属按钮
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.Barcodes.Commands.BarcodeBelongCommand")]
    public class BarcodeBelongCommand : ViewCommand
    {
        /// <summary>
        /// 执行条码归属操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>条码归属结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var data = args.Data.ToJsonObject<BarcodeBelongViewModel>();
            errMsg = RT.Service.Resolve<BarcodeController>().BarcodeBelong(data);
            if (errMsg.Length == 0)
                return "条码归属成功";
            else
                return errMsg;
        }
    }
}
