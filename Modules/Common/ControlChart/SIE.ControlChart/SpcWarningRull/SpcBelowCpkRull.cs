using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-CPK低于K值预警
    /// </summary>
    [Serializable]
    public class SpcBelowCpkRull : SpcRull
    {
        /// <summary>
        /// cpk
        /// </summary>
        public double? Cpk { get; set; }

        /// <summary>
        /// CPK低于K值预警
        /// </summary>
        public SpcBelowCpkRull(double? k = null)
        {
            RullDescription = "CPK低于{0}".L10nFormat(k);
            if (k.HasValue)
            {
                Cpk = k.Value;
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
                if (Cpk.HasValue && Datas[i].Value < Cpk.Value)
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
                if (Cpk.HasValue && ((SampleGroup)datas[0]).Cpk < Cpk.Value)
                {
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, null, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                }
            }
            return violateRullEvents;
        }
    }
}
