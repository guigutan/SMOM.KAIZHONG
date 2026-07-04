using System;
using System.Collections.Generic;

namespace SIE.EMS.ApiModel
{

    /// <summary>
    /// 附件集合信息
    /// </summary>
    [Serializable]
    public class EmsAttachmentInfoList
    {
        /// <summary>
        /// 附件信息集合  
        /// </summary>
        public List<EmsAttachmentInfo> AttachmentList { get; set; }


    }
}
