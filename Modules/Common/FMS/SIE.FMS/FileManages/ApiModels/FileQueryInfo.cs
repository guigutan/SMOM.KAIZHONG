using System;

namespace SIE.FMS.FileManages.ApiModels
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [Serializable]
    public class FileQueryInfo
    {
        /// <summary>
        /// 审核人（非必填）
        /// </summary>
        public double AuditorName { get; set; }

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

    }
}
