using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Routings.RoutingBoms.Commands
{
    /// <summary>
    /// 导入日志附件下载
    /// </summary>
    [JsCommand("SIE.Web.MES.Routings.RoutingBoms.Commands.DownloadImportFileCommand")]
    public class DownloadImportFileCommand : FtpDownloadCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">实体参数</param>
        /// <param name="scope">scope</param>
        /// <returns>返回结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var id = args.Data.ToJsonObject<double>();
            var attachment = RF.GetById<RoutingBomAttachment>(id);
            if (attachment == null)
                throw new ValidationException("数据异常，找不到此附件".L10N());
            args.Data = "{\"FileName\":\"" + attachment.FileName + "\",\"FilePath\":\"" + attachment.FilePath + "\"}";
            return base.Excute(args, scope);
        }
    }
}
