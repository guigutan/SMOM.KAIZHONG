using SIE.ObjectModel;

namespace SIE.FMS
{
    /// <summary>
    /// 文件状态
    /// </summary>
    public enum FileState
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Label("草稿")]
        Created,

        /// <summary>
        /// 发布
        /// </summary>
        [Label("发布")]
        Release,

        /// <summary>
        /// 修订
        /// </summary>
        [Label("修订")]
        Edit,

        /// <summary>
        /// 审核中
        /// </summary>
        [Label("待审核")]
        Audit,

        /// <summary>
        /// 待发布
        /// </summary>
        [Label("待发布")]
        ToRelease,

        /// <summary>
        /// 作废待审核
        /// </summary>
        [Label("作废待审核")]
        ToScrap,

        /// <summary>
        /// 作废待发布
        /// </summary>
        [Label("作废待发布")]
        ScrapToRelease,

        /// <summary>
        /// 作废
        /// </summary>
        [Label("作废")]
        Scrap,
    }
}
