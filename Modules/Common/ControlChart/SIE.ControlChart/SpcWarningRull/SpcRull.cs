using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则基类
    /// </summary>
    [Serializable]
    public class SpcRull
    {
        /// <summary>
        /// 连接N个点
        /// </summary>
        public int N { get; set; }
        /// <summary>
        /// 数据点
        /// </summary>
        public List<DataPoint> Datas { get; set; } = new List<DataPoint>();
        /// <summary>
        /// 规则说明
        /// </summary>
        public string RullDescription { get; set; }
        /// <summary>
        /// 获取绘制信息
        /// </summary>
        public virtual List<ViolateSpcRullEvent> GetDrawInfo() { throw new NotImplementedException(); }
        /// <summary>
        /// 判断数据是否触发规则预警
        /// </summary>
        /// <param name="datas">数据</param>
        /// <returns>规则预警信息列表</returns>
        public virtual List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas) { throw new NotImplementedException(); }

        /// <summary>
        /// 获取样本数
        /// </summary>
        /// <returns></returns>
        public virtual SimpleSpcRull GetSimple()
        {
            SimpleSpcRull result = new SimpleSpcRull()
            {
                N = N,
                RullDescription = RullDescription
            };
            return result;
        }
    }
}
