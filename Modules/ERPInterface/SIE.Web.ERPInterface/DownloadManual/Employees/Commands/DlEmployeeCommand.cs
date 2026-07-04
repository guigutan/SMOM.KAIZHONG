using SIE.ERPInterface.Ebs.Download.Employees;
using SIE.ERPInterface.Smom.Download;
using SIE.Web.Command;
using SIE.Web.ERPInterface.DownloadManual.Common.ViewModels;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Employees.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlEmployeeCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {          
            var resultSmom = RT.Service.Resolve<EbsEmployeeController>().Download(RT.InvOrg, true);
            return "员工数据结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg);
        }
    }
}
