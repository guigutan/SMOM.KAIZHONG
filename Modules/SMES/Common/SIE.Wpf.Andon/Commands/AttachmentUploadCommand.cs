using Microsoft.Win32;
using SIE.Andon.Andons;
using SIE.Domain.Validation;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.IO;

namespace SIE.Wpf.Andon.Commands
{
    /// <summary>
    /// 附件上传命令
    /// </summary>
    [Command(ImageName = "Upload", Label = "附件上传", GroupType = CommandGroupType.Edit)]
    public class AttachmentUploadCommand : DetailViewCommand
    {
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;

            var fd = new OpenFileDialog();

            if (fd.ShowDialog() == true)
            {
                var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
                var fileExtesion = Path.GetExtension(fd.FileName);

                if (!exts.Contains(fileExtesion))
                {
                    throw new ValidationException("只能上传图片格式的文件".L10N());
                }

                const string prePath = "AndonManageImage";
                var path = $"{prePath}/{Guid.NewGuid()}";
                var fileName = Path.GetFileName(fd.FileName);
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, File.ReadAllBytes(fd.FileName), path);
                var fliePath = $"{path}/{fileName}";
                andonManage.PhotoFile = fliePath;
            }
        }
    }
}
