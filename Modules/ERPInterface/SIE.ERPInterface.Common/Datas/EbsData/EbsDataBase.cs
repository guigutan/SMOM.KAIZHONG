using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 基类数据
    /// </summary>
    [Serializable]
    public class EbsDataBase
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int ROW_NUM { get; set; }

        /// <summary>
        /// KEY
        /// </summary>
        public string Pri_Key { get; set; }

        /// <summary>
        /// 最后下载时间
        /// </summary>
        public DateTime? LAST_UPDATE_DATE { get; set; }

        /// <summary>
        /// 重复的数据标记
        /// </summary>
        public bool IsRepeat { get; set; }
    }
}
