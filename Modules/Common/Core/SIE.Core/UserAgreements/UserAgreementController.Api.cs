using SIE.Api;
using SIE.Core.ApiModels;

namespace SIE.Core.UserAgreements
{
    public partial class UserAgreementController : DomainController
    {
        /// <summary>
        /// 获取电子签名用户协议PDF内容
        /// </summary>
        /// <returns></returns>
        [ApiService("获取电子签名PDA协议")]
        [return: ApiReturn("获取电子签名PDA协议")]
        public virtual AttachmentInfo GetPdaUserAgreement()
        {
            return GetEnableUserAgreement() as AttachmentInfo;
        }
    }
}
