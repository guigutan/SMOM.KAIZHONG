using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-PPK低于K值预警
    /// </summary>
    [Serializable]
    public class SpcBelowPpkRull : SpcRull
    {
        /// <summary>
        /// K值
        /// </summary>
        public double? Ppk { get; set; }

        /// <summary>
        /// PPK低于K值预警
        /// </summary>
        public SpcBelowPpkRull(double? k = null)
        {
            RullDescription = "PPK低于{0}".L10nFormat(k);
            if (k.HasValue)
            {
                Ppk = k.Value;
            }
        }

        /// <summary>
        /// 获取绘制控制图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            for (var i = 0; i < Datas.Count; i++)
            {
                if (Ppk.HasValue && Datas[i].Value < Ppk.Value)
                {
                    Datas[i].Warnings.Add(GetSimple());
                    Datas[i].IsWarnPoint = true;
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, null, new List<double>() { Datas[i].Value }, Datas[i].SamplingTime));
                }
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
            if (null == datas || datas.Count == 0)
                return violateRullEvents;
            for (var i = 0; i < datas.Count; i++)
            {
                if (Ppk.HasValue && ((SampleGroup)datas[0]).Ppk < Ppk.Value)
                {
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, null, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                }
            }
            return violateRullEvents;
        }
    }
}
