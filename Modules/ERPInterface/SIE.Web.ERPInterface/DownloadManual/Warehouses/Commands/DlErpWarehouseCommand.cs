using SIE.ERPInterface.Download.Warehouses;
using SIE.Web.Command;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Warehouses.Commands
{
    /// <summary>
    /// 手动下载ERP子库数据
    /// </summary>
    public class DlErpWarehouseCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var resultSmom = RT.Service.Resolve<EbsWarehouseController>().Download(RT.InvOrg);           //执行业务表下载
            return "EBS子库基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg);           
        }
    }
}
