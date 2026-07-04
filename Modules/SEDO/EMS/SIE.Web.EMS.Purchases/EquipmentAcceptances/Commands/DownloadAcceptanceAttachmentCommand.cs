using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Documents;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.IO;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands
{
    /// <summary>
    /// CAM文件下载
    /// </summary>
    public class DownloadAcceptanceAttachmentCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            
            
            var fileObj = JsonConvert.DeserializeObject<FileObj>(args.Data);

            FileStreamResult fsr;

            if (fileObj.FilePath.Split("/").Length > 2)
            {
                var fileName = fileObj.FilePath.Substring(fileObj.FilePath.LastIndexOf("/") + 1);
                var filePath = fileObj.FilePath.Substring(0, fileObj.FilePath.LastIndexOf("/"));
                var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, fileName);
                fsr = new FileStreamResult(new MemoryStream(fileBytes), "application/octet-stream");
                fsr.FileDownloadName = fileName;
            }
            else
            {
                throw new ValidationException("文件路径不正确！".L10N());
            }

            return fsr;
        }
    }
    public class FileObj
    {
        public string FilePath { get; set; }

        public double SelectedIds { get; set; }
    }
}