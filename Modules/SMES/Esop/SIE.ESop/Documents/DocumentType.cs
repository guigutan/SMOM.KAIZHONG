using SIE.ObjectModel;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档类型
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// 文档
        /// </summary>
        [Label("文档")]
        Document = 0,

        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        Img = 1,

        /// <summary>
        /// 视频
        /// </summary>
        [Label("视频")]
        Video = 2,
    }
}