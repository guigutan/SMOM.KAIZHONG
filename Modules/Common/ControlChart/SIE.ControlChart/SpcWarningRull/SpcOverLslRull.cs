using SIE.ControlChart.SpcUtils;
using System;
using System.Collections.Generic;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则-超出规格下限预警
    /// </summary>
    [Serializable]
    public class SpcOverLslRull : SpcRull
    {
        /// <summary>
        /// 规格下线
        /// </summary>
        public double? Lsl { get; set; }
      
        /// <summary>
        /// 超出规格下限预警构造函数
        /// </summary>
        public SpcOverLslRull()
        {
            RullDescription = "超出了规格下限".L10N();
        }

        /// <summary>
        /// 获取绘制控制图信息
        /// </summary>
        public override List<ViolateSpcRullEvent> GetDrawInfo()
        {
            var violateRullEvents = new List<ViolateSpcRullEvent>();
            for (var i = 0; i < Datas.Count; i++)
            {
                if (Lsl.HasValue && Datas[i].Value < Lsl.Value)
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
                if (Lsl.HasValue && datas[i].Value < Lsl.Value)
                {
                    violateRullEvents.Add(new ViolateSpcRullEvent(RullDescription, null, new List<double>() { datas[i].Value }, Datas[i].SamplingTime));
                }
            }
            return violateRullEvents;
        }
    }
}
