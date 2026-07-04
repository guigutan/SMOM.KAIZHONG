using System;

namespace SIE.LES.PrepareItems.Models
{
    /// <summary>
    /// 推式备料 Job 执行返回信息
    /// </summary>
    [Serializable]
    public class PrepareItemPushResult
    {
        /// <summary>
        /// 累计匹配（XXX）（资源+物料+触发方式）的数据
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 共有（XXX）个发放的工单
        /// </summary>
        public int WoCount { get; set; }

        /// <summary>
        /// 共有（XXX）个（工单+物料+触发方式）满足触发条件
        /// </summary>
        public int FitDataCount { get; set; }

        /// <summary>
        /// 合计生成（XXX）个备料需求
        /// </summary>
        public int PrepareDataCount { get; set; }

        /// <summary>
        /// 生成（XXX）个备料单
        /// </summary>
        public int StockOrderCount { get; set; }
    }
}
