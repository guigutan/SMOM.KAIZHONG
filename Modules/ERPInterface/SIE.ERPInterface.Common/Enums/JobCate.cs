using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 任务类别
    /// </summary>
    public enum JobCate
    {
        /// <summary>
        /// 下载
        /// </summary>
        [Label("下载")]
        Download = 0,
        /// <summary>
        /// 上传
        /// </summary>
        [Label("上传")]
        Upload = 1,
    }
}