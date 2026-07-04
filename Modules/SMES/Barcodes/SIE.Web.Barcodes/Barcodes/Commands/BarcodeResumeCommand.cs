using SIE.Barcodes;
using SIE.Web.Command;

namespace SIE.Web.Barcodes.Barcodes.Commands
{
    /// <summary>
    /// 条码恢复
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.BarcodeResumeCommand")]
    public class BarcodeResumeCommand : DeleteCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<BarcodeController>();
            var resumeDatas = args.SelectedIds;
            ctl.BarcodeResume(resumeDatas);
            return true;
        }
    }
}