using SIE.ERPInterface.Ebs.Download.Suppliers;
using SIE.ERPInterface.Smom.Download;
using SIE.Web.Command;
using SIE.Web.ERPInterface.DownloadManual.Common.ViewModels;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Suppliers.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlSupplierCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var resultSmom = RT.Service.Resolve<EbsSupplierController>().Download(RT.InvOrg);           //执行业务表下载
            return "供应商业务表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg);
        }
    }
}
