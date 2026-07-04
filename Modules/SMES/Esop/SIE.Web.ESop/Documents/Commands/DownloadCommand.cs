using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.ESop.Documents.Commands
{
    /// <summary>
    /// 附件下载
    /// </summary>
    [JsCommand("SIE.Web.ESop.Documents.Commands.DownloadCommand")]
    public class DownloadCommand : ViewCommand
    {
        /// <summary>
        /// 附件配置地址，从web.config中的节点client.attachmentDownloadUrl 获取
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var rootUrl = RT.Service.Resolve<AttachmentController>().GetDownloadPath().Replace(@"\", "/");
            return rootUrl;
        }
    }
}
