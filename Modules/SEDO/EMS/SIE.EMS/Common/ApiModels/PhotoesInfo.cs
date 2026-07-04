using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Common.ApiModels
{
    /// <summary>
    /// 照片上传信息
    /// </summary>
    [Serializable]
    public class PhotoesInfo
    {
        /// <summary>
        /// 附件ID
        /// </summary>
        public double? Id { get; set; }

        /// <summary>
        /// 文件名(需带扩展名)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public string Content { get; set; }
    }
}
