using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ApiModels
{
    /// <summary>
    /// 附件信息
    /// </summary>
    [Serializable]
    public class AttachmentData
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtesion { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        ///  内容
        /// </summary>
        public string Content { get; set; }
    }
}
