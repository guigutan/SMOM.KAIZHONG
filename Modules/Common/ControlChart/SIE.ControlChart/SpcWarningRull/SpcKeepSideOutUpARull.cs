using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-连续n个点大于外控上限
    /// </summary>
    [Serializable]
    public class SpcKeepSideOutUpARull : SpcRull
    {
        /// <summary>
        /// 控制上线
        /// </summary>
        public double Ucl { get; set; }
        /// <summary>
        /// 连续n个点大于外控上限构造函数
        /// </summary>
        /// <param name="n"></param>
        public SpcKeepSideOutUpARull(int? n = null)
        {
            RullDescription = "连续n个点大于外控上限".L10N();
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
            for (var i = 1; i < Datas.Count; i++)
            {
                if (Datas[i].Value > Ucl)
                    count++;
                else
                {
                    if (count >= N)
                        toDrawOver(i - count - 1, i - 1);
                    count = 0;
                }
            }
            if (count >= N)
                toDrawOver(Datas.Count - count - 1, Datas.Count - 1);
            return violateRullEvents;
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

        /// <summary>
        /// 判断数据是否触发判异规则
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public override List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas)
        {
            throw new NotImplementedException();
        }
    }
}
