using SIE.ObjectModel;

namespace SIE.EMS.Logs
{
    /// <summary>
    /// 上传类型
    /// </summary>
    public enum UploadType
    {
        /// <summary>
        /// 设备盘点
        /// </summary>
        [Label("设备盘点")]
        Machine,

        /// <summary>
        /// 工治具盘点
        /// </summary>
        [Label("工治具盘点")]
        Encode,

        /// <summary>
        /// 点检执行
        /// </summary>
        [Label("点检执行")]
        Check,
    }
}
