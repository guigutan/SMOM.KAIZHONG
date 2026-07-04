using SIE.Domain.Validation;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 备件验收查询器
    /// </summary>
    public class FixtureAcceptanceDataQueryer : DataQueryer
    {
        /// <summary>
        /// 保存备件验收附件
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <returns>路径</returns>
        public string SaveFixtureAcceptanceAttachment(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1)
            {
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string prePath = "FixtureAcceptanceAttachment";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
                return $"{path}/{fileName}";
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }
    }
}
