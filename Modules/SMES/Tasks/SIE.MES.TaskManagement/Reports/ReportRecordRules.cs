using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    #region 报工记录数量验证规则
    /// <summary>
    /// 报工记录数量验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("报工记录数量验证规则")]
    [System.ComponentModel.Description("报工记录，不合格数大于0时，必须录入缺陷，不合格数等于0时，不能录入缺陷")]
    public class ReportRecordQtyRule : EntityRule<ReportRecord>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var data = entity as ReportRecord;
            if (data != null && data.NgQty > 0 && data.Defects.Count == 0)
            {
                e.BrokenDescription = "不合格数大于0时，必须录入缺陷".L10nFormat();
            }
            else if (data != null && data.NgQty <= 0 && data.Defects.Count > 0)
            {
                data.Defects.Clear();
                //e.BrokenDescription = "不合格数为0时，不能录入缺陷".L10nFormat();
            }
        }
    }
    #endregion
}
