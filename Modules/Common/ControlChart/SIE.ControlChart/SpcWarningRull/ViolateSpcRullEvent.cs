using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// 违反SPC规则事件
    /// </summary>
    [Serializable]
    public class ViolateSpcRullEvent
    {
        /// <summary>
        /// 违反SPC规则事件构造函数
        /// </summary>
        /// <param name="rullDescription"></param>
        /// <param name="k"></param>
        /// <param name="datas"></param>
        /// <param name="lastDataTime"></param>
        public ViolateSpcRullEvent(string rullDescription, int? k, List<double> datas, DateTime? lastDataTime)
        {
            ViolateTime = DateTime.Now;
            RullDescription = rullDescription;
            K = k;
            Datas = datas;
            LastDataTime = lastDataTime;
        }

        /// <summary>
        /// K值
        /// </summary>
        public int? K { get; set; }

        /// <summary>
        /// 违反规则时间
        /// </summary>
        public DateTime ViolateTime { get; private set; }

        /// <summary>
        /// 规则说明
        /// </summary>
        public string RullDescription { get; set; }


        /// <summary>
        /// 最后一个数据的时间
        /// </summary>
        public DateTime? LastDataTime { get; set; }

        /// <summary>
        /// 违反规则的数据
        /// </summary>
        public List<double> Datas { get; set; }

    }
}
