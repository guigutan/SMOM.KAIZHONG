using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配规则明细行号不重复
    /// </summary>
    [DisplayName("分配规则明细行号不重复")]
    [Description("分配规则明细行号不重复")]
    class AssignRuleDetailSubmitting : OnSubmitting<AssignRuleDetail>
    {
        protected override void Invoke(AssignRuleDetail entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert && RT.Service.Resolve<RuleController>().AssignRuleDetailIsExistLineNo(entity.AssignRuleId, entity.LineNo))
                throw new ValidationException("分配规则明细行号已存在!".L10N());
            //if (e.Action != SubmitAction.Delete)
            //{
            //    if ((entity.AssignBase == AssignBase.PackageFirst) && entity.PackageLevel == null)
            //        throw new ValidationException("整包优先的明细，分配包装层级必须有值".L10N());
            //}
        }
    }

    /// <summary>
    /// 周转规则明细行号不重复
    /// </summary>
    [DisplayName("周转规则明细行号不重复")]
    [Description("周转规则明细行号不重复")]
    class TurnOverRuleDetailSubmitting : OnSubmitting<TurnOverRuleDetail>
    {
        protected override void Invoke(TurnOverRuleDetail entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert && RT.Service.Resolve<RuleController>().TurnOverRuleDetailIsExistLineNo(entity.TurnOverRuleId, entity.LineNo))
                throw new ValidationException("周转规则明细行号已存在!".L10N());
        }
    }

    /// <summary>
    /// 上架规则明细行号不重复
    /// </summary>
    [DisplayName("上架规则明细行号不重复")]
    [Description("上架规则明细行号不重复")]
    class ShelvesRuleDetailSubmitting : OnSubmitting<OnShelvesRuleDetail>
    {
        protected override void Invoke(OnShelvesRuleDetail entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert && RT.Service.Resolve<RuleController>().OnshelvesRuleDetailIsExistLineNo(entity.OnShelvesRuleId, entity.LineNo))
                throw new ValidationException("上架规则明细行号已存在!".L10N());
        }
    }
}
