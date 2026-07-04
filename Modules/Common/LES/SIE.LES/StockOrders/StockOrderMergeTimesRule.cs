using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 合并时间段验证
    /// </summary>
    [DisplayName("合并时间段验证")]
    [Description("合并时间段验证")]
    public class StockOrderMergeTimesRule : EntityRule<StockOrderMergeTimes>
    {
        /// <summary>
        /// 
        /// </summary>
        public StockOrderMergeTimesRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var stockOrderMergeTimes = entity as StockOrderMergeTimes;
            if (stockOrderMergeTimes.StartTime == stockOrderMergeTimes.EndTime && !stockOrderMergeTimes.IsCrossDay)
            {
                e.BrokenDescription = "起始需求时间不能大于等于结束需求时间".L10N();
            }
            if (RT.Service.Resolve<StockOrderMergeIssuedController>().IsIntersection(stockOrderMergeTimes))
            {
                e.BrokenDescription = "时间段存在交集".L10N();
            }
        }
    }
}
