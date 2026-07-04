using SIE.ERPInterface.Smom.Download;
using SIE.Web.Command;
using SIE.Web.ERPInterface.DownloadManual.Common.ViewModels;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Customers.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlCustomerCommand : ViewCommand
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
            var result = RT.Service.Resolve<DownloadCustomerController>().DownloadManual(data.KeyWord);

            if (result.IsNullOrEmpty())
                return "OK";
            else
                return result;
        }
    }
}
