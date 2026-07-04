using System;

namespace SIE.LES.PrepareItems.Models
{
    /// <summary>
    /// 拉式备料 Job 执行返回信息
    /// </summary>
    [Serializable]
    public class PrepareItemPullResult
    {
        /// <summary>
        /// 累计匹配（xxx）笔（仓库+物料）
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 满足触发方式的共有（XXX）笔数据
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
