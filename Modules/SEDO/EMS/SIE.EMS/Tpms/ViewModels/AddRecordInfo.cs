using System;
using System.Collections.Generic;

namespace SIE.EMS.Tpms.ViewModels
{
    /// <summary>
    /// 添加TPM操作记录信息
    /// </summary>
    [Serializable]
    public class AddRecordInfo
    {
        /// <summary>
        /// TPM操作记录
        /// </summary>
        public TpmRecord TpmRecord { get; set; }

        /// <summary>
        /// TPM评分明细
        /// </summary>
        public List<TpmRecordDetail> TpmRecordDetailList { get; set; }
    }
}
