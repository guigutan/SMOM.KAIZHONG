using SIE.ERPInterface.Smom.Download;
using SIE.Web.Command;
using SIE.Web.ERPInterface.DownloadManual.Common.ViewModels;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.ProductBoms.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlProductBomCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<DownloadViewModel>();
            var result = RT.Service.Resolve<DownloadProductBomController>().DownloadManual(data.KeyWord);

            if (result.IsNullOrEmpty())
                return "OK";
            else
                return result;
        }
    }
}
