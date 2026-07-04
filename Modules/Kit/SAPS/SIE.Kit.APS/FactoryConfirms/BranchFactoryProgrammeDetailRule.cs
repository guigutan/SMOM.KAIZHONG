using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

using System.ComponentModel;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 分配规则验证规则
    /// </summary>
    [DisplayName("重复验证")]
    [Description("分配方案明细不能重复")]
    class BranchFactoryProgrammeDetailRule : NotDuplicateRule<BranchFactoryProgrammeDetail>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public BranchFactoryProgrammeDetailRule()
        {
            Properties.Add(BranchFactoryProgrammeDetail.BranchFactoryProgrammeIdProperty);
            Properties.Add(BranchFactoryProgrammeDetail.ProgrammeRuleProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as BranchFactoryProgrammeDetail;
                return "分配方案中已经存在此规则".L10N().FormatArgs(r.BranchFactoryProgramme.Code);
            };
        }
    }
}
