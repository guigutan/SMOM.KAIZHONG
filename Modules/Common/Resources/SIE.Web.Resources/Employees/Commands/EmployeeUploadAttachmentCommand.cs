using SIE.Resources.Employees;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System.Collections.Generic;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 员工维护上传签名
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.EmployeeUploadAttachmentCommand")]
    public class EmployeeUploadAttachmentCommand : UploadAttachmentCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var attachments = args.Data.ToJsonObject<List<UploadAttachmentViewArgs>>();
            foreach (var attachment in attachments)
            {
                //var fileStream = new MemoryStream(attachment.Attachment.Content);
                RT.Service.Resolve<EmployeeController>().BatchSavePhoto(attachment.Attachment.Content);
            }
            //RT.Service.Resolve<EmployeeController>().BatchSavePhoto(attachments);
            return true;
        }
    }
}
