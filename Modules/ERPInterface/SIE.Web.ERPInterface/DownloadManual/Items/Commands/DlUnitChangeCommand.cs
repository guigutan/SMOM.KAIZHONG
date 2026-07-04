using SIE.ERPInterface.Ebs.Download.Items;
using SIE.Web.Command;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Items.Commands
{
    /// <summary>
    /// 手动下载数据
    /// </summary>
    public class DlUnitChangeCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string rst = string.Empty;
            var resultSmom = RT.Service.Resolve<EbsUnitController>().Download(RT.InvOrg);           //执行业务表下载
            rst += ("物料单位基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
            resultSmom = RT.Service.Resolve<EbsItemUnitChangeController>().Download(RT.InvOrg);           //执行业务表下载
            rst += ("物料单位转换基础表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
            return rst;
        }
    }
}
