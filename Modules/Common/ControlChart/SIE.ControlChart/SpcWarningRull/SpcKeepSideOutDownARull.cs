using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-连续n个点小于外控下限
    /// </summary>
    [Serializable]
    public class SpcKeepSideOutDownARull : SpcRull
    {
        /// <summary>
        /// 控制下线
        /// </summary>
        public double Lcl { get; set; }
        /// <summary>
        /// 连续n个点小于外控下限构造函数
        /// </summary>
        /// <param name="n"></param>
        public SpcKeepSideOutDownARull(int? n = null)
        {
            RullDescription = "连续n个点小于外控下限".L10N();
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
                if (Datas[i].Value < Lcl)
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
        /// <summary>
        /// 判读数据是否触发判异规则
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public override List<ViolateSpcRullEvent> JudgeViolationRull(List<DataPoint> datas)
        {
            throw new NotImplementedException();
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
