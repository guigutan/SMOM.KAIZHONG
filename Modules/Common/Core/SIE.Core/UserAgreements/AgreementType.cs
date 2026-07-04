using SIE.ObjectModel;

namespace SIE.Core.UserAgreements
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public enum AgreementType
    {
        /// <summary>
        /// 服务协议
        /// </summary>
        [Label("服务协议")]
        Serve,

        /// <summary>
        /// 隐私协议
        /// </summary>
        [Label("隐私协议")]
        privacy,
    }
}
