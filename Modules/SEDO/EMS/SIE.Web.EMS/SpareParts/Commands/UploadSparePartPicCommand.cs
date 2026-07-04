using Newtonsoft.Json.Linq;
using SIE.Domain.Validation;
using SIE.EMS.Common.Utils;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Core.Common.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 上传图片
    /// </summary>
    public class UploadSparePartPicCommand : UploadZipAttachmentCommand
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            JArray ja = JArray.Parse(args.Data);
            for (int i = 0; i < ja.Count; i++)
            {
                JObject jo = JObject.Parse(ja[i].ToString());
                var fileExtesion = jo["Attachment"]["FileExtesion"].ToString();
                var fileName = jo["Attachment"]["FileName"].ToString();
                FileUrlHelper.ValidationFileExtesionIsImage(fileExtesion, fileName);
            }
            return base.Excute(args, scope);
        }
    }
}
