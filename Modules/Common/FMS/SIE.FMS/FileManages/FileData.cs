using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件数据
    /// </summary>
    [Serializable]
    public class FileData
    {
        /// <summary>
        /// 文件夹Id
        /// </summary>
        public double? FolderId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string FileExtesion { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }       
    }
}
