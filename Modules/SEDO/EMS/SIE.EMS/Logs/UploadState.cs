using SIE.ObjectModel;

namespace SIE.EMS.Logs
{
    /// <summary>
    /// 上传状态
    /// </summary>
    public enum UploadState
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Label("成功")]
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Fail,
    }
}
