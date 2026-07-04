using SIE.ERPInterface.Ebs.Download.Projects;
using SIE.Web.Command;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Suppliers.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlProjectCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var resultSmom = RT.Service.Resolve<EbsProjectNoController>().Download(RT.InvOrg);           //执行业务表下载
            return ("项目号基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }
    }
}
