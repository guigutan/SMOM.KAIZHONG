using System.Collections.Generic;
using System.Linq;
using System;
using SIE.ControlChart.SpcUtils;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-连续m个点中有n个点在同一侧B区外
    /// </summary>
    [Serializable]
    public class SpcKeepOneSideOutBRull : SpcRull
    {
        /// <summary>
        /// 中心线
        /// </summary>
        public int M { get; set; }
        /// <summary>
        /// 均值图上BA分区值
        /// </summary>
        public double Uba { get; set; }
        /// <summary>
        /// 均值图上BA分区值
        /// </summary>
        public double Lba { get; set; }
        /// <summary>
        /// 连续m个点中有n个点在同一侧B区外构造函数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        public SpcKeepOneSideOutBRull(int? m = null, int? n = null)
        {
            RullDescription = "{0} 个点中有{1}个点，距离中心线（同侧）大于2个标准差".L10nFormat(n+1,n);
            if (m.HasValue)
                M = m.Value;
            if (n.HasValue)
                N = n.Value;
        }
        /// <summary>
        /// 获取绘图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            for (var i = M - 1; i < Datas.Count; i++)
            {
                var c_b = 0;
                for (var j = i - (M - 1); j <= i; j++)
                {
                    if (Datas[j].Value > Uba)
                        c_b++;
                }
                if (c_b >= N)
                {
                    toDrawOver(i - (M - 1), i);
                    violateRullEvents.Add(new ViolateSpcRullEvent("K+1 个点中有K个点，距离中心线（上侧）大于2个标准差", N, Datas.GetRange(i - (M - 1), M).Select(d => d.Value).ToList(), Datas[i].SamplingTime));
                }

                c_b = 0;
                for (var j = i - (M - 1); j <= i; j++)
                {
                    if (Datas[j].Value < Lba)
                        c_b++;
                }
                if (c_b >= N)
                {
                    toDrawOver(i - (M - 1), i);
                    violateRullEvents.Add(new ViolateSpcRullEvent("K+1 个点中有K个点，距离中心线（下侧）大于2个标准差", N, Datas.GetRange(i - (M - 1), M).Select(d => d.Value).ToList(), Datas[i].SamplingTime));
                }
            }
            return violateRullEvents;
        }

        /// <summary>
        /// 判断数据是否触发预警规则
        /// </summary>
        /// <param name="datas">数据点集合</param>
        /// <returns></returns>
        public override List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas)
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (null == datas || datas.Count < M || datas.Count == 0)
                return violateRullEvents;
            for (var i = M - 1; i < datas.Count; i++)
            {
                var c_b = 0;
                for (var j = i - (M - 1); j <= i; j++)
                {
                    if (datas[j].Value > Uba)
                        c_b++;
                }
                if (c_b >= N)
                    violateRullEvents.Add(new ViolateSpcRullEvent("K+1 个点中有K个点，距离中心线（上侧）大于2个标准差", N, datas.GetRange(i - (M - 1), M).Select(d => d.Value).ToList(), Datas[i].SamplingTime));

                c_b = 0;
                for (var j = i - (M - 1); j <= i; j++)
                {
                    if (datas[j].Value < Lba)
                        c_b++;
                }
                if (c_b >= N)
                    violateRullEvents.Add(new ViolateSpcRullEvent("K+1 个点中有K个点，距离中心线（下侧）大于2个标准差", N, datas.GetRange(i - (M - 1), M).Select(d => d.Value).ToList(), Datas[i].SamplingTime));
            }
            return violateRullEvents;
        }

        private void toDrawOver(int beginIndex, int endIndex)
        {

            for (var j = beginIndex + 1; j <= endIndex; j++)
                Datas[j].IsWarnLine = true;
            for (var i = beginIndex; i <= endIndex; i++)
            {
                Datas[i].IsWarnPoint = true;
                Datas[i].Warnings.Add(GetSimple());
            }
        }
    }
}
