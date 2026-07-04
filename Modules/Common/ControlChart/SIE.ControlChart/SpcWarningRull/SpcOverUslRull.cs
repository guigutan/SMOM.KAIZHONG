using System.Collections.Generic;
using System;
using SIE.ControlChart.SpcUtils;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-超出规格上限预警
    /// </summary>
    [Serializable]
    public class SpcOverUslRull : SpcRull
    {
        /// <summary>
        /// 规则上线
        /// </summary>
        public double? Usl { get; set; }

        /// <summary>
        /// 超出规格上限预警构造函数
        /// </summary>
        public SpcOverUslRull()
        {
            RullDescription = "超出了规格上限".L10N();
        }

        /// <summary>
        /// 获取绘制控制图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            for (var i = 0; i < Datas.Count; i++)
            {
                if (Usl.HasValue && Datas[i].Value > Usl.Value)
                {
                    Datas[i].Warnings.Add(GetSimple());
                    Datas[i].IsWarnPoint = true;
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, null, new List<double>() { Datas[i].Value }, Datas[i].SamplingTime));
                }
            }
            return violateRullEvents;
        }

        /// <summary>
        /// 判断数据是否触发规则预警
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
                if (Usl.HasValue && datas[i].Value > Usl.Value)
                {
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, null, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                }
            }
            return violateRullEvents;
        }
    }
}
