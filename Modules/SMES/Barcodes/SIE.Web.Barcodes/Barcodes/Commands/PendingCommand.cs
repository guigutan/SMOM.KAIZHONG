using SIE.Barcodes;
using SIE.Web.Command;

namespace SIE.Web.Barcodes.Barcodes.Commands
{
    /// <summary>
    /// 条码挂起
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.PendingCommand")]
    public class PendingCommand : ListViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">s</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            bool rst = false;
            var ctl = RT.Service.Resolve<BarcodeController>();
            var pendingDatas = args.Data.ToJsonObject<DataModel>();
            if (pendingDatas.BarCodeIds.Count > 0)
            {
                var barcodes = ctl.GetBarcodesByIds(pendingDatas.BarCodeIds);
                ctl.BarcodePending(barcodes, pendingDatas.Reason);
                rst = true;
            }
            return rst;
        }
    }

}
