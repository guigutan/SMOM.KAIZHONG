using System.Collections.Generic;
using System;
using System.Linq;
using SIE.ControlChart.SpcUtils;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-连续n个保持上下交替
    /// </summary>
    [Serializable]
    public class SpcKeepStaggeredRull : SpcRull
    {
        /// <summary>
        /// 连续n个保持上下交替构造函数
        /// </summary>
        /// <param name="n"></param>
        public SpcKeepStaggeredRull(int? n = null)
        {
            RullDescription = "连续{0}点，上下交错".L10nFormat(n);
            if (n.HasValue)
                N = n.Value;
        }

        /// <summary>
        /// 获取绘制控制图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (N <= 2)
                return violateRullEvents;
            var count = 0;
            for (var i = 2; i < Datas.Count; i++)
            {
                if (Datas[i].Value > Datas[i - 1].Value && Datas[i - 1].Value < Datas[i - 2].Value
                    || Datas[i].Value < Datas[i - 1].Value && Datas[i - 1].Value > Datas[i - 2].Value)
                    count++;
                else
                {
                    if (count >= N - 2)
                    {
                        toDrawOver(i - count - 2, i);
                        ContinousPointsViolationRull(Datas, violateRullEvents, count, i);
                    }
                    count = 0;
                }
            }
            if (count >= N - 2)
            {
                toDrawOver(Datas.Count - count - 2, Datas.Count);
                ContinousPointsViolationRull(Datas, violateRullEvents, count, Datas.Count);
            }
            return violateRullEvents;
        }
        /// <summary>
        /// 判断数据是否触发判异规则
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public override List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas)
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            if (N <= 2)
                return violateRullEvents;
            if (null == datas || datas.Count < N || datas.Count == 0)
                return violateRullEvents;
            var count = 0;
            for (var i = 2; i < datas.Count; i++)
            {
                if (datas[i].Value > datas[i - 1].Value && datas[i - 1].Value < datas[i - 2].Value
                    || datas[i].Value < datas[i - 1].Value && datas[i - 1].Value > datas[i - 2].Value)
                    count++;
                else
                {
                    ContinousPointsViolationRull(datas, violateRullEvents, count, i);
                    count = 0;
                }
            }
            ContinousPointsViolationRull(datas, violateRullEvents, count, datas.Count);
            return violateRullEvents;
        }

        /// <summary>
        /// 连续N点，上下交错（N > K时，N为违反SPC判异规则的连续数据点数，K为控制图规则中判异规则设定的值）
        /// </summary>
        /// <param name="datas">连续的数据点</param>
        /// <param name="violateRullEvents">违反SPC判异规则事件</param>
        /// <param name="count"></param>
        /// <param name="i"></param>
        private void ContinousPointsViolationRull(List<DataPoint> datas, List<ViolateSpcRullEvent> violateRullEvents, int count, int i)
        {
            if (count >= N - 2)
            {
                if (count > N - 2)
                {
                    //以下逻辑是为了处理违反SPC判异规则的连续数据点数超过了控制图规则中判异规则设定值的时候，将两次预警的数据一起推送的问题
                    var list = datas.GetRange(i - count - 2, N).Select(d => d.Value).ToList();
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, list, datas.GetRange(i - count - 2, N).LastOrDefault()?.SamplingTime));
                    if (i - count - 2 == 0)
                    {
                        for (int j = 0; j < datas.Count - list.Count; j++)
                        {
                            violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, datas.GetRange(j + 1, N).Select(d => d.Value).ToList(), datas.GetRange(j + 1, N).LastOrDefault()?.SamplingTime));
                        }
                    }
                }
                else
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, datas.GetRange(i - count - 2, count + 2).Select(d => d.Value).ToList(), datas.GetRange(i - count - 2, count + 2).LastOrDefault()?.SamplingTime));
            }
        }

        private void toDrawOver(int beginIndex, int endIndex)
        {

            for (var j = beginIndex + 1; j < endIndex; j++)
                Datas[j].IsWarnLine = true;
            for (var i = beginIndex; i < endIndex; i++)
            {
                Datas[i].IsWarnPoint = true;
                Datas[i].Warnings.Add(GetSimple());
            }
        }

    }
}
