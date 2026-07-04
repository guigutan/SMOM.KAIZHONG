using SIE.ObjectModel;

namespace SIE.Dock.DockAppoints
{
    /// <summary>
    /// 月台来源类型
    /// </summary>
    public enum DockSourceType
    {
        /// <summary>
        /// PC
        /// </summary>
        [Label("PC端自建")]
        PC,

        /// <summary>
        /// ASN单
        /// </summary>
        [Label("ASN单")]
        ASN,

        /// <summary>
        /// 微信
        /// </summary>
        [Label("微信")]
        WECHAT,
    }
}
