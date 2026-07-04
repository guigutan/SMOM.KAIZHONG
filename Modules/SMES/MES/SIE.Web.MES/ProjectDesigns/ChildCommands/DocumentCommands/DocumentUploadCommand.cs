using DevExpress.XtraRichEdit.Import.Doc;
using SIE.MES.ProjectDesigns;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands.DocumentCommands
{
    /// <summary>
    /// 工艺资料-文档上传命令
    /// </summary>
    public class DocumentUploadCommand : UploadAttachmentCommand
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            base.SavingAttachement += CheckDocType;
            try
            {
                return base.Excute(args, scope);
            }
            finally
            {
                base.SavingAttachement -= CheckDocType; // 防止内存泄漏 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private string CheckDocType(UploadAttachmentViewArgs args)
        {
            var att = args.Attachment;
            RT.Service.Resolve<ProjectDesignController>().CheckDocType(att.FileExtesion);
            return string.Empty;
        }
    }
}
