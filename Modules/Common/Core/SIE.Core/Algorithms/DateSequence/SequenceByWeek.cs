using SIE.Common.Algorithm;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Algorithms.DateSequence
{
    /// <summary>
    ///序列生成算法(区分当月日期)
    /// </summary>
    [Algorithm("序列生成算法(区分当周日期)", typeof(SequenceConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    [Label("序列生成算法(区分当周日期)")]
    public class SequenceByWeek : SequenceAlgorithm
    {
        /// <summary>
        /// 查询序列
        /// </summary>
        /// <returns></returns>
        protected override SequenceBase GetSequenceBase(int startValue)
        {
            var dateNow = DateTime.Now;
            //星期一为第一天  
            int weeknow = Convert.ToInt32(dateNow.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            DateTime FirstDay = dateNow.AddDays(daydiff);
            return RT.Service.Resolve<AlgorithmController>().GetDateSequence(Context.DetailId, FirstDay.Date, startValue);
        }
    }
}
