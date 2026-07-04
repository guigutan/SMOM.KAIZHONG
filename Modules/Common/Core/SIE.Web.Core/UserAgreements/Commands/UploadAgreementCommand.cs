using Newtonsoft.Json.Linq;
using SIE.Core.Common;
using SIE.Core.UserAgreements;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Linq;

namespace SIE.Web.Core.UserAgreements.Commands
{
    /// <summary>
    /// 导入协议命令
    /// </summary>
    [AllowAnonymous]
    internal class UploadAgreementCommand : UploadAttachmentCommand
    {
        /// <summary>
        /// 命令对应的服务端事件
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var attach = DeserializeCustomData(args.Data);
            if (attach == null)
            {
                throw new ValidationException("请选择协议。".L10N());
            }

            var supportFileExts = new string[] { ".pdf" };

            if (!supportFileExts.Contains(attach.FileExtesion))
            {
                throw new ValidationException("请上传{0}格式的协议。".L10nFormat(string.Join(StringCommon.SplitChar, supportFileExts)));
            } 

            RT.Service.Resolve<UserAgreementController>().UploadAgreement(attach);

            return null;
        }

        private UserAgreementAttachment DeserializeCustomData(string data)
        {
            const string attachmentStr = "Attachment";
            UserAgreementAttachment attachment = new UserAgreementAttachment();
            if (!string.IsNullOrEmpty(data))
            {
                JObject jObject = JObject.Parse(data);
                attachment.FileExtesion = jObject[attachmentStr]!["FileExtesion"]!.ToString();
                attachment.FileSize = jObject[attachmentStr]!["FileSize"]!.ToString();
                attachment.FileName = jObject[attachmentStr]!["FileName"]!.ToString();
                attachment.OwnerId = Convert.ToDouble(jObject[attachmentStr]!["OwnerId"]);
                attachment.AgreementType = (AgreementType)Enum.Parse(typeof(AgreementType), jObject["AgreementType"].ToString());
                string text = jObject[attachmentStr]!["Content"]!.ToString();
                const string text2 = "base64,";
                int num = text.IndexOf(text2) + text2.Length;
                string s = "";
                if (text.Length > num && text.StartsWith(text2))
                {
                    s = text.Substring(num);
                }
                else
                    s = text;

                attachment.Content = Convert.FromBase64String(s);
            }
            else
                return null;

            return attachment;
        }
    }
}
