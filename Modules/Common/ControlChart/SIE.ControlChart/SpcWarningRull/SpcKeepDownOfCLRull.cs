using System.Collections.Generic;
using System;
using System.Linq;
using SIE.ControlChart.SpcUtils;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-连续n个点保持在中心线下侧
    /// </summary>
    [Serializable]
    public class SpcKeepDownOfCLRull : SpcRull
    {
        /// <summary>
        /// Spc计算公式常量C1
        /// </summary>
        public double Cl { get; set; }
        /// <summary>
        /// 连续n个点保持在中心线下侧构造函数
        /// </summary>
        /// <param name="n"></param>
        public SpcKeepDownOfCLRull(int? n = null)
        {
            RullDescription = "连续{0}点，在中心线的下侧".L10nFormat(n);
            if (n.HasValue)
                N = n.Value;
        }
        /// <summary>
        /// 获取绘制控制图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            var count = 0;
            var violateRullDatas = new List<Tuple<double, DateTime?>>();
            for (var i = 0; i < Datas.Count; i++)
            {
                if (Datas[i].Value < Cl)
                {
                    count++;
                    violateRullDatas.Add(new Tuple<double, DateTime?>(Datas[i].Value, Datas[i].SamplingTime));
                }
                else
                {
                    if (count >= N)
                    {
                        toDrawOver(i - count - 1, i - 1);
                        KeepDownOfCLPointsViolationRull(violateRullEvents, violateRullDatas);
                    }
                    violateRullDatas.Clear();
                    count = 0;
                }
            }
            if (count >= N)
            {
                toDrawOver(Datas.Count - count - 1, Datas.Count - 1);
                KeepDownOfCLPointsViolationRull(violateRullEvents, violateRullDatas);
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
            if (null == datas || datas.Count < N || datas.Count == 0)
                return violateRullEvents;
            var violateRullDatas = new List<Tuple<double, DateTime?>>();
            for (var i = 0; i < datas.Count; i++)
            {
                if (datas[i].Value < Cl)
                {
                    violateRullDatas.Add(new Tuple<double, DateTime?>(datas[i].Value, datas[i].SamplingTime));
                }
                else
                {
                    KeepDownOfCLPointsViolationRull(violateRullEvents, violateRullDatas);
                    violateRullDatas.Clear();
                }
            }
            KeepDownOfCLPointsViolationRull(violateRullEvents, violateRullDatas);
            return violateRullEvents;
        }

        /// <summary>
        /// 连续N个点保持在中心线下侧（N为违反SPC判异规则的连续数据点数）
        /// </summary>
        /// <param name="violateRullEvents">违反SPC判异规则事件</param>
        /// <param name="violateRullDatas">违反SPC判异规则的数据点集合</param>
        private void KeepDownOfCLPointsViolationRull(List<ViolateSpcRullEvent> violateRullEvents, List<Tuple<double, DateTime?>> violateRullDatas)
        {
            if (violateRullDatas.Count >= N)
            {
                if (violateRullDatas.Count > N)
                {
                    //以下逻辑是为了处理违反SPC判异规则的连续数据点数超过了控制图规则中判异规则设定值的时候，将两次预警的数据一起推送的问题
                    var list = violateRullDatas.GetRange(0, N).ToList();
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, list.Select(p => p.Item1).ToList(), list.LastOrDefault()?.Item2));
                    for (int j = 0; j < violateRullDatas.Count - list.Count; j++)
                    {
                        violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, violateRullDatas.GetRange(j + 1, N).Select(p => p.Item1).ToList(), violateRullDatas.GetRange(j + 1, N).LastOrDefault()?.Item2));
                    }
                }
                else
                {
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, N, violateRullDatas.Select(p => p.Item1).ToList(), violateRullDatas.LastOrDefault()?.Item2));
                }
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
