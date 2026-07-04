using SIE.Common.Attachments;
using SIE.Core.Common;
using SIE.Domain;
using SIE.IO;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Core.Common.Commands
{
    /// <summary>
    /// 压缩上传图片
    /// </summary>
    public class UploadZipAttachmentCommand : UploadAttachmentCommand
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            base.SavingAttachement += ImgZipOperate;
            try
            {
                return base.Excute(args, scope);
            }
            finally
            {
                base.SavingAttachement -= ImgZipOperate; // 防止内存泄漏 
            }
        }

        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="viewArgs">文件参数</param>
        /// <returns></returns>
        private string ImgZipOperate(UploadAttachmentViewArgs viewArgs)
        {
            var item = viewArgs.Attachment;
            var isImg = RT.Service.Resolve<BasePictureController>().ValidationFileExtesionIsImage(item.FileExtesion, item.FileName);
            if (isImg)
            {
                (item.Content, item.FileSize) = RT.Service.Resolve<BasePictureController>().ToZipPictureBeforeSaving(item.Content);
            }
            return string.Empty;
        }
    }
}
