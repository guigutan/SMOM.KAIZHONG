using System;

namespace SIE.Core.ApiModels
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [Serializable]
    public class AttachmentInfo
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string FlieExtension { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FliePath { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ContentBase64 { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// OwnerId
        /// </summary>
        public double? OwnerId { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// 完整文件路径
        /// </summary>
        public string FullFilePath { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public double? CreateUserId { get; set; }

    }

    /// <summary>
    /// 上传成功后返回文件ID和文件地址
    /// </summary>
    public class UploadFileInfo
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FliePath { get; set; }
    }
}
