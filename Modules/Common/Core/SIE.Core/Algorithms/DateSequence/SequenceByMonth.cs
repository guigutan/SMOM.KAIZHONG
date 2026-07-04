using SIE.Common.Algorithm;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Algorithms.DateSequence
{
    /// <summary>
    ///序列生成算法(区分当月日期)
    /// </summary>
    [Algorithm("序列生成算法(区分当月日期)", typeof(SequenceConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    [Label("序列生成算法(区分当月日期)")]
    public class SequenceByMonth : SequenceAlgorithm
    {
        /// <summary>
        /// 查询序列
        /// </summary>
        /// <returns></returns>
        protected override SequenceBase GetSequenceBase(int startValue)
        {
            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, 1);
            return RT.Service.Resolve<AlgorithmController>().GetDateSequence(Context.DetailId, date, startValue);
        }
    }
}
