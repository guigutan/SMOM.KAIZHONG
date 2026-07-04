using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 合并下发规则明细保存
    /// </summary>
    [System.ComponentModel.DisplayName("合并下发规则明细保存")]
    [System.ComponentModel.Description("合并下发规则明细保存")]
    public class StockOrderMergeTimesSubmmiting : OnSubmitting<StockOrderMergeTimes>
    {
        /// <summary>
        /// 合并下发规则明细保存
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">行为</param>
        protected override void Invoke(StockOrderMergeTimes entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                if (entity.StartTimeText.IsNullOrEmpty() || entity.EndTimeText.IsNullOrEmpty())
                {
                    throw new ValidationException("起始时间和结束时间必须填写".L10N());
                }
                entity.StartTimeText = entity.StartTimeText.Replace("：", ":");
                entity.EndTimeText = entity.EndTimeText.Replace("：", ":");
                if (!DateTime.TryParse("2000-01-01 " + entity.StartTimeText, out DateTime d1))
                    throw new ValidationException("起始时间格式不正确，正确格式12:01:01".L10N());
                entity.StartTime = d1;
                if (!DateTime.TryParse("2000-01-01 " + entity.EndTimeText, out DateTime d2))
                    throw new ValidationException("起始时间格式不正确，正确格式12:01:01".L10N());
                entity.EndTime = d2;
                if (entity.StartTime == entity.EndTime && !entity.IsCrossDay)
                {
                    throw new ValidationException("起始需求时间不能大于等于结束需求时间".L10N());
                }
                if (RT.Service.Resolve<StockOrderMergeIssuedController>().IsIntersection(entity))
                {
                    throw new ValidationException("时间段存在交集".L10N());
                }
            }
        }
    }
}
