using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.Tech.Processs
{

    /// <summary>
    /// 工序引用验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序加工时长验证规则")]
    [System.ComponentModel.Description("工序加工时长要大于0")]
    public class ProcessDurationRule : EntityRule<ProcessDuration>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessDurationRule()
        {            
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var processDuration = entity as ProcessDuration;
            if (processDuration!=null && processDuration.Durations <= 0)
            {
                e.BrokenDescription = "工序[{0}]产品[{1}]的[加工时长]小于或等于0，不能保存！"
                    .L10nFormat(processDuration.Process, processDuration.Product);
            }
        }
    }

    /// <summary>
    /// 工序加工时长重复性验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序加工时长重复性验证规则")]
    [System.ComponentModel.Description("工序加工时长重复性验证规则")]
    public class ProcessDurationNotDuplicateRule : NotDuplicateRule<ProcessDuration>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessDurationNotDuplicateRule()
        {
            Properties.Add(ProcessDuration.ProcessIdProperty);
            Properties.Add(ProcessDuration.ProductIdProperty);
            MessageBuilder = (e) => { return "已存在一条工序和产品都相同的记录，不能重复添加！".L10N(); };
        }
    }
}