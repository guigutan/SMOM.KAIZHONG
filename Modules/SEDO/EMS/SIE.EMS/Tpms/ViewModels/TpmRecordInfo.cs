using System;

namespace SIE.EMS.Tpms.ViewModels
{
    /// <summary>
    /// TPM操作记录信息
    /// </summary>
    [Serializable]
    public class TpmRecordInfo
    {
        /// <summary>
        /// TPM操作记录
        /// </summary>
        public TpmRecord Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
