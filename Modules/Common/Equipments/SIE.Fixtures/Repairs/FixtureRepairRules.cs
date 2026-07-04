using SIE.Domain.Validation;
using System;

namespace SIE.Fixtures.Repairs
{
    #region 工治具异常详情的工治具台帐非重验证规则
    /// <summary>
    /// 工治具异常详情的工治具台帐非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具异常详情的工治具台帐非重验证规则")]
    [System.ComponentModel.Description("工治具异常详情的工治具台帐非重验证规则")]
    public class NotDuplicateRepairDetail : NotDuplicateRule<FixtureRepairDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateRepairDetail()
        {
            Properties.Add(FixtureRepairDetail.FixtureRepairIdProperty);
            Properties.Add(FixtureRepairDetail.FixtureAccountIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as FixtureRepairDetail;
                return "已存在工治具台帐[{0}]的工治具异常详情".L10nFormat(entity.FixtureAccountCode);
            };
        }
    }
    #endregion
}
