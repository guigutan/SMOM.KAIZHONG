using SIE.Common.Algorithm;
using SIE.Common.NumberRules;
using SIE.Data;
using SIE.Data.Transaction;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Algorithms
{
    /// <summary>
    /// 算法类控制器
    /// </summary>
    public class AlgorithmExtController : AlgorithmController
    {
        /// <summary>
        /// 获取 日期序列日志
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="date"></param>
        /// <param name="startValue"></param>
        /// <returns></returns>
        public override SIE.Common.Algorithm.DateSequence GetDateSequence(double detailId, DateTime date, int startValue)
        {
            if (RF.GetById<NumberRuleDetail>(detailId) == null)
            {
                throw new EntityNotFoundException(typeof(NumberRuleDetail), detailId);
            }

            var dateSequence = DB.Query<SIE.Common.Algorithm.DateSequence>().Where(p => p.DetailId == detailId && p.Date == date.Date).FirstOrDefault();
            if (dateSequence == null)
            {
                if (startValue == 0)
                {
                    AppRuntime.Logger.Warn($"时间序列开始值为0警告, detailId: {detailId} date: {date}");
                }

                using (var trans = DB.TransactionScope(RF.Find<NumberRuleDetail>()))
                {
                    DB.Update<NumberRuleDetail>().Set(p => p.Id, detailId).Where(p => p.Id == detailId).Execute();
                    dateSequence = DB.Query<SIE.Common.Algorithm.DateSequence>().Where(p => p.DetailId == detailId && p.Date == date.Date).FirstOrDefault();

                    if (dateSequence == null)
                    {
                        dateSequence = new SIE.Common.Algorithm.DateSequence();
                        dateSequence.CurrentValue = startValue;
                        dateSequence.Date = date;
                        dateSequence.DetailId = detailId;
                        RF.Save(dateSequence);
                    }

                    trans.Complete();
                }
            }

            return dateSequence;
        }

        /// <summary>
        /// 获取 普通序列日志
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="startValue"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Sequence GetSequence(double detailId, int startValue, string key)
        {

            if (RF.GetById<NumberRuleDetail>(detailId) == null)
            {
                throw new EntityNotFoundException(typeof(NumberRuleDetail), detailId);
            }

            string seqKey = key ?? "";
            var sequence = DB.Query<Sequence>().Where(p => p.DetailId == detailId && p.Key == seqKey).FirstOrDefault();
            if (sequence == null)
            {
                using (var trans = DB.TransactionScope(RF.Find<NumberRuleDetail>()))
                {
                    DB.Update<NumberRuleDetail>().Set(p => p.Id, detailId).Where(p => p.Id == detailId).Execute();
                    sequence = DB.Query<Sequence>().Where(p => p.DetailId == detailId && p.Key == seqKey).FirstOrDefault();

                    if (sequence == null)
                    {
                        sequence = new Sequence();
                        sequence.CurrentValue = startValue;
                        sequence.DetailId = detailId;
                        sequence.Key = key;
                        RF.Save(sequence);
                    }

                    trans.Complete();
                }
            }

            return sequence;
        }
    }
}
