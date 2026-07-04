using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 附件API信息类
    /// </summary>
    [Serializable]
    public class AttachmentInfo
    {

        /*/// <summary>
        /// 内容
        /// </summary>
        public byte[] Content { get; set; }*/

        /// <summary>
        /// 内容
        /// </summary>
        public string ContentBase64 { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtesion { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /*/// <summary>
        /// 文件路径--PDA端不传输
        /// </summary>
        public string FilePath { get; set; }*/

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
    }
}
