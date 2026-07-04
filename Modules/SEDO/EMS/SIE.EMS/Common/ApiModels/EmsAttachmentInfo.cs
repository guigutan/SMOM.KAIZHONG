using System;

namespace SIE.EMS.ApiModel
{
    /// <summary>
    /// 附件信息
    /// </summary>
    [Serializable]
    public class EmsAttachmentInfo
    {
        /// <summary>
        /// 附件Id  (上传时为空)
        /// </summary>
        public double Id { get; set; }


        /// <summary>
        /// 单据Id  
        /// </summary>
        public double? OwnerId { get; set; }

        /// <summary>
        /// 文件内容  
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文件大小  
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 扩展名  
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 文件名  
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径  (服务器的文件相对路径，需要拼接地址访问，上传时为空)
        /// </summary>
        public string FilePath { get; set; }
    }
}
