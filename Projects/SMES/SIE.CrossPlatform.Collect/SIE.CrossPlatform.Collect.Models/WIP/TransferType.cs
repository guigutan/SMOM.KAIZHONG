using SIE.CrossPlatform.Collect.Models.Attributes;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 工序交接类型
    /// </summary>
    public enum TransferType
    {

        /// <summary>
        /// 转入转出
        /// </summary>\
        [Label("转入转出")]
        TransferInOut = 0,
        /// <summary>
        /// 转入
        /// </summary>
        [Label("转入")]
        TransferIn = 1,
        /// <summary>
        /// 转出
        /// </summary>
        [Label("转出")]
        TransferOut = 2,

    }
}
