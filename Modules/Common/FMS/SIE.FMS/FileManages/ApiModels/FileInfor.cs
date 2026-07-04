using System;

namespace SIE.FMS.FileManages.ApiModels
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [Serializable]
    public class FileInfor
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public double FileId { get; set; }

        /// <summary>
        /// 文件编号 
        /// </summary>
        public string FileCode { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 文件路径 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public string Size { get; set; }


    }
}
