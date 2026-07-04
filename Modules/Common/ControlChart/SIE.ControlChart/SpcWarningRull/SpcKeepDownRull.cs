using System.Collections.Generic;
using System;
using System.Linq;
using SIE.ControlChart.SpcUtils;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-连续n个点保持递减
    /// </summary>
    [Serializable]
    public class SpcKeepDownRull : SpcRull
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="n">数据点</param>
        public SpcKeepDownRull(int? n = null)
        {
            RullDescription = "连续{0}点，连续递减".L10nFormat(n);
            if (n.HasValue)
                N = n.Value;
        }

        /// <summary>
        /// 获取绘图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (Datas.IsNullOrEmpty())
                return violateRullEvents;
            var lastValue = Datas[0].Value;
            var count = 0;
            for (var i = 1; i < Datas.Count; i++)
            {
                if (Datas[i].Value < lastValue)
                    count++;
                else
                {
                    if (count >= N - 1)
                    {
                        toDrawOver(i - count - 2, i - 1);
                        KeepDownPointsViolationRull(Datas, violateRullEvents, count, i);
                    }
                    count = 0;
                }
                lastValue = Datas[i].Value;
            }
            if (count >= N - 1)
            {
                toDrawOver(Datas.Count - count - 2, Datas.Count - 1);
                KeepDownPointsViolationRull(Datas, violateRullEvents, count, Datas.Count);
            }
            return violateRullEvents;
        }

        /// <summary>
        /// 连续N个点保持递减（N为违反SPC判异规则的连续数据点数）
        /// </summary>
        /// <param name="datas">连续的数据点</param>
        public override List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas)
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (null == datas || datas.Count < N || datas.Count == 0)
                return violateRullEvents;
            var lastValue = datas[0].Value;
            var count = 0;
            for (var i = 1; i < datas.Count; i++)
            {
                if (datas[i].Value < lastValue)
                    count++;
                else
                {
                    KeepDownPointsViolationRull(datas, violateRullEvents, count, i);
                    count = 0;
                }
                lastValue = datas[i].Value;
            }
            KeepDownPointsViolationRull(datas, violateRullEvents, count, datas.Count);
            return violateRullEvents;
        }

        /// <summary>
        /// 连续N个点保持递增（N为违反SPC判异规则的连续数据点数）
        /// </summary>
        /// <param name="datas">连续的数据点</param>
        /// <param name="violateRullEvents">违反SPC判异规则事件</param>
        /// <param name="count"></param>
        /// <param name="i"></param>
        private void KeepDownPointsViolationRull(List<DataPoint> datas, List<ViolateSpcRullEvent> violateRullEvents, int count, int i)
        {
            if (count >= N - 1)
            {
                if (count > N - 1)
                {
                    //以下逻辑是为了处理违反SPC判异规则的连续数据点数超过了控制图规则中判异规则设定值的时候，将两次预警的数据一起推送的问题
                    var list = datas.GetRange(i - count - 1, N).Select(d => d.Value).ToList();
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, list, datas.GetRange(i - count - 1, N).LastOrDefault()?.SamplingTime));
                    if (i - count - 1 == 0)
                    {
                        for (int j = 0; j < datas.Count - list.Count; j++)
                        {
                            violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, datas.GetRange(j + 1, N).Select(d => d.Value).ToList(), datas.GetRange(j + 1, N).LastOrDefault()?.SamplingTime));
                        }
                    }
                }
                else
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, datas.GetRange(i - count - 1, count + 1).Select(d => d.Value).ToList(), datas.GetRange(i - count - 1, count + 1).LastOrDefault()?.SamplingTime));
            }
        }

        private void toDrawOver(int beginIndex, int endIndex)
        {

            for (var j = beginIndex + 2; j <= endIndex; j++)
                Datas[j].IsWarnLine = true;
            for (var i = beginIndex + 1; i <= endIndex; i++)
            {
                Datas[i].IsWarnPoint = true;
                Datas[i].Warnings.Add(GetSimple());
            }
        }
    }
}
