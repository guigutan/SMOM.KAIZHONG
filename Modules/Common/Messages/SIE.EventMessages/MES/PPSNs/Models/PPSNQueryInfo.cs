using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.PPSNs
{
	/// <summary>
	/// PPSN查询信息
	/// </summary>
	[Serializable]
    public class PPSNQueryInfo
    {
        /// <summary>
        /// ReelID集合
        /// </summary>
        public List<string> SnList { get; set; } = new List<string>();


        /// <summary>
        /// 物料批次号
        /// </summary>
        public List<string> ItemBatchList { get; set; } = new List<string>();

        /// <summary>
        /// 物料编码
        /// </summary>
        public double ItemId { get; set; }


        /// <summary>
        /// 开始时间（ReelId绑定时间）
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间（ReelId绑定时间）
        /// </summary>
        public DateTime? EndDate { get; set; }

    }
}