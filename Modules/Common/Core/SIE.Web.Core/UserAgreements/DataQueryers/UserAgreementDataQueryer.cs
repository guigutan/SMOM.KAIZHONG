using SIE.Common.Attachments;
using SIE.Core.ApiModels;
using SIE.Core.UserAgreements;
using SIE.Domain;
using SIE.Web.Data;
using System.IO;

namespace SIE.Web.Core.UserAgreements.DataQueryers
{
    /// <summary>
    /// 用户协议查询器
    /// </summary>
    internal class UserAgreementDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取用户协议
        /// </summary>
        /// <returns></returns>
        public EntityList GetUserAgreements()
        {
            return RT.Service.Resolve<UserAgreementController>().GetUserAgreements();
        }

        /// <summary>
        /// 查询用户协议文件
        /// </summary>
        /// <returns></returns>    
        public AttachmentInfo GetUserAgreementAttach(double agreementId)
        {
            var attach = RT.Service.Resolve<UserAgreementController>().GetUserAgreementAttach(agreementId);
            AttachmentController attachmentController = RT.Service.Resolve<AttachmentController>();

            var result = new AttachmentInfo()
            {
                FileName = attach.FileName,
                FliePath = attach.FilePath,
                FlieExtension = attach.FileExtesion,
                FullFilePath = Path.Combine(attachmentController.GetDownloadPath(), attach.FilePath)
            };

            return result;
        }
    }
}
