using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工步 参数验证规则
    /// </summary>
    [DisplayName("工步 参数验证规则")]
    [Description("工步 参数验证规则")]
    public class WorkStepRule : EntityRule<WorkStep>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public WorkStepRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工序清单参数</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var workStep = entity as WorkStep;

            var lst = RT.Service.Resolve<SIE.Tech.Processs.ProcessController>().GetWorkSteps(workStep.ProcessId);
            bool isExists = lst.Any(t => t.Code == workStep.Code && t.Id != workStep.Id);
            if (isExists)
            {
                e.BrokenDescription = "当前工序的工步编码[{0}]不能重复。".L10nFormat(workStep.Code);
                return;
            }
            isExists = lst.Any(t => t.Name == workStep.Name && t.Id != workStep.Id);
            if (isExists)
            {
                e.BrokenDescription = "当前工序的名称[{1}]不能重复。".L10nFormat(workStep.Code, workStep.Name);
                return;
            }
            isExists = lst.Any(t => t.SeqNumber == workStep.SeqNumber && t.Id != workStep.Id);
            if (isExists)
            {
                e.BrokenDescription = "工步[{0}][{1}]顺序不能重复。".L10nFormat(workStep.Code, workStep.Name);
                return;
            }
        }
    }

}
