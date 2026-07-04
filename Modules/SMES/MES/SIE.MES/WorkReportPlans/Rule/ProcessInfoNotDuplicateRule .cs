using SIE.Domain.Validation;
using System;

namespace SIE.MES.WorkReportPlans.Rule
{


    #region 工序验证
    /// <summary>
    /// 工序验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序验证规则")]
    [System.ComponentModel.Description("工序已存在于其它的报工方案")]
    public class ProcessInfoNotDuplicateRule : NotDuplicateRule<ProcessInfo>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessInfoNotDuplicateRule()
        {
            Properties.Add(ProcessInfo.ProcessIdProperty);
            MessageBuilder = (e) =>
            {
                var model = e as ProcessInfo;
                return "工序【{0}】已存在报工方案".L10nFormat(model.Process.Name);
            };
        }
    }
    #endregion
}
